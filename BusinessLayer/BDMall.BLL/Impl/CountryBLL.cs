using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
