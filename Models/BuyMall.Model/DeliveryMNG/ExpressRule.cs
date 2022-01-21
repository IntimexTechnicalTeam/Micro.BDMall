﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressRule : BaseEntity<Guid>
    {

        public int Seq { get; set; }

        public decimal WeightFrom { get; set; }

        public decimal WeightTo { get; set; }

        public decimal FirstPrice { get; set; }

        public decimal AddWeight { get; set; }

        public decimal AddPrice { get; set; }

        public Guid ExpressId { get; set; }

        public Guid MerchantId { get; set; }


    }
}