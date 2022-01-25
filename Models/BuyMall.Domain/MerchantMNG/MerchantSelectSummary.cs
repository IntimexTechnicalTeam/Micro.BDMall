using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantSelectSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; }

    }
}
