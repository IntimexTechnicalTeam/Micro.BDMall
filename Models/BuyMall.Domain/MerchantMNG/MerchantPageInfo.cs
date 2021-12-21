using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class MerchantPageInfo : PageInfo
    {
        public MerchantSrchCond Condition { get; set; } =new MerchantSrchCond();
    }
}
