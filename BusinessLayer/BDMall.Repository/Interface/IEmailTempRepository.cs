namespace BDMall.Repository
{
    public interface IEmailTempRepository : IDependency
    {
        List<EmailTempItemDto> GetEmailTypeTempItems(MailType type);

        List<EmailTemplateDto> Search(EmailTempCondition cond);

        List<EmailTemplateDto> GetTemplateByMailType(MailType type);
    }
}
