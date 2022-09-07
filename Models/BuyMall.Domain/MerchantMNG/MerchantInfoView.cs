namespace BDMall.Domain
{
    public class MerchantInfoView
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public Guid MerchantId { get; set; }

        public string MerchantName { get; set; }
        public string Cover { get; set; }

        public string MobileCover { get; set; }

        public string Logo { get; set; }

        public List<MerchantPromotionBannerView> Banners { get; set; } = new List<MerchantPromotionBannerView>();
        public List<MerchantPromotionProductView> ProductList { get; set; } = new List<MerchantPromotionProductView>();

        public string ExpCompleteDays { get; set; }

        public string Description { get; set; }
        public string PromIntroduction { get; set; }
        public string PromName { get; set; }

        /// <summary>
        /// 商店條款
        /// </summary>
        public string TandC { get; set; }

        //public bool IsGS1 { get; set; }

        public Language Lang { get; set; }

        public decimal Score { get; set; }

        public bool IsFavorite { get; set; }

        public string Notice { get; set; }

        /// <summary>
        /// 退換貨條款
        /// </summary>
        public string ReturnTerms { get; set; }

        public string Code { get; set; }

        public bool IsHongKong { get; set; }

        public string ContactEmail { get; set; }

        public string CustomUrl { get; set; }
    }
}
