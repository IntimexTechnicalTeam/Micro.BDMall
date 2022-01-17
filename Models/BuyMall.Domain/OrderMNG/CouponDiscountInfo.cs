using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CouponDiscountInfo
    {
        public DiscountType DiscountType { get; set; }
        public string DiscountTypeName
        {
            get
            {
                string name = string.Empty;
                switch (DiscountType)
                {
                    case DiscountType.Coupon:
                        name = BDMall.Resources.Message.Coupon;
                        break;
                    case DiscountType.MemberGroup:
                        name = BDMall.Resources.Message.MemberGroup;
                        break;
                    case DiscountType.PromotionCode:
                        name = BDMall.Resources.Message.PromotionCode;
                        break;
                    case DiscountType.PromotionRule:
                        name = BDMall.Resources.Message.PromotionRule;
                        break;
                    case DiscountType.MallFun:
                        name = BDMall.Resources.Label.MallFun;
                        break;

                }
                return name;
            }
            set
            {

            }
        }
        public decimal DiscountVaule { get; set; }
        public CouponUsage DiscountUsage { get; set; }
        public string DiscountUsageName
        {
            get
            {
                string name = string.Empty;
                switch (DiscountUsage)
                {
                    case CouponUsage.DeliveryCharge:
                        name = BDMall.Resources.Label.CouponTypeDeliveryCharge;
                        break;
                    case CouponUsage.Price:
                        name = BDMall.Resources.Label.CouponTypeGoodsPrice;
                        break;
                }
                return name;
            }
            set
            {

            }
        }
        public string Code { get; set; }

        public decimal DiscountPrice { get; set; }
    }
}
