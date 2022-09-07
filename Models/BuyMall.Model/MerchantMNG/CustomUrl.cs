namespace BDMall.Model
{
    /// <summary>
    /// 自定义Url
    /// </summary>
    public class CustomUrl : BaseEntity<int>
    {
        /// <summary>
        /// MappingId(产品Code、商家Id)
        /// </summary>
        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 3)]
        public string KeyId { get; set; }

        /// <summary>
        /// 自定义Url类型
        /// </summary>
        [Column(Order = 4)]
        public CustomUrlType KeyType { get; set; }

        /// <summary>
        /// 配对的Url
        /// </summary>
        [MaxLength(500)]
        [Column(TypeName = "varchar", Order = 5)]
        public string Url { get; set; }
    }
}
