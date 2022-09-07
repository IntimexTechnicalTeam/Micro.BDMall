namespace BDMall.Model
{
    /// <summary>
    /// 產品價格時段
    /// </summary>
    public class ProductPriceHour : BaseEntity<Guid>
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 產品ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 產品Code
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 時段價格
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column(Order = 6)]
        public decimal TimePrice { get; set; }

        /// <summary>
        /// 時間段(PreExcuteTime时间戳)
        /// </summary>
        [Column(Order = 7)]
        public long TimeField { get; set; }

        /// <summary>
        /// 時段狀態（開啓:1,關閉:0）
        /// </summary>
        [Column(Order = 8)]
        [Required]
        [DefaultValue(false)]
        public bool IsTimeStatus { get; set; }


        /// <summary>
        /// 貨幣Code
        /// </summary>
        [Column(Order = 9)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 生效开始日期
        /// </summary>
        [Column(Order = 10)]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 生效结束日期
        /// </summary>
        [Column(Order = 11)]
        public DateTime EndTime { get; set; }


        /// <summary>
        ///  備注是否在時段價格内修改過價格
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 12)]
        [StringLength(100)]
        public string ReMark { get; set; }

        /// <summary>
        /// 是否恢復原價（是:1,否:0）
        /// </summary>
        [Column(Order = 13)]
        [Required]
        [DefaultValue(false)]
        public bool IsReStatus { get; set; }

        /// <summary>
        ///  执行时间
        /// </summary>
        [Column(Order = 14)]
        public DateTime PreExcuteTime { get; set; }

        ///// <summary>
        /////  执行时间段（毫秒）
        ///// </summary>
        //[Column(Order = 15)]
        //public int ExcuteTimeField { get; set; }

        ///// <summary>
        /////  执行结束时间段（毫秒）
        ///// </summary>
        //[Column(Order = 16)]
        //public int ExcuteEndTimeField { get; set; }

        /// <summary>
        ///  时间排序
        /// </summary>
        [NotMapped]
        public long SeqTimeField { get; set; }

    }
}
