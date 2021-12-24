using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantShipMethodView
    {
        public string ShipMethodCode { get; set; }

        public string ShipMethodName { get; set; }

        public bool IsEffect { get; set; }
    }
}
