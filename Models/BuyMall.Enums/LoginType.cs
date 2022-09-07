namespace BDMall.Enums
{
    public enum LoginType
    {      
        /// <summary>
        /// 商家
        /// </summary>
        Merchant ,

        /// <summary>
        /// 第三方商家
        /// </summary>
        ThirdMerchantLink,

        /// <summary>
        /// admin
        /// </summary>
        Admin,

        //如果要有后台功能，请加在Admin前

        /// <summary>
        /// 会员
        /// </summary>
        Member,

        TempUser,

    }
}
