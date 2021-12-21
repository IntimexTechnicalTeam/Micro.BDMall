using Autofac;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    /// <summary>
    /// 商家管理 Controller
    /// </summary> 
    public class MerchantController : BaseMvcController
    {
        public MerchantController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        // GET: Merchant
        //  [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Search })]
        public ActionResult Index()
        {
            ViewBag.Status = -1;
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;
            return View();
        }

        /// <summary>
        /// 待審批商家信息主頁
        /// </summary>
        /// <returns></returns>
      //  [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Search })]
        public ActionResult WaitingApprove()
        {
            ViewBag.Status = ApproveType.WaitingApprove.ToInt();
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;
            return View("Index");
        }

        /// <summary>
        /// 商家增刪改頁面
        /// </summary>
        /// <param name="id">記錄ID</param>
        /// <param name="para2">頁面操作類型（新增/修改/審批）</param>
       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Edit })]
        public ActionResult MerchantEdit(string id, string para2)
        {
            ViewBag.MerchantID = id;
            ViewBag.EditType = para2;
            return View();
        }

        /// <summary>
        /// 商家賬戶設定
        /// </summary>
        /// <returns></returns>
       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Edit })]
        public ActionResult MerchantAccount()
        {
            return View();
        }


        /// <summary>
        /// 选择merchant的页面
        /// </summary>
        /// <returns></returns>
       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public ActionResult MerchantPromotionHeadPage()
        {
            return View();
        }
        /// <summary>
        /// 商家推廣頁面
        /// </summary>
        /// <param name="id">商家ID</param>
        /// <param name="para2">編輯模式</param>
        /// <param name="para3">瀏覽模式</param>
        /// <returns></returns>
       //[ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public ActionResult MerchantPromotion(string id, string para2)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.Empty.ToString();
            }
            if (string.IsNullOrEmpty(para2))
            {
                para2 = PageEditType.Modify.ToString();
            }

            ViewBag.MerchantID = id;
            ViewBag.EditType = para2;
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;

            return View();
        }

       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Delivery_Method })]
        public ActionResult ShipMethodMapping()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;
            return View();
        }

      //  [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public ActionResult MerchantFreeChargeSetting()
        {
            return View();
        }



       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Approve_Promt })]
        public ActionResult ApproveMerchantPromotion()
        {
            return View();
        }


        /// <summary>
        /// 產品審批詳細頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApproveMerchantHistory(string id)
        {
            ViewBag.Id = id;
            return View();
        }

     //   [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public ActionResult PromotionBannerClickRate()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;
            return View();
        }

      //  [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_CC_Setting })]
        public ActionResult MerchantCounterCollection()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                ViewBag.IsMerchant = 1;
            return View();
        }

       // [ActionAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_CC_Setting })]
        public ActionResult MerchantCCEdit(string id)
        {
            ViewBag.MerchantId = id;
            return View();
        }

        public ActionResult SelectMerchant()
        {
            return View();
        }


    }
}