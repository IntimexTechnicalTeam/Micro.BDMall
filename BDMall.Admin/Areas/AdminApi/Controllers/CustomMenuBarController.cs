using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
    [ApiController]
    public class CustomMenuBarController : BaseApiController
    {
        ICustomMenuBLL _customMenuBLL;
        ISettingBLL _settingBLL;

        public CustomMenuBarController(IComponentContext services) : base(services)
        {
            _customMenuBLL = Services.Resolve<ICustomMenuBLL>();
            _settingBLL = Services.Resolve<ISettingBLL>();
        }

        [HttpGet]
        public List<MenuTree> GetRootMenuTree(int position)
        {
            List<MenuTree> result = new List<MenuTree>();
            result = _customMenuBLL.GetRootMenuTree(position);
            return result;
        }

        [HttpGet]
        public List<KeyValue> GetMenuPosition(string key)
        {
            return _customMenuBLL.GetMenuPosition(CodeMasterFunction.CustomMenuPosition.ToString(), key, 0);
        }

        [HttpGet]
        public List<KeyValue> GetMenuType(string key)
        {
            return _customMenuBLL.GetMenuPosition(CodeMasterFunction.CustomMenuType.ToString(), key, 0);
        }

        [HttpGet]
        public List<KeyValue> GetMenuTypeWithNoLink(string key)
        {
            return _customMenuBLL.GetMenuPosition(CodeMasterFunction.CustomMenuType.ToString(), key, 1);
        }

        [HttpGet]
        public List<MenuTree> GetMenuTreeById(Guid id)
        {
            List<MenuTree> result = new List<MenuTree>();
            result = _customMenuBLL.GetMenuTreeById(id);
            return result;
        }

        [HttpGet]
        public SystemResult GetHeaderMenu(Guid id)
        {
            SystemResult result = new SystemResult();
            result = _customMenuBLL.GetHeaderMenu(id);
            return result;
        }

        [HttpPost]
        public SystemResult SaveMenu([FromForm] MenuDetailInfo info)
        {
            SystemResult result = new SystemResult();
            result = _customMenuBLL.SaveMenu(info);

            string key = CacheKey.MenuBars.ToString();

            var fields = _settingBLL.GetSupportLanguages().Select(s => $"{CacheField.Menu.ToString()}_{s.Code}").ToArray();
            RedisHelper.HDel(key, fields);

            return result;
        }

        [HttpGet]
        public SystemResult GetDeleteMenu(Guid menuId)
        {
            SystemResult result = new SystemResult();
            result = _customMenuBLL.DeleteMenu(menuId);
            return result;
        }

        [HttpPost]

        public PageData<TypeDetail> SearchResult([FromForm] TypeDetailCond cond)
        {
            return _customMenuBLL.SearchResult(cond);
        }

        [HttpPost]
        public SystemResult VerifySeq([FromForm] MenuDetailInfo info)
        {
            SystemResult result = new SystemResult();
            result.Succeeded = _customMenuBLL.VerifySeq(info);
            return result;
        }

        [HttpPost]
        public SystemResult SaveSeq([FromForm]MenuCond cond)
        {
            SystemResult result = new SystemResult();
            result = _customMenuBLL.SaveSeq(cond);
            return result;
        }
    }

}
