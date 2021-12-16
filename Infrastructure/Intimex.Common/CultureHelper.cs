using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Intimex.Common
{
    public class CultureHelper
    {
        private static readonly List<string> validCultures = new List<string>() { "C", "E", "S", "J" };
        private static readonly List<string> cultures = new List<string>() { "zh-HK", "en-US", "zh-CN", "ja-JP" };

        public static bool IsRightToLeft()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        }

        public static string GetSupportCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return GetDefaultCulture();
            }
            //如果不是系统开通的有效的语言，返回默认语言
            if (!validCultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return GetDefaultCulture();
            }
            //如果是标准格式，直接返回返回
            if (cultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return name;
            }

            var n = GetStandardCulture(name);
            return n;
        }

        public static string GetDefaultCulture()
        {
            return cultures[0];
        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurrentSupportCulture()
        {
            return GetStandardCulture(Thread.CurrentThread.CurrentCulture.Name);
        }

        public static string GetStandardCulture(string name)
        {
            string n = name.ToUpper();
            if (!n.Contains("-"))
            {
                if (n == "C")
                {
                    return "zh-HK";
                }
                else if (n == "S")
                {
                    return "zh-CN";
                }
                else if (n == "J")
                {
                    return "ja-JP";
                }
                else
                {
                    return "en-US";
                }

            }
            else
            {
                return name;
            }
        }


    }
}
