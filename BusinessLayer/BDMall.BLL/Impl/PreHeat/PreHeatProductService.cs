
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    /// <summary>
    /// 产品预热
    /// </summary>
    public class PreHeatProductService : AbstractPreHeatService
    {
        public PreHeatProductService(IServiceProvider services) : base(services)
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

            var query = await GetDataSourceAsync(Id);
            await SetDataToHashCache(query, Language.C);
            await SetDataToHashCache(query, Language.E);           
            await SetDataToHashCache(query, Language.S);

           
            return result;
        }

        public async Task<IQueryable<HotProduct>> GetDataSourceAsync(Guid Id,string Code="")
        {
            var query = from p in baseRepository.GetList<Product>()
                        join t in baseRepository.GetList<Translation>() on p.NameTransId equals t.TransId into pt
                        from vv in pt.DefaultIfEmpty()
                        where p.IsActive && !p.IsDeleted && p.Status == ProductStatus.OnSale
                        select new HotProduct
                        {
                            ProductId = p.Id,
                            DefaultImageId = p.DefaultImage,
                            Name = vv.Value,
                            MchId = p.MerchantId,
                            CurrencyCode = p.CurrencyCode,
                            InternalPrice = p.InternalPrice,
                            MarkUpPrice = p.MarkUpPrice,
                            OriginalPrice = p.OriginalPrice,
                            SalePrice = p.SalePrice,
                            LangType = vv.Lang,
                            Status = p.Status,
                            ProductCode = p.Code,
                            UpdateDate = p.UpdateDate ?? new DateTime(1970, 01, 01),
                            CreateDate = p.CreateDate,
                            CatalogId = p.CatalogId
                        };

            if (Id != Guid.Empty)
                query = query.Where(x => x.ProductId == Id);

            if (!string.IsNullOrEmpty(Code))
                query = query.Where(x => x.Code == Code);

            return query;
        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<HotProduct> list,Language language)
        {
            var result = new SystemResult() { Succeeded = true };
            string key = $"{PreHotType.Hot_Products}_{language}";

            var hotList = list.Where(x => x.LangType == language).ToList();
            if (hotList != null && hotList.Any())
            {
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