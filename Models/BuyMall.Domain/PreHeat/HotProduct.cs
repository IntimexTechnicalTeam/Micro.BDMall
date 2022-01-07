using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class HotProduct
    {
        /// <summary>
        /// 取Translation表主键
        /// </summary>
        //public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MchId { get; set; }

        public decimal SalePrice { get; set; }

        public decimal OriginalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal InternalPrice { get; set; }

        /// <summary>
        /// 附加价钱
        /// </summary>
        public decimal MarkUpPrice { get; set; }

        public Guid CatalogId { get; set; }

        /// <summary>
        /// 默认图片Id ,可通过这个直接从ProductImageLists表读商品图片
        /// </summary>
        public Guid DefaultImageId { get; set; }

        public ProductStatus Status { get; set; }

        public int Lang => (int)LangType;

        public Language LangType { get; set; }

        /// <summary>
        /// DateTime.Ticks
        /// </summary>
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public ProductType IconType { get; set; }

        public string ProductCode { get; set; }

        public string Code => this.ProductCode;

        public bool IsFavorite { get; set; }

        public decimal Score { get; set; }

        public decimal ListPrice => this.OriginalPrice;

        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }
    }
}
