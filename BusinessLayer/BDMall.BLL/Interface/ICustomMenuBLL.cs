using BDMall.Domain;
using BDMall.Model;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

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
    }
}
