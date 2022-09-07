namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IEmailTypeItemBLL : IDependency
    {
        EmailTypeItemsView GetEmailTypeItem(MailType emailType);


        /// <summary>
        /// 獲取配對了郵件模板的佔位符
        /// </summary>
        /// <param name="emailType"></param>
        /// <returns></returns>
        List<EmailTempItemDto> GetEmailTempItem(MailType emailType);

        List<EmailTempItemDto> GetEmailTypeTempItems(MailType emailType);

        List<EmailTempItemDto> GetEmailTempItems();

        SystemResult SaveEmailTypeItems(EmailTypeItemsView entity);
    }
}
