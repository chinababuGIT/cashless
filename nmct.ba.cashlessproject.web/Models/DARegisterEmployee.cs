using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.web.Helper;
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
    public class DARegisterEmployee
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<RegisterEmployee> Load(IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> lijst = new List<RegisterEmployee>();
            string sql = "SELECT b.EmployeeID,b.RegisterID,a.Name as RegisterName,a.Device as RegisterDevice,a.Hidden as RegisterHidden,b.[From],b.[Until],c.Address as EmployeeAddress,c.Email as EmployeeEmail,c.Name as EmployeeName,c.Phone as EmployeePhone, c.Picture as EmployeePicture, c.RegisterNumber as EmployeeRegisterNumber FROM tblRegisters as a INNER JOIN tblRegistersEmployees as b on a.ID=b.RegisterID INNER JOIN tblEmployees as c on c.ID=b.employeeID";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    RegisterEmployee geg = Create(data);
                    if (geg != null) lijst.Add(geg);

                }
                data.NextResult();
            }

            return lijst;

        }

        public static RegisterEmployee Load(int id, IEnumerable<Claim> claims)
        {

            string sql = "SELECT b.EmployeeID,b.RegisterID,a.Name as RegisterName,a.Device as RegisterDevice,a.Hidden as RegisterHidden,b.[From],b.[Until],c.Address as EmployeeAddress,c.Email as EmployeeEmail,c.Name as EmployeeName,c.Phone as EmployeePhone, c.Picture as EmployeePicture, c.RegisterNumber as EmployeeRegisterNumber FROM tblRegisters as a INNER JOIN tblRegistersEmployees as b on a.ID=b.RegisterID INNER JOIN tblEmployees as c on c.ID=b.employeeID";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

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
        public static int Update(RegisterEmployee registerEmployee, IEnumerable<Claim> claims)
        {
            int id;
            DateTime from = TimeConverter.ToDateTime(registerEmployee.From);
            DateTime until = TimeConverter.ToDateTime(registerEmployee.Until);

            string sql = "UPDATE tblRegistersEmployees SET [Until]=@Until WHERE RegisterID=@RegisterID AND EmployeeID=@EmployeeID AND [From]=@From";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "RegisterID", registerEmployee.RegisterID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "EmployeeID", registerEmployee.EmployeeID);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "From", from);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Until", until);

            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);

            return id;
        }

        public static int Insert(RegisterEmployee registerEmployee, IEnumerable<Claim> claims)
        {
            int id = 0;
            DateTime from = TimeConverter.ToDateTime(registerEmployee.From);
            DateTime until = TimeConverter.ToDateTime(registerEmployee.Until);

            string sql = "INSERT INTO tblRegistersEmployees VALUES(@RegisterID,@EmployeeID,@From,@Until)";

            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "RegisterID", registerEmployee.RegisterID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "EmployeeID", registerEmployee.EmployeeID);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "From", from);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Until", until);

            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);


            return id;
        }


        private static RegisterEmployee Create(IDataRecord data)
        {

            try
            {
                return new RegisterEmployee()
                {
                    EmployeeID = (int)data["EmployeeID"],
                    EmployeeAddress = data["EmployeeAddress"].ToString().Trim(),
                    EmployeeEmail = data["EmployeeEmail"].ToString().Trim(),
                    EmployeeName = data["EmployeeName"].ToString().Trim(),
                    EmployeePhone = data["EmployeePhone"].ToString().Trim(),
                    EmployeePicture = (data["EmployeePicture"] is DBNull) ? null : (byte[])data["EmployeePicture"],
                    EmployeeRegisterNumber = data["EmployeeRegisterNumber"].ToString().Trim(),
                    RegisterID = (int)data["RegisterID"],
                    RegisterName = data["RegisterName"].ToString().Trim(),
                    RegisterDevice = data["RegisterDevice"].ToString().Trim(),
                    RegisterHidden = (bool)data["RegisterHidden"],
                    From = (long)TimeConverter.ToUnixTimeStamp((DateTime)data["From"]),
                    Until = (long)TimeConverter.ToUnixTimeStamp((DateTime)data["Until"])


                };
            }
            catch (Exception ex)
            {

                return null;
            }

        }
       

    }
}