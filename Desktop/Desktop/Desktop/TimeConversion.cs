using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    public class TimeConversion
    {
        public static DateTime UnixTimeToDateTime(double unixTimeStamp)
        {
            var timeSpan = TimeSpan.FromSeconds(unixTimeStamp);
            return new DateTime(timeSpan.Ticks).ToLocalTime();
        }

        public static long DateTimeToUnixTime(DateTime datetime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) (datetime.ToLocalTime() - epoch).TotalSeconds;
        }
    }
}
