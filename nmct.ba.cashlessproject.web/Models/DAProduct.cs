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
    public class DAProduct
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<Product> Load(IEnumerable<Claim> claims)
        {
            List<Product> lijst = new List<Product>();
            string sql = "SELECT * FROM tblProducts WHERE Hidden=0";
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

        public static Product Load(int id,IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tblProducts WHERE ID=@id";
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

        public static int Update(Product product, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "UPDATE tblProducts SET Name=@Name,Picture=@Picture,Price=@Price,Description=@Description,Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", product.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", (product.Name==null)?string.Empty:product.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Price", product.Price);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Description", (product.Description == null) ? string.Empty : product.Description);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Picture", (product.Picture == null) ? new Byte[0] : product.Picture);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "Hidden",  product.Hidden);
            id= Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4,par5,par6);

            return id;
        }

        public static int Insert(Product product, IEnumerable<Claim> claims)
        {
            int id;

            #region exists
            string sql = "SELECT * FROM tblProducts WHERE Name=@Name";
            DbParameter parName = Database.AddParameter(CreateConnectionString(claims), "Name", product.Name);
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parName);

            data.Read();

            if (data.HasRows)
            {
                product.ID = (int)data["ID"];
                product.Hidden = false;
                data.Close();
                return Update(product, claims);
            }
            #endregion

            string sql2 = "INSERT INTO tblProducts VALUES(@Name,@Picture,@Price,@Description,@Hidden)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", (product.Name==null)?string.Empty:product.Name);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Price", product.Price);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Description", (product.Description == null) ? string.Empty : product.Description);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Picture", (product.Picture == null) ? new Byte[0] : product.Picture);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Hidden", product.Hidden);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql2, par1, par2, par3,par4,par5);

            return id;
        }
        public static void Remove(int id, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE tblProducts SET Hidden=@Hidden WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", id);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Hidden", 1);

            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1,par2);

        }

        private static Product Create(IDataRecord data)
        {
            try
            {
                return new Product()
                {
                    ID = (int)data["ID"],
                    Name = data["Name"].ToString().Trim(),
                    Picture = (data["Picture"] is DBNull) ? null : (byte[])data["Picture"],
                    Price = decimal.ToDouble((decimal)data["Price"]),
                    Description = data["Description"].ToString().Trim(),
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