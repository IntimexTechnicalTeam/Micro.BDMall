namespace BDMall.Domain
{
    public class PRDiscountPrice
    {
        public Guid PromotionRuleId { get; set; }

        public Guid ProductId { get; set; }

        public Guid DeliveryId { get; set; }

        /// <summary>
        /// 每件产品优惠的价钱
        /// </summary>
        public decimal SingleDiscountPrice { get; set; }

        /// <summary>
        /// 优惠的价钱
        /// </summary>
        public decimal DiscountPrice { get; set; }
    }
}
