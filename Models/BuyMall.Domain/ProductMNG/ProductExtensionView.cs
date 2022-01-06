using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductExtensionView
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProdID { get; set; } =Guid.Empty;
        /// <summary>
        /// 產品瀏覽層次
        /// </summary>
        public int PermissionLevel { get; set; } = 1;

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnSale { get; set; } = false;


        /// <summary>
        /// 是否售罄
        /// </summary>
        public bool IsSaleOff { get; set; } = false;

        public int MinPurQty { get; set; } = 1;

        public int MaxPurQty { get; set; } = 0;

        public int SafetyStock { get; set; } = 1;
        public int SalesQuota { get; set; } = 0;

        public bool IsLimit { get; set; } = false;

        public bool NoRefund { get; set; } = false;

        public ProductType ProductType { get; set; } = ProductType.Basic;
        public string YoutubeLink { get; set; }
        public string YoukuLink { get; set; }

        public string Gtin { get; set; }

        public string HSCode { get; set; }

        public GS1Status GS1Status { get; set; }
    }
}
