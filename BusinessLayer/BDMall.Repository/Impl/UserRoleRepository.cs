using BDMall.Model;
using Microsoft.Data.SqlClient;
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

                var RolePermissions = baseRepository.GetList<RolePermission>().Where(x => x.IsActive && !x.IsDeleted && x.RoleId == item.Id).Select(s => s.PermissionId).ToList();
                item.PermissionList = baseRepository.GetList<Permission>().Where(x => RolePermissions.Contains(x.Id)).ToList();
            }
            return roles;
        }

        public bool CheckMerchantAccountExist(Guid merchantId)
        {
            string sql = $"select 1 from Users u inner join UserRoles ur on u.Id = ur.UserId inner join Roles r on r.Id = ur.RoleId where u.MerchantId =@MerchantId";
            List<SqlParameter> paramList = new List<SqlParameter>();

            paramList.Add(new SqlParameter("@MerchantId", merchantId));

            var result = baseRepository.IntFromSql(sql, paramList.ToArray());
            return result > 0 ? true : false;
        }

        public List<Permission> GetUserPermissionByRoleId(Guid RoleId)
        {
            var list = from a in baseRepository.GetList<RolePermission>().Where(x => x.IsActive && !x.IsDeleted)
                       join b in baseRepository.GetList<Permission>().Where(x => x.IsActive && !x.IsDeleted) on a.PermissionId equals b.Id
                       where a.RoleId == RoleId
                       select b;

            return list.ToList();

        }
    }
}
