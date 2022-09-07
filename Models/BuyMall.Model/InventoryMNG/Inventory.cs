namespace BDMall.Model
{
    public class Inventory : BaseEntity<Guid>
    {
        /// <summary>
        /// 倉庫記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid WHId { get; set; }
        //[ForeignKey("WHId")]
        //public virtual Warehouse WarehouseInfo { get; set; }
        /// <summary>
        /// Sku
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid Sku { get; set; }
        //[ForeignKey("Sku")]
        //public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 倉存數量
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public int Quantity { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public Guid MerchantId { get; set; }
    }
}
