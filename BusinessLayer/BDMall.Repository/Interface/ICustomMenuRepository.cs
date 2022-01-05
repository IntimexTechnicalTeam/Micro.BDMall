using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface ICustomMenuRepository : IDependency
    {
        List<MenuTree> GetRootMenuTree(int position);

        List<MenuTree> GetMenuTreeById(Guid id);

        SystemResult GetHeaderMenu(Guid id);

        PageData<TypeDetail> SearchResult(TypeDetailCond cond);

        MenuModel GetMenuBar();

        List<MutiLanguage> GetMutiLanguage(Guid transId);

        CustomMenuDto GetCustomMenuById(Guid id);

        List<CustomMenuDetailDto> GetCustomMenuDetailByMenuId(Guid menuId);

        bool VerifySeq(MenuDetailInfo info);

        int GetCustomMenuSeq(MenuDetailInfo info);
    }
}
