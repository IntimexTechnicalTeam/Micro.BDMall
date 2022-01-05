using Microsoft.AspNetCore.Http;
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
        //public static string GetPMServer()
        //{
        //    var server = System.Configuration.ConfigurationManager.AppSettings["PMServer"];
        //    if (string.IsNullOrEmpty(server))
        //    {
        //        return GetSiteRoot();
        //    }
        //    else
        //    {

        //        return server;
        //    }
        //}
        ///// <summary>
        /////  獲取網站訪問根目錄 http://localhost:555
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSiteRoot()
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        var request = HttpContext.Current.Request;
        //        string path = request.Url.Scheme + "://" + request.Url.Host;
        //        if (request.Url.Port == 80)
        //        {
        //            return path;
        //        }
        //        else
        //        {
        //            return path + ":" + request.Url.Port;
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

    }
}
