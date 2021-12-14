using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    //[ServiceFilter(typeof(AdminApiAuthorizeAttribute))]
    public class SystemMenuController : BaseApiController
    {
        public ISystemMenuBLL menuBLL;

        public SystemMenuController(IComponentContext service) : base(service)
        {
            menuBLL = this.Services.Resolve<ISystemMenuBLL>();
        }

        [HttpGet]        
        [AdminApiAuthorize(Module = ModuleConst.PersonalSetting)]
        public async Task<IEnumerable<TreeNode>> GetMenuTreeByUser()
        { 
            var result = new SystemResult() { Succeeded =true};
            var menuList = this.menuBLL.GetMenuTreeNodes(new UserDto());          
            return menuList;
        }
    }
}
