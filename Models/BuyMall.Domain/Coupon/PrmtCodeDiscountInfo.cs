namespace BDMall.Domain
{
    public class PrmtCodeDiscountInfo : DiscountInfo
    {
        /// <summary>
        /// 是否限定商品券
        /// </summary>
        public bool IsProdCoupon { get; set; }

        /// <summary>
        /// 單個產品可優惠的使用量
        /// </summary>
        public int MaxUsagePerProd { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// 限定商品的優惠資料清單
        /// </summary>
        public List<PromotionCodeProd> OriginalProdList { get; set; }

        /// <summary>
        /// 限定商品的優惠資料清單
        /// </summary>
        public List<PromotionCodeProd> DiscountProdList { get; set; }

        /// <summary>
        /// 商品清單
        /// </summary>
        public List<PrmtCodeProdProfiles> SettlementProfilesList { get; set; }
    }
}
