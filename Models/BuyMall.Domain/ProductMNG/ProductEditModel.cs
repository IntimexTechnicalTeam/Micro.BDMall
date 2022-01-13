using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class ProductEditModel
    {

        /// <summary>
        /// 新产品ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 原产品ID
        /// </summary>
        public Guid OriginalId { get; set; }

        public Guid MerchantId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string MerchantSupplierId { get; set; } = string.Empty;

        /// <summary>
        /// 产品名称
        /// </summary>
        public List<MutiLanguage> ProductNames { get; set; } = new List<MutiLanguage>();

        public Guid NameTransId { get; set; }
        /// <summary>
        /// 页面标题
        /// </summary>
        public List<MutiLanguage> PageTitles { get; set; }
        public Guid TitleTransId { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public List<MutiLanguage> SeoKeywords { get; set; } = new List<MutiLanguage>();
        public Guid KeyWordTransId { get; set; }
        /// <summary>
        /// 关键字描述
        /// </summary>
        public List<MutiLanguage> SeoDescs { get; set; } = new List<MutiLanguage>();
        public Guid SeoDescTransId { get; set; }
        /// <summary>
        /// 产品简介
        /// </summary>
        public List<MutiLanguage> ProductBriefs { get; set; } = new List<MutiLanguage>();
        public Guid IntroductionTransId { get; set; }
        /// <summary>
        /// 产品详细信息
        /// </summary>
        public List<MutiLanguage> ProductDetail { get; set; } = new List<MutiLanguage>();
        public Guid DetailTransId { get; set; }

        /// <summary>
        /// 所在目錄樹結構
        /// </summary>

        public List<ProdCatatogInfo> CatTreeNodes { get; set; } = new List<ProdCatatogInfo>();

        ///// <summary>
        ///// 貨幣
        ///// </summary>
        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 貨幣Code
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;



        /// <summary>
        /// 網上銷售價
        /// </summary>

        public decimal SalePrice { get; set; }

        /// <summary>
        /// 原價
        /// </summary>
        public decimal OriginalPrice { get; set; } = 0;

        public decimal MarkupPrice { get; set; } = 0;


        public ProductStatus Status { get; set; } = ProductStatus.Editing;

        public Guid Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryPath { get; set; } = string.Empty;

        public Guid DefaultImage { get; set; } = Guid.Empty;

        /// <summary>
        /// 產品圖片
        /// </summary>
        public List<string> Images { get; set; } = new List<string>();

        /// <summary>
        /// 附加圖片
        /// </summary> 
        public List<string[]> AdditionalImages { get; set; } 


        /// <summary>
        /// 产品库存属性列表
        /// </summary>  
        public List<AttributeObjectView> InveAttrList { get; set; } = new List<AttributeObjectView>();
        //ProductAttribute
        /// <summary>
        /// 产品非库存属性列表
        /// </summary>
        public List<AttributeObjectView> NonInveAttrList { get; set; } = new List<AttributeObjectView>();


        /// <summary>
        /// 产品规格信息
        /// </summary>
        public ProductSpecificationView Specification { get; set; } = new ProductSpecificationView();

        /// <summary>
        /// 产品附加信息
        /// </summary>
        public ProductExtensionView SpecifExtension { get; set; } = new ProductExtensionView();

        /// <summary>
        /// 產品佣金計算配置
        /// </summary>
        public ProductCommissionView CommissionConfig { get; set; } = new ProductCommissionView();

        public DateTime? ActiveTimeFrom { get; set; }
        /// <summary>
        /// 产品有效开始时间
        /// </summary>

        public DateTime? ActiveTimeTo { get; set; }

        /// <summary>
        /// 發售時間
        /// </summary>
        public DateTime? SaleTime { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int VisitCounter { get; set; } = 0;

        /// <summary>
        /// 购买次数
        /// </summary>
        public int PurchaseCounter { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } =string.Empty;

        public bool? IsActive { get; set; } = false;
        /// <summary>
        /// 是否审批
        /// </summary>
        public bool IsApprove { get; set; } = false;

        /// <summary>
        /// 是否有库存
        /// </summary>
        public bool IsExistInvRec { get; set; } = false;

        /// <summary>
        /// 操作(Add、Modify、NewVer)
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 拒送国家列表
        /// </summary>
        public List<int> CountryIds { get; set; } = new List<int>();

        /// <summary>
        /// GS1狀態
        /// </summary>
        public GS1Status GS1Status { get; set; } = GS1Status.NONGS1;

        public decimal InternalPrice { get; set; }


        public string GS1StatusString
        {
            get
            {
                var result = "";
                switch (GS1Status)
                {
                    case GS1Status.ACTIVE:
                        result = GS1Status.ACTIVE.ToString();
                        break;
                    case GS1Status.INACTIVE:
                        result = GS1Status.INACTIVE.ToString();
                        break;
                    case GS1Status.NONGS1:
                        result = "";
                        break;
                    default:
                        break;
                }
                return result;
            }
            set { }
        }

        /// <summary>
        /// 時段價格
        /// </summary>

        public decimal TimePrice { get; set; } = 0;

        /// <summary>
        /// 時段價格
        /// </summary>

        public bool IsPriceRemark { get; set; }

        /// <summary>
        /// 优惠时段是否生效
        /// </summary>

        public bool IsTimeStatus => TimePrice != SalePrice;

        public virtual void Validate()
        {
            if (ToolUtil.CheckHasHTMLTag(this))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            //检查多语言值中是否有HTML标签
            if (ToolUtil.CheckMultLangListHasHTMLTag(ProductNames.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(PageTitles.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(SeoKeywords.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ProductBriefs.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");
        }
    }
}
