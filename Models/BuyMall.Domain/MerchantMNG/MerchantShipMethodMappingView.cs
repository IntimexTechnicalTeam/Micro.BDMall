using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantShipMethodMappingView
    {
        public Guid MerchantId { get; set; }

        public List<MerchantShipMethodView> MerchantShipMethods { get; set; } = new List<MerchantShipMethodView>();


    }
}
