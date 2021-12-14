using Autofac;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
    [ApiController]
    public class ProductController : BaseApiController
    {
        public ProductController(IComponentContext services) : base(services)
        {
        }

        [HttpGet]
        public Dictionary<string, object> GetClickRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            //var data = _productBLL.GetClickRateView(topMonthQty, topWeekQty, topDayQty);
            //Dictionary<string, ClickRateSummaryView>

            return new Dictionary<string, object>();
        }

        [HttpGet]
        public Dictionary<string, object> GetSearchRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            //var data = _productBLL.GetSearchRateView(topMonthQty, topWeekQty, topDayQty);
            return new Dictionary<string, object>();
        }
    }
}
