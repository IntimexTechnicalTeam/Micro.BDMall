using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class SystemMenuBLL : BaseBLL, ISystemMenuBLL
    {
        public SystemMenuBLL(IServiceProvider services) : base(services)
        {
        }

        public List<TreeNode> GetMenuTreeNodes(UserDto account)
        {
            if (!CurrentUser.Roles.Any()) return new List<TreeNode>();

            //後期需要根據賬戶權限，獲取菜單列表生成樹狀節點
            //var menus = GetMenus();
            var menus = new List<MenuItem>();

            if (CurrentUser.Roles[0].Name == SystemUserRole.SuperAdmin.ToString())
            {
                menus = GetActiveMenus();
            }
            else
            {
                menus = GetMenusByUser();
            }

            var result = GenTreeNodes(menus, 0, null, 0);
            if (menus.Any())//處理用戶有子節點權限，不過沒有父節點權限。將有權限的子節點添加到樹的最後
            {
                foreach (var item in menus)
                {
                    TreeNode node = new TreeNode();
                    node.Text = item.Name;
                    node.Code = item.Code;
                    node.Id = item.Id;
                    node.Level = 1;
                    node.Img = item.ImgUrl;
                    //node.ImgPath = PathUtil.GetMenuIconImagePath(mWebStoreConfig.ClientId.ToString(), item.ImgUrl);
                    node.Url = item.PageUrl;
                    node.Collapse = true;
                    node.Path = item.PageUrl;
                    node.Seq = item.Seq;
                    node.IsMobileEnable = item.IsMobileEnable;
                    node.IsHomeItem = item.IsHomeItem;
                    // node.IsActive = item.IsActive;
                    result.Add(node);

                }
            }
            return result;

        }

        public List<MenuItem> GetActiveMenus()
        {
            var result = new List<MenuItem>();

            var query = (from a in baseRepository.GetList<SystemMenu>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = a.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         where a.IsDeleted == false && a.IsActive == true && tt.IsActive && !tt.IsDeleted
                         
                         select new MenuItem
                         {
                             Id = a.Id,
                             Code = a.Code,
                             Img = "",
                             ImgUrl = a.ImgUrl,
                             PageUrl = a.PageUrl,
                             ParentId = a.ParentId,
                             NameTransId = a.NameTransId,
                             FunctionId = a.FunctionId,
                             ModuleId = a.ModuleId,
                             Seq = a.Seq,
                             IsMobileEnable = a.IsMobileEnable,
                             IsHomeItem = a.IsHomeItem,
                             Name = tt.Value,
                         }).Distinct();
           
            result = query.OrderBy(o => o.ParentId).ThenBy(o => o.Seq).ToList();
            return result;
        }

        public List<MenuItem> GetMenusByUser()
        {
            var result = new List<MenuItem>();

            var query =( from s in baseRepository.GetList<User>(x => x.IsActive && !x.IsDeleted)
                        join sr in baseRepository.GetList<UserRole>(x => x.IsActive && !x.IsDeleted) on s.Id equals sr.UserId
                        join rp in baseRepository.GetList<RolePermission>(x => x.IsActive && !x.IsDeleted) on sr.RoleId equals rp.RoleId
                        join d in baseRepository.GetList<SystemMenu>(x => x.IsActive && !x.IsDeleted) on rp.PermissionId equals d.PermissionId
                        join t in baseRepository.GetList<Translation>(x => x.IsActive && !x.IsDeleted) on d.NameTransId equals t.TransId into tc
                        from tt in tc.DefaultIfEmpty()
                        where s.Id == Guid.Parse(CurrentUser.UserId) && tt.Lang == CurrentUser.Lang
                        select new MenuItem
                        {
                            Id = d.Id,
                            Code = d.Code,
                            Img = "",
                            ImgUrl = d.ImgUrl,
                            PageUrl = d.PageUrl,
                            ParentId = d.ParentId,
                            NameTransId = d.NameTransId,
                            FunctionId = d.FunctionId,
                            ModuleId = d.ModuleId,
                            Seq = d.Seq,
                            IsMobileEnable = d.IsMobileEnable,
                            IsHomeItem = d.IsHomeItem,
                            Name = tt.Value,
                        }).Distinct();

            result = query.OrderBy(o => o.ParentId).ThenBy(o => o.Seq).ToList();
            return result;
        }

        private List<TreeNode> GenTreeNodes(List<MenuItem> menus, int parentId, TreeNode parent, int level)
        {
            var result = new List<TreeNode>();
            List<MenuItem> subMenus = menus.Where(d => d.ParentId == parentId).ToList();
            foreach (var a in subMenus)
            {
                menus.Remove(a);
            }

            foreach (var item in subMenus)
            {
                TreeNode node = new TreeNode();
                node.Code = item.Code;
                node.Text = item.Name;
                node.Id = item.Id;
                node.Level = level + 1;
                node.Img = item.ImgUrl;
                //node.ImgPath = PathUtil.GetMenuIconImagePath(mWebStoreConfig.ClientId.ToString(), item.ImgUrl);
                node.Url = item.PageUrl;
                node.Collapse = true;
                node.Path = item.PageUrl;
                node.Seq = item.Seq;
                node.IsMobileEnable = item.IsMobileEnable;
                node.IsHomeItem = item.IsHomeItem;
                //node.IsActive = item.IsActive;
                result.Add(node);

                var childs = menus.Where(d => d.ParentId == item.Id);
                if (childs.Count() > 0)
                {
                    node.Children = GenTreeNodes(menus, node.Id, node, node.Level);
                }
            }

            return result;

        }
    }
}
