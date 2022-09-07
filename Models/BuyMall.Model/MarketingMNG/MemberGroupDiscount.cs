namespace BDMall.Model
{
    public class MemberGroupDiscount : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public decimal MeetAmount { get; set; }

        /// <summary>
        /// 是否折扣
        /// </summary>
        [Column(Order = 4)]
        public bool IsDiscount { get; set; }

        [Column(Order = 5)]
        public decimal DiscountAmount { get; set; }

        [Column(Order = 6)]
        public Guid TitleTranId { get; set; }

        //public virtual ICollection<MemberGroupDiscountItem> MemberGroupDiscountItems { get; set; }
    }
}
