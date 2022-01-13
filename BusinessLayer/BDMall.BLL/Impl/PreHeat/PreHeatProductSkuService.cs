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
    public class PreHeatProductSkuService : AbstractPreHeatService
    {
        public PreHeatProductSkuService(IServiceProvider services) : base(services)
        {

        }

        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {
            var result = new SystemResult();
            var product = await baseRepository.GetModelAsync<Product>(x => x.Id == Id && x.IsActive && !x.IsDeleted);

            var query = await GetDataSourceAsync(Guid.Empty, product.Code);
            await SetDataToHashCache(query);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">没有用的参数</param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<List<ProductSku>> GetDataSourceAsync(Guid Id, string Code)
        {
            var query = await baseRepository.GetListAsync<ProductSku>(x=>x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrEmpty(Code))
                query = query.Where(x => x.ProductCode == Code);

            return query.ToList();
        }

        public async Task<SystemResult> SetDataToHashCache(List<ProductSku> list)
        {
            var result = new SystemResult();
            string key = $"{PreHotType.ProductSku}";

            var hotList = list.ToList();
            if (hotList != null && hotList.Any())
            {
                //await CacheClient.RedisHelper.DelAsync(key);
                foreach (var item in hotList)
                {
                    await RedisHelper.HSetAsync(key, item.ProductCode, item);
                }
                result.Succeeded = true;
            }
            return result;
        }
    }
}
