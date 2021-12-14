using Autofac;
using BDMall.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        ITestService testService;

        /// <summary>
        /// 如果需要getset,必须public 否则不能为空,而且只能在Controller层Get Set
        /// </summary>
        ////public IProductService productService { get; set; }

        public TestController(IComponentContext service) : base(service)
        {
            testService =this.Services.Resolve<ITestService>();
           
        }

        /// <summary>
        /// Just a Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]     
        public async Task<SystemResult> Test()
        { 
            var result = new SystemResult() { Succeeded =true,Message="234123" };

            testService.Hello("2314");

            return result;
        }

        [HttpGet("Test2")]
        [AllowAnonymous]
        public async Task<SystemResult> Test2(string id)
        {
            var result = new SystemResult() { Succeeded = true, Message = "可以匿名访问的action" };

            testService.Hello("1123");

            return result;
        }

    }
}
