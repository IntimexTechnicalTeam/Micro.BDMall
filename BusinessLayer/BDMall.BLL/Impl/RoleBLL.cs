namespace BDMall.BLL
{
    public class RoleBLL : BaseBLL, IRoleBLL
    {
        public IRoleRepository roleRepository;
        public ITranslationRepository translationRepository;
        public IUserRoleRepository userRoleRepository;


        public RoleBLL(IServiceProvider services) : base(services)
        {
            roleRepository= Services.Resolve<IRoleRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            userRoleRepository = Services.Resolve<IUserRoleRepository>();
        }

        public List<RoleDto> Search(RoleCondition cond)
        {
            var data = roleRepository.Search(cond);
            foreach (var item in data)
            {
                //item.DisplayName = item.FullNames?.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc;
                item.DisplayName = item.FullNames?.FirstOrDefault(d => d.Language == cond.Language)?.Desc;
            }
            return data;
        }

        public RoleDto GetById(Guid id)
        {
            var role = roleRepository.GetRoleByEager(id);
            
            role.DisplayName = role.FullNames?.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc;
            role.Remark = role.Remarks?.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc;
            
            var permissionList = userRoleRepository.GetUserPermissionByRoleId(role.Id);
            role.PermissionList = AutoMapperExt.MapTo<List<PermissionDto>>(permissionList);

            return role;
        }

        public SystemResult Save(RoleDto model)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;

            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }
            model.FullNameTransId = translationRepository.InsertMutiLanguage(model.FullNames, TranslationType.Role);
            model.RemarkTransId = translationRepository.InsertMutiLanguage(model.Remarks, TranslationType.Role);

            if (model.PermissionList?.Any() ?? false)
            {
                foreach (var p in model.PermissionList)
                {
                    RolePermission rp = new RolePermission();
                    rp.Id = Guid.NewGuid();
                    rp.PermissionId = p.Id;
                    rp.RoleId = model.Id;
                    baseRepository.Insert(rp);
                }
            }

            var dbModel = AutoMapperExt.MapTo<Role>(model);        
            baseRepository.Insert(dbModel);
            UnitOfWork.Submit();

            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;
            return result;
        }

        public SystemResult Update(RoleDto model)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;

            var entity = baseRepository.GetModelById<Role>(model.Id);
            if (entity == null)
            {
                throw new BLException("角色不存在，请检查后重试。");
            }
            entity.FullNameTransId = translationRepository.UpdateMutiLanguage(model.FullNameTransId, model.FullNames, TranslationType.Role);
            entity.RemarkTransId = translationRepository.UpdateMutiLanguage(model.RemarkTransId, model.Remarks, TranslationType.Role);

            if (model.PermissionList?.Any() ?? false)
            {
                var rolePermissions = baseRepository.GetList<RolePermission>().Where(d => d.RoleId == model.Id).ToList();
                baseRepository.Delete(rolePermissions);
                foreach (var p in model.PermissionList)
                {
                    RolePermission rp = new RolePermission();
                    rp.Id = Guid.NewGuid();
                    rp.PermissionId = p.Id;
                    rp.RoleId = model.Id;
                    baseRepository.Insert(rp);
                }
            }

            baseRepository.Update(entity);
            UnitOfWork.Submit();

            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.UpdateSuccess;
            return result;
        }

        public SystemResult Remove(RoleDto model)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;

            var entity = baseRepository.GetModelById<Role>(model.Id);
            if (entity == null)
            {
                //throw new BLException("角色不存在，请检查后重试。"); 
                throw new BLException(BDMall.Resources.Message.NoRecord);
            }
            entity.IsDeleted = true;
            baseRepository.Update(entity);

            UnitOfWork.Submit();

            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.DeleteSucceeded;

            return result;
        }


        public SystemResult Restore(RoleDto model)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;

            var entity = baseRepository.GetModelById<Role>(model.Id);
            if (entity == null)
            {
                //throw new BLException("角色不存在，请检查后重试。"); 
                throw new BLException(BDMall.Resources.Message.NoRecord);
            }
            entity.IsDeleted = false;
            baseRepository.Update(entity);

            UnitOfWork.Submit();

            result.Succeeded = true;

            return result;
        }
    }
}
