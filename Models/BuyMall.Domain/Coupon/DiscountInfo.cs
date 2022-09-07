namespace BDMall.Domain
{
    public class DiscountInfo
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 優惠券標題
        /// </summary>
        public string Title { get; set; }

        public string Remark { get; set; }

        public Guid MerchantId { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 是否百分比
        /// </summary>
        public bool IsPercent { get; set; }

        public CouponUsage CouponType { get; set; }

        /// <summary>
        /// 滿多少可用
        /// </summary>
        public decimal DiscountRange { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime? EffectDateFrom { get; set; }

        public DateTime? EffectDateTo { get; set; }

        public bool IsActive { get; set; }

        public DiscountType DiscountType { get; set; }

        public string Image { get; set; }
    }
}
