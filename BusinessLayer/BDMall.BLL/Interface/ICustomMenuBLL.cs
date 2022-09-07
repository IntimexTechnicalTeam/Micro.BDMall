using Menu = BDMall.Domain.Menu;

namespace BDMall.BLL
{
    public interface ICustomMenuBLL : IDependency
    {
        List<MenuTree> GetRootMenuTree(int position);

        List<MenuTree> GetMenuTreeById(Guid id);

        SystemResult GetHeaderMenu(Guid id);

        PageData<TypeDetail> SearchResult(TypeDetailCond cond);

        SystemResult GetMenuBar();

        List<MutiLanguage> GetMutiLanguage(Guid transId);

        CustomMenuDto GetCustomMenuById(Guid id);

        List<CustomMenuDetailDto> GetCustomMenuDetailByMenuId(Guid menuId);

        SystemResult DeleteMenu(Guid id);

        SystemResult SaveMenu(MenuDetailInfo info);

        List<KeyValue> GetMenuPosition(string functions, string key, int type);

        bool VerifySeq(MenuDetailInfo info);

        SystemResult SaveSeq(MenuCond cond);

        Task<List<Menu>> GetMenuBarAsync();
    }
}
