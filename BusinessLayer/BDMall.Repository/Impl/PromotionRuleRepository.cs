namespace BDMall.Repository
{
    public class PromotionRuleRepository : PublicBaseRepository, IPromotionRuleRepository
    {
        public PromotionRuleRepository(IServiceProvider service) : base(service)
        {
        }

        public PromotionRuleView GetProductPromotionRule(Guid merchantId, string productCode)
        {
            var nowDate = DateUtil.ConvertoDateTime(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd");
            var discount = (from d in baseRepository.GetList < PromotionRule>()
                            join i in baseRepository.GetList<PromotionRuleProduct>() on d.Id equals i.PromotionRuleId
                            join t in baseRepository.GetList<Translation>() on new { a1 = d.TitleTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                            from tt in tc.DefaultIfEmpty()
                            join r in baseRepository.GetList<Translation>() on new { a1 = d.RemarkTransId, a2 = CurrentUser.Lang } equals new { a1 = r.TransId, a2 = r.Lang } into rc
                            from rr in rc.DefaultIfEmpty()
                            where d.IsActive && !d.IsDeleted && d.MerchantId == merchantId
                            //&& d.PromotionRuleProducts.Select(d => d.ProductCode).Contains(productCode)
                            && i.ProductCode == productCode
                            && d.EffectDateFrom <= nowDate && d.EffectDateTo >= nowDate
                            select new PromotionRuleView
                            {
                                Id = d.Id,
                                MerchantId = d.MerchantId,
                                PromotionRule = d.Type,
                                X = d.X,
                                Y = d.Y,
                                Title = tt == null ? "" : tt.Value,
                                Remark = rr == null ? string.Empty : rr.Value
                            }).FirstOrDefault();

            return discount;
        }
    }
}
