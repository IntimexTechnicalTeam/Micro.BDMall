using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class DiscountGroup
    {
        public List<DiscountInfo> DeliveryChargeCoupons { get; set; }

        public List<DiscountInfo> PriceCoupons { get; set; }

    }
}
