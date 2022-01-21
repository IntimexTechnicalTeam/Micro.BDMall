using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IOrderDeliveryRepository:IDependency
    {
        void UpdateOrderDeliveryStatus(OrderDelivery delivery);

        void UpdateOrderDeliveryArrangedStatus(OrderDelivery delivery);
        void UpdateOrderDeliveryTrackingNo(OrderDelivery delivery);
        void UpdateDeliverySendBackCount(OrderDelivery delivery);
    }
}
