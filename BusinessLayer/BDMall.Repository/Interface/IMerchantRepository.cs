namespace BDMall.Repository
{
    public interface IMerchantRepository:IDependency
    {
        PageData<MerchantView> SearchMerchByCond(MerchantPageInfo condition);
    }
}
