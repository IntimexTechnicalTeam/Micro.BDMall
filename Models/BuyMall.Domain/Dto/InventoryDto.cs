namespace BDMall.Domain
{
    public class InventoryDto :BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 倉庫記錄ID
        /// </summary>
      
        public Guid WHId { get; set; }
        //[ForeignKey("WHId")]
        //public virtual Warehouse WarehouseInfo { get; set; }
        /// <summary>
        /// Sku
        /// </summary>
       
        public Guid Sku { get; set; }
        //[ForeignKey("Sku")]
        //public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 倉存數量
        /// </summary>
       
        public int Quantity { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
       
        public Guid MerchantId { get; set; }
    }
}
