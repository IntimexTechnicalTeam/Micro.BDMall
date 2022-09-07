namespace BDMall.Model
{
    /// <summary>
    /// 退換單
    /// </summary>
    public class ReturnOrder : BaseEntity<Guid>
    {
        /// <summary>
        /// 退換單號
        /// </summary>
        [Required]
        [StringLength(20)]
        [Column(Order = 3, TypeName = "varchar")]
        public string RONo { get; set; }
        /// <summary>
        /// 申請退換類型
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public ReturnOrderType ApplyType { get; set; }
        /// <summary>
        /// 審批退換方式
        /// </summary>
        [Column(Order = 5)]
        public ReturnOrderType? ApproveType { get; set; }
        /// <summary>
        /// 原訂單ID
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public Guid OrderId { get; set; }
        /// <summary>
        /// 會員ID
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public Guid MemberId { get; set; }
        [Required]
        [DefaultValue(0)]
        [Column(Order = 8)]
        public decimal MallFun { get; set; }
        /// <summary>
        /// 退換單狀態
        /// </summary>
        [Required]
        [Column(Order = 9)]
        public ReturnOrderStatus Status { get; set; }
        /// <summary>
        /// 退款額
        /// </summary>
        [Required]
        [Column(Order = 10)]
        public decimal Refund { get; set; }

        ///// <summary>
        ///// 退換單明細
        ///// </summary>
        //public virtual ICollection<ReturnOrderDetail> ReturnOrderDetails { get; set; }

        ///// <summary>
        ///// 退換單留言列表
        ///// </summary>
        //public virtual ICollection<ReturnOrderMessage> ReturnOrderMsgs { get; set; }

        //[NotMapped]
        //public string OrderNo { get; set; }
        ///// <summary>
        ///// 當次留言
        ///// </summary>
        //[NotMapped]
        //public string RtnOrderMessage { get; set; }
        ///// <summary>
        ///// 選中的產品Sku列表
        ///// </summary>
        //[NotMapped]
        //public List<Guid> SelectedProductSkus { get; set; }
    }
}
