namespace BDMall.Domain
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }

        public string MerchantName { get; set; }

        public Guid ProductId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 网上销价
        /// </summary>
        public decimal SalePrice { get; set; }
        public decimal SalePrice2 { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrice2 { get; set; }

        public decimal MarkupPrice { get; set; }
        public decimal MarkupPrice2 { get; set; }

        public List<string> Imgs { get; set; } = new List<string>();

        public string CurrencyCode { get; set; }

        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }

        public int Seq { get; set; }

        public ProductType IconType { get; set; }

        public string IconLUrl { get; set; }

        public bool IsFavorite { get; set; }

        /// <summary>
        /// 右角標類別
        /// </summary>
        public ProductType? IconRType { get; set; }
        /// <summary>
        /// 右角標路徑
        /// </summary>
        public string IconRUrl { get; set; }
        /// <summary>
        /// 左角標類別
        /// </summary>
        public ProductType? IconLType { get; set; }

        public Guid NameTransId { get; set; }

        public Guid MerchantNameId { get; set; }

        public string IconString { get; set; } = "";

        public string PromotionRuleTitle { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDiscount => this.SalePrice < this.OriginalPrice;

        public bool RuleFlag { get; set; }

        public List<string> EventCodes { get; set; } = new List<string>();

        public bool IsEventProduct
        {

            get
            {
                if (EventCodes == null)
                {
                    return false;
                }
                else
                {
                    if (EventCodes.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            set
            { }
        }
    }
}
