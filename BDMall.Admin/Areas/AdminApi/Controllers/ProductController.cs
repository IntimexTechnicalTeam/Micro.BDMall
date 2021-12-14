using Autofac;
using BDMall.BLL;
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
        IProductBLL productBLL;

        public ProductController(IComponentContext services) : base(services)
        {
            productBLL = Services.Resolve<IProductBLL>();
        }

        [HttpGet]
        public Dictionary<string, ClickRateSummaryView> GetClickRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var data = productBLL.GetClickRateView(topMonthQty, topWeekQty, topDayQty);
            return data;
        }

        [HttpGet]
        public Dictionary<string, ClickRateSummaryView> GetSearchRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var data = productBLL.GetSearchRateView(topMonthQty, topWeekQty, topDayQty);
            return data;
        }
    }
}
