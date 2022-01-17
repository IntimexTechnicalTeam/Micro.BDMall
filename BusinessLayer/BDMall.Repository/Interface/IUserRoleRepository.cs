using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IUserRoleRepository:IDependency
    {
        List<Role> GetUserRoles(Guid userId);

        bool CheckMerchantAccountExist(Guid merchantId);

        List<Permission> GetUserPermissionByRoleId(Guid RoleId);

    }
}
