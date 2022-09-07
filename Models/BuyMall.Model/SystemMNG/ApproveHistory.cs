namespace BDMall.Model
{
    public class ApproveHistory : BaseEntity<Guid>
    {

        [Column(Order = 4)]
        public ApproveResult ApproveType { get; set; }

        [Column(Order = 5)]
        public Guid MerchantId { get; set; }

        [Column(Order = 6)]
        public Guid OperateId { get; set; }

        [Column(Order = 7)]
        public DateTime OperateDate { get; set; }

        [MaxLength(500)]
        [Column(Order = 8)]
        public string Remark { get; set; }

        [Column(Order = 9)]
        public ApproveModule ApproveModule { get; set; }

        [Column(Order = 10)]
        public Guid ModuleId { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }
    }
}
