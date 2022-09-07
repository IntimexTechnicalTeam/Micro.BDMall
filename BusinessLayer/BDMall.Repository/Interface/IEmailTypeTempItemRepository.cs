namespace BDMall.Repository
{
    public interface IEmailTypeTempItemRepository : IDependency
    {
        List<EmailTempItemDto> FindTempItemByEmailType(MailType type);
    }
}
