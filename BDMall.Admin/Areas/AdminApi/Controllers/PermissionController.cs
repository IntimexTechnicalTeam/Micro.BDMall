using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
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
    public class PermissionController : BaseApiController
    {
        IPermissionBLL _permissionBLL;

        public PermissionController(IComponentContext services) : base(services)
        {
            _permissionBLL = Services.Resolve<IPermissionBLL>();
        }


        /// <summary>
        /// 獲取全部角色的列表
        /// </summary>
        /// <returns></returns>
        [ActionName("GetList")]
        public IEnumerable<PermissionDto> GetList()
        {

            var data = _permissionBLL.GetAll();
            return data;
        }

        /// <summary>
        /// 根據條件搜尋角色
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult Search(PermissionCondition cond)
        {


            SystemResult systemResult = new SystemResult();

            systemResult.ReturnValue = _permissionBLL.Search(cond);
            systemResult.Succeeded = true;

            return systemResult;
        }



        /// <summary>
        /// 獲取權限模塊
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PermissionDto> GetPermissionModule()
        {

            var data = _permissionBLL.GetModule();

            return data;
        }
        /// <summary>
        /// 獲取權限模塊的功能
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PermissionDto>> GetPermissionFunction(Guid moduleId)
        {
            var data = new List<PermissionDto>();
            var mod = _permissionBLL.GetById(moduleId);
            if (mod != null)
            {
                data = _permissionBLL.GetFunction(mod.Module);
            }
            
            return data;

        }
        /// <summary>
        /// 根據ID獲取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetById")]
        public Permission GetById(Guid id)
        {

            return _permissionBLL.GetById(id);
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Save")]
        public SystemResult Save([FromForm] Permission value)
        {

            return _permissionBLL.SaveOrUpdate(value);

        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Update")]
        public SystemResult Update([FromForm] Permission value)
        {

            return _permissionBLL.SaveOrUpdate(value);
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

            return _permissionBLL.Remove(id);

        }



        /// <summary>
        /// 獲取權限模塊的下拉列表
        /// </summary>
        /// <returns></returns>
        public List<BTTreeNode> GetTreeNode()
        {
            var result = new List<KeyValue>();
            var data = _permissionBLL.GetAll();


            var nodes = new List<BTTreeNode>();
            var modules = data.Where(d => d.Function == null || d.Function == "");
            foreach (var item in modules)
            {
                BTTreeNode m = new BTTreeNode();
                if (CurrentUser.Lang == Language.E)
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

                nodes.Add(m);


                var funcs = data.Where(d => d.Module == item.Module && !string.IsNullOrEmpty(d.Function)).OrderBy(d => d.Seq);
                if (funcs.Count() > 0)
                {
                    m.Nodes = new List<BTTreeNode>();
                    m.State = new state() { Checked = false, Expanded = false };
                    foreach (var f in funcs)
                    {
                        BTTreeNode fm = new BTTreeNode();
                        if (CurrentUser.Lang == Language.E)
                        {
                            fm.Text = f.Description;
                        }
                        else if (CurrentUser.Lang == Language.C)
                        {
                            fm.Text = f.DescriptionTC;
                        }
                        else
                        {
                            fm.Text = f.DescriptionSC;
                        }
                        fm.Title = f.Description;
                        fm.Id = f.Id.ToString();

                        m.Nodes.Add(fm);
                    }
                    m.Tags = new List<string>() { m.Nodes.Count().ToString() };
                }
            }

            return nodes;
        }

        /// <summary>
        /// 獲取權限模塊
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetModules()
        {
            var result = new List<KeyValue>();
            var data = _permissionBLL.GetModule();

            foreach (var item in data)
            {
                result.Add(new KeyValue { Id = item.Id.ToString(), Text = item.Module });
            }

            return result;
        }

        /// <summary>
        /// 獲取權限模塊的功能
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetFunctions(string module)
        {
            var result = new List<KeyValue>();
            var data = _permissionBLL.GetFunction(module);

            foreach (var item in data)
            {
                result.Add(new KeyValue { Id = item.Id.ToString(), Text = item.Description });
            }

            return result;
        }
    }
}
