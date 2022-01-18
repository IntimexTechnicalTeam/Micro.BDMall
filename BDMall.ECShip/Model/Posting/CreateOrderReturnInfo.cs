using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Posting
{
    public class CreateOrderReturnInfo
    {
        public string AdditionalDocument { get; set; }
        public decimal DeliveryCharge { get; set; }
        public string ErrMessage { get; set; }

        public decimal InsurPermFee { get; set; }

        public string ExpressNo { get; set; }
        public string ECShipNo { get; set; }

        public string Status { get; set; }

    }
}
