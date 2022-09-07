namespace BDMall.BLL
{
    public interface ICouponBLL:IDependency
    {
        DiscountGroup GetCouponGroup(VaildCouponCond cond);

        DiscountInfo CheckHasGroupOrRuleDiscount();

        DiscountInfo CheckHasGroupOrRuleDiscount(decimal prodAmount);

        DiscountInfo GetPromotionCodeCoupon(Guid merchantId, string code, decimal price);

        PrmtCodeDiscountInfo GetPromotionCodeCouponV2(PromotionCodeCond cond);

        PageData<CouponInfo> GetMemberCoupon(CouponPager cond);
    }
}
