namespace BDMall.Repository
{
    public class EmailTypeTempItemRepository : PublicBaseRepository, IEmailTypeTempItemRepository
    {

        public EmailTypeTempItemRepository(IServiceProvider service) : base(service)
        {

        }
        public List<EmailTempItemDto> FindTempItemByEmailType(MailType type)
        {

            var query = (from e in baseRepository.GetList<EmailTypeTempItem>()
                         join l in baseRepository.GetList<EmailTempItem>() on e.ItemId equals l.Id
                         where e.MailType == type && e.IsActive == true && e.IsDeleted == false
                         select l
                ).ToList();

            var dtos = AutoMapperExt.MapToList<EmailTempItem, EmailTempItemDto>(query);
            return dtos;
        }

    }
}
