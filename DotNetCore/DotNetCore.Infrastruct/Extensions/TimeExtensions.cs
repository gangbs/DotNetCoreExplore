using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.Extensions
{
   public static class TimeExtensions
    {
        public static DateTime ToDatetime(this long unixTime)
        {
            DateTime start = new DateTime(1970, 1, 1);//TimeZone.CurrentTimeZone.ToLocalTime();
            DateTime dt = start.AddMilliseconds(unixTime);
            long a = dt.ToLong();
            return dt;
        }

        public static long ToLong(this DateTime dt)
        {

            long unixTime = (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return unixTime;
        }
    }
}
