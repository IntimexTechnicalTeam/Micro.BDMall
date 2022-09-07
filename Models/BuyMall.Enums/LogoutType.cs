namespace BDMall.Enums
{
    public enum LogoutType
    {
        /// <summary>
        /// 用戶登出
        /// </summary>
        UserLogout = 1,

        /// <summary>
        /// 令牌過期
        /// </summary>
        TokenExpire = 2,

        /// <summary>
        /// 登入闲置
        /// </summary>
        TimeOut = 3,
    }
}
