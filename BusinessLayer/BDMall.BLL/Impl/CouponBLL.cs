using BDMall.Domain;
using BDMall.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class CouponBLL : BaseBLL, ICouponBLL
    {
        public IPromotionCodeCouponRepository _promotionCodeCouponRepository;
        public IMemberGroupDiscountRepository _memberGroupDiscountRepository;
        public ICouponRepository _couponRepository;

        public CouponBLL(IServiceProvider services) : base(services)
        {
            _promotionCodeCouponRepository = Services.Resolve<IPromotionCodeCouponRepository>();
            _memberGroupDiscountRepository = Services.Resolve<IMemberGroupDiscountRepository>();
            _couponRepository= Services.Resolve<ICouponRepository>();
        }

        public DiscountGroup GetCouponGroup(VaildCouponCond cond)
        {

            var group = new DiscountGroup();
            group = _couponRepository.GetVaildCouponGroup(cond);
            return group;

        }

        public DiscountInfo CheckHasGroupOrRuleDiscount()
        {
            var memberGroup = CheckHasMemberGroupDiscount();
            if (memberGroup != null)
            {
                return memberGroup;
            }

            return null;
        }

        public DiscountInfo CheckHasGroupOrRuleDiscount(decimal prodAmount)
        {
            var memberGroup = CheckHasMemberGroupDiscount();
            if (memberGroup != null)
            {
                if (memberGroup.DiscountRange <= prodAmount)
                {
                    return memberGroup;
                }
            }
            return null;
        }

        public DiscountInfo GetPromotionCodeCoupon(Guid merchantId, string code, decimal price)
        {
            DiscountInfo discount = new DiscountInfo();

            if (_promotionCodeCouponRepository.CheckCodeUserUsed(merchantId, code))
            {
                discount = _promotionCodeCouponRepository.GetPromotinoCodeCoupon(merchantId, code, price);
                return discount;
            }
            return null;
        }

        public PrmtCodeDiscountInfo GetPromotionCodeCouponV2(PromotionCodeCond cond)
        {

            if (cond == null) return null;


            PrmtCodeDiscountInfo discount = new PrmtCodeDiscountInfo()
            {
                OriginalProdList = new List<PromotionCodeProd>(),
                DiscountProdList = new List<PromotionCodeProd>()
            };

            if (_promotionCodeCouponRepository.CheckCodeUserUsed(cond.MerchantId, cond.Code))
            {
                discount = _promotionCodeCouponRepository.GetPromotinoCodeCoupon(cond);
                return discount;
            }

            return null;
        }

      
        //public decimal GetMallFun()
        //{
           
        //    return _memberAccountRepository.GetMemberFun();
      
        //}

        public PageData<CouponInfo> GetMemberCoupon(CouponPager cond)
        {
            return _couponRepository.GetMemberCoupons(cond);
        }

        private DiscountInfo CheckHasMemberGroupDiscount()
        {
            return _memberGroupDiscountRepository.CheckHasMemberGroupDiscount();
        }

    }
}
