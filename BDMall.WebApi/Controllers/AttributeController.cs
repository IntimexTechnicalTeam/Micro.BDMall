using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : BaseApiController
    {
        public IAttributeBLL attributeBLL;

        public AttributeController(IComponentContext services) : base(services)
        {
            attributeBLL = Services.Resolve<IAttributeBLL>();
        }

        /// <summary>
        /// 获取非库存属性
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("GetCatalogs")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> GetFrontAttribute(ProdAttCond cond)
        {
            var result = await attributeBLL.GetFrontAttributeAsync(cond);
            return result;
        }
    }
}
