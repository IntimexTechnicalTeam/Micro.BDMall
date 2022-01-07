using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class CountryRepository : PublicBaseRepository, ICountryRepository
    {
        public CountryRepository(IServiceProvider service) : base(service)
        {
        }

        public List<KeyValue> GetList(Language lang)
        {         
            var lists = baseRepository.GetList<Country>(x => !x.IsDeleted).ToList();
            List<KeyValue> list = lists.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = NameUtil.GetCountryName(lang.ToString(), d),
            }).ToList();
            return list;
        }
    }
}
