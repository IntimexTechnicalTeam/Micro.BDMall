namespace BDMall.Repository
{
    public class CityRepository : PublicBaseRepository, ICityRepository
    {
        public CityRepository(IServiceProvider service) : base(service)
        {

        }

        public List<CityDto> GetListByProvince(int provinceId)
        {
            var dbList = baseRepository.GetList<City>().Where(d => d.ProvinceId == provinceId && d.IsDeleted == false).ToList();

            var dtos = AutoMapperExt.MapToList<City, CityDto>(dbList);

            return dtos;

        }


    }
}
