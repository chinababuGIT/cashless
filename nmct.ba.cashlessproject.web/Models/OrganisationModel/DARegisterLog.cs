using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models.OrganisationModel
{
    public class DARegisterLog
    {
       
        private const string CONNECTIONSTRING = "DB";
        public static List<RegisterLog> Load()
        {
            List<RegisterLog> lijst = new List<RegisterLog>();
            string sql = " SELECT a.ID as RegisterID,a.Name as RegisterName,a.Device as RegisterDevice,a.PurchaseDate as RegisterPurchaseDate,a.ExpiresDate as RegisterExpiresDate,[Timestamp] as LogTimestamp,[Message] as LogMessage,Stacktrace as LogStackTrace FROM tblRegisters as a INNER JOIN tblLogs as b ON a.ID=b.RegisterID";
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

        public static RegisterLog Load(int id)
        {
            RegisterLog log = new RegisterLog();
            string sql = " SELECT a.ID as RegisterID,a.Name as RegisterName,a.Device as RegisterDevice,a.PurchaseDate as RegisterPurchaseDate,a.ExpiresDate as RegisterExpiresDate,[Timestamp] as LogTimestamp,[Message] as LogMessage,Stacktrace as LogStackTrace FROM tblRegisters as a INNER JOIN tblLogs as b ON a.ID=b.RegisterID WHERE RegisterID=@RegisterID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "RegisterID", id);
            DbDataReader data = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);

            data.Read();
            log = Create(data);
            return log;
        }
 
      
        private static RegisterLog Create(DbDataReader data)
        {
            try
            {
                return new RegisterLog()
                {
                    RegisterID = (int)data["RegisterID"],
                    RegisterName = data["RegisterName"].ToString().Trim(),
                    RegisterDevice = data["RegisterDevice"].ToString().Trim(),
                    RegisterPurchaseDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterPurchaseDate"]),
                    RegisterExpiresDate = TimeConverter.ToUnixTimeStamp((DateTime)data["RegisterExpiresDate"]),
                    LogMessage = data["LogMessage"].ToString().Trim(),
                    LogStackTrace = data["LogStackTrace"].ToString().Trim(),
                    LogTimestamp = TimeConverter.ToUnixTimeStamp((DateTime)data["LogTimestamp"])
                };
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}