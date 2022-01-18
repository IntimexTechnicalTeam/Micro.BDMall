using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class DiscountView
    {
        public Guid Id { get; set; }

        public bool IsPercent { get; set; }

        public decimal DiscountValue { get; set; }

        public decimal DiscountPrice { get; set; }

        public DiscountType DiscountType { get; set; }

        public CouponUsage CouponType { get; set; }

        public string Code { get; set; }

        public Guid ProductId { get; set; }

        public string DiscountTypeName
        {
            get
            {
                var result = "";
                switch (DiscountType)
                {
                    case DiscountType.Coupon:
                        switch (CouponType)
                        {
                            case CouponUsage.Price:
                                result = Resources.Label.CashCoupon;
                                break;
                            case CouponUsage.DeliveryCharge:
                                result = Resources.Label.DeliveryChargeCoupon;
                                break;
                        }
                        break;
                    case DiscountType.PromotionCode:
                        result = Resources.Label.PromotionCode;
                        break;
                    case DiscountType.PromotionRule:
                        result = Resources.Label.PromotionRule;
                        break;
                    case DiscountType.MemberGroup:
                        result = Resources.Label.MemberGroupDiscount;
                        break;
                    case DiscountType.MallFun:
                        result = "MallFun";
                        break;
                    case DiscountType.FreeChargeProduct:
                        result = Resources.Label.FreeChargeProduct;
                        break;

                }
                return result;
            }
            set { }
        }
    }
}

