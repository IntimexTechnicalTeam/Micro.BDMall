
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
  //  [ActionAuthorize(Module = ModuleConst.OrderModule)]
    public class OrderController : BaseMvcController
    {
        public OrderController(IComponentContext service) : base(service)
        {
        }

        // GET: Order
        public ActionResult Index()
        {
            ViewBag.Status = -1;
            return View();
        }
        //public ActionResult OldOrders()
        //{
        //    ViewBag.Status = -1;
        //    return View();
        //}

        //public ActionResult RecevedOrderIndex()
        //{
        //    ViewBag.Status = (int)OrderStatus.ReceivedOrder;
        //    return View("Index");
        //}

        //public ActionResult PaymentConfirmedIndex()
        //{
        //    ViewBag.Status = (int)OrderStatus.PaymentConfirmed;
        //    return View("Index");
        //}

        //public ActionResult DeliveryArrangedIndex()
        //{
        //    ViewBag.Status = (int)OrderStatus.DeliveryArranged;
        //    return View("Index");
        //}

        //public ActionResult Edit(Guid id)
        //{
        //    ViewBag.OrderId = id;
        //    return View();
        //}
        //public ActionResult OldEdit(string id)
        //{
        //    ViewBag.OrderId = id;
        //    return View();
        //}
        

        //public ActionResult ConfirmOrderDetailList()
        //{
        //    return View();
        //}

        //public ActionResult DeliveryECShipLabel()
        //{
        //    return View();
        //}

        //public ActionResult ECShipLabelDetail(string id)
        //{
        //    ViewBag.Id = id;
        //    return View();
        //}

        ///// <summary>
        ///// 顯示自動發貨的訂單
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult MassProcessFlow()
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 獲得送貨單詳細的html格式內容
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="clientId"></param>
        ///// <param name="appId"></param>
        ///// <param name="lang"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public string GetOrderDeliveryHtmlContent(Guid orderId, Guid clientId, string appId, string lang)
        //{
        //    string htmlContent = string.Empty;
        //    try
        //    {
        //        var myOrderBLL = OrderBLL as OrderBLL;
        //        if (myOrderBLL.CurrentUser == null)
        //        {
        //            myOrderBLL.CurrentUser = new Model.UserMNG.User();
        //            myOrderBLL.CurrentUser.ClientId = clientId;
        //            myOrderBLL.CurrentWebStore = BDMall.BLL.Core.CredentialManager.GetWebStoreInfoByAppId(appId);

        //            myOrderBLL.CurrentUser.Language = GetLanguage(lang);


        //            string cultureName = CultureHelper.GetSupportCulture(lang);
        //            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        //            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        //        }

        //        var orderVw = OrderBLL.GetOrderThinData(orderId);
        //        if (orderVw == null) htmlContent = string.Empty;
        //        if (orderVw.Deliveries?.Count() < 1) htmlContent = string.Empty;


        //        // TempData["OrderDeliveryList"] = orderVw.Deliveries;
        //        TempData["OrderInfoView"] = orderVw;
        //        htmlContent = HtmlUtil.RenderPartialViewToString(this, "ConfirmOrderDetailList");
        //    }
        //    catch (Exception ex)
        //    {
        //        var loger = log4net.LogManager.GetLogger("BLL");
        //        loger.Error(ex);
        //    }
        //    return htmlContent;
        //}

        //private Language GetLanguage(string lang)
        //{
        //    if (!string.IsNullOrEmpty(lang))
        //    {
        //        lang = lang.Trim().ToUpper();
        //        if (lang == Language.C.ToString())
        //            return Language.C;
        //        else if (lang == Language.E.ToString())
        //            return Language.E;
        //        else if (lang == Language.J.ToString())
        //            return Language.J;
        //        else if (lang == Language.P.ToString())
        //            return Language.P;
        //        else if (lang == Language.S.ToString())
        //            return Language.S;
        //        else
        //            return Language.E;
        //    }
        //    else
        //    {
        //        return Language.E;
        //    }
        //}
    }
}