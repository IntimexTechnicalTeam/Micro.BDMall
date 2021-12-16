using Autofac;
using BDMall.BLL;
using BDMall.Domain;
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
        public IMemberBLL memberBLL;

        public MemberController(IComponentContext services) : base(services)
        {
            memberBLL = Services.Resolve<IMemberBLL>();
        }

        [HttpGet]
        public RegSummary GetMbrSummary()
        {
            var  mbrSummary = memberBLL.GetRegSummary();
            return mbrSummary;
        }
    }
}
