using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantPromotionProductView
    {
        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }

        public Guid ProductId { get; set; }
        public string Code { get; set; }
        //public int Sku { get; set; }
        public Guid NameTransId { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        //public string Image { get; set; }

        public List<string> Imgs { get; set; }

        public bool IsDelete { get; set; }

        public string CurrencyCode { get; set; }
        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }

        public decimal ListPrice { get; set; }
        public decimal ListPrice2 { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal SalesPrice2 { get; set; }

        public decimal MarkUpPrice { get; set; }
        public decimal MarkUpPrice2 { get; set; }

        public Guid MerchantId { get; set; }

        public decimal Score { get; set; }

        /// <summary>
        /// 角標類別
        /// </summary>
        public ProductType? IconType { get; set; }

        /// <summary>
        /// 是否香港信心產品
        /// </summary>
        public bool IsGS1 { get; set; }

        public string PromotinoRuleTitle { get; set; }

        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
