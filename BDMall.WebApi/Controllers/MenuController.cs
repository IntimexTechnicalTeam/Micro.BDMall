using Autofac;
using BDMall.BLL;
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
    public class MenuController : BaseApiController
    {
        public ICustomMenuBLL customMenuBLL;

        public MenuController(IComponentContext services) : base(services)
        {
            customMenuBLL = Services.Resolve<ICustomMenuBLL>();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        [HttpGet("GetMenuBar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> GetMenuBar()
        {
            var result = await customMenuBLL.GetMenuBarAsync();
            return result;
        }


    }
}
