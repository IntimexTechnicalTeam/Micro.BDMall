namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IAuditTrailBLL : IDependency
    {
        PageData<MemberLoginRecordDto> GetMemAuditTrail(MemLoginRecPager pageInfo);
    }
}
