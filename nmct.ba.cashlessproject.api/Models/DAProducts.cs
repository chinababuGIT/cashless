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
    public class DAProducts
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
            string sql = "SELECT * FROM tblProducts";
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
            string sql = "UPDATE tblProducts SET Name=@Name, Price=@Price,Description=@Description WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", product.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Name", product.Name);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Price", product.Price);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Description", product.Description);
            id= Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);

            return id;
        }

        public static int Insert(Product product, IEnumerable<Claim> claims)
        {
            int id;
            string sql = "INSERT INTO tblProducts VALUES(@Name,@Price,@Description)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "Name", product.Name);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "Price", product.Price);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "Description", product.Description);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);

            return id;
        }
        public static void Remove(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM tblProducts WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", id);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

        }

        private static Product Create(IDataRecord data)
        {

            return new Product()
            {
                ID = (int)data["ID"],
                Name= data["Name"].ToString(),
                Price=decimal.ToDouble((decimal) data["Price"]),
                Description=data["Description"].ToString()
            };
        }
    }
}