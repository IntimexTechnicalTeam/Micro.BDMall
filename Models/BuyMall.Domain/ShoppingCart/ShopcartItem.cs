using BDMall.Enums;
using System;
using System.Collections.Generic;


namespace BDMall.Domain
{
    public class ShopcartItem
    {
        public Guid Id { get; set; }

        public ProductSummary Product { get; set; }


        public ProductAttrValueDto AttrValue1 { get; set; }

        public ProductAttrValueDto AttrValue2 { get; set; }

        public ProductAttrValueDto AttrValue3 { get; set; }

        public Guid SkuId { get; set; }


        public ProductAttrDto Attr1 { get; set; }

        public ProductAttrDto Attr2 { get; set; }

        public ProductAttrDto Attr3 { get; set; }

        public int Qty { get; set; }

        public int FreeQty { get; set; }

        public decimal DiscountTotalPrice
        {
            get
            {
                decimal result = 0;
                //result = Product.SalePrice * (Qty - FreeQty) - GroupSaleDiscountPrice;
                result = (Product.SalePrice + AttrValue1.AdditionalPrice + AttrValue2.AdditionalPrice + AttrValue3.AdditionalPrice) * (Qty - FreeQty);// - GroupSaleDiscountPrice;
                return result;
            }
            set { }
        }
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 毛重（KG）
        /// </summary>
        public decimal GrossWeight { get; set; }

        /// <summary>
        /// Promotion Rule Price
        /// </summary>
        public decimal GroupSaleDiscountPrice { get; set; }

        /// <summary>
        /// 每件优惠的价钱
        /// </summary>
        public decimal SingleDiscountPrice { get; set; }

        ///// <summary>
        ///// 贈品
        ///// </summary>
        //public List<ShopcartItem> FreeItems { get; set; }

        /// <summary>
        /// 優惠規則Id
        /// </summary>
        public Guid RuleId { get; set; }

        ///// <summary>
        ///// 在用的推廣優惠信息
        ///// </summary>
        //public PromotionRuleView InUsePromotionRule { get; set; }

        /// <summary>
        /// 是否贈品
        /// </summary>
        public bool IsFree { get; set; }

        ///// <summary>
        ///// 規則類型
        ///// </summary>
        //public PromotionRuleType RuleType { get; set; }

        //public int SaleQuota { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        public ShoppingCartItemType CartItemType { get; set; }

        ///// <summary>
        ///// 郵品訂購服務支付類型
        ///// </summary>
        //public SOSPayType? SOSPayType { get; set; }

        /// <summary>
        /// 产品属性列表
        /// </summary>        
        public List<ProductAttrDto> AttrList { get; set; }

    }
}
