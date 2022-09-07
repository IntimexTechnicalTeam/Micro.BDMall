namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IMemberBLL : IDependency
    {
        PageData<MemberDto> SearchMember(MbrSearchCond cond);

        SystemResult Register(RegisterMember member);

        Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang);

        Task<SystemResult> ChangeCurrencyCode(CurrentUser currentUser, string CurrencyCode);

        RegSummary GetRegSummary();

        Task<SystemResult> AddFavMerchant(string merchCode);

        Task<SystemResult> RemoveFavMerchant(string merchCode);

        Task<SystemResult> AddFavProduct(Guid productId);

        Task<SystemResult> RemoveFavProduct(Guid productId);

        Task<PageData<MicroMerchant>> MyFavMerchant(FavoriteCond cond);

        Task<PageData<MicroProduct>> MyFavProduct(FavoriteCond cond);

        Task<CurrentUser<MemberUser>> GetMemberInfo();

        Task<PageData<MicroProduct>> MyProductTrack(TrackCond cond);
    }
}
