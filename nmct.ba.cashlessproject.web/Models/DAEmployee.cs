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
    public class DAEmployee
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<Employee> Load(IEnumerable<Claim> claims)
        {
            List<Employee> lijst = new List<Employee>();
            string sql = "SELECT * FROM tblEmployees WHERE Hidden=0";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    Employee emloyee = Create(data);
                    if (emloyee != null)
                    {
                        lijst.Add(emloyee);
                    }

                }
                data.NextResult();
            }

            return lijst;

        }

        public static Employee Load(int id, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblEmployees WHERE ID=@id";
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
        public static Employee Load(string registerNumber, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblEmployees WHERE RegisterNumber=@RegisterNumber";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", registerNumber);
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            data.Read();

            if (data.HasRows)
            {
                return Create(data);
            }
            else
            {
                return new Employee();
            }


        }

        public static int Update(Employee employee, IEnumerable<Claim> claims)
        {
            int id;


            string sql = "UPDATE tblEmployees SET Name=@Name,SurName=@SurName,Picture=@Picture,Address=@Address,Email=@Email,Phone=@Phone,RegisterNumber=@RegisterNumber,Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", employee.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", (employee.Name == null) ? string.Empty : employee.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "SurName", (employee.SurName == null) ? string.Empty : employee.SurName);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Address", (employee.Address == null) ? string.Empty : employee.Address);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Email", (employee.Email == null) ? string.Empty : employee.Email);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "Phone", (employee.Phone == null) ? string.Empty : employee.Phone);
            DbParameter par7 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", (employee.RegisterNumber == null) ? string.Empty : employee.RegisterNumber);
            DbParameter par8 = Database.AddParameter(CreateConnectionString(claims), "Picture", (employee.Picture == null) ? new Byte[0] : employee.Picture);
            DbParameter par9 = Database.AddParameter(CreateConnectionString(claims), "Hidden", employee.Hidden);
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8,par9);

            return id;
        }

        public static int Insert(Employee employee, IEnumerable<Claim> claims)
        {
            int id;

            #region exists
            if (employee.RegisterNumber != null && employee.RegisterNumber != "")
            {
                string sql = "SELECT * FROM tblEmployees WHERE RegisterNumber=@RegisterNumber";
                DbParameter parReg = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", employee.RegisterNumber);
                DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parReg);

                data.Read();

                if (data.HasRows)
                {
                    employee.ID = (int)data["ID"];
                    employee.Hidden = false;
                    data.Close();
                    return Update(employee, claims);
                }
            }
            #endregion


            string sql2 = "INSERT INTO tblEmployees VALUES(@Name,@SurName,@Picture,@Address,@Email,@Phone,@RegisterNumber,@Hidden)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", (employee.Name==null)?string.Empty:employee.Name);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "SurName", (employee.SurName == null) ? string.Empty : employee.SurName);         
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Address", (employee.Address==null)? string.Empty:employee.Address);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Email", (employee.Email==null)?string.Empty:employee.Email);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Phone", (employee.Phone==null)? string.Empty:employee.Phone);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", (employee.RegisterNumber==null)? string.Empty:employee.RegisterNumber);
            DbParameter par7 = Database.AddParameter(CreateConnectionString(claims), "Picture", (employee.Picture == null) ? new Byte[0] : employee.Picture);
            DbParameter par8 = Database.AddParameter(CreateConnectionString(claims), "Hidden", employee.Hidden);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql2, par1, par2, par3,par4,par5,par6,par7,par8);

            return id;
        }
        public static void Remove(int id, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE tblEmployees SET Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", id);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Hidden", 1);

            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);

        }

        private static Employee Create(IDataRecord data)
        {
            try
            {
                return new Employee()
                {
                    ID = (int)data["ID"],
                    Name = data["Name"].ToString().Trim(),
                    SurName = data["SurName"].ToString().Trim(),
                    Picture = (data["Picture"] is DBNull) ? null : (byte[])data["Picture"],
                    Address = data["Address"].ToString().Trim(),
                    Email = data["Email"].ToString().Trim(),
                    Phone = data["Phone"].ToString().Trim(),
                    Hidden = (bool)data["Hidden"],
                    RegisterNumber = data["RegisterNumber"].ToString().Trim(),

                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}