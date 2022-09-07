namespace BDMall.Repository
{
    public class EmailTempItemRepository : PublicBaseRepository, IEmailTempItemRepository
    {

        public EmailTempItemRepository(IServiceProvider service) : base(service)
        {

        }
        public List<EmailTempItemDto> Search(EmailTempItemCondition cond)
        {
            List<EmailTempItemDto> result = new List<EmailTempItemDto>();

            var query = from m in baseRepository.GetList<EmailTempItem>()
                        join t in baseRepository.GetList<Translation>() on new { a1 = m.DescId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                        from tt in tc.DefaultIfEmpty()

                        select new EmailTempItemDto
                        {
                            CreateBy = m.CreateBy,
                            CreateDate = m.CreateDate,
                            DescId = m.DescId,
                            Id = m.Id,
                            Description = tt.Value,
                            IsActive = m.IsActive,
                            IsDeleted = m.IsDeleted,
                            ObjectType = m.ObjectType,
                            PlaceHolder = m.PlaceHolder,
                            Propertity = m.Propertity,
                            Remark = m.Remark,
                            UpdateBy = m.UpdateBy,
                            UpdateDate = m.UpdateDate,
                        };
            if (cond.IsDeleted.HasValue)
            {
                query = query.Where(d => d.IsDeleted == cond.IsDeleted.Value);
            }
            if (!string.IsNullOrEmpty(cond.Value))
            {
                query = query.Where(d => d.PlaceHolder.Contains(cond.Value));
            }

            var datas = query.Distinct().OrderByDescending(d => d.UpdateDate).ToList();

            return datas;

        }

    }
}
