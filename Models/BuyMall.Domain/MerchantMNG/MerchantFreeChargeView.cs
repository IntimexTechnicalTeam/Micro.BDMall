﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantFreeChargeView
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }

        public List<string> ShipCodes { get; set; }

        public List<MerchantFreeChargeProductView> Products { get; set; }

        public string Action { get; set; }
    }
}
