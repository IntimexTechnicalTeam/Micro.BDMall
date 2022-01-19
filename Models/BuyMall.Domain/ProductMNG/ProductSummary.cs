using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductSummary
    {
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
        /// 產品簡介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string DescriptionDetail { get; set; }
        /// <summary>
        /// 網上銷售價
        /// </summary>
        public decimal SalePrice { get; set; }
        public decimal SalePrice2 { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrice2 { get; set; }

        public decimal MarkupPrice { get; set; }
        public decimal MarkupPrice2 { get; set; }

        public decimal GrossWeight { get; set; }

        public WeightUnit WeightUnit { get; set; }

        /// <summary>
        /// SalePrice+附加价钱
        /// </summary>
        public decimal TotalPrice { get; set; }

        public bool HasDiscount
        {
            get
            {
                return this.SalePrice < this.OriginalPrice;
            }
            set { }
        }

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

        public bool? IsActive { get; set; }

        public bool IsDeleted { get; set; }
        //public bool IsApprove { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public decimal Score { get; set; } = 0;

        /// <summary>
        /// 購買次數
        /// </summary>
        public int PurchaseCounter { get; set; }
        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int VisitCounter { get; set; }
        /// <summary>
        /// 用於Promotion Product 排序
        /// </summary>
        public int Seq { get; set; }

        public bool IsApprove { get; set; }

        public ProductStatus ApproveType { get; set; }

        public string ApproveTypeString
        {
            get
            {
                string result = "";
                switch (ApproveType)
                {
                    case ProductStatus.Editing:
                        result = Resources.Value.Editing;
                        break;
                    case ProductStatus.WaitingApprove:
                        result = Resources.Value.WaitingApprove;
                        break;
                    case ProductStatus.Reject:
                        result = Resources.Value.Reject;
                        break;
                    case ProductStatus.Pass:
                        result = Resources.Value.Pass;
                        break;
                    case ProductStatus.OnSale:
                        result = Resources.Value.OnSale;
                        break;
                    case ProductStatus.ManualOffSale:
                        result = Resources.Value.ManualOffSale;
                        break;
                    case ProductStatus.AutoOffSale:
                        result = Resources.Value.OffSale;
                        break;
                    case ProductStatus.WaitingOnSale:
                        result = Resources.Value.WaitingOnSale;
                        break;


                }
                return result;
            }
            set { }
        }

        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 優惠描述
        /// </summary>
        public string IconString { get; set; }

       
        public string IconLUrl { get; set; }

        public string UpdateDateString =>UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
        /// <summary>
        /// 是否香港信心產品
        /// </summary>
        public bool IsGS1 { get; set; }
        /// <summary>
        /// 香港信心產品圖標路徑
        /// </summary>
        public string GS1Url { get; set; }
        /// <summary>
        /// 尺寸描述
        /// </summary>
        public string PackagingInfo { get; set; }
        public DateTime? CreateDate { get; set; }

        public string CreateDateString=> CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
        public DateTime? UpdateDate { get; set; }

        public string PromotionRuleTitle { get; set; }

        /// <summary>
        /// 是否限量產品
        /// </summary>
        public bool IsLimit { get; set; }

        /// <summary>
        /// 是否支持退貨
        /// </summary>
        public bool IsSalesReturn { get; set; }

        /// <summary>
        /// 視頻鏈接
        /// </summary>
        public string VideoLink { get; set; }

        public string YoukuLink { get; set; }

        /// <summary>
        /// 銷售限額
        /// </summary>
        public int SalesQuota { get; set; }

        /// <summary>
        /// 是否在發售時間
        /// </summary>
        public bool IsSelling
        {
            get
            {
                if (SaleTime == null)
                {
                    return true;
                }
                else
                {
                    if (DateTime.Now >= SaleTime)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            set { }
        }

        /// <summary>
        /// 發售時間
        /// </summary>
        public DateTime? SaleTime { get; set; }

        public string ImgPath { get; set; }

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

    }
}
