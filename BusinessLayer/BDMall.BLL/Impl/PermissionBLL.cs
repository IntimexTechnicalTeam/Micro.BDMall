using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class PermissionBLL : BaseBLL, IPermissionBLL
    {
        //private ITranslationRepository _translationRepo;

        public PermissionBLL(IServiceProvider services) : base(services)
        {
        }
        public List<PermissionDto> GetModule()
        {
            List<PermissionDto> pmList = new List<PermissionDto>();

            var query = baseRepository.GetList<Permission>().Where(d => d.IsActive && !d.IsDeleted && d.Function == "" || d.Function == null).OrderBy(d => d.Module).ToList();

            var modules = AutoMapperExt.MapToList<Permission, PermissionDto>(query);

            return modules;

        }

        public List<PermissionDto> GetModuleByUser()
        {

            var myPermission = new List<PermissionDto>();
            foreach (var item in CurrentUser.Roles)
            {
                myPermission.AddRange(item.PermissionList);
            }
            myPermission = myPermission.Distinct().ToList();


            return myPermission;
        }
        public List<PermissionDto> GetAll()
        {
            List<PermissionDto> pmList = new List<PermissionDto>();

            var query = baseRepository.GetList<Permission>().Where(d => d.IsActive && !d.IsDeleted).OrderBy(d => new { d.Module, d.Function }).ToList();

            var alls = AutoMapperExt.MapToList<Permission, PermissionDto>(query);

            return alls;

        }

        public List<PermissionDto> GetFunction(string module)
        {
            List<PermissionDto> pmList = new List<PermissionDto>();

            var query = baseRepository.GetList<Permission>().Where(d => d.IsActive && !d.IsDeleted && d.Module == module && d.Function != null).OrderBy(d => d.Function).ToList();

            pmList = AutoMapperExt.MapToList<Permission, PermissionDto>(query);

            return pmList;
        }
        public PageData<PermissionDto> Search(PermissionCondition cond)
        {
            PageData<PermissionDto> result = new PageData<PermissionDto>(cond);

            var query = baseRepository.GetList<Permission>().Where(d => d.IsActive && !d.IsDeleted);

            if (!string.IsNullOrEmpty(cond.Module))
            {
                query = query.Where(d => d.Module == cond.Module);
            }
            if (!string.IsNullOrEmpty(cond.Function))
            {
                query = query.Where(d => d.Function == cond.Function);
            }


            result.TotalRecord = query.Count();

            var list = query.OrderBy(d => d.Module).Skip(cond.Offset).Take(cond.PageSize).ToList();

            var pmList = AutoMapperExt.MapToList<Permission, PermissionDto>(list);

            result.Data = pmList;

            return result;
        }

        public Permission GetById(Guid id)
        {
            return baseRepository.GetList<Permission>().FirstOrDefault(p => p.Id == id);
        }

        public SystemResult SaveOrUpdate(Permission model)
        {
            SystemResult result = new SystemResult();

            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                baseRepository.Insert(model);
            }
            else
            {
                baseRepository.Update(model);
            }
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;

            return result;
        }

        public SystemResult Remove(Guid id)
        {
            SystemResult result = new SystemResult();

            var module = GetById(id);
            if (module.Function == null)
            {
                var funcList = GetFunction(module.Module);
                if (funcList.Count > 0)
                {
                    result.Message = "模块存在功能权限，不能删除。";
                    return result;
                }
            }
            baseRepository.Delete(module);
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.DeleteSucceeded;

            return result;
        }
    }
}
