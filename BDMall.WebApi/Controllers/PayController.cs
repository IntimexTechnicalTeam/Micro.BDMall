using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : BaseApiController
    {
        public IPaymentBLL paymentBLL;
        public IOrderBLL orderBLL;

        public PayController(IComponentContext services) : base(services)
        {
            paymentBLL = Services.Resolve<IPaymentBLL>();
            orderBLL = Services.Resolve<IOrderBLL>();
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult<List<PaymentMethodView>>), 200)]
        [HttpGet("GetPaymentMethod")]
        public SystemResult<List<PaymentMethodView>> GetPaymentMethod()
        {
            var result = new SystemResult<List<PaymentMethodView>>();
            result.ReturnValue = paymentBLL.GetPaymentTypes();            
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 更新訂單的支付狀態
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(SystemResult), 200)]
        [HttpGet("UpdateOrderPaymentStatus")]
        public async Task<SystemResult> UpdateOrderPaymentStatus(Guid orderId, string type)
        {
            SystemResult result = new SystemResult();

            if (type == "S")    //支付成功
            {
                await orderBLL.UpdateOrderPayStatus(orderId);
                result.Succeeded = true;
            }
            else                    //支付失败
            {
                await orderBLL.UpdateOrderCancelStatus(orderId);
                result.Succeeded = true;
            }
            return result;

        }

    }
}
