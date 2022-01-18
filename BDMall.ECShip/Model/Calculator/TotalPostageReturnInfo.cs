using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Calculator
{
    public class TotalPostageReturnInfo
    {
        public decimal AdditionalCharge { get; set; }
        public string ErrMessage { get; set; }
        public decimal InsurancePremiumFee { get; set; }

        public string Status { get; set; }

        public decimal TotalPostage { get; set; }

    }
}
