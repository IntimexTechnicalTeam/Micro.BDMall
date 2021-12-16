
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class PreHeatProductCatalogService : AbstractPreHeatService
    {
        public PreHeatProductCatalogService(IServiceProvider services) : base(services)
        {
        }

        public override Task<SystemResult> CreatePreHeat(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ProductCatalogDto>> GetDataSourceAsync(Guid Id)
        {
            var query = from pc in baseRepository.GetList<ProductCatalog>(x=>x.IsActive && !x.IsDeleted)
                               join t in baseRepository.GetList<Translation>(x => x.IsActive && !x.IsDeleted) on pc.NameTransId equals t.TransId into pct
                               from v in pct.DefaultIfEmpty()
                               select new ProductCatalogDto
                               { 
                               
                               };

            if (Id != Guid.Empty)
                query = query.Where(x=>x.Id == Id);

            return query;
        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<ProductCatalogDto> list, Language language)
        {
            var result = new SystemResult() { Succeeded = true };
            
            return result;
        }
    }
}
