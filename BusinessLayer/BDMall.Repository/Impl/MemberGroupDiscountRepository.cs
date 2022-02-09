using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class MemberGroupDiscountRepository : PublicBaseRepository, IMemberGroupDiscountRepository
    {
        public MemberGroupDiscountRepository(IServiceProvider service) : base(service)
        {
        }

        public PageData<MarketingDiscount> SearchDiscountHistory(MemberGroupDiscountCond cond)
        {

            PageData<MarketingDiscount> result = new PageData<MarketingDiscount>();
            var query = baseRepository.GetList<MemberGroupDiscount>(p => p.IsActive && !p.IsDeleted );

            var list = query.OrderByDescending(o => o.CreateDate).Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();
            result.TotalRecord = query.Count();

            result.Data = list.Select(d => new MarketingDiscount
            {
                Id = d.Id,
                CreateDate = DateUtil.DateTimeToString(d.CreateDate, "yyyy-MM-dd HH:mm:ss"),
                DiscountRange = d.MeetAmount,
                DiscountMoney = d.DiscountAmount,
                IsDiscount = d.IsDiscount
            }).ToList();

            return result;

        }

        public DiscountInfo CheckHasMemberGroupDiscount()
        {
            var discounts = (from m in baseRepository.GetList <MemberGroupDiscount>()
                             join mi in baseRepository.GetList<MemberGroupDiscountItem>() on m.Id equals mi.DiscountId
                             join g in baseRepository.GetList<MemberGroup>() on mi.MemberGroupId equals g.Id
                             join mb in baseRepository.GetList <Member>() on g.Id equals mb.GroupId
                             where mb.Id == Guid.Parse(CurrentUser.UserId)  && m.IsActive && !m.IsDeleted
                             select new
                             {
                                 discountInfo = new DiscountInfo
                                 {
                                     Id = m.Id,
                                     DiscountRange = m.MeetAmount,
                                     DiscountValue = m.DiscountAmount,
                                     DiscountType = DiscountType.MemberGroup,
                                     Title = "",
                                     EffectDateFrom = null,
                                     EffectDateTo = null,
                                     IsActive = true,
                                     IsPercent = m.IsDiscount
                                 },
                                 range = m.MeetAmount

                             }).ToList();

            decimal totalAmount = 0;
            var cartItemList = baseRepository.GetList<ShoppingCartItem>(p => p.MemberId == Guid.Parse(CurrentUser.UserId) && !p.IsDeleted);
            if (cartItemList.Count() > 0)
            {
                totalAmount = cartItemList.Sum(s => (((decimal)s.Qty) * s.Product.SalePrice));
            }

            var discount = discounts.Where(p => totalAmount >= p.range).OrderByDescending(o => o.range).OrderByDescending(o => o.discountInfo.DiscountValue).Select(d => d.discountInfo).FirstOrDefault();

            return discount;
        }

    }
}
