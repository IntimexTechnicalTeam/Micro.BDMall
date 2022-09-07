namespace BDMall.Domain
{
    public class ExpressCondition
    {
        /// <summary>
        /// 送貨地址ID
        /// </summary>
        public Guid DeliveryAddrId { get; set; }
        /// <summary>
        /// 快遞方式ID
        /// </summary>
        public Guid ExpressId { get; set; }
        public string ShipCode { get; set; }
        /// <summary>
        /// 商品總重量
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// 商品價格小計（不含運費）
        /// </summary>
        public decimal ItemAmount { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 站點編號
        /// </summary>
        public string StationCode { get; set; }

        public List<ProductWeightInfo> ProductWeightInfo { get; set; }
    }
}
