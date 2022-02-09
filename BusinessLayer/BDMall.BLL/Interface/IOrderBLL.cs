using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IOrderBLL:IDependency
    {
        PageData<OrderSummaryView> GetSimpleOrders(OrderCondition cond);

        OrderInfoView GetOrder(Guid orderId);

        Task<SystemResult> BuildOrder(NewOrder checkout);

        Task<SystemResult> UpdateOrderStatus(UpdateStatusCondition cond);

        void UpdateDeliveryInfo(OrderDeliveryInfo delivery);

        void UpdateProductSaleSummary(OrderDto order);

        void AddInventoryReserved(OrderDto order, out List<Guid> skuIdList);

        void UpdateInventoryQty(OrderDto order, UpdateStatusCondition cond);

        /// <summary>
        /// 销售退回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SystemResult> DeliverySalesReturn(Guid id);

        void DeliverySendBack(Guid id);

        /// <summary>
        /// 支付失败，取消订单
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateOrderCancelStatus(Guid orderId);

        /// <summary>
        /// 支付成功，更新订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<bool> UpdateOrderPayStatus(Guid orderId);

        /// <summary>
        /// 支付超时，取消订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cond"></param>
        void UpdateOrderStatusToECancel(OrderDto order, UpdateStatusCondition cond);
    }
}
