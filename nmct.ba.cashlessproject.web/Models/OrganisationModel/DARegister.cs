using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models.OrganisationModel
{
    public class DARegister
    {
        private const string CONNECTIONSTRING = "DB";
        public static List<Register> Load()
        {
            List<Register> lijst = new List<Register>();
            string sql = "SELECT * FROM tblRegisters";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    lijst.Add(Create(data));

                }
                data.NextResult();
            }
            #region 0
            List<Register> emptyList = new List<Register>();
            emptyList.Add(new Register()
            {

                ID = 0,
                Name = "Geen kassa"
            });
            lijst.InsertRange(0, emptyList);
            #endregion
            return lijst;
        }

        public static List<Register> LoadEmptyRegister()
        {
            List<Register> lijst = new List<Register>();
            List<int> id= new List<int>();
            string sql = "SELECT c.ID as RegisterID, a.ID as OrganisationID FROM tblOrganisations as a LEFT JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID RIGHT JOIN tblRegisters as c ON b.RegisterID=c.ID";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    int a;
                    if (Int32.TryParse(data["OrganisationID"].ToString(),out a)==false)
                    {
                        id.Add((int)data["RegisterID"]);
                    }
                    //id.Add((int)data["OrganisationID"]);

                }
                data.NextResult();
            }
            for (int i = 0; i < id.Count; i++)
            {
                lijst.Add(Load(id[i]));
            }
            return lijst;
        }

        public static Register Load(int id)
        {
            Register register = new Register();
            string sql = "SELECT * FROM tblRegisters WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            data.Read();
            register = Create(data);
            return register;
        }
        public static List<Register> LoadByOrganisationID(int id)
        {
            List<Register> lijst = new List<Register>();
            string sql = "SELECT c.ID,c.Name,c.Device,c.PurchaseDate, c.ExpiresDate FROM tblOrganisations as a INNER JOIN tblOrganisationsRegisters as b ON a.ID=b.OrganisationID INNER JOIN tblRegisters as c ON b.RegisterID=c.ID WHERE a.ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

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

        public static int Update(Register register)
        {
            int id;
            DateTime time = TimeConverter.ToDateTime(register.ExpiresDate);
            DateTime time2 = TimeConverter.ToDateTime(register.ExpiresDate);

            string sql = "UPDATE tblRegisters SET Name=@Name,Device=@Device,PurchaseDate=@PurchaseDate,ExpiresDate=@ExpiresDate WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", register.ID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Name", register.Name);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Device", (register.Device==null)?string.Empty:register.Device);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "PurchaseDate", time);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "ExpiresDate", time2);

            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2, par3, par4, par5);

            return id;
        }

        public static int Insert(Register register)
        {
            int id = 0;
            DateTime time = TimeConverter.ToDateTime(register.ExpiresDate);
            DateTime time2 = TimeConverter.ToDateTime(register.ExpiresDate);

            string sql = "INSERT INTO tblRegisters VALUES(@Name,@Device,@PurchaseDate,@ExpiresDate)";

            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Name", register.Name);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Device", (register.Device==null)?string.Empty:register.Device);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "PurchaseDate", time);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "ExpiresDate", time2);
            id = Database.InsertData(Database.GetConnection(CONNECTIONSTRING), sql, par2, par3, par4, par5);

            return id;
        }
        public static int Delete(int id)
        {
            
            string sql = "UPDATE tblRegisters SET ExpiresDate=@ExpiresDate WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ExpiresDate",DateTime.Now);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2);
            return id;
        }
       
        private static Register Create(DbDataReader data)
        {
            try
            {
                return new Register()
                {
                    ID = (int)data["ID"],
                    Name = data["Name"].ToString().Trim(),
                    Device = data["Device"].ToString().Trim(),
                    PurchaseDate = TimeConverter.ToUnixTimeStamp((DateTime)data["PurchaseDate"]),
                    ExpiresDate = TimeConverter.ToUnixTimeStamp((DateTime)data["ExpiresDate"])
                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }




        
    }
}