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
    public class DACustomers
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<Customer> Load(IEnumerable<Claim> claims)
        {
            List<Customer> lijst = new List<Customer>();
            string sql = "SELECT * FROM tblCustomers";
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


        public static Customer Load(int id, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblCustomers WHERE ID=@id";
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



        public static int Update(Customer customer, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "UPDATE tblCustomers SET Name=@Name, Address=@Address,Picture=@Picture,Balance=@Balance,RegisterNumber=@RegisterNumber WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", customer.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", customer.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Address", customer.Address);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Picture", customer.Picture);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Balance", customer.Balance);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", customer.RegisterNumber);
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5,par6);

            return id;
        }

        public static int Insert(Customer customer, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "INSERT INTO tblCustomers VALUES(@Name,@Address,@Picture,@Balance,@RegisterNumber)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", customer.Name);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Address", customer.Address);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Picture", customer.Picture);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Balance", customer.Balance);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", customer.RegisterNumber);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4,par5);

            return id;
        }




        private static Customer Create(IDataRecord data)
        {

            return new Customer()
            {
                ID = (int)data["ID"],
                Name = data["Name"].ToString(),
                Address=data["Address"].ToString(),
                Picture=(data["Picture"] is DBNull) ? null : (byte[])data["Picture"],
                Balance = decimal.ToDouble((decimal)data["Balance"]),
                RegisterNumber=data["RegisterNumber"].ToString()

            };
        }
    }
}