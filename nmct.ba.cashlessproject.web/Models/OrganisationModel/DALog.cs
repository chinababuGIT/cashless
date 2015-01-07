using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models.OrganisationModel
{
    public class DALog{
        private const string CONNECTIONSTRING = "DB";
        public static List<Log> Load()
        {
            List<Log> lijst = new List<Log>();
            string sql = "SELECT * FROM tblLogs";
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql);

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

        public static Log Load(int id)
        {
            Log log = new Log();
            string sql = "SELECT * FROM tblLogs WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "ID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            data.Read();
            log = Create(data);
            return log;
        }


        public static int Update(Log log, IEnumerable<Claim> claims)
        {
            int id;
            DateTime time = TimeConverter.ToDateTime((long)log.Timestamp);

            string sql = "UPDATE tblLogs SET RegisterID=@RegisterID,Timestamp=@Timestamp,Message=@Message,Stacktrace=@Stacktrace WHERE RegisterID=@RegisterID AND Timestamp=@Timestamp";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", log.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Timestamp", time);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Message", (log.Message==null)?string.Empty:log.Message);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "StackTrace", (log.StackTrace==null)?string.Empty:log.StackTrace);
            id = Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2, par3, par4);

            return id;
        }

        public static int Insert(Log log, IEnumerable<Claim> claims,DbTransaction trans=null)
        {
            int id;
            DateTime time = TimeConverter.ToDateTime((long)log.Timestamp);
            if (log.Timestamp == 0) time = DateTime.Now;
            #region exists
            if (log.RegisterID!=0 && log.RegisterID!=null)
            {
                string sql = "SELECT * FROM tblLogs WHERE RegisterID=@RegisterID AND Timestamp=@Timestamp";
                DbParameter parID = Database.AddParameter(CONNECTIONSTRING, "RegisterID", log.RegisterID);
                DbParameter parTimestamp = Database.AddParameter(CONNECTIONSTRING, "Timestamp", time);
                DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, parID, parTimestamp);

                data.Read();

                if (data.HasRows)
                {
                    log.RegisterID = (int)data["RegisterID"];
                    data.Close();
                    return Update(log, claims);
                }
            }
           
            #endregion


            string sql2 = "INSERT INTO tblLogs VALUES(@RegisterID,@Timestamp,@Message,@StackTrace)";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", log.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "Timestamp",time);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "Message", (log.Message==null)? string.Empty:log.Message);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "StackTrace", (log.StackTrace==null)? string.Empty:log.StackTrace);
            id = Database.InsertData(trans, sql2, par1, par2, par3, par4);

            return id;
        }
        public static List<int> Insert(List<Log> logs, IEnumerable<Claim> claims)
        {
             DbTransaction trans =null;
            try
            {
                trans = Database.BeginTransaction(CONNECTIONSTRING);
                List<int> lijst = new List<int>();
                for (int i = 0; i < logs.Count; i++)
                {
                    lijst.Add(Insert(logs[i], claims, trans));
                }
                trans.Commit();
                return lijst;
               
            }
            catch (Exception ex)
            {
                trans.Rollback();

                return null;
            }
           
        }

        private static Log Create(DbDataReader data)
        {
            try
            {
                return new Log()
                {
                    RegisterID = (int)data["RegisterID"],
                    Message = data["Message"].ToString().Trim(),
                    StackTrace = data["StackTrace"].ToString().Trim(),
                    Timestamp = TimeConverter.ToUnixTimeStamp((DateTime)data["Timestamp"])
                };
            }
            catch (Exception)
            {

                return null;
            }
           
        }



    }
}