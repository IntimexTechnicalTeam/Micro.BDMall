namespace BDMall.Repository
{
    public interface IProvinceRepository : IDependency
    {
        List<ProvinceDto> GetListByCountry(int countryId);
        ProvinceDto GetById(int id);
    }
}
