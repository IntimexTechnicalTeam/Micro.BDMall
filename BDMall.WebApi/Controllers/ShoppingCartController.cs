using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : BaseApiController
    {
        public IShoppingCartBLL shoppingCartBLL;
 
        public ShoppingCartController(IComponentContext services) : base(services)
        {
            shoppingCartBLL = Services.Resolve<IShoppingCartBLL>();
        }

        /// <summary>
        /// 获取购物车
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetShopCart")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> GetShopCart()
        {
            SystemResult result = new SystemResult { Succeeded = true };
            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded)return result;

            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            //读缓存
            var cacheData = await RedisHelper.GetAsync<ShopCartInfo>(key);
            if (cacheData == null)
            {
                var aaa = shoppingCartBLL.GetShoppingCartItem(); 
            }
            result.ReturnValue = cacheData;
            return result;
        }

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("AddItem")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddItem([FromBody] CartItem item)
        {
            SystemResult result = new SystemResult { Succeeded = true };
            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded)
            {
                result.ReturnValue = OrderErrorEnum.NeedLogin;
                result.Message = "";
                return result;
            }

            result = await shoppingCartBLL.AddtoCartAsync(item);
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        /// <summary>
        /// 更新购物车项目数量
        /// </summary>
        /// <param name="id">购物车项目Id</param>
        /// <param name="qty">更新后的数量</param>
        /// <returns></returns>
        [HttpGet("UpdateItemQty")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> UpdateItemQty(Guid id, int qty)
        {
            SystemResult result = new SystemResult();

            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded)
            {
                result.ReturnValue = OrderErrorEnum.NeedLogin;
                result.Message = "";
                return result;
            }

           result = await shoppingCartBLL.UpdateCartItemAsync(id, qty);

            //直接删除
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        /// <summary>
        /// 删除购物车项目
        /// </summary>
        /// <param name="id">购物车项目Id</param>
        /// <returns></returns>
        [HttpGet("RemoveItem")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveItem(Guid id)
        {
            SystemResult result = new SystemResult();

            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded)
            {
                result.ReturnValue = OrderErrorEnum.NeedLogin;
                result.Message = "";
                return result;
            }

            result = await shoppingCartBLL.RemoveFromCart(id);

            //直接删除
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }



    }
}
