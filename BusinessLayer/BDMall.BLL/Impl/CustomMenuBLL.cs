using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Runtime;
using BDMall.Utility;
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
    public class CustomMenuBLL : BaseBLL, ICustomMenuBLL
    {
        //private static IDictionary<string, List<SimpleCurrency>> AllCurrencies;
        //private static SimpleCurrency DefaultCurrency;
        //ITranslationRepository translationRepository;
        ////public ICurrExchangeRateRwpository CurrExchangeRateRwpository { get; set; }
        //ISettingBLL settingBLL;

        ICodeMasterRepository _codeMasterRepository;
        ITranslationRepository _translationRepository;
        ICustomMenuRepository CustomMenuRepo;

        public CustomMenuBLL(IServiceProvider services) : base(services)
        {
            _codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            _translationRepository = Services.Resolve<ITranslationRepository>();
            CustomMenuRepo = Services.Resolve<ICustomMenuRepository>();
        }

        public List<KeyValue> GetMenuPosition(string functions, string key, int type)
        {
            List<KeyValue> lis = new List<KeyValue>();
            var query = _codeMasterRepository.GetCodeMasters(Guid.Empty, CodeMasterModule.Setting.ToString(), functions, key);
            if (query.Count > 0)
            {
                foreach (var item in query)
                {
                    KeyValue keyValue = new KeyValue();
                    keyValue.Id = item.Value;
                    keyValue.Text = item.Description;
                    lis.Add(keyValue);
                }
            }

            if (functions == CodeMasterFunction.CustomMenuType.ToString())
            {
                if (type == 0)
                {
                    return lis;
                }
                else if (type == 1)
                {
                    var data = lis.Where(p => p.Id != "0").ToList();
                    return data;
                }
            }
            else if (functions == CodeMasterFunction.CustomMenuPosition.ToString())
            {
                return lis;
            }
            return null;
        }


        public List<MenuTree> GetRootMenuTree(int position)
        {
            return CustomMenuRepo.GetRootMenuTree(position);
        }

        public List<MenuTree> GetMenuTreeById(Guid id)
        {
            return CustomMenuRepo.GetMenuTreeById(id);
        }

        public SystemResult GetHeaderMenu(Guid id)
        {
            return CustomMenuRepo.GetHeaderMenu(id);
        }

        public PageData<TypeDetail> SearchResult(TypeDetailCond cond)
        {
            return CustomMenuRepo.SearchResult(cond);
        }

        public SystemResult GetMenuBar()
        {
            SystemResult result = new SystemResult();

            //var key = CacheTypeModule.MenuBar.ToString() + "_" + CurrentUser.Language.ToString();
            //var menuBar = CacheManager.Get<MenuModel>(key);

            try
            {
                //if (menuBar == null)
                //{
                var menu = CustomMenuRepo.GetMenuBar();
                //CacheManager.Insert(key, menu);
                result.Succeeded = true;
                result.ReturnValue = menu;
                //}
                //else
                //{
                //    result.Succeeded = true;
                //    result.ReturnValue = menuBar;
                //}
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ReturnValue = null;
            }

            return result;
        }

        public List<MutiLanguage> GetMutiLanguage(Guid transId)
        {
            return CustomMenuRepo.GetMutiLanguage(transId);
        }

        public CustomMenuDto GetCustomMenuById(Guid id)
        {
            return CustomMenuRepo.GetCustomMenuById(id);
        }

        public List<CustomMenuDetailDto> GetCustomMenuDetailByMenuId(Guid menuId)
        {
            return CustomMenuRepo.GetCustomMenuDetailByMenuId(menuId);
        }

        public bool VerifySeq(MenuDetailInfo info)
        {
            return CustomMenuRepo.VerifySeq(info);
        }

        /// <summary>
        /// 刪除自定義菜單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemResult DeleteMenu(Guid id)
        {
            SystemResult result = new SystemResult();
            try
            {
                var menu = GetCustomMenuById(id);
                if (menu != null)
                {
                    menu.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    menu.UpdateDate = DateTime.Now;
                    menu.IsDeleted = true;

                    var menuDetails = GetCustomMenuDetailByMenuId(id);
                    if (menuDetails != null)
                    {
                        foreach (var item in menuDetails)
                        {
                            item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                            item.UpdateDate = DateTime.Now;
                            item.IsDeleted = true;
                        }
                    }
                    UnitOfWork.Submit();
                }
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 保存自定义排序
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public SystemResult SaveSeq(MenuCond cond)
        {
            SystemResult result = new SystemResult();
            try
            {
                UnitOfWork.IsUnitSubmit = true;
                foreach (var item in cond.menus)
                {
                    var menu = baseRepository.GetList<CustomMenu>().FirstOrDefault(p => p.Id == item.Id);// GetCustomMenuById(item.Id);
                    if (menu != null)
                    {
                        menu.Seq = item.Seq;
                        baseRepository.Update(menu);
                    }
                }
                UnitOfWork.Submit();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 保存自定義菜單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public SystemResult SaveMenu(MenuDetailInfo info)
        {
            SystemResult result = new SystemResult();
            try
            {
                UnitOfWork.IsUnitSubmit = true;

                //info.Seq = CustomMenuRepo.GetCustomMenuSeq(info);

                if (info.Id == Guid.Empty)
                {
                    InsertCusMenu(info);
                }
                else
                {
                    UpdateCusMenu(info);
                }

                UpdateCusMenuDetais(info);

                UnitOfWork.Submit();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        private void UpdateCusMenuDetais(MenuDetailInfo info)
        {
            var dbDetails = GetCustomMenuDetailByMenuId(info.Id);
            if (dbDetails != null)
            {
                baseRepository.Delete(dbDetails);
            }

            if (info.Details != null)
            {
                List<CustomMenuDetail> details = new List<CustomMenuDetail>();
                foreach (var item in info.Details)
                {
                    if (!string.IsNullOrEmpty(item.ValueId))
                    {
                        CustomMenuDetail detail = new CustomMenuDetail();
                        detail.Id = Guid.NewGuid();
                        detail.CreateBy = Guid.Parse(CurrentUser.UserId);
                        detail.CreateDate = DateTime.Now;
                        detail.IsActive = true;
                        detail.IsDeleted = false;
                        detail.MenuId = info.Id;
                        detail.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        detail.UpdateDate = DateTime.Now;
                        detail.Seq = item.Seq;
                        if (info.Type == (int)CustomMenuType.Link)
                        {
                            detail.Value = item.ValueName ?? "";
                        }
                        else
                        {
                            detail.Value = item.ValueId;
                        }

                        if (info.Type == (int)CustomMenuType.CMSCategory || info.Type == (int)CustomMenuType.CMSContent)
                        {
                            detail.IsAnchor = item.IsAnchor;
                        }
                        else
                        {
                            detail.IsAnchor = false;
                        }
                        details.Add(detail);
                    }
                }

                if (details.Count > 0)
                {
                    baseRepository.Insert(details);
                }
            }
        }

        private void UpdateCusMenu(MenuDetailInfo info)
        {
            var menu = GetCustomMenuById(info.Id);
            if (menu != null)
            {
                menu.LinkType = info.Type;
                menu.Seq = info.Seq;
                menu.ShowNext = info.ShowSub;
                menu.UpdateBy = Guid.Parse(CurrentUser.UserId);
                menu.UpdateDate = DateTime.Now;
                menu.RedirectLinkType = info.RedirectType;
                menu.RedirectLinkValue = info.RedirectValue ?? Guid.Empty.ToString();
                menu.IsNewWin = info.IsNewWin;
                menu.IsShow = info.IsShow;

                if (info.Type == (int)CustomMenuType.Link)
                {
                    menu.PlaceTop = false;
                }
                else
                {
                    menu.PlaceTop = info.PlacedTop;
                }

                if (info.Details?.Count() != 1)
                {
                    menu.PlaceTop = false;
                }
                MoveImage(menu.ImageTransId, info.Images);
                _translationRepository.UpdateMutiLanguage(menu.NameTransId, info.Names, TranslationType.CustomMenu);
                _translationRepository.UpdateMutiLanguage(menu.TitleTransId, info.Titles, TranslationType.CustomMenu);
            }
        }

        private void InsertCusMenu(MenuDetailInfo info)
        {
            CustomMenu menu = new CustomMenu();
            menu.Id = Guid.NewGuid();
            menu.ParentId = info.ParentId;
            menu.CreateBy = Guid.Parse(CurrentUser.UserId);
            menu.CreateDate = DateTime.Now;
            menu.ImageTransId = MoveImage(Guid.Empty, info.Images);
            menu.NameTransId = _translationRepository.InsertMutiLanguage(info.Names, TranslationType.CustomMenu);
            menu.TitleTransId = _translationRepository.InsertMutiLanguage(info.Titles, TranslationType.CustomMenu);
            menu.Seq = info.Seq;
            menu.Key = StringUtil.GenerateDateString("CM");
            menu.LinkType = info.Type;
            menu.PositionType = info.Position;
            menu.ShowNext = info.ShowSub;
            menu.IsNewWin = info.IsNewWin;
            menu.IsActive = true;
            menu.IsDeleted = false;
            menu.IsShow = info.IsShow;
            menu.UpdateBy = Guid.Parse(CurrentUser.UserId);
            menu.UpdateDate = DateTime.Now;
            menu.PlaceTop = info.PlacedTop;
            menu.RedirectLinkType = info.RedirectType;
            menu.RedirectLinkValue = info.RedirectValue ?? Guid.Empty.ToString();
            if (info.Details?.Count() != 1)
            {
                menu.PlaceTop = false;
            }
            info.Id = menu.Id;
            baseRepository.Insert(menu);
        }

        private Guid MoveImage(Guid imgId, List<MutiLanguage> images)
        {
            List<string> delList = new List<string>();

            foreach (var img in images)
            {
                var fileName = Path.GetFileName(img.Desc);
                var tempFilePath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.CustomMenu) + fileName;

                var newImgName = Guid.NewGuid().ToString() + Path.GetExtension(img.Desc);
                var menuImgPath = PathUtil.GetRelativePath(CurrentUser.MerchantId.ToString(), FileFolderEnum.CustomMenu) + "/" + newImgName;

                if (File.Exists(tempFilePath))
                {
                    FileUtil.CopyFile(tempFilePath, menuImgPath, newImgName);
                    img.Desc = menuImgPath;
                }
            }

            delList = delList.Distinct().ToList();

            foreach (var item in delList)
            {
                FileUtil.DeleteFile(item);
            }

            if (imgId == Guid.Empty)
            {
                return _translationRepository.InsertMutiLanguage(images, TranslationType.CustomMenu);
            }
            else
            {
                return _translationRepository.UpdateMutiLanguage(imgId, images, TranslationType.CustomMenu);
            }
        }

    }
}
