using nmct.ba.cashlessproject.web.Helper;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models
{
    public class DARegister
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<Register> Load(IEnumerable<Claim> claims)
        {
            List<Register> lijst = new List<Register>();
            string sql = "SELECT * FROM tblRegisters WHERE Hidden=0";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

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

        

        public static Register Load(int id, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblRegisters WHERE ID=@id";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "id", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            data.Read();

            if (data.HasRows)
            {
                return Create(data);
            }
            else
            {
                return null;
            }


        }

        public static int Update(Register register, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "UPDATE tblRegisters SET Name=@Name,Device=@Device,Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", register.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", (register.Name==null)?string.Empty:register.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Device", (register.Device==null)?string.Empty:register.Device);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Hidden", register.Hidden);
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);

            return id;
        }

        public static int Insert(Register register, IEnumerable<Claim> claims)
        {
            int id;

            #region exists
            if (register.ID!=0 && register.ID!=null)
            {
                string sql = "SELECT * FROM tblRegisters WHERE ID=@ID";
                DbParameter parID = Database.AddParameter(CreateConnectionString(claims), "ID", register.ID);
                DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);

                data.Read();

                if (data.HasRows)
                {
                    register.ID = (int)data["ID"];
                    data.Close();
                    return Update(register, claims);
                }
            }
           
            #endregion

            string sql2 = "INSERT INTO tblRegisters VALUES(@ID,@Name,@Device,@Hidden)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", register.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", (register.Name==null)?string.Empty:register.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Device", (register.Device==null)?string.Empty:register.Device);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Hidden", register.Hidden);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql2, par1, par2, par3, par4);

            return id;
        }
        public static List<int> Insert(List<Register> registers, IEnumerable<Claim> claims)
        {
            List<int> lijst = new List<int>();
            for (int i = 0; i < registers.Count; i++)
            {
                lijst.Add(Insert(registers[i],claims));
            }
            return lijst;
        }


      

        private static Register Create(IDataRecord data)
        {
            try
            {
                return new Register()
                {
                    ID = (int)data["ID"],
                    Name = data["Name"].ToString().Trim(),
                    Device = data["Device"].ToString().Trim(),
                    Hidden = (bool)data["Hidden"]

                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}