using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MarketingDiscount
    {
        public Guid Id { get; set; }
        public decimal DiscountRange { get; set; }
        public decimal DiscountMoney { get; set; }
        public string CreateDate { get; set; }
        public bool IsDiscount { get; set; }

        public List<string> MemberGroups { get; set; }

        public List<MutiLanguage> Titles { get; set; }

        public string Title { get; set; }
    }
}
