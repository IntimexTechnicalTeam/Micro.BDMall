namespace BDMall.Model
{
    public class Coupon : BaseEntity<Guid>
    {


        [Column(Order = 3)]
        public Guid MemberId { get; set; }

        /// <summary>
        /// 規則ID
        /// </summary>
        [Column(Order = 4)]
        public Guid RuleId { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        [Column(Order = 5)]
        public DateTime? UseDate { get; set; }



        ///// <summary>
        ///// 是否折扣券
        ///// </summary>
        //[Column(Order = 6)]
        //public bool IsDiscount { get; set; }

        ///// <summary>
        ///// 優惠金額(如果IsDiscount==true則是折扣，反之則是現金優惠)
        ///// </summary>
        //[Column(Order = 7)]
        //public decimal DiscountAmount { get; set; }


        ///// <summary>
        ///// 滿足優惠金額
        ///// </summary>
        //[Column(Order = 8)]
        //public decimal MeetAmount { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 6)]
        public DateTime? EffectDateFrom { get; set; }


        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 7)]
        public DateTime? EffectDateTo { get; set; }


        ///// <summary>
        ///// 優惠券規則類型
        ///// </summary>
        //[Column(Order = 11)]
        //public CouponRuleType Type { get; set; }

        ///// <summary>
        ///// 優惠券圖片
        ///// </summary>
        //[MaxLength(300)]
        //[Column(TypeName = "varchar", Order = 12)]
        //public string Image { get; set; }

        ///// <summary>
        ///// 備註
        ///// </summary>
        //[MaxLength(500)]
        //[Column(Order = 13)]
        //public string Remark { get; set; }


        //[Column(Order = 8)]
        //public Guid MerchantId { get; set; }

        [Column(Order = 8)]
        public Guid OrderId { get; set; }

        [Column(Order = 9)]
        public Guid DeliveryId { get; set; }

        //[Column(Order = 13)]
        //public string Remark { get; set; }
        //[Column(Order = 14)]
        //public CouponUsage CouponUsage { get; set; }

        //[Column(Order = 20)]
        //public bool IsSuperimposed { get; set; }




    }
}
