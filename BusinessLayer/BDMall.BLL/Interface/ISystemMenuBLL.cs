using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface ISystemMenuBLL:IDependency
    {
        List<TreeNode> GetMenuTreeNodes(UserDto account);

        List<MenuItem> GetActiveMenus();

        List<MenuItem> GetMenusByUser();
    }
}
