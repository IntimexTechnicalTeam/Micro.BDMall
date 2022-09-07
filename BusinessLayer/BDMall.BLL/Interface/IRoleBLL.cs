namespace BDMall.BLL
{
    public interface IRoleBLL:IDependency
    {
        List<RoleDto> Search(RoleCondition cond);

        RoleDto GetById(Guid id);

        SystemResult Save(RoleDto model);

        SystemResult Update(RoleDto model);

        SystemResult Remove(RoleDto model);

        SystemResult Restore(RoleDto model);
    }
}
