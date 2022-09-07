namespace BDMall.Domain
{
    public  class ProductAttrValueDto
    {
        public Guid Id { get; set; }
       
        public Guid ClientId { get; set; }

        /// <summary>
        /// 产品配对属性Id
        /// </summary>     
        public Guid ProdAttrId { get; set; }

        /// <summary>
        /// 屬性值ID
        /// </summary>       
        public Guid AttrValueId { get; set; }

        /// <summary>
        /// 序號
        /// </summary>       
        public int Seq { get; set; }

        /// <summary>
        /// 附加价钱
        /// </summary>
        public decimal AdditionalPrice { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
       
        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }

    }
}
