using Autofac;
using BDMall.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        IUserBLL userBLL;

        public UserController(IComponentContext services) : base(services)
        {
            userBLL = Services.Resolve<IUserBLL>();
        }

        [HttpGet]
        public async Task<SystemResult> CreateMerchantAccount(string merchantIds)
        {
            SystemResult result = userBLL.CreateAccountForMerchant(merchantIds);          
            return result;

        }
    }
}
