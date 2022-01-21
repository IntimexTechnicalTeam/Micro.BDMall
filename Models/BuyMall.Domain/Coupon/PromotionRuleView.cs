using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PromotionRuleView
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }

        public Guid ProductId { get; set; }

        public string MerchantName { get; set; }

        public PromotionRuleType PromotionRule { get; set; }

        public string PromotionRuleString
        {
            get
            {
                string result = "";
                switch (PromotionRule)
                {
                    case PromotionRuleType.BuySend:
                        result = Resources.Label.BuySend;
                        break;
                    case PromotionRuleType.GroupSale:
                        result = Resources.Label.GroupSale;
                        break;
                }
                return result;
            }
            set { }
        }

        public decimal X { get; set; }

        public decimal Y { get; set; }


        public string Title { get; set; }

        public string Remark { get; set; }

        public bool IsActive { get; set; }


    }
}
