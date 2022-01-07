using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        ISystemMenuBLL _menuBLL;
        ISettingBLL _settingBLL;

        public SystemMenuController(IComponentContext service) : base(service)
        {
            _menuBLL = this.Services.Resolve<ISystemMenuBLL>();
            _settingBLL = this.Services.Resolve<ISettingBLL>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public MenuItem GetMenuInfo(int id)
        {
            var langs = _settingBLL.GetSupportLanguages();
            if (id == 0)
            {
                MenuItem newItem = new MenuItem()
                {
                    NameTranslation = new List<MutiLanguage>()
                };
                foreach (var item in langs)
                {
                    newItem.NameTranslation.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                }
                return newItem;
            }
            //ISystemMenuBLL bll = BLLFactory.Create(CurrentWebStore).CreateSystemMenuBLL();
            var result = _menuBLL.GetMenu(id);

            return result;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.PersonalSetting)]
        public async Task<IEnumerable<TreeNode>> GetMenuTreeByUser()
        {
            var result = new SystemResult() { Succeeded = true };
            var menuList = this._menuBLL.GetMenuTreeNodes(new UserDto());
            return menuList;
        }
        /// <summary>
        /// 获取菜单数据所有节点
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeNode> GetMenuTree()
        {

            return _menuBLL.GetMenuTreeNodes();
        }
        /// <summary>
        /// 获取所有菜单项目的数据集合
        /// </summary>
        /// <returns></returns>
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public IEnumerable<MenuItem> GetList()
        {
            return _menuBLL.GetMenus();
        }
        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult Save([FromForm]MenuItem menu)
        {
            SystemResult result = new SystemResult();


            //ISystemMenuBLL bll = BLLFactory.Create(CurrentWebStore).CreateSystemMenuBLL();
            _menuBLL.SaveMenu(menu);
            result.Succeeded = true;


            return result;
        }
        /// <summary>
        /// 检查MenuCode是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult CheckMenuCodeIsExists(string code)
        {
            SystemResult result = new SystemResult();

            var isExists = _menuBLL.CheckMenuCodeIsExists(code);
            result.Succeeded = true;
            result.ReturnValue = isExists;
            if (isExists)
            {
                result.Message = Resources.Message.MenuCodeIsExist;
            }
            else
            {
                result.Message = "";
            }

            return result;
        }

        /// <summary>
        /// 更新各個catalog的順序
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult UpdateSeq(List<TreeNode> tree)
        {
            SystemResult result = new SystemResult();


            var list = TreeUtil.TreeToList(tree);
            list = list.Where(p => p.IsChange == true).ToList();
            _menuBLL.UpdateSystemMenuSeq(list);
            result.Succeeded = true;

            return result;
        }



        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult Delete(int id)
        {
            SystemResult result = new SystemResult();
            _menuBLL.RemoveMenuItem(id);
            result.Succeeded = true;

            return result;

        }
    }
}
