namespace BDMall.Repository
{
    public class CouponRepository : PublicBaseRepository, ICouponRepository
    {
        public CouponRepository(IServiceProvider service) : base(service)
        {
        }

        public DiscountInfo GetCouponById(Guid id)
        {
            var coupon = (from r in baseRepository.GetList<CouponRule>()
                          join c in baseRepository.GetList<Coupon>() on r.Id equals c.RuleId
                          join t in baseRepository.GetList<Translation>() on new { a1 = r.TitleTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                          from tt in tc.DefaultIfEmpty()
                          where c.IsActive && !c.IsDeleted && c.Id == id
                          select new DiscountInfo
                          {
                              Id = c.Id,
                              DiscountValue = r.DiscountAmount,
                              DiscountType = DiscountType.Coupon,
                              IsActive = true,
                              IsPercent = r.IsDiscount,
                              Title = tt == null ? "" : tt.Value,
                              EffectDateFrom = c.EffectDateFrom,
                              EffectDateTo = c.EffectDateTo,
                              Image = r.Image,
                              CouponType = r.CouponUsage
                          }).FirstOrDefault();

            return coupon;
        }

        public PageData<CouponInfo> GetMemberCoupons(CouponPager cond)
        {
            PageData<CouponInfo> data = new PageData<CouponInfo>();

            var nowDay = DateTime.Parse(DateTime.Now.ToShortDateString());

            var query = (from c in baseRepository.GetList<Coupon>()
                         join r in baseRepository.GetList<CouponRule>() on c.RuleId equals r.Id
                         join rt in baseRepository.GetList<Translation>() on new { a1 = r.TitleTransId, a2 = CurrentUser.Lang } equals new { a1 = rt.TransId, a2 = rt.Lang } into rtc
                         from rtt in rtc.DefaultIfEmpty()
                         join ct in baseRepository.GetList<Translation>() on new { a1 = r.RemarkTransId, a2 = CurrentUser.Lang } equals new { a1 = ct.TransId, a2 = ct.Lang } into ctc
                         from ctt in ctc.DefaultIfEmpty()
                         join m in baseRepository.GetList<Merchant>() on r.MerchantId equals m.Id into mt
                         from mm in mt.DefaultIfEmpty()
                         join t in baseRepository.GetList<Translation>() on new { a1 = mm.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         where c.IsActive && !c.IsDeleted && c.MemberId == Guid.Parse(CurrentUser.UserId)
                         select new
                         {
                             coupon = c,
                             couponRule = r,
                             couponRuleTitle = rtt,
                             remark = ctt,
                             merchantName = tt
                         });

            if (cond.status == CouponStatus.Active)
            {
                query = query.Where(p => p.coupon.EffectDateFrom <= nowDay && p.coupon.EffectDateTo >= nowDay && p.coupon.UseDate == null);
            }

            if (cond.status == CouponStatus.DisActive)
            {
                query = query.Where(p => p.coupon.EffectDateFrom > nowDay || p.coupon.EffectDateTo < nowDay);
            }

            if (cond.status == CouponStatus.Used)
            {
                query = query.Where(p => p.coupon.UseDate != null);
            }

            if (cond.status == CouponStatus.NoUsed)
            {
                query = query.Where(p => p.coupon.EffectDateFrom <= nowDay && p.coupon.EffectDateTo >= nowDay && p.coupon.UseDate == null);
            }
            data.TotalRecord = query.Count();

            if (cond.Page == 0 && cond.PageSize == 0)
            {
                query = query.OrderByDescending(o => o.coupon.CreateDate);
            }
            else
            {
                query = query.OrderByDescending(o => o.coupon.CreateDate).Skip(cond.Offset).Take(cond.PageSize);
            }



            var result = (from c in query
                          select new CouponInfo
                          {
                              Id = c.coupon.Id,
                              Code = "",
                              CouponType = c.couponRule.CouponUsage,
                              DiscountRange = c.couponRule.MeetAmount,
                              DiscountType = DiscountType.Coupon,
                              DiscountValue = c.couponRule.DiscountAmount,
                              EffectDateFrom = c.coupon.EffectDateFrom,
                              EffectDateTo = c.coupon.EffectDateTo,
                              Image = c.couponRule.Image,
                              MerchantId = c.couponRule.MerchantId,
                              MerchantName = c.merchantName == null ? "" : c.merchantName.Value,
                              Title = c.couponRuleTitle == null ? "" : c.couponRuleTitle.Value,
                              Remark = c.remark == null ? "" : c.remark.Value,
                              UsedDate = c.coupon.UseDate,
                              IsUsed = c.coupon.UseDate != null,
                              IsActive = c.coupon.IsActive,
                              IsPercent = c.couponRule.IsDiscount
                          }).ToList();

            foreach (var item in result)
            {
                item.CouponStatus = GetCouponStatus(item);
            }

            data.Data = result;

            return data;
        }

        public DiscountGroup GetVaildCouponGroup(VaildCouponCond cond)
        {

            var nowDay = DateTime.Parse(DateTime.Now.ToShortDateString());
            var coupons = from r in baseRepository.GetList<CouponRule>()
                           join c in baseRepository.GetList<Coupon>() on r.Id equals c.RuleId
                           join t in baseRepository.GetList<Translation>() on new { a1 = r.TitleTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                           from tt in tc.DefaultIfEmpty()
                           where  c.IsActive && !c.IsDeleted
                           && r.MerchantId == cond.MerchantId
                           && c.MemberId == Guid.Parse(CurrentUser.UserId)
                           && c.EffectDateFrom <= nowDay && c.EffectDateTo >= nowDay
                           && c.UseDate == null
                           select new
                           {
                               rule = r,
                               coupon = c,
                               tran = tt
                           };


            DiscountGroup group = new DiscountGroup();
            group.DeliveryChargeCoupons = coupons.Where(p => cond.TotalPrice >= p.rule.MeetAmount && p.rule.CouponUsage == CouponUsage.DeliveryCharge).Select(d => new DiscountInfo
            {
                Id = d.coupon.Id,
                DiscountValue = d.rule.DiscountAmount,
                DiscountType = DiscountType.Coupon,
                IsActive = true,
                IsPercent = d.rule.IsDiscount,
                Title = d.tran == null ? "" : d.tran.Value,
                EffectDateFrom = d.coupon.EffectDateFrom,
                EffectDateTo = d.coupon.EffectDateTo,
                Image = d.rule.Image,
                CouponType = d.rule.CouponUsage

            }).ToList();

            group.PriceCoupons = coupons.Where(p => cond.TotalPrice >= p.rule.MeetAmount && p.rule.CouponUsage == CouponUsage.Price).Select(d => new DiscountInfo
            {
                Id = d.coupon.Id,
                DiscountValue = d.rule.DiscountAmount,
                //DiscountRange = d.rule.MeetAmount,
                DiscountType = DiscountType.Coupon,
                IsActive = true,
                IsPercent = d.rule.IsDiscount,
                Title = d.tran == null ? "" : d.tran.Value,
                EffectDateFrom = d.coupon.EffectDateFrom,
                EffectDateTo = d.coupon.EffectDateTo,
                Image = d.rule.Image,
                CouponType = d.rule.CouponUsage
            }).ToList();

            return group;
        }

        private CouponStatus GetCouponStatus(CouponInfo couponInfo)
        {

            var nowDay = DateTime.Parse(DateTime.Now.ToShortDateString());

            if (couponInfo.EffectDateFrom <= nowDay && couponInfo.EffectDateTo >= nowDay)
            {
                return CouponStatus.Active;
            }
            else
            {
                return CouponStatus.DisActive;
            }
        }
    }
}
