
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
    public  class PreHeatFavoriteService : AbstractPreHeatService
    {
        public PreHeatFavoriteService(IServiceProvider services) : base(services)
        {
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">Member.Id</param>       
        /// <returns></returns>
        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {            
            var result = new SystemResult() { Succeeded = true };
            string key = PreHotType.Favorite.ToString();
            Favorite favorite = await GetDataSourceAsync(Id);            
            await SetDataToHashCache(Id, favorite);
            return result;
        }

        public async Task<Favorite> GetDataSourceAsync(Guid Id)
        {
            Favorite favorite = new Favorite();

            var productFav = baseRepository.GetList<MemberFavorite>(x => x.IsActive && !x.IsDeleted && x.MemberId == Id).ToList();
            var mchFav = baseRepository.GetList<MerchantFavorite>(x => x.IsActive && !x.IsDeleted && x.MemberId == Id).ToList();

            favorite.MchList = mchFav?.Select(s => s.MerchId).ToList();
            favorite.ProductList = productFav?.Select(s => s.ProductCode).ToList();

            return favorite;
        }

        public async Task<SystemResult> SetDataToHashCache(Guid Id,Favorite favorite)
        {
            var result = new SystemResult() { Succeeded = true };
            string key = PreHotType.Favorite.ToString();
            await RedisHelper.HSetAsync(key, Id.ToString(), favorite);
            return result;
        }
    }
}
