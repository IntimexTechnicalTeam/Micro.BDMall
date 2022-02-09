using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : BaseApiController
    {
        public ICouponBLL couponBLL;
        public CouponController(IComponentContext services) : base(services)
        {
            couponBLL = Services.Resolve<ICouponBLL>();
        }

        /// <summary>
        /// 检查优惠卷
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckHasGroupOrRuleDiscount")]
        [ProducesResponseType(typeof(SystemResult<DiscountInfo>), 200)]
        public SystemResult<DiscountInfo> CheckHasGroupOrRuleDiscount()
        {
            var result = new SystemResult<DiscountInfo>();
            result.ReturnValue = couponBLL.CheckHasGroupOrRuleDiscount();
            result.Succeeded = true;
            return result;
        }
    }
}
