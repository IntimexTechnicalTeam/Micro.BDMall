using BDMall.Domain;
using BDMall.Model;
using BDMall.Repository;
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
    /// 当已安排发货后，更新InventoryQty
    /// </summary>
    [Dependency]
    public class UpdateInventoryQtyHandler : BaseBLL, INotificationHandler<DeliveryArrangedRequest<OrderDto>>
    {
        public IOrderBLL orderBLL;
        
        public UpdateInventoryQtyHandler(IServiceProvider services) : base(services)
        {
            orderBLL  = Services.Resolve<IOrderBLL>();
        }

        public Task Handle(DeliveryArrangedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            orderBLL.UpdateInventoryQty(notification.Param,notification.cond);
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// 当已安排发货后，更新OrderDeliveryDetail
    /// </summary>
    [Dependency]
    public class UpdateDeliveryDetailTrackingNoHandler : BaseBLL, INotificationHandler<DeliveryArrangedRequest<OrderDto>>
    {
        public IOrderBLL orderBLL;
        public IOrderDeliveryRepository orderDeliveryRepository;

        public UpdateDeliveryDetailTrackingNoHandler(IServiceProvider services) : base(services)
        {
            orderBLL = Services.Resolve<IOrderBLL>();
            orderDeliveryRepository = Services.Resolve<IOrderDeliveryRepository>();
        }

        public Task Handle(DeliveryArrangedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            var deliveryData = notification.Param.OrderDeliverys.FirstOrDefault(p => p.Id == notification.cond.DeliveryTrackingInfo.FirstOrDefault().Id);

            var trackingNoList = deliveryData.TrackingNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var trackingNO in trackingNoList)              //如果是EMS的快遞單，有機會出現一章送貨單對應多張快遞單號
            {               
                var deliveryDetails = deliveryData.DeliveryDetails.Where(w=>w.DeliveryId == deliveryData.Id).OrderBy(o => o.Id).ToList();
                foreach (var item in deliveryDetails)
                {
                    item.TrackingNo = trackingNO;
                    item.LocationId = deliveryData.LocationId;
                    orderDeliveryRepository.UpdateOrderDeliveryDetail(item);
                    //baseRepository.Update(item);
                }
                
            }
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// 当已按排送货后，检查并发送低库存邮件
    /// </summary>
    [Dependency]
    public class SendLowQtyEmailWhenDeliveryArrangedHandler : BaseBLL, INotificationHandler<DeliveryArrangedRequest<OrderDto>>
    {
        public IInventoryBLL inventoryBLL;

        public SendLowQtyEmailWhenDeliveryArrangedHandler(IServiceProvider services) : base(services)
        {
            inventoryBLL = Services.Resolve<IInventoryBLL>();
        }

        public Task Handle(DeliveryArrangedRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            var orderDeliveryInfo = notification.cond.DeliveryTrackingInfo.FirstOrDefault();

            var orderDeliveries = notification.Param.OrderDeliverys;

            var orderDelivery = orderDeliveries.FirstOrDefault(p => p.Id == orderDeliveryInfo.Id);

            var deliveryItems = orderDelivery.DeliveryDetails.GroupBy(g => g.SkuId).Select(d => new
            {
                SkuId = d.Key,
                Qty = d.Sum(a => a.Qty)
            }).ToList();

            foreach (var product in deliveryItems)
            {
                var reserve = new InventoryReservedDto();
                reserve.Sku = product.SkuId;
                reserve.SubOrderId = orderDelivery.Id;
                reserve.WHId = orderDeliveryInfo.LocationId;
                reserve.OrderId = notification.Param.Id;
                inventoryBLL.InventoryChangeCheckAndNotify(reserve);
            }
            return Task.CompletedTask;
        }
    }
}
