namespace BDMall.BLL
{
    public interface IMailServerBLL : IDependency
    {
        MailServerInfo GetById(Guid id);
        PageData<MailServerView> GetMailServerSettings(MailServerCond cond);

        SystemResult SaveMailServer(MailServerInfo info);

        SystemResult DeleteMailServer(List<string> ids);

        SystemResult GetMailServerByMail(string mail);
    }
}
