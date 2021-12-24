using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IBasePreHeatService : IDependency
    {
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<SystemResult> CreatePreHeat(Guid Id);

        Task UpdatePreHeat(string key, string field, object value);

        Task DeletePreHeat(string key, string field);

        //Task DeletePromotionMerchant(string key, Guid MchId);

        //Task UpdatePromotionMerchant(string key, Guid MchId);

        //Task UpdatePromotionProduct(string key, string Code);

        //Task UpdatePromotionProductByMchId(string key, Guid MchId);

        ///// <summary>
        ///// 下架商品并更新Product缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="Code">ProductCode</param>
        ///// <returns></returns>
        //Task UpdateProductWhenOffSale(string key, string Code);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="Code">ProductCode</param>
        ///// <returns></returns>
        //Task DeletePromotionProduct(string key, string Code);

        //Task DeletePromotionProductByMchId(string key, Guid MchId);
    }
}

