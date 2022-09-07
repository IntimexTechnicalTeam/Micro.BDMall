namespace BDMall.Model
{
    public class PromotionCodeCoupon : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        //[MaxLength(20)]
        //[Column(TypeName = "varchar", Order = 4)]
        //public string Code { get; set; }

        /// <summary>
        /// 是否折扣券
        /// </summary>
        [Column(Order = 5)]
        public bool IsDiscount { get; set; }

        /// <summary>
        /// 優惠金額(如果IsDiscount==true則是折扣，反之則是現金優惠)
        /// </summary>
        [Column(Order = 6)]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 滿足優惠金額
        /// </summary>
        [Column(Order = 7)]
        public decimal MeetAmount { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 8)]
        public DateTime? EffectDateFrom { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 9)]
        public DateTime? EffectDateTo { get; set; }

        /// <summary>
        /// 優惠券圖片
        /// </summary>
        [MaxLength(300)]
        [Column(TypeName = "varchar", Order = 10)]
        public string Image { get; set; }

        [Column(Order = 11)]
        public CouponUsage CouponUsage { get; set; }

        [Column(Order = 12)]
        public int Limit { get; set; }

        [Column(Order = 13)]
        public int UseCount { get; set; }

        [Column(Order = 14)]
        public Guid TitleTransId { get; set; }

        /// <summary>
        /// 是否部分商品券
        /// </summary>
        [Column(Order = 15)]
        public bool IsProdCoupon { get; set; }

        /// <summary>
        /// 單個產品的最大優惠使用量
        /// 默認為零，則不限制可使用優惠的產品數量
        /// </summary>
        [Column(Order = 16)]
        public int MaxUsagePerProd { get; set; }

        //public virtual ICollection<PromotionCodeList> CodeList { get; set; }

        //public virtual ICollection<PromotionCodeProduct> ProductList { get; set; }
    }
}
