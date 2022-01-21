using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class BuyItem
    {
        public Guid Sku { get; set; }

        public int Qty { get; set; }

        /// <summary>
        /// PromotionRule Id
        /// </summary>
        public Guid RuleId { get; set; }

        public PromotionRuleType RuleType { get; set; }
        /// <summary>
        /// 是否贈品
        /// </summary>
        public bool IsFree { get; set; }
    }
}
