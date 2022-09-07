namespace BDMall.Model
{
    public class ProductAttribute : BaseEntity<Guid>
    {
        [Column(Order = 3, TypeName = "varchar")]
        [StringLength(20)]
        public string Code { get; set; }
        [Column(Order = 4)]
        public Guid DescTransId { get; set; }

        //public bool IsBatchOrder { get; set; }
        /// <summary>
        /// 是否庫存屬性
        /// </summary>
        [Column(Order = 5)]
        public bool IsInvAttribute { get; set; }

        /// <summary>
        /// 屬性的佈局
        /// </summary>
        [Column(Order = 6)]
        public AttrLayout Layout { get; set; }
     
    }
}
