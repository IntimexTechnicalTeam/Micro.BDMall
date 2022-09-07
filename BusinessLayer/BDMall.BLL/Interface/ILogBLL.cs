namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface ILogBLL : IDependency
    {
        PageData<SystemEmailsView> GetEmails(SystemEmailsCond cond);

    }
}
