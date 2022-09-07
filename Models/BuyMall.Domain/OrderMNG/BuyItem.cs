namespace BDMall.Domain
{
    public class BuyItem
    {
        /// <summary>
        /// Sku
        /// </summary>
        [Required(ErrorMessage = "Sku必填")]
        public Guid Sku { get; set; }=Guid.Empty;

        /// <summary>
        /// 购买数量
        /// </summary>
        [Required(ErrorMessage ="购买数量必填")]
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
