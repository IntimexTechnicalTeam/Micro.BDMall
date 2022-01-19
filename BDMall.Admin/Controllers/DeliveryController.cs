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
    public class DeliveryController : BaseMvcController
    {
        public DeliveryController(IComponentContext service) : base(service)
        {
        }
        ///<summary>
        /// 國家設定頁
        /// </summary>

        /// <returns></returns>
        public ActionResult Country()
        {
            return View();
        }
        ///<summary>
        /// 省份設定頁
        /// </summary>
        /// <param name="id">国家ID</param>
        /// <returns></returns>
        public ActionResult Province(int id)
        {
            ViewBag.CountryID = id;
            return View();
        }
        ///<summary>
        /// 快遞設定頁
        /// </summary>
        /// <param name="id">规则ID</param>
        /// <param name="para2">快递ID</param>
        /// <param name="para3">商家Id</param>
        /// <returns></returns>
        public ActionResult TransRuleEdit(string id, string para2, string para3)
        {
            ViewBag.RuleId = new Guid(id);
            ViewBag.exId = new Guid(para2);
            ViewBag.MerchId = new Guid(para3);
            return View();
        }
        ///<summary>
        /// 地區設定頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Zone()
        {
            return View();
        }
        ///<summary>
        /// 運費設定頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Charge()
        {
            return View();
        }

        ///<summary>
        /// 快递
        /// </summary>
        /// <returns></returns>
        public ActionResult Expressage()
        {
            return View();
        }
        ///<summary>
        /// 快递
        /// </summary>
        /// <returns></returns>
        public ActionResult Discount()
        {
            return View();
        }
        ///<summary>
        /// 地区設定頁
        /// </summary>
        /// <param name="id">地区ID</param>
        /// <returns></returns>
        public ActionResult ZoneEdit(string id)
        {
            ViewBag.Id = new Guid(id);
            return View();
        }

    }
}