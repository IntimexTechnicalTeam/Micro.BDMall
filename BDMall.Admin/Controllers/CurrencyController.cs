using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Web.Mvc;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;
using BDMall.Enums;

namespace BDMall.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
   // [ActionAuthorize(Module = ModuleConst.ProductModule)]
    public class CurrencyController : BaseMvcController
    {
        public CurrencyController(IComponentContext service) : base(service)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CurrencySetting()
        {
            return View();
        }

        public ActionResult CurrencyEdit(string id, string para2)
        {
            ViewBag.Code = para2;
            ViewBag.EditType = id;
            return View();
        }

    }
}