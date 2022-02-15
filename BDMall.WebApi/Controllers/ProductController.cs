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
        [AllowAnonymous]
        [HttpGet("GetCatalogs")]    
        [ProducesResponseType(typeof(SystemResult<List<Catalog>>), 200)]
        public async Task<SystemResult<List<Catalog>>> GetCatalogs()
        { 
            var result = new SystemResult<List<Catalog>>();
            result.ReturnValue= await productCatalogBLL.GetCatalogAsync();
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("GetProducts")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroProduct>>), 200)]
        public async Task<SystemResult<PageData<MicroProduct>>> GetProducts([FromBody] ProductCond cond)
        {
            var result = new SystemResult<PageData<MicroProduct>>();
            result.ReturnValue = await productBLL.GetProductListAsync(cond);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取商品明细
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("GetByCode")]
        [AllowAnonymous]
        [ProductViewTrackFilter]
        [ProducesResponseType(typeof(SystemResult<MicroProductDetail>), 200)]
        public async Task<SystemResult<MicroProductDetail>> GetByCode(string code)
        {
            var result = new SystemResult<MicroProductDetail>();
            result.ReturnValue = await productBLL.GetMicroProductDetail(code);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 检测产品Sku状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="attr1"></param>
        /// <param name="attr2"></param>
        /// <param name="attr3"></param>
        /// <param name="saleTime"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Check")]
        [ProducesResponseType(typeof(SystemResult<ProductCheck>), 200)]
        public async Task<SystemResult<ProductCheck>> Check(string code, Guid attr1, Guid attr2, Guid attr3, string saleTime)
        {
            var result  = await productBLL.CheckSkuStateAsync(code, attr1, attr2, attr3, saleTime);            
            return result;
        }
    }
}
