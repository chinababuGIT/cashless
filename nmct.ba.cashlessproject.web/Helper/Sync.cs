using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.web.Helper
{
    public class Sync
    {
        public static void Start()
        {
            List<Organisation> organisations = DAOrganisation.Load();
            List<model.ITBedrijf.Register> registers_ITBedrijf = nmct.ba.cashlessproject.web.Models.OrganisationModel.DARegister.Load();
            for (int i = 0; i < organisations.Count; i++)
            {
                if (organisations[i].ID!=null && organisations[i].ID!=0)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("DbUsername", organisations[i].DbUsername));
                    claims.Add(new Claim("DbPassword", organisations[i].DbPassword));
                    claims.Add(new Claim("DbName", organisations[i].DbName));
                    claims.Add(new Claim("ID", organisations[i].ID.ToString()));
                    CheckInDatabaseCustomer(organisations[i], claims);
                }
               
            }
        }

        private static void CheckInDatabaseCustomer(Organisation organisation, List<Claim> claims)
        {
            List<OrganisationRegister> organisationRegisters = DAOrganisationRegister.LoadByOrganisation(organisation.ID);
            List<model.Kassa.Log> logs = nmct.ba.cashlessproject.web.Models.DALog.Load(claims);

            CheckRegisters(organisationRegisters, claims);
            CheckLogs(logs, claims);
        }

        private static void CheckLogs(List<model.Kassa.Log> logs, List<Claim> claims)
        {
            List<model.ITBedrijf.Log> newLogs = new List<model.ITBedrijf.Log>();
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].RegisterID!=null && logs[i].RegisterID!=0)
                {
                    newLogs.Add(new model.ITBedrijf.Log()
                    {
                        RegisterID = logs[i].RegisterID,
                        Message = logs[i].Message,
                        StackTrace = logs[i].StackTrace,
                        Timestamp = logs[i].Timestamp
                    });
                }
               

            }

            nmct.ba.cashlessproject.web.Models.OrganisationModel.DALog.Insert(newLogs, claims);
        }

        private static void CheckRegisters(List<OrganisationRegister> organisationRegisters, List<Claim> claims)
        {
            List<model.Kassa.Register> newRegisters = new List<model.Kassa.Register>();
            for (int i = 0; i < organisationRegisters.Count; i++)
            {
                //als untildate groter is dan datum van vandaag
                if (organisationRegisters[i].UntilDate >= TimeConverter.ToUnixTimeStamp(DateTime.Now))
                {
                    newRegisters.Add(new model.Kassa.Register()
                    {
                        ID = organisationRegisters[i].RegisterID,
                        Name = organisationRegisters[i].RegisterName,
                        Device = organisationRegisters[i].RegisterDevice,
                        Hidden = false
                    });
                }
                else//als untildate kleiner is dan vandaag, kassa wordt op hidden gezet
                {
                    newRegisters.Add(new model.Kassa.Register()
                    {
                        ID = organisationRegisters[i].RegisterID,
                        Name = organisationRegisters[i].RegisterName,
                        Device = organisationRegisters[i].RegisterDevice,
                        Hidden = true
                    });


                }

            }

            nmct.ba.cashlessproject.web.Models.DARegister.Insert(newRegisters, claims);
        }



    }
}