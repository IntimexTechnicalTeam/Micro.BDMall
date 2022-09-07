namespace BDMall.Repository
{
    public interface ICityRepository : IDependency
    {
        List<CityDto> GetListByProvince(int provinceId);
    }
}
