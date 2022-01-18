using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Domain
{
    public class ShoppingCartItemView
    {
        /// <summary>
        /// shoppingCart Item Id
        /// </summary>
        public Guid Id { get; set; }

        public Guid SkuId { get; set; }
        /// <summary>
        /// 屬性值1Id
        /// </summary>
        public Guid AttrValue1 { get; set; }
        /// <summary>
        /// 屬性值2Id
        /// </summary>
        public Guid AttrValue2 { get; set; }
        /// <summary>
        /// 屬性值3Id
        /// </summary>
        public Guid AttrValue3 { get; set; }

        /// <summary>
        /// 屬性值1名稱Id
        /// </summary>
        public Guid AttrValueName1 { get; set; }
        /// <summary>
        /// 屬性值2名稱Id
        /// </summary>
        public Guid AttrValueName2 { get; set; }
        /// <summary>
        /// 屬性值3名稱Id
        /// </summary>
        public Guid AttrValueName3 { get; set; }

        /// <summary>
        /// 屬性1Id
        /// </summary>
        public Guid AttrId1 { get; set; }
        /// <summary>
        /// 屬性2Id
        /// </summary>
        public Guid AttrId2 { get; set; }
        /// <summary>
        /// 屬性3Id
        /// </summary>
        public Guid AttrId3 { get; set; }

        /// <summary>
        /// 屬性1名稱
        /// </summary>
        public Guid AttrName1 { get; set; }
        /// <summary>
        /// 屬性2名稱
        /// </summary>
        public Guid AttrName2 { get; set; }
        /// <summary>
        /// 屬性3名稱
        /// </summary>
        public Guid AttrName3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Qty { get; set; }

        public MerchantView Merchant { get; set; }

        public ProductSummary Product { get; set; }

        public WeightUnit WeightUnit { get; set; }

        public decimal GrossWeight { get; set; }

        public int SaleQuota { get; set; }

       
        /// <summary>
        /// 屬性值1的附加價錢
        /// </summary>
        public decimal AttrValue1AddPrice { get; set; }
        /// <summary>
        /// 屬性值2的附加價錢
        /// </summary>
        public decimal AttrValue2AddPrice { get; set; }
        /// <summary>
        /// 屬性值3的附加價錢
        /// </summary>
        public decimal AttrValue3AddPrice { get; set; }

    }

}
