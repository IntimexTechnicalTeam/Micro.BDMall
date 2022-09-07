namespace BDMall.Repository
{
    public interface ICouponRepository:IDependency
    {
        DiscountInfo GetCouponById(Guid id);

        DiscountGroup GetVaildCouponGroup(VaildCouponCond cond);

        PageData<CouponInfo> GetMemberCoupons(CouponPager cond);
    }
}
