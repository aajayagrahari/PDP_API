using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public static class ConvertTimeZone
    {
        public static DateTime ConvertFromUtcTimeZone(string zoneID)
        {
            try
            {
                DateTime utcTime = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(zoneID);
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cstZone);
                return cstTime;
            }
            catch (TimeZoneNotFoundException)
            {
                //Console.WriteLine("The registry does not define the Central Standard Time zone.");
                return DateTime.Now;
            }
            catch (InvalidTimeZoneException)
            {
                //Console.WriteLine("Registry data on the Central Standard Time zone has been corrupted.");
                return DateTime.Now;
            }
        }
    }
}
