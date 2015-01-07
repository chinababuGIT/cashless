using nmct.ba.cashlessproject.web.Helper;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Security.Claims;

namespace nmct.ba.cashlessproject.web.Models.OrganisationModel
{
    public class DAOrganisation
    {
      
        public const string CONNECTIONSTRING = "DB";
        public static Organisation Login(string username, string password)
        {
            string sql = "SELECT * FROM tblOrganisations WHERE Username=@Username AND Password=@Password";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "Username", username);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Password", password);

            DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2);

            reader.Read();
            if (reader.HasRows)
            {
                try
                {
                    return Create(reader);
                }
                catch (Exception)
                {

                    return null;
                }
                
            }

            return null;

        }
        public static int ChangePassword(Organisation organisation, IEnumerable<Claim> claims)
        {
            string id_string = claims.FirstOrDefault(c => c.Type == "ID").Value;
            int id;
            Int32.TryParse(id_string, out id);
            string sql = "UPDATE tblOrganisations SET Password=@Password WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Password", organisation.Password);
           

            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2);

            return id;
        }
        public static Organisation GetUser(IEnumerable<Claim> claims)
        {
            string id_string = claims.FirstOrDefault(c => c.Type == "ID").Value;

            string sql = "SELECT * FROM tblOrganisations WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID",id_string);

            DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            reader.Read();
            if (reader.HasRows)
            {
                try
                {
                   Organisation org= Create(reader);
                   org.DbName = "";
                   org.DbPassword = "";
                   org.DbUsername = "";                 
                   org.Password = "";
                   org.RepeatPassword = "";
                   org.Registers = null;
                   return org;
                }
                catch (Exception)
                {

                    return null;
                }
                
            }

            return null;
        }

        public static List<Organisation> Load()
        {
            List<Organisation> lijst = new List<Organisation>();
            string sql = "SELECT * FROM tblOrganisations";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

            while (data.HasRows)
            {
                while (data.Read())
                {
                    
                    lijst.Add(Create(data));

                }
                data.NextResult();
            }

            #region add 0
            List<Organisation> emptyList= new List<Organisation>();
            emptyList.Add(new Organisation(){

                ID=0,
                Name="Geen verenging"
            });
            lijst.InsertRange(0, emptyList);
            #endregion

            return lijst;
        }

        public static Organisation Load(int id)
        {
            Organisation organisation = new Organisation();
            string sql = "SELECT * FROM tblOrganisations WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql,par1);

            data.Read();
            organisation = Create(data);
            return organisation;
        }
        public static bool HasUser(string username)
        {
            Organisation organisation = new Organisation();
            string sql = "SELECT * FROM tblOrganisations WHERE Username=@Username";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "Username", username);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            if (data.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static int Update(Organisation organisation)
        {
            int id;
            string sql = "UPDATE tblOrganisations SET Username=@Username,Password=@Password,DbName=@DbName,DbUsername=@DbUsername,DbPassword=@DbPassword,Name=@Name,Logo=@Logo,Address=@Address,Email=@Email,Phone=@Phone WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID",organisation.ID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Username", (organisation.Username == null) ? string.Empty : organisation.Username);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Password", (organisation.Password == null) ? string.Empty : organisation.Password);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "DbName", (organisation.DbName == null) ? string.Empty : organisation.DbName);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "DbUsername", (organisation.DbUsername == null) ? string.Empty : organisation.DbUsername);
            DbParameter par6 = Database.AddParameter(CONNECTIONSTRING, "DbPassword", (organisation.DbPassword == null) ? string.Empty : organisation.DbPassword);
            DbParameter par7 = Database.AddParameter(CONNECTIONSTRING, "Name", (organisation.Name == null) ? string.Empty : organisation.Name);
            DbParameter par8 = Database.AddParameter(CONNECTIONSTRING, "Address", (organisation.Address == null) ? string.Empty : organisation.Address);
            DbParameter par9 = Database.AddParameter(CONNECTIONSTRING, "Email", (organisation.Email == null) ? string.Empty : organisation.Email);
            DbParameter par10 = Database.AddParameter(CONNECTIONSTRING, "Phone", (organisation.Phone == null) ? string.Empty : organisation.Phone);
            DbParameter par11 = Database.AddParameter(CONNECTIONSTRING, "Logo", (organisation.Logo == null) ? new byte[0] : organisation.Logo);

            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2, par3, par4,par5,par6,par7,par8,par9,par10,par11);

            return id;
        }

        public static int Insert(Organisation organisation)
        {
            
            int id = 0 ;
            if (HasUser(organisation.Username)==true)
            {
                return 0;
            }
           
            string sql = "INSERT INTO tblOrganisations VALUES(@Username,@Password,@DbName,@DbUsername,@DbPassword,@Name,@Logo,@Address,@Email,@Phone)";
           
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Username", (organisation.Username==null)?string.Empty:organisation.Username);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Password", (organisation.Password==null)?string.Empty:organisation.Password);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "DbName", (organisation.DbName==null)?string.Empty:organisation.DbName);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "DbUsername", (organisation.DbUsername==null)?string.Empty:organisation.DbUsername);
            DbParameter par6 = Database.AddParameter(CONNECTIONSTRING, "DbPassword", (organisation.DbPassword==null)?string.Empty:organisation.DbPassword);
            DbParameter par7 = Database.AddParameter(CONNECTIONSTRING, "Name", (organisation.Name==null)?string.Empty:organisation.Name);
            DbParameter par8 = Database.AddParameter(CONNECTIONSTRING, "Address", (organisation.Address==null)?string.Empty:organisation.Address);
            DbParameter par9 = Database.AddParameter(CONNECTIONSTRING, "Email", (organisation.Email==null)?string.Empty:organisation.Email);
            DbParameter par10 = Database.AddParameter(CONNECTIONSTRING, "Phone", (organisation.Phone==null)?string.Empty:organisation.Phone);
            DbParameter par11 = Database.AddParameter(CONNECTIONSTRING, "Logo", (organisation.Logo ==null)? new byte[0]:organisation.Logo);

            //try en anders alles terug zetten als ervoor
            
           bool ok=CreateDatabase(organisation);
           if (ok==true)
           {
               id = Database.InsertData(Database.GetConnection(CONNECTIONSTRING), sql, par2, par3, par4, par5, par6, par7, par8, par9, par10, par11);
           }
           return id;
            
        }




        private static bool CreateDatabase(Organisation o)
        {
            // create the actual database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); //only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", o.DbName).Replace("@@DbUsername", o.DbUsername).Replace("@@DbPassword", Cryptography.Decrypt(o.DbPassword));
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction(CONNECTIONSTRING);

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", o.DbName).Replace("@@DbUsername", o.DbUsername).Replace("@@DbPassword", Cryptography.Decrypt(o.DbPassword));

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }


        private static Organisation Create(DbDataReader data)
        {
            try
            {
                return new Organisation()
                {
                    ID = (int)data["ID"],
                    Username = data["Username"].ToString().Trim(),
                    Password = data["Password"].ToString().Trim(),
                    DbName = data["DbName"].ToString().Trim(),
                    DbUsername = data["DbUsername"].ToString().Trim(),
                    DbPassword = data["DbPassword"].ToString().Trim(),
                    Name = data["Name"].ToString().Trim(),
                    Logo = (data["Logo"] is DBNull) ? null : (byte[])data["Logo"],
                    Address = data["Address"].ToString().Trim(),
                    Email = data["Email"].ToString().Trim(),
                    Phone = data["Phone"].ToString().Trim(),
                    Registers = DARegister.LoadByOrganisationID((int)data["ID"])
                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }



        
    }
}