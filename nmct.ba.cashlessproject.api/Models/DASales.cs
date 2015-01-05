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
    public class DASales
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "DbUsername").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "DbPassword").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "DbName").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", "STIJN", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static List<_Sale> Load(IEnumerable<Claim> claims)
        {
            List<_Sale> lijst = new List<_Sale>();
            string sql = "SELECT a.ID,a.Amount,a.TotalPrice,a.CustomerID,a.RegisterID,a.TimeStamp,a.ProductID,b.Name as CustomerName,b.Address as CustomerAddress,b.Picture as CustomerPicture,b.Balance as CustomerBalance,c.Name as RegisterName,c.Device as RegisterDevice,d.Name as ProductName, d.Price as ProductPrice,d.Description as ProductDescription FROM tblSales as a INNER JOIN tblCustomers as b on a.CustomerID=b.ID INNER JOIN tblRegisters as c on a.RegisterID=c.ID INNER JOIN tblProducts as d on a.ProductID=d.ID";
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


        public static _Sale Load(int id, IEnumerable<Claim> claims)
        {

            string sql = "SELECT * FROM tbl_Sales WHERE ID=@id";
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


        public static int Update(_Sale sale, IEnumerable<Claim> claims)
        {
            int id;
            


            string sql = "UPDATE tblSales SET CustomerID=@CustomerID,RegisterID=@RegisterID,ProductID=@ProductID,Amount=@Amount,TotalPrice=@TotalPrice WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "ID", sale.ID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "CustomerID", sale.CustomerID);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "RegisterID", sale.RegisterID);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "ProductID", sale.ProductID);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "Amount", sale.Amount);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "TotalPrice", sale.TotalPrice);
            id = Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4,par5,par6);

            return id;
        }

        public static int Insert(_Sale sale, IEnumerable<Claim> claims)
        {
            int id,id2;
            //
            double balance=0;
            //
            DateTime time = TimeConverter.Controle(sale);

            DbTransaction trans = Database.BeginTransaction(CreateConnectionString(claims));
       
            string sql = "INSERT INTO tblSales(CustomerID,RegisterID,ProductID,Amount,TotalPrice,Timestamp) VALUES(@CustomerID,@RegisterID,@ProductID,@Amount,@TotalPrice,@Timestamp)";
            DbParameter par1 = Database.AddParameter(CreateConnectionString(claims), "CustomerID", sale.CustomerID);
            DbParameter par2 = Database.AddParameter(CreateConnectionString(claims), "RegisterID", sale.RegisterID);
            DbParameter par3 = Database.AddParameter(CreateConnectionString(claims), "ProductID", sale.ProductID);
            DbParameter par4 = Database.AddParameter(CreateConnectionString(claims), "Amount", sale.Amount);
            DbParameter par5 = Database.AddParameter(CreateConnectionString(claims), "TotalPrice", sale.TotalPrice);
            DbParameter par6 = Database.AddParameter(CreateConnectionString(claims), "Timestamp", time);
            id = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4,par5,par6);



            string sql_ = "SELECT * FROM tblCustomers WHERE ID=@ID";
            DbParameter par_1 = Database.AddParameter(CreateConnectionString(claims), "ID", sale.CustomerID);
            DbDataReader data = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql_, par_1);

            data.Read();

            if (data.HasRows)
            {
                balance = decimal.ToDouble((decimal)data["Balance"]);
            }
            //berekening
            balance = balance - sale.TotalPrice;
            //


            string sql__ = "UPDATE tblCustomers Set Balance=@Balance WHERE ID=@ID";
            DbParameter par__1 = Database.AddParameter(CreateConnectionString(claims), "ID", sale.CustomerID);
            DbParameter par__2 = Database.AddParameter(CreateConnectionString(claims), "Balance", balance);
            id2 = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql__, par__1, par__2);



            return id;
        }



        private static _Sale Create(IDataRecord data)
        {

            return new _Sale()
            {
                ID = (int)data["ID"],
                Amount = decimal.ToDouble((decimal)data["Amount"]),
                TotalPrice = decimal.ToDouble((decimal) data["TotalPrice"]),
                CustomerID = (int)data["CustomerID"],
                RegisterID = (int)data["RegisterID"],
                Timestamp=(long) TimeConverter.ToUnixTimeStamp((DateTime)data["TimeStamp"]),
                ProductID = (int)data["ProductID"],
                CustomerName = data["CustomerName"].ToString(),
                CustomerAddress = data["CustomerAddress"].ToString(),
                CustomerPicture = (data["CustomerPicture"] is DBNull) ? null : (byte[])data["CustomerPicture"],
                CustomerBalance =decimal.ToDouble((decimal) data["CustomerBalance"]),
                RegisterName = data["RegisterName"].ToString(),
                RegisterDevice = data["RegisterDevice"].ToString(),
                ProductName = data["ProductName"].ToString(),
                ProductPrice =decimal.ToDouble((decimal)  data["ProductPrice"]),
                ProductDescription = data["ProductDescription"].ToString()

            };
        }

    }
}