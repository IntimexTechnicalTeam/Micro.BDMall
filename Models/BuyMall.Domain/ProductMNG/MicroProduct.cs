namespace BDMall.Domain
{
    public class MicroProduct
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal SalePrice {get;set;}

        public decimal SalePrice2 { get;set;}

        public SimpleCurrency Currency { get; set; }    

        public SimpleCurrency Currency2 { get; set; }

        public bool IsFavorite { get; set; }

        public string ImagePath { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Score { get; set; }

        public int PurchaseCounter { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }    
    }
}
