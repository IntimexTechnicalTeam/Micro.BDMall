﻿using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class CouponPager : PageInfo
    {
        public CouponStatus status { get; set; }
    }
}