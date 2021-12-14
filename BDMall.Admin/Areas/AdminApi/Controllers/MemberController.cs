using Autofac;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = "", Modules = new string[] { ModuleConst.MemberModule, ModuleConst.MerchantModule })]
    public class MemberController : BaseApiController
    {
        public MemberController(IComponentContext services) : base(services)
        {
        }

        [HttpGet]
        public object GetMbrSummary()
        {

            return new { };
        }
    }
}
