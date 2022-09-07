namespace BDMall.BLL
{
    public class CountryBLL : BaseBLL, ICountryBLL
    {
        public ICountryRepository countryRepository;

        public CountryBLL(IServiceProvider services) : base(services)
        {
            countryRepository= Services.Resolve<ICountryRepository>();
        }

        public List<KeyValue> GetCountry()
        {
            var countries = countryRepository.GetList(CurrentUser.Lang);
            return countries;
        }
    }
}
