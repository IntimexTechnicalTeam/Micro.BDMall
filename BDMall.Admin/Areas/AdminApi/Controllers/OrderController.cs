using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.OrderModule)]
    public class OrderController : BaseApiController
    {
        public IOrderBLL OrderBLL;
        public IDealProductQtyCacheBLL DealProductQtyCacheBLL;

        public OrderController(IComponentContext services) : base(services)
        {
            OrderBLL=Services.Resolve<IOrderBLL>();
            DealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
        }

        /// <summary>
        /// 獲取訂單列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        public object Search(OrderCondition condition)
        {
            PageData<OrderSummaryView> data = OrderBLL.GetSimpleOrders(condition);

            return new { total = data.TotalRecord, rows = data.Data };
        }

        /// <summary>
        /// 獲取訂單信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public OrderInfoView GetById(Guid orderId)
        {
            var data  = OrderBLL.GetOrder(orderId);           
            return data;
        }

        [HttpPost]
        public async Task<SystemResult> UpdateOrderStatus(UpdateStatusCondition orderStatusInfo)
        {
            var result = await OrderBLL.UpdateOrderStatus(orderStatusInfo);
            return result;  
        }
    }
}
