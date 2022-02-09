using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        public ICodeMasterBLL CodeMasterBLL;

        public OrderController(IComponentContext services) : base(services)
        {
            OrderBLL = Services.Resolve<IOrderBLL>();
            DealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            CodeMasterBLL = Services.Resolve<ICodeMasterBLL>();
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
            var data = OrderBLL.GetOrder(orderId);
            return data;
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderStatusInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SystemResult> UpdateOrderStatus([FromForm] UpdateStatusCondition orderStatusInfo)
        {
            var result = await OrderBLL.UpdateOrderStatus(orderStatusInfo);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OrderStatusGroup GetOrderStatus()
        {
            OrderStatusGroup data = new OrderStatusGroup();

            var codeMasters = CodeMasterBLL.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.OrderStatus);
            data.RecevedOrder = codeMasters.Where(p => p.Value == ((int)OrderStatus.ReceivedOrder).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            data.Processing = codeMasters.Where(p => p.Value == ((int)OrderStatus.Processing).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            data.PaymentConfirmed = codeMasters.Where(p => p.Value == ((int)OrderStatus.PaymentConfirmed).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            data.DeliveryArranged = codeMasters.Where(p => p.Value == ((int)OrderStatus.DeliveryArranged).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            data.OrderCompleted = codeMasters.Where(p => p.Value == ((int)OrderStatus.OrderCompleted).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            data.OrderCancelled = codeMasters.Where(p => p.Value == ((int)OrderStatus.SCancelled).ToString()).Select(d => new KeyValue { Id = d.Id.ToString(), Text = d.Description }).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// 更新送货单
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult UpdateDeliveryInfo([FromForm] OrderDeliveryInfo delivery)
        {
            SystemResult result = new SystemResult();

            OrderBLL.UpdateDeliveryInfo(delivery);
            result.Succeeded = true;
            result.Message = Resources.Message.SaveSuccess;
            return result;
        }

        /// <summary>
        /// 销售退回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SystemResult> DeliverySalesReturn(Guid id)
        {
            var result = await OrderBLL.DeliverySalesReturn(id);
            return result;
        }

        /// <summary>
        /// 快递重寄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult DeliverySendBack(Guid id)
        {
            SystemResult result = new SystemResult();
            OrderBLL.DeliverySendBack(id);
            result.Succeeded = true;
            return result;
        }
    }
}
