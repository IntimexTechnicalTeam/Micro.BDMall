namespace BDMall.Model
{
    public class Order : BaseEntity<Guid>
    {
        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string OrderNO { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string InvoiceNO { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        public Guid PaymentMethodId { get; set; }

        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 兑换率
        /// </summary>
        public decimal ExchangeRate { get; set; }


        /// <summary>
        /// 订单商品总价
        /// </summary>

        public decimal TotalPrice { get; set; }
        /// <summary>
        ///  订单合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 數量合計
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// 订单商品总重量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 折扣后後的商品總價
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 折扣后的訂單合計金額
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 折扣后的運費
        /// </summary>
        public decimal DiscountFreight { get; set; }

        public decimal MallFun { get; set; }

        /// <summary>
        /// 運費折扣
        /// </summary>
        public decimal FreightDiscount { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Remark { get; set; }

        /// <summary>
        ///  订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 是否已經支付
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 計算免運費后的運費
        /// </summary>
        public decimal FreeShippingFreight { get; set; }

     
        /// <summary>
        /// bank authorization code
        /// </summary>
        [StringLength(20)]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string BankAuthCode { get; set; }

        /// <summary>
        /// 银行交易id
        /// </summary>
        [StringLength(20)]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 卡种类      
        ///  AMEX - American Express
        ///  CHINA_UNIONPAY - China UnionPay
        ///  DINERS_CLUB - Diners Club
        ///  DISCOVER - Discover
        ///  JCB - JCB(Japan Credit Bureau)
        ///  MASTERCARD - MasterCard
        ///  OTHER - The scheme of the card used in the transaction could not be identified.
        ///  UATP - UATP (Universal Air Travel Plan)
        ///  VISA - Visa
        /// </summary>
        [StringLength(20)]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string Acquirer { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }


}
