﻿namespace BDMall.Domain
{
    public class MemLoginRecPager : PageInfo
    {
        public string Email { get; set; }

        public DateTime? LoginDateFrom { get; set; }

        public DateTime? LoginDateTo { get; set; }


    }
}
