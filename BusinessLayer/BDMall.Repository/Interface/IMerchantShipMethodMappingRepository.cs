namespace BDMall.Repository
{
    public interface IMerchantShipMethodMappingRepository:IDependency
    {
        List<MerchantActiveShipMethodDto> GetShipMethidByMerchantId(Guid merchantId);
    }
}
