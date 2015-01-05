using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class DARegisters
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
            string sql = "SELECT * FROM tblRegisters";
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

        public static List<RegisterEmployee> Load_RegisterEmployee(IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> lijst = new List<RegisterEmployee>();
            string sql = "SELECT b.EmployeeID,b.RegisterID,a.Name as RegisterName,a.Device,b.[From],b.[Until],c.[Address] as EmployeeAddress,c.Email as EmployeeEmail,c.Name as EmployeeName,c.Phone as EmployeePhone FROM tblRegisters as a INNER JOIN tblRegistersEmployees as b on a.ID=b.RegisterID INNER JOIN tblEmployees as c on c.ID=b.employeeID";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    lijst.Add(Create_RegisterEmployee(data));

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


        public static RegisterEmployee Load_RegisterEmployee(int id,IEnumerable<Claim> claims)
        {

            string sql = "SELECT b.EmployeeID,b.RegisterID,a.Name as RegisterName,a.Device,b.[From],b.[Until],c.[Address] as EmployeeAddress,c.Email as EmployeeEmail,c.Name as EmployeeName,c.Phone as EmployeePhone FROM tblRegisters as a INNER JOIN tblRegistersEmployees as b on a.ID=b.RegisterID INNER JOIN tblEmployees as c on c.ID=b.employeeID";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            data.Read();

            if (data.HasRows)
            {
                return Create_RegisterEmployee(data);
            }
            else
            {
                return null;
            }

        }

        private static Register Create(IDataRecord data)
        {

            return new Register()
            {
                ID = (int)data["ID"],
                Name= data["Name"].ToString(),
                Device=data["Device"].ToString()

            };
        }

        private static RegisterEmployee Create_RegisterEmployee(IDataRecord data)
        {

            return new RegisterEmployee()
            {
                EmployeeID = (int)data["EmployeeID"],
                RegisterID=(int) data["RegisterID"],
                RegisterName = data["RegisterName"].ToString(),
                Device = data["Device"].ToString(),
                From= (long) TimeConverter.ToUnixTimeStamp((DateTime)data["From"]),
                Until=(long) TimeConverter.ToUnixTimeStamp((DateTime)data["Until"]),
                EmployeeAddress= data["EmployeeAddress"].ToString(),
                EmployeeEmail = data["EmployeeEmail"].ToString(),
                EmployeeName=data["EmployeeName"].ToString(),
                EmployeePhone=data["EmployeePhone"].ToString()

            };
        }

    }
}