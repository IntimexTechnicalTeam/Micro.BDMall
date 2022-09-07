namespace BDMall.Repository
{
    public interface IUserRoleRepository:IDependency
    {
        List<Role> GetUserRoles(Guid userId);

        bool CheckMerchantAccountExist(Guid merchantId);

        List<Permission> GetUserPermissionByRoleId(Guid RoleId);

    }
}
