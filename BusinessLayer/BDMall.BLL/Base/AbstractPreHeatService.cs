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
    public abstract class AbstractPreHeatService :BaseBLL, IBasePreHeatService
    {
        protected AbstractPreHeatService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public abstract Task<SystemResult> CreatePreHeat(Guid Id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual async Task UpdatePreHeat(string key, string field, object value)
        {
            await RedisHelper.HDelAsync(key, field);
            await RedisHelper.HSetNxAsync(key, field, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual async Task DeletePreHeat(string key, string field)
        {

            await RedisHelper.HDelAsync(key, field);
        }

        ///// <summary>
        ///// 下架商品并更新Product缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="Code"></param>
        ///// <returns></returns>
        public virtual async Task UpdateProductWhenOffSale(string key, string Code)
        {
            key = $"{PreHotType.Hot_Products}_{Language.C}";
            var product = await RedisHelper.HGetAsync<HotProduct>(key, Code);
            if (product != null)
            {
                product.Status = ProductStatus.ManualOffSale;
                product.UpdateDate = DateTime.Now;
                await RedisHelper.HSetAsync(key, Code, product);
            }

            key = $"{PreHotType.Hot_Products}_{Language.E}";
            product = await RedisHelper.HGetAsync<HotProduct>(key, Code);
            if (product != null)
            {
                product.Status = ProductStatus.ManualOffSale;
                product.UpdateDate = DateTime.Now;
                await RedisHelper.HSetAsync(key, Code, product);
            }

            key = $"{PreHotType.Hot_Products}_{Language.S}";
            product = await RedisHelper.HGetAsync<HotProduct>(key, Code);
            if (product != null)
            {
                product.Status = ProductStatus.ManualOffSale;
                product.UpdateDate = DateTime.Now;
                await RedisHelper.HSetAsync(key, Code, product);
            }
        }

        public virtual async Task UpdatePromotionMerchant(string key, Guid MchId)
        {
            ////根据MchId反查PromotionMerchant表
            //var promotionMerchants = baseRepository.GetList<PromotionMerchant>(x => x.IsActive && !x.IsDeleted && x.MerchantId == MchId)
            //                                            .Select(pp => new HotPromotionMerchant
            //                                            {
            //                                                Id = pp.Id,
            //                                                PrmtDtlId = pp.PrmtDtlId,
            //                                                MerchantId = pp.MerchantId,
            //                                                PrmtID = pp.PrmtID,
            //                                                Seq = pp.Seq,
            //                                            });
            //if (promotionMerchants != null && promotionMerchants.Any())
            //{
            //    await DeletePromotionMerchant(key, MchId);
            //    //重新更新这个商家的PromotionMerchant缓存
            //    foreach (var item in promotionMerchants.ToList())
            //    {
            //        await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
            //    }
            //}
        }

        /// <summary>
        /// 删除PromotionMerchant缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="MchId"></param>
        /// <returns></returns>
        public virtual async Task DeletePromotionMerchant(string key, Guid MchId)
        {
            //var cacheData = await RedisHelper.HGetAllAsync<HotPromotionMerchant>(key);
            //if (cacheData?.Values?.Any(x => x.MerchantId == MchId) ?? false)
            //{
            //    var delData = cacheData.Values.Where(x => x.MerchantId == MchId).Select(s => s.Id.ToString());
            //    await RedisHelper.HDelAsync(key, delData.ToArray());
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public virtual async Task UpdatePromotionProduct(string key, string Code)
        {
            ////根据Code反查PromotionProducts表
            //var promotionProducts = baseRepository.GetList<PromotionProduct>(x => x.IsActive && !x.IsDeleted && x.ProdCode == Code)
            //                                            .Select(pp => new HotPromotionProduct
            //                                            {
            //                                                Id = pp.Id,
            //                                                ProdCode = pp.ProdCode,
            //                                                PrmtDtlId = pp.PrmtDtlId,
            //                                                ProductId = pp.ProductId,
            //                                                PromotionId = pp.PromotionId,
            //                                                Seq = pp.Seq,
            //                                            });

            //if (promotionProducts != null && promotionProducts.Any())
            //{
            //    //删除缓存中和这个商品有关的PromotionProduct            
            //    await DeletePromotionProduct(key, Code);
            //    //重新更新这个商品有关的PromotionProduct 缓存
            //    foreach (var item in promotionProducts.ToList())
            //    {
            //        item.MchId = baseRepository.GetModel<Product>(x => x.Code == item.ProdCode)?.MerchantId ?? Guid.Empty;
            //        await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
            //    }
            //}
        }

        public virtual async Task UpdatePromotionProductByMchId(string key, Guid MchId)
        {
            ////检查商家是不是PromotionMerchant，如果是,就更新缓存
            //var flag = baseRepository.Any<PromotionMerchant>(x => x.IsActive && !x.IsDeleted && x.MerchantId == MchId);
            //if (flag)
            //{
            //    //反查找到这个商家的所有PromotionProduct
            //    var products = baseRepository.GetList<Product>(x => x.MerchantId == MchId && x.IsActive && !x.IsDeleted).Select(s => s.Code);
            //    var promotionProducts = baseRepository.GetList<PromotionProduct>(x => x.IsActive && !x.IsDeleted && products.Contains(x.ProdCode))
            //                                            .Select(pp => new HotPromotionProduct
            //                                            {
            //                                                Id = pp.Id,
            //                                                ProdCode = pp.ProdCode,
            //                                                PrmtDtlId = pp.PrmtDtlId,
            //                                                ProductId = pp.ProductId,
            //                                                PromotionId = pp.PromotionId,
            //                                                Seq = pp.Seq,
            //                                                MchId = MchId
            //                                            });

            //    if (promotionProducts != null && !promotionProducts.Any())
            //    {
            //        //删除这个商家缓存中所有PromotionProduct数据
            //        await DeletePromotionProductByMchId(key, MchId);
            //        foreach (var item in promotionProducts.ToList())
            //        {
            //            await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
            //        }
            //    }
            //}
        }


        ///// <summary>
        ///// 根据Code删除PromotionProduct缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="Code">ProductCode</param>
        ///// <returns></returns>
        public virtual async Task DeletePromotionProduct(string key, string Code)
        {
            //var cacheData = await RedisHelper.HGetAllAsync<HotPromotionProduct>(key);
            //if (cacheData?.Values?.Any(x => x.ProdCode == Code) ?? false)
            //{
            //    var delData = cacheData.Values.Where(x => x.ProdCode == Code).Select(s => s.Id.ToString());
            //    await RedisHelper.HDelAsync(key, delData.ToArray());
            //}
        }

        public virtual async Task DeletePromotionProductByMchId(string key, Guid MchId)
        {
            //var cacheData = await RedisHelper.HGetAllAsync<HotPromotionProduct>(key);
            //if (cacheData?.Values?.Any(x => x.MchId == MchId) ?? false)
            //{
            //    var delData = cacheData.Values.Where(x => x.MchId == MchId).Select(s => s.Id.ToString());
            //    await RedisHelper.HDelAsync(key, delData.ToArray());
            //}
        }

    }
}
