using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Helper
{
    public class TimeConverter
    {
        public static long ToUnixTimeStamp(DateTime time)
        {
            long timeStamp = time.Ticks - new DateTime(1970, 1, 1).Ticks;
            timeStamp = timeStamp / TimeSpan.TicksPerSecond;
            return timeStamp;
        }
        public static DateTime FromUnixTimeStamp(long time)
        {
            DateTime startDate = new DateTime(1970, 1, 1);
            long ticks = time * TimeSpan.TicksPerSecond;

            return new DateTime(startDate.Ticks + ticks);
        }


        public static DateTime Controle(nmct.ba.cashlessproject.model.Kassa.Log obj)
        {
            DateTime time;
            long controle = (long)obj.Timestamp;
            if (controle == 0)
            {
                time = DateTime.Now;
            }
            else
            {
                time = TimeConverter.FromUnixTimeStamp(controle);
            }
            return time;
        }
        public static DateTime Controle(_Sale obj)
        {
            DateTime time;
            long controle = (long)obj.Timestamp;
            if (controle == 0)
            {
                time = DateTime.Now;
            }
            else
            {
                time = TimeConverter.FromUnixTimeStamp(controle);
            }
            return time;
        }
    }
}