using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Calculator
{
    public class PostageInfo
    {
        public string CountryCode { get; set; }
        public string ShipCode { get; set; }
        
        public decimal Weight { get; set; }

        public string MailType { get; set; }
    }
}
