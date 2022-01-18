using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class TransRulePrice
    {
        public List<ExpressPrice> charges { get; set; }
        public ExpressRule rule { get; set; }

    }
}
