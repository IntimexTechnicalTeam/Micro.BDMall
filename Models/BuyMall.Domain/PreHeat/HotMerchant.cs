namespace BDMall.Domain
{
    public class HotMerchant
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MchId { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }

        public string Code { get; set; }

        public decimal Score { get; set; }

        public string LogoB { get; set; }

        public int Lang => (int)LangType;

        public Language LangType { get; set; }

        public Guid MerchantId => this.MchId;

        public string MerchantName => this.Name;

        public string ContactEmail { get; set; }

        public string CustomUrl { get; set; }

        public string Notice { get; set; }

        public string PromName { get; set; }

        public string PromIntroduction { get; set; }

        public string ExpCompleteDays { get; set; }

        public string Logo { get; set; }

        public string Cover { get; set; }

        public bool IsFavorite { get; set; }

        public string Description { get; set; }

        public string ReturnTerms { get; set; }

        public string TandC { get; set; }

        public MerchantType MerchantType { get; set; }

        public List<MerchantPromotionBannerView> Banners { get; set; } = new List<MerchantPromotionBannerView>();

        public List<MerchantPromotionProductView> ProductList { get; set; } = new List<MerchantPromotionProductView>();

    }
}
