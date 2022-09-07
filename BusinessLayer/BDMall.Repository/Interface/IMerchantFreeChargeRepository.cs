namespace BDMall.Repository
{
    public interface IMerchantFreeChargeRepository : IDependency
    {
        List<MerchantFreeCharge> GetByMerchantId(Guid id);
        MerchantFreeChargeView GetMerchantFreeChargeInfo(Guid id, List<string> shipCodes);

        List<MerchantFreeCharge> GetByCode(string code);
    }
}
