namespace BDMall.BLL
{
    public interface ISystemMenuBLL:IDependency
    {
        List<TreeNode> GetMenuTreeNodes();
        List<TreeNode> GetMenuTreeNodes(UserDto account);

        List<MenuItem> GetActiveMenus();

        List<MenuItem> GetMenusByUser();

        List<MenuItem> GetMenus();

        MenuItem GetMenu(int menuId);
        /// <summary>
        /// 保存菜單
        /// </summary>
        /// <param name="item"></param>
        void SaveMenu(MenuItem item);

        /// <summary>
        /// 更改菜單的排序
        /// </summary>
        /// <param name="list"></param>
        void UpdateSystemMenuSeq(List<TreeNode> list);

        /// <summary>
        /// 检查菜单的Code是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckMenuCodeIsExists(string code);

        /// <summary>
        /// 根据ID删除菜单项目
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        bool RemoveMenuItem(int menuId);
    }
}
