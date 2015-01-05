using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.helper
{
    public class TimeConverter
    {
        public static long ToUnixTimeStamp(DateTime time)
        {
            try
            {
                if (time == DateTime.MinValue) return 0;

                long timeStamp = time.Ticks - new DateTime(1970, 1, 1).Ticks;
                timeStamp = timeStamp / TimeSpan.TicksPerSecond;
                return timeStamp;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }


        }
        public static DateTime ToDateTime(long time)
        {
            try
            {
                if (time == 0) return DateTime.MinValue;

                DateTime startDate = new DateTime(1970, 1, 1);
                long ticks = time * TimeSpan.TicksPerSecond;

                return new DateTime(startDate.Ticks + ticks);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
                throw;
            }


        }

    }
}