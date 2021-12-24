using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class PathUtil
    {
        public static string GetBoostrapTableLang(string lang)
        {
            //string lang = WSCookie.GetUserLanguage();
            string url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-TW.min.js";
            if (lang == "E")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-en-US.min.js";
            }
            else if (lang == "C")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-TW.min.js";

            }
            else if (lang == "S")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-CN.min.js";
            }
            return url;
        }
    }
}
