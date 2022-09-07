namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IPaymentBLL : IDependency
    {
        /// <summary>
        /// 保存支付方式的詳細信息
        /// </summary>
        /// <param name="payItems"></param>
        SystemResult SavePayMethodItem(PaymentMethodDto payItems);

        /// <summary>
        /// 获取支付类型列表
        /// </summary>  
        /// <returns></returns>
        List<PaymentMethodView> GetPaymentTypes();

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="id"></param>      
        /// <returns></returns>
        PaymentMethodView GetPaymentType(Guid id);

        /// <summary>
        /// 獲取支付方式詳細信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PaymentMethodDto GetPaymentMenthod(Guid id);

        //decimal GetTotalAmount(int orderId);
        /// <summary>
        /// 獲取支付方式列表
        /// </summary>
        List<PaymentMethodDto> GetPaymentMenthods();

        void DeletePayMethods(Guid[] id);

        void DeleteMothodImage(Guid id);


        string GetPaymentName(Guid id, Language lang);
    }
}
