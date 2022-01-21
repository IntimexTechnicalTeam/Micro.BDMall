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
    }
}
