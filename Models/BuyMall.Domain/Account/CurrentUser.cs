namespace BDMall.Domain
{
    /// <summary>
    /// 当前用户信息实体
    /// </summary>
    public class CurrentUser: TokenInfo
    { 
        public string Token { get; set; } = "";

        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public Guid MerchantId { get; set; }

        public bool IsMerchant => LoginType <= LoginType.ThirdMerchantLink ? true : false;
    }


    public class CurrentUser<T> : CurrentUser
    {
        public T UserData { get; set; }
    }
}
