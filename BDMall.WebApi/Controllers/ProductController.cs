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
    public class ProductController : BaseApiController
    {
        public IProductCatalogBLL productCatalogBLL;
        public IProductBLL productBLL;
   
        public ProductController(IComponentContext services) : base(services)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productBLL = Services.Resolve<IProductBLL>();   
        }

        /// <summary>
        /// 获取商品类目
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCatalogs")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> GetCatalogs()
        { 
            var result = await productCatalogBLL.GetCatalogAsync();
            return result;
        }

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("GetProducts")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> GetProducts(ProductCond cond)
        {
            var result = new SystemResult();
            result.ReturnValue = await productBLL.GetProductListAsync(cond);
            result.Succeeded = true;
            return result;
        }
    }
}
