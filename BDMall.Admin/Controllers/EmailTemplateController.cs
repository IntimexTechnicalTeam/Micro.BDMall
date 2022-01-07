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
    public class EmailTemplateController : BaseMvcController
    {
        public EmailTemplateController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        // GET: Admin/EmailTemplate
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增和修改頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TemplateEdit(string id)
        {
            ViewBag.TId = id;
            return View();
        }


        public ActionResult TempItem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditTempItem(Guid? id)
        {
            ViewBag.TId = id ?? Guid.Empty;
            return View();

        }

        public ActionResult EmailTypeItems()
        {
            return View();
        }


        public ActionResult EditEmailTypeItems()
        {
            return View();
        }

    }
}