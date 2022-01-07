using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class SystemMenuBLL : BaseBLL, ISystemMenuBLL
    {
        ISettingBLL settingBLL;
        ITranslationRepository _translationRepo;
        public SystemMenuBLL(IServiceProvider services) : base(services)
        {
            settingBLL = Services.Resolve<ISettingBLL>();
            _translationRepo = Services.Resolve<ITranslationRepository>();
        }
        public List<TreeNode> GetMenuTreeNodes()
        {
            var menus = GetMenus();
            var tree = GenTreeNodes(menus, 0, null, 0);
            return tree;
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

            var query = (from s in baseRepository.GetList<User>(x => x.IsActive && !x.IsDeleted)
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
        public List<MenuItem> GetMenus()
        {
            var result = new List<MenuItem>();

            //var langs = GetSupportLanguage();

            var query = (from a in baseRepository.GetList<SystemMenu>()
                         select new MenuItem
                         {
                             Id = a.Id,
                             Code = a.Code,
                             Img = "",//Path.GetFileName(a.ImgUrl)
                             ImgUrl = a.ImgUrl,
                             PageUrl = a.PageUrl,
                             ParentId = a.ParentId,
                             NameTransId = a.NameTransId,
                             FunctionId = a.FunctionId,
                             ModuleId = a.ModuleId,
                             Seq = a.Seq,
                             IsActive = a.IsActive,
                             IsDeleted = a.IsDeleted,
                             IsMobileEnable = a.IsMobileEnable,
                             IsHomeItem = a.IsHomeItem,
                         }).ToList();
            foreach (var item in query)
            {
                item.NameTranslation = _translationRepo.GetMutiLanguage(item.NameTransId);
                item.Name = item.NameTranslation.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
                result.Add(item);
            }

            return result;
        }

        public MenuItem GetMenu(int menuId)
        {

            MenuItem menuItem = new MenuItem();
            var result = baseRepository.GetList<SystemMenu>().FirstOrDefault(p => p.Id == menuId);

            if (result != null)
            {
                //var langs = SettingBLL.GetSupportLanguages();
                //var trans = _translationRepo.GetTranslation(result.NameTransId);
                menuItem.Id = result.Id;
                menuItem.Code = result.Code;
                menuItem.Img = Path.GetFileName(result.ImgUrl);
                menuItem.ImgUrl = result.ImgUrl;
                menuItem.PageUrl = result.PageUrl;
                menuItem.ParentId = result.ParentId;
                menuItem.NameTransId = result.NameTransId;
                menuItem.FunctionId = result.FunctionId;
                menuItem.ModuleId = result.ModuleId;
                menuItem.Seq = result.Seq;
                menuItem.IsActive = result.IsActive;
                menuItem.IsHomeItem = result.IsHomeItem;
                menuItem.IsMobileEnable = result.IsMobileEnable;

                menuItem.NameTranslation = _translationRepo.GetMutiLanguage(result.NameTransId);
            }


            return menuItem;


        }

        public void SaveMenu(MenuItem item)
        {
            string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.TempPath);
            //保存圖片
            UnitOfWork.IsUnitSubmit = true;

            if (!string.IsNullOrEmpty(item.Img))
            {
                string tempFileFullName = Path.Combine(tempPath, item.Img);
                string targetFileName = item.Code + Path.GetExtension(item.Img);
                string targetPhysicalPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.MenuIcon);
                string targetRelativePartPath = PathUtil.GetRelativePath(CurrentUser.MerchantId.ToString(), FileFolderEnum.MenuIcon);

                if (File.Exists(tempFileFullName))
                {
                    var imageSize = settingBLL.GetSmallProductImageSize();

                    ImageUtil.CreateImg(tempFileFullName, targetPhysicalPath, targetFileName, imageSize.Width, imageSize.Length);//生成100*100的缩略图

                    item.Img = PathUtil.Combine(targetRelativePartPath, targetFileName);
                }
                else
                {
                    item.Img = PathUtil.Combine(targetRelativePartPath, targetFileName);
                }
            }

            if (item.Id == 0)
            {
                AddMenuItem(item);
            }
            else
            {
                UpadteMenuItem(item);
            }

            //更新多語言
            foreach (var lang in item.NameTranslation)
            {
                var trans = _translationRepo.GetTranslation(item.NameTransId);
                var oldTrans = trans.FirstOrDefault(p => p.Lang == (Language)Enum.Parse(typeof(Language), lang.Lang.Code));
                if (oldTrans != null)
                {
                    oldTrans.Value = lang.Desc;
                    oldTrans.UpdateDate = DateTime.Now;
                    oldTrans.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    baseRepository.Update(oldTrans);
                }
                else
                {
                    Translation newTrans = new Translation();
                    newTrans.Id = Guid.NewGuid();
                    newTrans.TransId = item.NameTransId;
                    newTrans.Lang = (Language)Enum.Parse(typeof(Language), lang.Lang.Code);
                    newTrans.Value = lang?.Desc ?? "";
                    newTrans.Module = TranslationType.SystemMenu.ToString();
                    newTrans.CreateDate = DateTime.Now;
                    newTrans.CreateBy = Guid.Parse(CurrentUser.UserId);

                    baseRepository.Insert(newTrans);
                }

                //if (lang.Lang != null)
                //{

                //}
                //else
                //{
                //    Translation newTrans = new Translation();
                //    newTrans.Id = Guid.NewGuid();
                //    newTrans.TransId = item.NameTransId;
                //    newTrans.Lang = (Language)Enum.Parse(typeof(Language), lang.Lang.Code);
                //    newTrans.Value = lang?.Desc ?? "";
                //    newTrans.Module = TranslationType.SystemMenu.ToString();
                //    baseRepository.Insert(newTrans);
                //}

            }

            UnitOfWork.Submit();


        }

        public SystemResult AddMenuItem(MenuItem item)
        {
            if (item.NameTranslation == null || item.NameTranslation.Count == 0)
            {
                throw new BLException("Name can not be empty.");
            }
            SystemResult result = new SystemResult();

            var transId = Guid.NewGuid();
            var systemMenu = new SystemMenu
            {
                ImgUrl = item.Img,
                Code = item.Code,
                PageUrl = item.PageUrl,
                ParentId = item.ParentId,
                ModuleId = item.ModuleId,
                FunctionId = item.FunctionId,
                IsHomeItem = item.IsHomeItem,
                IsMobileEnable = item.IsMobileEnable
            };
            if (item.FunctionId == Guid.Empty)
            {
                systemMenu.PermissionId = item.ModuleId;
            }
            else
            {
                systemMenu.PermissionId = item.FunctionId;
            }
            systemMenu.NameTransId = transId;

            baseRepository.Insert(systemMenu);

            UnitOfWork.Submit();
            result.Succeeded = true;
            item.NameTransId = transId;

            return result;
        }

        public SystemResult UpadteMenuItem(MenuItem item)
        {
            SystemResult result = new SystemResult();
            var systemMenu = baseRepository.GetList<SystemMenu>().FirstOrDefault(p => p.Id == item.Id);
            if (systemMenu != null)
            {
                systemMenu.ImgUrl = item.Img;
                systemMenu.Code = item.Code;
                systemMenu.ParentId = item.ParentId;
                systemMenu.PageUrl = item.PageUrl;
                systemMenu.ParentId = item.ParentId;
                systemMenu.FunctionId = item.FunctionId;
                systemMenu.ModuleId = item.ModuleId;
                systemMenu.IsActive = item.IsActive;
                systemMenu.IsHomeItem = item.IsHomeItem;
                systemMenu.IsMobileEnable = item.IsMobileEnable;

                if (item.FunctionId == Guid.Empty)
                {
                    systemMenu.PermissionId = item.ModuleId;
                }
                else
                {
                    systemMenu.PermissionId = item.FunctionId;
                }
                baseRepository.Update(systemMenu);
                UnitOfWork.Submit();
                result.Succeeded = true;
            }
            else
            {
                throw new BLException("没有找到对应的菜单项目");
            }

            return result;
        }

        public bool RemoveMenuItem(MenuItem item)
        {
            return RemoveMenuItem(item.Id);
        }

        public bool RemoveMenuItem(int menuId)
        {
            var record = baseRepository.GetList<SystemMenu>().FirstOrDefault(p => p.Id == menuId);
            baseRepository.Delete(record);
            return true;
            //if (record > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

        }

        public void UpdateSystemMenuSeq(List<TreeNode> list)
        {
            list = list.Where(p => p.IsChange == true).ToList();
            foreach (TreeNode item in list)
            {
                var menu = baseRepository.GetList<SystemMenu>().FirstOrDefault(p => p.Id == item.Id);
                //var menu = mDBContext.SystemMenu.Where(p => p.MenuId == item.Id).FirstOrDefault();
                if (menu != null)
                {
                    menu.Seq = item.Seq;
                    baseRepository.Update(menu);
                }

            }
            UnitOfWork.Submit();
            //mDBContext.SubmitChanges();
        }

        public bool CheckMenuCodeIsExists(string code)
        {

            //ISystemMenuBLL bll = BLLFactory.Create(mWebStoreConfig).CreateSystemMenuBLL();
            var menu = baseRepository.GetList<SystemMenu>().FirstOrDefault(p => p.Code == code);
            if (menu != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
