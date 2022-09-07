namespace BDMall.Domain
{
    public class ShopCartInfo
    {
        public List<ShopcartItem> Items { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Qty { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal DicountAmount { get; set; }

        public decimal ItemsAmount { get; set; }

        /// <summary>
        /// 稅縂額
        /// </summary>
        public decimal ItemsTaxAmount { get; set; }

        public decimal DeliveryCharge { get; set; }

        public SimpleCurrency Currency { get; set; }

        public string PayMethodType { get; set; }

        public string PayMethodTypeId { get; set; }

        /// <summary>
        /// 是否临时购物车，未登入用户为true
        /// </summary>
        public bool IsTemp { get; set; }

        /// <summary>
        /// 總重量（毛重）
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 活动产品合计
        /// </summary>
        public decimal EventProductPrice { get; set; }
     
    }
}
