using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    /// <summary>
    ///  当已确认付款后，更新产品销售量 
    /// </summary>
    [Dependency]
    public class UpdateProductSaleSummaryHandler :  BaseBLL, INotificationHandler<PaymentConfirmedRequest<OrderDto>>
    {
        public IOrderBLL orderBLL;

        public UpdateProductSaleSummaryHandler(IServiceProvider services) : base(services)
        {
            orderBLL = Services.Resolve<IOrderBLL>();
        }

        public Task Handle(PaymentConfirmedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            orderBLL.UpdateProductSaleSummary(notification.Param);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    ///  当已确认付款后，增加库存预留记录
    /// </summary>
    [Dependency]
    public class AddInventoryReservedHandler : BaseBLL, INotificationHandler<PaymentConfirmedRequest<OrderDto>>
    {
        public IOrderBLL orderBLL;
        public AddInventoryReservedHandler(IServiceProvider services) : base(services)
        {
            orderBLL = Services.Resolve<IOrderBLL>();
        }

        public Task Handle(PaymentConfirmedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {             
            orderBLL.AddInventoryReserved(notification.Param, out var o);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 当已确认付款后，检查并发送低库存邮件
    /// </summary>
    [Dependency]
    public class SendLowQtyEmailWhenPaymentConfirmHandler : BaseBLL, INotificationHandler<PaymentConfirmedRequest<OrderDto>>
    {
        public IInventoryChangeNotifyBLL inventoryChangeNotifyBLL;

        public SendLowQtyEmailWhenPaymentConfirmHandler(IServiceProvider services) : base(services)
        {
            inventoryChangeNotifyBLL = Services.Resolve<IInventoryChangeNotifyBLL>();
        }

        public Task Handle(PaymentConfirmedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            inventoryChangeNotifyBLL.CheckAndNotifyAsync(notification.Param.skuList);
            return Task.CompletedTask;
        }
    }

}
