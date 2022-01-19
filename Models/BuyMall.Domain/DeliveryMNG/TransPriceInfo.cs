using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class TransPriceInfo
    {
        public decimal Weight { get; set; }
        public List<ExpressPrice> zoneCharge { get; set; }
    }
}
