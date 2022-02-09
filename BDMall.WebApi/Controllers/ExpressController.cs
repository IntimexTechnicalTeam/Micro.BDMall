using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressController : BaseApiController
    {
        public IDeliveryBLL deliveryBLL;

        public ExpressController(IComponentContext services) : base(services)
        {
            deliveryBLL = Services.Resolve<IDeliveryBLL>();
        }

        /// <summary>
        /// 获取快递数据
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetDeliveryActiveInfo")]
        [ProducesResponseType(typeof(SystemResult<DeliveryTypeActiveView>), 200)]
        public SystemResult<DeliveryTypeActiveView> GetDeliveryActiveInfo(Guid merchantId)
        {
            var sysRslt = new SystemResult<DeliveryTypeActiveView>();
            var data = deliveryBLL.GetDeliveryActiveInfo(merchantId);

            sysRslt.ReturnValue = data;
            sysRslt.Succeeded = true;
            return sysRslt;
        }


    }
}
