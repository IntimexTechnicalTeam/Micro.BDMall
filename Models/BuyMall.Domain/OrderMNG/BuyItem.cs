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
        public Guid Sku { get; set; }=Guid.Empty;

        public int Qty { get; set; }

        /// <summary>
        /// 一般为PromotionRule Id
        /// </summary>
        public Guid RuleId { get; set; } = Guid.Empty;

        public PromotionRuleType RuleType { get; set; } = PromotionRuleType.BuySend;
        /// <summary>
        /// 是否贈品
        /// </summary>
        public bool IsFree { get; set; }
    }
}
