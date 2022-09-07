namespace BDMall.BLL
{
    public class LogBLL : BaseBLL, ILogBLL
    {
        private ICodeMasterRepository _codeMasterRepo;
        //private ITranslationRepository _translationRepo;

        public LogBLL(IServiceProvider services) : base(services)
        {
        }

        public PageData<SystemEmailsView> GetEmails(SystemEmailsCond cond)
        {
            PageData<SystemEmailsView> result = new PageData<SystemEmailsView>();
            var query = (from e in baseRepository.GetList<SystemEmail>()
                         where (cond.Email == "" || e.SendTo == cond.Email)
                         && e.IsSucceeded == cond.IsSucceed
                         select new SystemEmailsView
                         {
                             Id = e.Id,
                             SendFrom = e.SendFrom,
                             SendTo = e.SendTo,
                             IsSucceeded = e.IsSucceeded,
                             FailCount = e.FailCount,
                             CreateDate = e.CreateDate,
                             Status = e.Status,
                             Subject = e.Subject,
                             Type = e.Type
                         });

            result.TotalRecord = query.Count();

            query = query.OrderByDescending(o => o.CreateDate).Skip(cond.Offset).Take(cond.PageSize);

            result.Data = query.ToList();

            return result;
        }
    }
}
