namespace BDMall.Model
{
    public class MemberFavorite : BaseEntity<Guid>
    {
        /// <summary>
        /// 收藏時的產品id
        /// </summary>
        [Column(Order = 4)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 會員ID
        /// </summary>
        [Column(Order = 5)]
        public Guid MemberId { get; set; }

        /// <summary>
        /// 收藏產品的編號
        /// </summary>
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 6)]
        public string ProductCode { get; set; }
    }
}
