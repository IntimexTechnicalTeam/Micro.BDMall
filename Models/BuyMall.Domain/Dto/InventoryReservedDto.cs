namespace BDMall.Domain
{
    public class InventoryReservedDto:BaseDto
    {
        public Guid Id { get; set; }    

        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        public Guid Sku { get; set; }
        //[ForeignKey("Sku")]
        //public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 訂單記錄ID
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 訂單資料
        /// </summary>
        //[ForeignKey("OrderId")]
        //public virtual Order Order { get; set; }
        /// <summary>
        /// 預留數量
        /// </summary>
        public int ReservedQty { get; set; }
        /// <summary>
        /// 預留類型
        /// </summary>
        public InvReservedType ReservedType { get; set; }
        /// <summary>
        /// 預留處理狀態
        /// </summary>

        public InvReservedState ProcessState { get; set; }

      
        public Guid SubOrderId { get; set; }
        /// <summary>
        /// 倉庫ID
        /// </summary>

        public Guid? WHId { get; set; }
    }
}
