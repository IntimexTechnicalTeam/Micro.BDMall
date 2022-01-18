using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;
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

        /// <summary>
        ///  查询会员列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public SystemResult Search([FromBody] MbrSearchCond condition)
        {
            var result = new SystemResult() { Succeeded = true };

            var testUser = CurrentUser;

            result.ReturnValue = memberBLL.SearchMember(condition);
            return result;
        }
    }
}
