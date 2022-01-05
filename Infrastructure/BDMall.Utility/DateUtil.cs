using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class DateUtil
    {
        public static DateTime ConvertoDateTime(string date, string format)
        {
            //DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            //dtFormat.LongTimePattern = format; 
            //return DateTime.Parse(date, dtFormat);

            return DateTime.ParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
