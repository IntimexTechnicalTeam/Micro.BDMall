using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        public IOrderBLL orderBLL;
        public ICodeMasterBLL codeMasterBLL;    

        public OrderController(IComponentContext services) : base(services)
        {
            orderBLL = Services.Resolve<IOrderBLL>();
            codeMasterBLL = Services.Resolve<ICodeMasterBLL>(); 
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Create([FromBody] NewOrder checkout)
        { 
            var result = await orderBLL.BuildOrder(checkout);
            return result;
        }

        /// <summary>
        /// 会员订单数据
        /// </summary>
        /// <param name="orderCond"></param>
        /// <returns></returns>
        [HttpPost("MyOrder")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroOrderView>>), 200)]
        public async Task<SystemResult<PageData<MicroOrderView>>> MyOrder([FromBody] MicroOrderCond orderCond)
        {
            var result = new SystemResult<PageData<MicroOrderView>>() { Succeeded = true };
            result.ReturnValue = orderBLL.MyOrder(orderCond);

            return result;
        }
    }
}
