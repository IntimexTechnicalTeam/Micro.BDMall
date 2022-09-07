namespace BDMall.Domain
{
    public class InvSummaryView
    {
        /// <summary>
        /// 產品圖片
        /// </summary>
        public string ProdImage { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProdCode { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProdName { get; set; }
        /// <summary>
        /// 庫存總數量
        /// </summary>
        public int InventoryTotalQty { get; set; }
        /// <summary>
        /// 預留總數量
        /// </summary>
        public int ReservedTotalQty { get; set; }
        /// <summary>
        /// 掛起總數量
        /// </summary>
        public decimal HoldingTotalQty { get; set; }
        /// <summary>
        /// 可銷售總數量
        /// </summary>
        public int SalesTotalQty { get; set; }
    }
}
