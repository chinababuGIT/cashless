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
    public class DAEmployees
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
            string sql = "SELECT * FROM tblEmployees";
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

        public static int Update(Employee product, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "UPDATE tblEmployees SET Name=@Name, Address=@Address,Email=@Email,Phone=@Phone WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", product.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", product.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Address", product.Address);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Email", product.Email);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Phone", product.Phone);
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4,par5);

            return id;
        }

        public static int Insert(Employee product, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "INSERT INTO tblEmployees VALUES(@Name,@Address,@Email,@Phone)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", product.Name);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Address", product.Address);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Email", product.Email);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Phone", product.Phone);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3,par4);

            return id;
        }
        public static void Remove(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM tblEmployees WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", id);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

        }

        private static Employee Create(IDataRecord data)
        {

            return new Employee()
            {
                ID = (int)data["ID"],
                Name = data["Name"].ToString(),
                Address = data["Address"].ToString(),
                Email = data["Email"].ToString(),
                Phone= data["Phone"].ToString()
            };
        }
    }
}