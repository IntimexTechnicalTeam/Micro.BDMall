using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProdouctListSummaryView
    {
        public ProdouctListSummaryView()
        {
            this.Score = 0M;
        }
        /// <summary>
        /// 商家名稱ID
        /// </summary>
        public Guid MerchantNameId { get; set; }

        public Guid MerchantId { get; set; }
        public string MerchantNo { get; set; }

        /// <summary>
        /// 商家名稱
        /// </summary>
        public string MerchantName { get; set; }
        public Guid ProductId { get; set; }
        public string Code { get; set; }

        public Guid NameTransId { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 網上銷售價
        /// </summary>
        public decimal SalePrice { get; set; }
        public decimal SalePrice2 { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrice2 { get; set; }

        public decimal MarkupPrice { get; set; }
        public decimal MarkupPrice2 { get; set; }


        public bool HasDiscount
        {
            get
            {
                return this.SalePrice < this.OriginalPrice;
            }
            set { }
        }

        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }

        public string CurrencyCode { get; set; }
        /// <summary>
        /// 產品所屬目錄ID
        /// </summary>
        public Guid CatalogId { get; set; }

        /// <summary>
        /// 權限Id
        /// </summary>
        //public string PmsnId { get; set; }
        /// <summary>
        /// 目录層級字符串
        /// 0,1,2,3,4,
        /// </summary>
        public string CatalogPath { get; set; }
        /// <summary>
        /// 目錄名稱
        /// </summary>
        public string CatalogName { get; set; }

        public List<string> Imgs { get; set; }


        public bool IsFavorite { get; set; }

        //public bool IsApprove { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public decimal Score { get; set; }


        /// <summary>
        /// Web版产品图标
        /// </summary>
        public ProductType IconType { get; set; }

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
        public string IconLUrl { get; set; }

        /// <summary>
        /// 是否香港信心產品
        /// </summary>
        public bool IsGS1 { get; set; }
        /// <summary>
        /// 香港信心產品圖標路徑
        /// </summary>

        public string PromotionRuleTitle { get; set; }

    }
}
