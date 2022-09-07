namespace BDMall.Model
{
    public class MemberGroupDiscountItem : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid DiscountId { get; set; }

        [Column(Order = 4)]
        public Guid MemberGroupId { get; set; }
    }
}
