namespace BDMall.Model
{
    public class SubOrderStatusHistory : BaseEntity<Guid>
    {

        /// <summary>
        /// 訂單ID
        /// </summary>
        [Column(Order = 3)]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 子訂單iD(原送貨單ID)
        /// </summary>
        [Column(Order = 4)]
        public Guid SubOrderId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 5)]
        public string Operator { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        [Column(Order = 6)]
        public OrderStatus Status { get; set; }
    }
}
