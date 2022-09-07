namespace BDMall.Repository
{
    public interface IPromotionCodeCouponRepository : IDependency
    {

        /// <summary>
        /// 查詢Coupon
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        ///PageData<PromotionCodeView> SearchPCoupon(PromotionCodeSearchCond condition);

        /// <summary>
        /// 检查用户是否使用Code
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCodeUserUsed(Guid merchantId, string code);

        DiscountInfo GetPromotinoCodeCoupon(Guid merchantId, string code);

        DiscountInfo GetPromotinoCodeCoupon(Guid merchantId, string code, decimal price);

        PrmtCodeDiscountInfo GetPromotinoCodeCoupon(PromotionCodeCond cond);

        /// <summary>
        /// 獲取商家推廣碼規則
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        List<PromotionCodeCoupon> GetActivePromotionCodeRules(Guid merchantId);

        PromotionCodeCoupon GetByCode(string code);
    }
}
