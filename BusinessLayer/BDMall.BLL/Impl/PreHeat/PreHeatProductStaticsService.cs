
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
    public class PreHeatProductStaticsService : AbstractPreHeatService
    {
        public PreHeatProductStaticsService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">Product.Id</param>
        /// <returns></returns>
        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {
            var result = new SystemResult();

            var product = await baseRepository.GetModelAsync<Product>(x => x.Id == Id && x.IsActive && !x.IsDeleted);

            var query =await GetDataSourceAsync(Guid.Empty,product.Code);               
            await SetDataToHashCache(query);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">没有用的参数</param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<IQueryable<HotPreProductStatistics>> GetDataSourceAsync(Guid Id,string Code)
        {
            var list = await baseRepository.GetListAsync<ProductStatistics>();
            var query = list.Select(s => new HotPreProductStatistics
            {
                Code = s.Code,
                PurchaseCounter = s.PurchaseCounter,
                Score = s.Score,
                SearchCounter = s.SearchCounter,
                VisitCounter = s.VisitCounter
            }).AsQueryable();

            if (!string.IsNullOrEmpty(Code))
                query = query.Where(x => x.Code == Code);

            return query;
        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<HotPreProductStatistics> list)
        {
            var result = new SystemResult();
            string key = $"{PreHotType.Hot_PreProductStatistics}";

            var hotList = list.ToList();
            if (hotList != null && hotList.Any())
            {
                //await CacheClient.RedisHelper.DelAsync(key);
                foreach (var item in hotList)
                {
                    await RedisHelper.HSetAsync(key, item.Code, item);
                }
                result.Succeeded = true;
            }
            return result;
        }
    }
}
