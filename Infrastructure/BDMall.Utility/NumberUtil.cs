using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class NumberUtil
    {
        public static double ConvertDbl(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                double cv;
                double.TryParse(text, out cv);
                return cv;
            }
            return 0;
        }

        public static decimal ConvertDec(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                decimal cv;
                decimal.TryParse(text, out cv);
                return cv;
            }
            return 0;
        }

        public static int ConvertToRounded(decimal num)
        {
            return (int)Math.Round(num);
        }
        public static int ConvertToCeling(decimal num)
        {
            return (int)Math.Ceiling(num);
        }
        public static int ConvertToFloor(decimal num)
        {
            return (int)Math.Floor(num);
        }

    }
}
