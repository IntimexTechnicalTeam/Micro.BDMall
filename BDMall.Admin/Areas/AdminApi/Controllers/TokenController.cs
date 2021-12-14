using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework;


namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("adminapi/[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        [AllowAnonymous]
        public async Task<SystemResult> Check()
        {
            var result = new SystemResult() { Succeeded = true };
            return result;
        }
    }
}
