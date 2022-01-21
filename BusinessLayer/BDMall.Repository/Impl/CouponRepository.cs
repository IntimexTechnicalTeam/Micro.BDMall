using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
