using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public class MemberController : BaseApiController
    {
        IMemberBLL memberBll;

        public MemberController(IComponentContext service) : base(service)
        {
            memberBll = this.Services.Resolve<IMemberBLL>();
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult Register([FromBody]RegisterMember member)
        {           
            var result = memberBll.Register(member);           
            return result;
        }

        /// <summary>
        /// 添加商家到收藏列表
        /// </summary>
        /// <param name="merchCode">商家編號</param>
        [HttpGet("AddFavMerchant")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddFavMerchant(string merchCode)
        {
            var result = await memberBll.AddFavMerchant(merchCode);
            return result;
        }

        /// <summary>
        /// 從收藏列表移除商家
        /// </summary>
        /// <param name="merchCode">商家編號</param>
        [HttpGet("RemoveFavMerchant")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveFavMerchant(string merchCode)
        {
            var result = await memberBll.RemoveFavMerchant(merchCode);
            return result;
        }

        /// <summary>
        /// 添加产品到收藏列表
        /// </summary>
        /// <param name="productId">產品ID</param>
        [HttpGet("AddFavProduct")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddFavProduct(Guid productId)
        {
            var result = await memberBll.AddFavProduct(productId);
            return result;
        }

        /// <summary>
        /// 從收藏列表移除产品
        /// </summary>
        /// <param name="productId">產品ID</param>
        [HttpGet("RemoveFavProduct")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveFavProduct(Guid productId)
        {
            var result = await memberBll.RemoveFavProduct(productId);
            return result;
        }

        /// <summary>
        /// 我收藏的商家列表
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("MyFavMerchant")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroMerchant>>), 200)]
        public async Task<SystemResult<PageData<MicroMerchant>>> MyFavMerchant([FromBody] FavoriteCond cond)
        {
            var result = new SystemResult<PageData<MicroMerchant>>() { Succeeded = true };
            result.ReturnValue = await memberBll.MyFavMerchant(cond);
            return result;
        }

        /// <summary>
        /// 我收藏的商品列表
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("MyFavProduct")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroProduct>>), 200)]
        public async Task<SystemResult<PageData<MicroProduct>>> MyFavProduct([FromBody] FavoriteCond cond)
        {
            var result = new SystemResult<PageData<MicroProduct>>() { Succeeded = true };
            result.ReturnValue = await memberBll.MyFavProduct(cond);
            return result;
        }

        /// <summary>
        /// 会员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMemberInfo")]
        [ProducesResponseType(typeof(SystemResult<CurrentUser<MemberUser>>), 200)]
        public async Task<SystemResult<CurrentUser<MemberUser>>> GetMemberInfo()
        {
            var result = new SystemResult<CurrentUser<MemberUser>> { Succeeded = true };
            result.ReturnValue  = await memberBll.GetMemberInfo();
            return result;
        }

        /// <summary>
        /// 我的足迹
        /// </summary>
        /// <returns></returns>
        [HttpPost("MyProductTrack")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroProduct>>), 200)]
        public async Task<SystemResult<PageData<MicroProduct>>> MyProductTrack(TrackCond cond)
        {
            var result = new SystemResult<PageData<MicroProduct>> { Succeeded = true };
            result.ReturnValue = await memberBll.MyProductTrack(cond);
            return result;
        }
    }
}
