namespace BDMall.Model
{
    /// <summary>
    /// 銷售退回資料
    /// </summary>
    public class SalesReturnOrder : BaseEntity<Guid>
    {
        /// <summary>
        /// 單號
        /// </summary>
        [Required]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 3)]
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 銷售單ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid SOId { get; set; }
         
        /// <summary>
        /// 銷售退回時間
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public DateTime ReturnDate { get; set; }
 
    }
}
