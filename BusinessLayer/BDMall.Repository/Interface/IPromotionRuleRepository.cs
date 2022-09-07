namespace BDMall.Repository
{
    public interface IPromotionRuleRepository:IDependency
    {
        PromotionRuleView GetProductPromotionRule(Guid merchantId, string productCode);
    }
}
