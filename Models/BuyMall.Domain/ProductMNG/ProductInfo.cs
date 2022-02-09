using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductInfo
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public Guid CatalogId { get; set; }

        /// <summary>
        /// 所在目錄樹結構
        /// </summary>

        public List<ProdCatatogInfo> CatTreeNodes { get; set; }

        /// <summary>
        /// 貨幣
        /// </summary>
        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }

        /// <summary>
        /// 網上銷售價
        /// </summary>

        public decimal SalePrice { get; set; }

        /// <summary>
        /// 最小購買量
        /// </summary>
        public int MinPurQty { get; set; }

        /// <summary>
        /// 最大購買量
        /// </summary>
        public int SaleQuota { get; set; }

        public decimal SalePrice2 { get; set; }

        /// <summary>
        /// 原價
        /// </summary>
        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrice2 { get; set; }

        public decimal MarkupPrice { get; set; }
        public decimal MarkupPrice2 { get; set; }

        /// <summary>
        /// 產品圖片
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// 附加圖片
        /// </summary> 
        public List<List<string>> AdditionalImages { get; set; }

        /// <summary>
        /// 产品属性列表
        /// </summary>        
        public List<ProdAtt> AttrList { get; set; }

        /// <summary>
        /// 产品详细信息，描述
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 产品规格信息
        /// </summary>
        public string PackagingInfo { get; set; }

        public int VisitCounter { get; set; }

        public decimal GrossWeight { get; set; }

        public bool IsDeleted { get; set; }

        public Guid MerchantId { get; set; }

        public decimal Score { get; set; }

        public string WeightUnit { get; set; }

        public string YoutubeUrl { get; set; }

        public string YoukuUrl { get; set; }

        public ProductType? ProductType { get; set; }

        /// <summary>
        /// 是否香港信心產品
        /// </summary>
        public bool IsGS1 { get; set; }

        public string GS1Link { get; set; }

        /// <summary>
        /// 是否限量产品
        /// </summary>
        public bool IsLimit { get; set; }

        /// <summary>
        /// 是否可以退货
        /// </summary>
        public bool IsSalesReturn { get; set; }

        /// <summary>
        /// 原merchant的MerchNo
        /// </summary>
        public string SupplierId { get; set; }

        public string PromotionRuleTitle { get; set; }

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

        /// <summary>
        /// 所屬商店資料
        /// </summary>
        public MerchantSummary MerchantInfo { get; set; }

        /// <summary>
        /// 拒送国家列表
        /// </summary>
        public List<string> RefuseCountry { get; set; }

        /// <summary>
        /// 可配送國家列表
        /// </summary>
        public List<string> SupportCountry { get; set; }

        /// <summary>
        /// 網上銷售價(時間段價格)
        /// </summary>

        public decimal TimePrice { get; set; }

        public decimal TimePrice2 { get; set; }

        /// <summary>
        /// 產品驗證圖標
        /// </summary>
        public List<string> ProductIcons { get; set; }
    }
}
