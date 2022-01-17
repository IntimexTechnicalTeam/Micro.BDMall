using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class RoleController : BaseApiController
    {
        private IRoleBLL roleBLL;
        private IPermissionBLL permissionBLL;
        public IMerchantRepository merchantRepository;
        public RoleController(IComponentContext services) : base(services)
        {
            roleBLL = Services.Resolve<IRoleBLL>(); 
            permissionBLL = Services.Resolve<IPermissionBLL>(); 
            merchantRepository = Services.Resolve<IMerchantRepository>();
        }

        /// <summary>
        /// 根据参数类型獲取角色的列表
        /// </summary>
        /// <returns></returns>
        [ActionName("GetRoles")]
        [HttpGet]
        public IEnumerable<RoleDto> GetRoles(int type)
        {
            IEnumerable<RoleDto> data = null;
            RoleCondition cond = new RoleCondition() { Language = Language.E };
            switch (type)
            {
                case 1://系统有效角色 
                    cond.IsActive = true;
                    cond.IsDeleted = false;
                    cond.IsSystem = true;
                    data = roleBLL.Search(cond);
                    break;

                case 2://非系统有效角色

                    cond.IsActive = true;
                    cond.IsDeleted = false;
                    cond.IsSystem = false;
                    data = roleBLL.Search(cond);
                    break;

                case 3://系统有效内部商家角色 
                    cond.IsActive = true;
                    cond.IsDeleted = false;
                    cond.IsSystem = true;
                    data = roleBLL.Search(cond).Where(d => d.Name.Contains("Merchant")).Where(d => d.Name.ToUpper().Contains("INT")).ToList();
                    break;

                case 4://系统有效外部商家角色 
                    cond.IsActive = true;
                    cond.IsDeleted = false;
                    cond.IsSystem = true;
                    data = roleBLL.Search(cond).Where(d => d.Name.Contains("Merchant")).Where(d => d.Name.ToUpper().Contains("EXT")).ToList();
                    break;

                default://所有角色
                    cond.IsActive = true;
                    data = roleBLL.Search(cond);
                    break;
            }
            return data;
        }

        /// <summary>
        /// 獲取權限模塊
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BTTreeNode> GetPermissionModule()
        {
            var data = permissionBLL.GetModuleByUser();
            var nodes = new List<BTTreeNode>();
            var modules = data.Where(d => d.IsActive && !d.IsDeleted && (d.Function == null || d.Function == ""));
            foreach (var item in modules)
            {
                BTTreeNode m = new BTTreeNode();
                if (CurrentUser.Lang== Language.E)
                {
                    m.Text = item.Description;
                }
                else if (CurrentUser.Lang == Language.C)
                {
                    m.Text = item.DescriptionTC;
                }
                else
                {
                    m.Text = item.DescriptionSC;
                }
                m.Title = item.Description;
                m.Id = item.Id.ToString();
                m.Checkable = true;
                m.State = new state() { Checked = false, Expanded = false };
                nodes.Add(m);


                var funcs = data.Where(d => d.Module == item.Module && !string.IsNullOrEmpty(d.Function)).OrderBy(d => d.Seq);
                if (funcs?.Any() ?? false)
                {
                    m.Nodes = new List<BTTreeNode>();

                    foreach (var f in funcs)
                    {
                        BTTreeNode fm = new BTTreeNode();
                        fm.State = new state() { Checked = false, Expanded = false };
                        if (CurrentUser.Lang == Language.E)
                        {
                            fm.Text = f.Description;
                        }
                        else if (CurrentUser.Lang== Language.C)
                        {
                            fm.Text = f.DescriptionTC;
                        }
                        else
                        {
                            fm.Text = f.DescriptionSC;
                        }
                        fm.Title = f.Description;
                        fm.Id = f.Id.ToString();
                        fm.Checkable = true;
                        m.Nodes.Add(fm);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// 根據條件搜尋角色
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult Search([FromForm]RoleCondition cond)
        {
            SystemResult result = new SystemResult();
            result.ReturnValue = roleBLL.Search(cond);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 根據ID獲取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]    
        public RoleDto GetById(Guid id)
        {           
            return roleBLL.GetById(id);
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Save")]
        public SystemResult Save([FromForm] RoleDto value)
        {          
            value.FullNames = new List<MutiLanguage>();
            value.FullNames.Add(new MutiLanguage()
            {
                Language = CurrentUser.Lang,
                Desc = value.DisplayName
            });
            if (value.Id == Guid.Empty)
            {
                return roleBLL.Save(value);
            }
            else
            {
                return roleBLL.Update(value);
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Update")]
        public SystemResult Update(int id, [FromBody] RoleDto value)
        {
            return roleBLL.Update(value);
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Remove")]
        public SystemResult Remove(Guid id)
        {
            return roleBLL.Remove(new RoleDto()
            {
                Id = id
            });
        }

        /// <summary>
        /// 恢復角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Restore")]
        public SystemResult Restore(Guid id)
        {           
            return roleBLL.Restore(new RoleDto() { Id = id });
        }

    }
}
