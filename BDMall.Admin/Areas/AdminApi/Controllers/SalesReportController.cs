using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.ReportModule)]
    [ApiController]
    public class SalesReportController : BaseApiController
    {
        ISalesReportBLL salesReportBLL;

        public SalesReportController(IComponentContext services) : base(services)
        {
            salesReportBLL = Services.Resolve<ISalesReportBLL>();
        }

        /// <summary>
        /// 本日，本月，本周热销产品列表
        /// </summary>
        /// <param name="topMonthQty"></param>
        /// <param name="topWeekQty"></param>
        /// <param name="topDayQty"></param>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, HotSalesSummaryView> GetHotSalesSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var data = salesReportBLL.GetHotSalesProductList(topMonthQty, topWeekQty,topDayQty, SortType.DESC);
            return data;
        }

        /// <summary>
        /// 獲取月份範圍內的銷售單匯總信息
        /// </summary>
        /// <param name="scope">月份跨度</param>
        [HttpGet]
        public async Task<SalesOrderSummaryView> GetMonthlySalesOrderSummary(int scope)
        {
            SalesOrderSummaryView salesOrderSumVw = new SalesOrderSummaryView();
            //salesOrderSumVw.SalesOrderDetailList = new List<SalesOrderDetailView>();
            //try
            //{
            //    salesOrderSumVw.TitleList = new List<string>();
            //    salesOrderSumVw.TitleList.Add(BDMall.Resources.Label.SalesOrderQty);

            //    var salesOrderSumLst = SalesReportBLL.GetSalesOrderSummaryLst(scope, TimePrecisionType.Month);
            //    if (salesOrderSumLst != null && salesOrderSumLst.Any())
            //        salesOrderSumVw.SalesOrderDetailList = salesOrderSumLst;
            //}
            //catch (Exception ex)
            //{
            //    salesOrderSumVw = null;
            //    throw CreateCustomException(ex);
            //}
            return salesOrderSumVw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cond"></param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<string, List<OrderShowCaseSummary>> GetOrderShowList([FromForm]OrderShowCond Cond)
        {
            var data = salesReportBLL.GetOrderShowList(Cond);
            return data;
        }

        /// <summary>
        /// 獲取待審批的產品列表
        /// </summary>
        /// <param name="getQty">需要獲取的數量</param>
        [HttpGet]
        public List<ProductSummary> GetWaitingApproveProdLst(int getQty)
        {
            var data = salesReportBLL.GetWaitingApproveProdLst(getQty);
            return data;
        }
    }
}
