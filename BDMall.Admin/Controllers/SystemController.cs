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
    public class SystemController : BaseMvcController
    {
        public SystemController(IComponentContext service) : base(service)
        {
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Store()
        {
            return View();
        }
        public ActionResult Logo()
        {
            Response.Headers.Add("Cache-Control", "no-cache"); //HTTP 1.1
            Response.Headers.Add("Pragma", "no-cache"); //HTTP 1.0
            Response.Headers.Add("Expires", "0"); //prevents caching at the proxy server
            return View();
        }


        public ActionResult IPostSetting()
        {
            return View();
        }
        public ActionResult IPostSettingEdit(int id)
        {
            ViewBag.cId = id;
            return View();
        }
        public ActionResult CollectionOfficeSetting()
        {
            return View();
        }
        public ActionResult CollectionOfficeSettingEdit(int id)
        {
            ViewBag.cId = id;
            return View();
        }

        public ActionResult MailTrackingTest()
        {
            return View();
        }

        public ActionResult ECShipTest()
        {
            return View();
        }

        public ActionResult ActiveShipMethodSetting()
        {
            return View();
        }

        public ActionResult MailServerSetting()
        {
            return View();
        }

        public ActionResult EditMailServer(string id)
        {
            ViewBag.Id = id;
            return View();
        }


        /// <summary>
        /// 系統功能定制化頁面
        /// </summary>
        //  [ActionAuthorize(Module = ModuleConst.TechnicalModule)]
        public ActionResult Customization()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ScheduleJob()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MemAuditTrail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportExportHistory()
        {
            return View();
        }

        /// <summary>
        /// 自定義头部菜單
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomHeaderMenu()
        {
            return View();
        }

        /// <summary>
        /// 自定義底部菜單
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomFooterMenu()
        {
            return View();
        }


        /// <summary>
        ///  修改自定义菜单页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para2">父節點Id</param>
        /// <param name="para3">Type</param>
        /// <returns></returns>
        public ActionResult EditCustomMenu(string id, string para2, string para3)
        {
            ViewBag.Id = id;
            ViewBag.ParentId = para2;
            ViewBag.Type = para3;
            return View();
        }
        /// <summary>
        /// 根據傳入的type獲取相應的數據以供Cusmenu選擇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para2">是否單選</param>
        /// <returns></returns>
        public ActionResult SelectCustomMenuData(string id, int para2)
        {
            ViewBag.Type = id;
            ViewBag.IsSingleSelect = para2;
            return View();
        }

        /// <summary>
        /// IP地址设定
        /// </summary>
        /// <returns></returns>
        public ActionResult IPAddressSetting()
        {
            return View();
        }

        /// <summary>
        /// IP地址设定修改
        /// </summary>
        /// <returns></returns>
        public ActionResult EditIPAddress(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 批量更新产品时段价格
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchUpdateProductPrice()
        {
            return View();
        }

    }
}