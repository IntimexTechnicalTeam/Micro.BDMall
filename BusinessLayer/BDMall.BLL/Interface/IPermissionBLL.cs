namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IPermissionBLL : IDependency
    {
        Permission GetById(Guid id);
        SystemResult SaveOrUpdate(Permission model);

        SystemResult Remove(Guid Id);

        /// <summary>
        /// 所有權限
        /// </summary>
        /// <returns></returns>
        List<PermissionDto> GetAll();

        /// <summary>
        /// 获取权限划分的模块
        /// </summary> 
        /// <returns></returns>
        List<PermissionDto> GetModule();

        List<PermissionDto> GetFunction(string module);

        /// <summary>
        /// 根據當前用戶获取权限模块
        /// </summary> 
        /// <returns></returns>
        List<PermissionDto> GetModuleByUser();



        /// <summary>
        /// 搜寻權限
        /// </summary> 
        /// <returns></returns>
        PageData<PermissionDto> Search(PermissionCondition cond);
    }
}
