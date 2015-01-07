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
    public class DACustomer
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
            string sql = "SELECT * FROM tblCustomers WHERE Hidden=0";
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    Customer customer=Create(data);
                    if (customer !=null)
                    {
                        lijst.Add(customer);   
                    }
                   
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
        public static Customer Load(string registerNumber, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblCustomers WHERE RegisterNumber=@RegisterNumber";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", registerNumber);
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            data.Read();

            if (data.HasRows)
            {
                return Create(data);
            }
            else
            {
                return new Customer();
            }


        }



        public static int Update(Customer customer, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "UPDATE tblCustomers SET Name=@Name,SurName=@SurName,Address=@Address,Picture=@Picture,Balance=@Balance,RegisterNumber=@RegisterNumber,Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", customer.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", (customer.Name == null) ? string.Empty : customer.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "SurName", (customer.SurName == null) ? string.Empty : customer.SurName);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Address", (customer.Address == null) ? string.Empty : customer.Address);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Picture", (customer.Picture == null) ? new Byte[0] : customer.Picture);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "Balance", (customer.Balance == null) ? 0.0 : customer.Balance);
            DbParameter par7 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", (customer.RegisterNumber == null) ? string.Empty : customer.RegisterNumber);
            DbParameter par8 = Database.AddParameter(CreateConnectionString(claims), "Hidden", customer.Hidden);

           
            
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5,par6,par7,par8);

            return id;
        }

        public static int Insert(Customer customer, IEnumerable<Claim> claims)
        {
            int id;
            #region exists
            if (customer.RegisterNumber!=null && customer.RegisterNumber !="")
            {
                string sql = "SELECT * FROM tblCustomers WHERE RegisterNumber=@RegisterNumber";
                DbParameter parReg = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", customer.RegisterNumber);
                DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parReg);

                data.Read();

                if (data.HasRows)
                {
                    customer.ID = (int)data["ID"];
                    customer.Hidden = false;
                    data.Close();
                    return Update(customer, claims);
                }
            }
            #endregion


            string sql2 = "INSERT INTO tblCustomers VALUES(@Name,@SurName,@Address,@Picture,@Balance,@RegisterNumber,@Hidden)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", (customer.Name==null)?string.Empty:customer.Name);
            DbParameter par7 = Database.AddParameter(CreateConnectionString(claims), "SurName", (customer.SurName == null) ? string.Empty : customer.SurName);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Address", (customer.Address==null)?string.Empty:customer.Address);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Picture", (customer.Picture==null) ?  new Byte[0] : customer.Picture);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Balance", (customer.Balance==null)? 0.0:customer.Balance);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "RegisterNumber", (customer.RegisterNumber == null) ? string.Empty : customer.RegisterNumber);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "Hidden", customer.Hidden);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql2, par1, par2, par3, par4,par5,par6,par7);

            return id;
        }

        public static void Remove(int id, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE tblCustomers SET Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", id);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Hidden", 1);

            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);

        }


        private static Customer Create(IDataRecord data)
        {
            try
            {
                return new Customer()
                {
                    ID = (int)data["ID"],
                    Name = data["Name"].ToString().Trim(),
                    SurName = data["SurName"].ToString().Trim(),
                    Address = data["Address"].ToString().Trim(),
                    Picture = (data["Picture"] is DBNull) ? null : (byte[])data["Picture"],
                    Balance = decimal.ToDouble((decimal)data["Balance"]),
                    RegisterNumber = data["RegisterNumber"].ToString().Trim()

                };
            }
            catch (Exception)
            {
                return null;
               
            }
            
        }
    }
}