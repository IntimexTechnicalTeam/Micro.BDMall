

namespace BDMall.Repository
{
    public class CountryRepository : PublicBaseRepository, ICountryRepository
    {
        public CountryRepository(IServiceProvider service) : base(service)
        {
        }

        public List<KeyValue> GetList(Language lang)
        {         
            var dbList = baseRepository.GetList<Country>(x => !x.IsDeleted).ToList();
            var list = AutoMapperExt.MapTo < List<CountryDto>>(dbList);

            var data = list.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = NameUtil.GetCountryName(lang.ToString(), d),
            }).ToList();
            return data;
        }
    }
}
