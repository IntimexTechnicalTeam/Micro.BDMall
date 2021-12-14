using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class UserRoleRepository : PublicBaseRepository, IUserRoleRepository
    {
        public UserRoleRepository(IServiceProvider service) : base(service)
        {
        }

        public List<Role> GetUserRoles(Guid userId)
        {

            var roles = baseRepository.GetList<UserRole>().Where(d => d.UserId == userId && d.IsActive && !d.IsDeleted).Select(d => d.Role).Where(d => d.IsActive && !d.IsDeleted).ToList();

            foreach (var item in roles)
            {
                //if (_unitWork.Operator != null)
                //{
                //    item.DisplayName = _unitWork.DataContext.Translations.FirstOrDefault(d => d.TransId == item.FullNameTransId && d.Lang == _unitWork.Operator.Language)?.Value ?? "";
                //}

                var RolePermissions = baseRepository.GetList<RolePermission>().Where(x => x.IsActive && !x.IsDeleted && x.RoleId == item.Id).Select(s=>s.PermissionId).ToList();
                item.PermissionList = baseRepository.GetList<Permission>().Where(x => RolePermissions.Contains(x.Id)).ToList();
            }
            return roles;
        }
    }
}
