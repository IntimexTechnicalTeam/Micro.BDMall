using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using Web.Framework;

namespace BDMall.Admin.Controllers
{
    public class ContentController : Controller
    {       
        public ContentResult LanguageScript()
        {
            System.Type stringType = typeof(string);
            System.Collections.Hashtable array = new System.Collections.Hashtable();
            array.Add("First", "首頁");
            array.Add("Close","關閉");

            string script = JsonUtil.ToJson(array);
            script = "var Resources =" + script + ";";

            ContentResult Content = new ContentResult();
            Content.Content = script;
            Content.ContentType = "application/x-javascript; charset=utf-8";
            return Content;
        }

    }
}
