namespace BDMall.Model
{
    public class Currency
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(10)]
        [Column(Order = 5, TypeName = "varchar")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(10)]
        [Column(Order = 6)]
        public string Name { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public int Seq { get; set; }



        /// <summary>
        /// 默認貨幣
        /// </summary>
        [Required]
        [Column(Order = 8)]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注多語言ID
        /// </summary>
        [Column(Order = 9)]
        public string RemarkTransId { get; set; }



    }
}
