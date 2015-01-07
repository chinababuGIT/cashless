using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models.OrganisationModel
{
    public class DAOrganisationRegister
    {
        private const string CONNECTIONSTRING = "DB";
        private const int AANTAL_JAREN = 5;
        public static List<OrganisationRegister> Load()
        {
            List<OrganisationRegister> lijst = new List<OrganisationRegister>();
            string sql = "SELECT a.ID as OrganisationID,a.Name as OrganisationName, a.Address as OrganisationAddress, a.Email as OrganisationEmail, a.Phone as OrganisationPhone,b.FromDate, b.UntilDate, c.ID as RegisterID,c.Name as RegisterName,c.Device as RegisterDevice,c.PurchaseDate as RegisterPurchaseDate, c.ExpiresDate as RegisterExpiresDate FROM tblOrganisations as a INNER JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID INNER JOIN tblRegisters as c ON b.RegisterID=c.ID";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    lijst.Add(Create(data));

                }
                data.NextResult();
            }

            return lijst;
        }

        public static List<OrganisationRegister> LoadByOrganisation(int id)
        {
            List<OrganisationRegister> organisationRegisters = new List<OrganisationRegister>();
            string sql = "SELECT a.ID as OrganisationID,a.Name as OrganisationName, a.Address as OrganisationAddress, a.Email as OrganisationEmail, a.Phone as OrganisationPhone,b.FromDate, b.UntilDate, c.ID as RegisterID,c.Name as RegisterName,c.Device as RegisterDevice,c.PurchaseDate as RegisterPurchaseDate, c.ExpiresDate as RegisterExpiresDate FROM tblOrganisations as a INNER JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID INNER JOIN tblRegisters as c ON b.RegisterID=c.ID WHERE OrganisationID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    organisationRegisters.Add(Create(data));
                }
                data.NextResult();
            }

            return organisationRegisters;
        }
        public static OrganisationRegister LoadByRegister(int id)
        {
            OrganisationRegister organisationRegister = new OrganisationRegister();
            string sql = "SELECT a.ID as OrganisationID,a.Name as OrganisationName, a.Address as OrganisationAddress, a.Email as OrganisationEmail, a.Phone as OrganisationPhone,b.FromDate, b.UntilDate, c.ID as RegisterID,c.Name as RegisterName,c.Device as RegisterDevice,c.PurchaseDate as RegisterPurchaseDate, c.ExpiresDate as RegisterExpiresDate FROM tblOrganisations as a INNER JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID right JOIN tblRegisters as c ON b.RegisterID=c.ID WHERE c.ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            data.Read();
            if (data["UntilDate"] is DBNull)
            {
               organisationRegister= CreateWithEmptyOrganisations(data);
            }
            else
            {
                organisationRegister = Create(data);

            }
            return organisationRegister;
        }
        public static List<OrganisationRegister> LoadWithEmptyOrganisations()
        {
            List<OrganisationRegister> lijst = new List<OrganisationRegister>();
            string sql = "SELECT a.ID as OrganisationID,a.Name as OrganisationName, a.Address as OrganisationAddress, a.Email as OrganisationEmail, a.Phone as OrganisationPhone,b.FromDate, b.UntilDate, c.ID as RegisterID,c.Name as RegisterName,c.Device as RegisterDevice,c.PurchaseDate as RegisterPurchaseDate, c.ExpiresDate as RegisterExpiresDate FROM tblOrganisations as a left JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID right JOIN tblRegisters as c ON b.RegisterID=c.ID";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    if (data["OrganisationName"] is DBNull)
                    {
                        lijst.Add(CreateWithEmptyOrganisations(data));
                    }
                    else
                    {
                        lijst.Add(Create(data));

                    }
                   

                }
                data.NextResult();
            }

            return lijst;
        }

        public static int Update(OrganisationRegister orgReg)
        {
            int id = 0;
            int id2 = 0;
            DateTime purchaseDate = TimeConverter.ToDateTime(orgReg.RegisterPurchaseDate);
            DateTime expiresDate = TimeConverter.ToDateTime(orgReg.RegisterExpiresDate);



            string sql = "UPDATE tblRegisters SET Name=@Name,Device=@Device,PurchaseDate=@PurchaseDate,ExpiresDate=@ExpiresDate WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", orgReg.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Name", orgReg.RegisterName);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Device", (orgReg.RegisterDevice==null)?string.Empty:orgReg.RegisterDevice);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "PurchaseDate", purchaseDate);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "ExpiresDate", expiresDate);
            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2, par3, par4,par5);

            if (id==0)
            {
               return Delete(orgReg);
            }

            #region update and insert
          
            //controle
            DateTime fromDate = TimeConverter.ToDateTime(orgReg.FromDate);
            if (orgReg.FromDate == 0) fromDate = DateTime.Now;

            DateTime untilDate = TimeConverter.ToDateTime(orgReg.FromDate);
            if (orgReg.UntilDate == 0) untilDate = DateTime.Now;
            

            string sql_ = "UPDATE tblOrganisationsRegisters SET OrganisationID=@OrganisationID,RegisterID=@RegisterID,FromDate=@FromDate WHERE RegisterID=@RegisterID";
            DbParameter par_1 = Database.AddParameter(CONNECTIONSTRING, "OrganisationID", orgReg.OrganisationID);
            DbParameter par_2 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", orgReg.RegisterID);
            DbParameter par_3 = Database.AddParameter(CONNECTIONSTRING, "FromDate", fromDate);
            DbParameter par_4 = Database.AddParameter(CONNECTIONSTRING, "UntilDate",untilDate);
            id2 = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql_, par_1, par_2, par_3,par_4);

            if (id2==0)
            {
                 string sql__ = "INSERT INTO tblOrganisationsRegisters VALUES(@OrganisationID,@RegisterID,@FromDate,@UntilDate)";
                 DbParameter par__1 = Database.AddParameter(CONNECTIONSTRING, "OrganisationID", orgReg.OrganisationID);
                 DbParameter par__2 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", orgReg.RegisterID);
                 DbParameter par__3 = Database.AddParameter(CONNECTIONSTRING, "FromDate", fromDate);
                 DbParameter par__4 = Database.AddParameter(CONNECTIONSTRING, "UntilDate", fromDate.AddYears(AANTAL_JAREN));
                 id2 = Database.InsertData(Database.GetConnection(CONNECTIONSTRING), sql__, par__1, par__2, par__3, par__4);
            }

            #endregion

            return id2;
        }

        public static int Insert(OrganisationRegister orgReg)
        {
            int id = 0;
            int id2 = 0;
            DbTransaction trans = null;
            DateTime time = TimeConverter.ToDateTime(orgReg.RegisterPurchaseDate);
            if (orgReg.RegisterPurchaseDate == 0) time = DateTime.Now;

            DateTime expireDate = time.AddYears(AANTAL_JAREN);

            DateTime time_ = TimeConverter.ToDateTime(orgReg.FromDate);
            if (orgReg.FromDate == 0) time_= DateTime.Now;


            try
            {

                trans = Database.BeginTransaction(CONNECTIONSTRING);

                string sql = "INSERT INTO tblRegisters VALUES(@Name,@Device,@PurchaseDate,@ExpiresDate)";
                DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "Name", orgReg.RegisterName);
                DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Device", (orgReg.RegisterDevice == null) ? string.Empty : orgReg.RegisterDevice);
                DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "PurchaseDate", time);
                DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "ExpiresDate", expireDate);

                id = Database.InsertData(trans, sql, par1, par2, par3, par4);

                string sql_ = "INSERT INTO tblOrganisationsRegisters VALUES(@OrganisationID,@RegisterID,@FromDate,@UntilDate)";
                DbParameter par_1 = Database.AddParameter(CONNECTIONSTRING, "OrganisationID", orgReg.OrganisationID);
                DbParameter par_2 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", id);
                DbParameter par_3 = Database.AddParameter(CONNECTIONSTRING, "FromDate", time_);
                DbParameter par_4 = Database.AddParameter(CONNECTIONSTRING, "UntilDate", expireDate);

                id2 = Database.InsertData(trans, sql_, par_1, par_2, par_3, par_4);

                trans.Commit();

                return id2;
            }
            catch (Exception)
            {

                trans.Rollback();
                return 0;
            }
           
        }

        public static int Delete(OrganisationRegister orgReg)
        {
           
            int id;

            string sql = "DELETE FROM tblOrganisationsRegisters WHERE RegisterID=@RegisterID AND OrganisationID=@OrganisationID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", orgReg.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "OrganisationID", orgReg.OrganisationID);
            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1,par2);
            return id;
        }
        private static OrganisationRegister Create(DbDataReader data)
        {
            try
            {
                return new OrganisationRegister()
                {
                    OrganisationID = (int)data["OrganisationID"],
                    OrganisationName = data["OrganisationName"].ToString().Trim(),
                    OrganisationAddress = data["OrganisationAddress"].ToString().Trim(),
                    OrganisationEmail = data["OrganisationEmail"].ToString().Trim(),
                    OrganisationPhone = data["OrganisationPhone"].ToString().Trim(),
                    FromDate = TimeConverter.ToUnixTimeStamp((DateTime)data["FromDate"]),
                    UntilDate = TimeConverter.ToUnixTimeStamp((DateTime)data["UntilDate"]),
                    RegisterID = (int)data["RegisterID"],
                    RegisterName = data["RegisterName"].ToString().Trim(),
                    RegisterDevice = data["RegisterDevice"].ToString().Trim(),
                    RegisterPurchaseDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterPurchaseDate"]),
                    RegisterExpiresDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterExpiresDate"])
                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        private static OrganisationRegister CreateWithEmptyOrganisations(DbDataReader data)
        {
            try
            {
                return new OrganisationRegister()
                {
                    OrganisationID = 0,
                    OrganisationName = "",
                    OrganisationAddress = "",
                    OrganisationEmail = "",
                    OrganisationPhone = "",
                    FromDate = 0,
                    UntilDate = 0,
                    RegisterID = (int)data["RegisterID"],
                    RegisterName = data["RegisterName"].ToString().Trim(),
                    RegisterDevice = data["RegisterDevice"].ToString().Trim(),
                    RegisterPurchaseDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterPurchaseDate"]),
                    RegisterExpiresDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterExpiresDate"])
                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }





       
    }
}