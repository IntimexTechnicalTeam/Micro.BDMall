using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class MerchantCond : PageInfo
    {
        public string Name { get; set; }

        public bool ShowPass { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsActiveCond { get; set; }
    }
}
