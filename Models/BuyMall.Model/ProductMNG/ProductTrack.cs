namespace BDMall.Model
{
    public class ProductTrack:BaseEntity<int>
    {
        [Column(Order = 3)]
        public string ProductCode { get; set; }

        [Column(Order = 4)]
        public Guid MemberId { get; set; }

    }
}
