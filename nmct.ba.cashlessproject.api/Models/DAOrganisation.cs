using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class DAOrganisation
    {
        private const string CONNECTIONSTRING = "DB";
        public static Organisation Login(string username, string password)
        {

            string sql = "SELECT * FROM tblOrganisations WHERE Username=@Username AND Password=@Password";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "Username", username);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Password", Cryptography.Encrypt(password));

            DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2);

            reader.Read();
            if (reader.HasRows)
            {
                return new Organisation()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbUsername = reader["DbUsername"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }

            return null;

        }

    }
}