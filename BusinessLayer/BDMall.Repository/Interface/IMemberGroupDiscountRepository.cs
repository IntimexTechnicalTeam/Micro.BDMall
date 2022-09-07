namespace BDMall.Repository
{
    public interface IMemberGroupDiscountRepository:IDependency
    {
        PageData<MarketingDiscount> SearchDiscountHistory(MemberGroupDiscountCond cond);

        DiscountInfo CheckHasMemberGroupDiscount();
    }
}
