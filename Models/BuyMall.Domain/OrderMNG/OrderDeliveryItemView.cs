using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class OrderDeliveryItemView : BuyItem
    {
        public Guid DeliveryId { get; set; }

    }
}
