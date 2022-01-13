using Autofac;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    /// <summary>
    /// 庫存清單Controller
    /// </summary>
   // [ActionAuthorize(Module = ModuleConst.IventoryModule)]
    public class InventoryController : BaseMvcController
    {
        public InventoryController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 倉庫設定
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseSetting()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 倉庫資料編輯
        /// </summary>
        /// <param name="id">記錄ID</param>
        /// <param name="para2">編輯類型</param>
        /// <returns></returns>
        public ActionResult WarehouseEdit(string id, string para2)
        {
            ViewBag.ID = id;
            ViewBag.EditType = para2;
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 供應商設定
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierSetting()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 供應商資料編輯
        /// </summary>
        /// <param name="id">記錄ID</param>
        /// <param name="para2">編輯類型</param>
        /// <returns></returns>
        public ActionResult SupplierEdit(string id, string para2)
        {
            ViewBag.ID = id;
            ViewBag.EditType = para2;
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 存貨清單搜尋
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 存貨清單詳細列表
        /// </summary>
        /// <param name="id">產品編號</param>
        /// <returns></returns>
        public ActionResult InventoryListDetail(string id)
        {
            ViewBag.ProductCode = id;
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 庫存流動報告
        /// </summary>
        /// <returns></returns>
        public ActionResult InventoryFlowRpt()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 庫存進貨\調貨管理
        /// </summary>
        /// <returns></returns>
        public ActionResult InventoryTransaction()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }

        /// <summary>
        /// 銷售退回
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesReturn()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            return View();
        }
    }
}