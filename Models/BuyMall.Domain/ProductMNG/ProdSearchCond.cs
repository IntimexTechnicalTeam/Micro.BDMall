using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class ProdSearchCond  
    {
        public PageInfo PageInfo { get; set; }

        public Language Language { get; set; }

        public Guid MerchantId { get; set; } = Guid.Empty;
        public string Key { get; set; }

        public string ProductCode { get; set; }
        public string KeyWordType { get; set; }
        public Guid Category { get; set; } = Guid.Empty;

        public Guid Attribute { get; set; } = Guid.Empty;
        public Guid AttributeValue { get; set; } = Guid.Empty;

        public bool OnSale { get; set; }
        public bool SaleOff { get; set; }

        public int PermissionLevel { get; set; }

        /// <summary>
        /// 是否權限範圍，如果true，則取範圍内的產品
        /// </summary>
        public bool IsPermissionRange { get; set; }
        public string SortedBy { get; set; }

        public string Sorted { get; set; }

        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }

        public int IsActive { get; set; }

        public int IsDeleted { get; set; }
        public int IsApprove { get; set; }

        public string ApproveStatus { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProductSearchType ProductSearchType { get; set; }

        /// <summary>
        /// 查询数据集的类型，0-Or模式、1-And模式
        /// </summary>
        //public int Type { get; set; }

        /// <summary>
        /// 目錄與屬性之間的類型,0-or模式,1-and模式
        /// </summary>
        //public int CatType { get; set; }
        public List<int> CatIds { get; set; }
        public List<AttrValues> Attrs { get; set; }

        /// <summary>
        /// 价格范围
        /// </summary>
        public List<int> Prices { get; set; }
    }
}
