using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductDeliveryArea
    {
        public Guid ProductId { get; set; }

        public string Code { get; set; }

        public Country Country { get; set; }
    }
}
