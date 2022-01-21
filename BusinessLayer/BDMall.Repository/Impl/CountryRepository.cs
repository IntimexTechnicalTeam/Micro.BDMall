using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

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
