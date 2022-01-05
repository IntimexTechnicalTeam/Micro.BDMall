using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Framework;

namespace BDMall.Repository
{
    public class CustomMenuRepository : PublicBaseRepository, ICustomMenuRepository
    {
        public CustomMenuRepository(IServiceProvider service) : base(service)
        {
        }
        /// <summary>
        /// 獲取根節點Menu
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<MenuTree> GetRootMenuTree(int position)
        {
            List<MenuTree> tree = new List<MenuTree>();
            var query = (from d in baseRepository.GetList<CustomMenu>()
                         where d.PositionType == position
                         && d.ParentId == Guid.Empty
                         && d.IsActive == true
                         && d.IsDeleted == false
                         orderby d.Seq
                         select d
                    ).ToList();

            foreach (var item in query)
            {
                tree.Add(new MenuTree
                {
                    Id = item.Id,
                    Name = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "",
                    Title = GetMutiLanguage(item.TitleTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "",
                    IsNewWin = item.IsNewWin,
                    Seq = item.Seq,
                });
            }
            return tree;
        }

        /// <summary>
        /// 獲取節點下的樹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MenuTree> GetMenuTreeById(Guid id)
        {

            List<MenuTree> tree = new List<MenuTree>();
            var query = (from d in baseRepository.GetList<CustomMenu>()
                         where d.ParentId == id
                         && d.IsActive == true
                         && d.IsDeleted == false
                         orderby d.Seq
                         select d).ToList();

            foreach (var item in query)
            {
                tree.Add(new MenuTree
                {
                    Id = item.Id,
                    Name = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "",
                    Title = GetMutiLanguage(item.TitleTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "",
                    IsNewWin = item.IsNewWin,
                });
            }
            return tree;

        }

        /// <summary>
        /// 獲取菜單信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemResult GetHeaderMenu(Guid id)
        {
            SystemResult result = new SystemResult();

            if (id == Guid.Empty)
            {
                MenuDetailInfo info = new MenuDetailInfo(); ;
                info.Id = Guid.Empty;
                info.ParentId = Guid.Empty;
                info.Images = GetMutiLanguage(Guid.NewGuid());
                info.IsShow = true;
                info.Names = GetMutiLanguage(Guid.NewGuid());
                info.Titles = GetMutiLanguage(Guid.NewGuid());
                info.Seq = 0;
                info.ShowSub = false;
                info.Type = (int)CustomMenuType.Link;
                info.Position = (int)CustomMenuPosition.Header;
                info.Details = new List<TypeDetail>();
                info.PlacedTop = false;
                info.RedirectType = -1;
                info.IsNewWin = false;
                info.Details.Add(new TypeDetail
                {
                    Id = Guid.Empty,
                    ValueId = "",
                    ValueName = "",
                    Seq = 0,
                    IsAnchor = false

                });

                GetMenuRedirect(info);
                result.ReturnValue = info;
            }
            else
            {
                var info = (from d in baseRepository.GetList<CustomMenu>()
                            where d.IsActive == true && d.IsDeleted == false && d.Id == id
                            select new MenuDetailInfo
                            {
                                Id = d.Id,
                                ParentId = d.ParentId,
                                ImageTransId = d.ImageTransId,
                                NameTransId = d.NameTransId,
                                TitleTransId = d.TitleTransId,
                                IsShow = d.IsShow,
                                Position = d.PositionType,
                                PlacedTop = d.PlaceTop,
                                Seq = d.Seq,
                                Type = d.LinkType,
                                ShowSub = d.ShowNext,
                                RedirectType = d.RedirectLinkType,
                                IsNewWin = d.IsNewWin,
                            }).FirstOrDefault();

                if (info != null)
                {
                    info.Details = GetMenuDeitals(info);
                    GetMenuRedirect(info);

                    info.Images = GetMutiLanguage(info.ImageTransId);
                    info.Names = GetMutiLanguage(info.NameTransId);
                    info.Titles = GetMutiLanguage(info.TitleTransId);
                }
                result.ReturnValue = info;
            }

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 根據menu的Type獲取對應的信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<TypeDetail> GetMenuDeitals(MenuDetailInfo info)
        {
            List<TypeDetail> list = new List<TypeDetail>();

            switch (info.Type)
            {
                case (int)CustomMenuType.Link:
                    list = (from d in baseRepository.GetList<CustomMenu>()
                            join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                            where d.Id == info.Id
                            select new TypeDetail
                            {
                                Id = a.Id,
                                ValueId = a.Value,
                                ValueName = a.Value,
                                Seq = a.Seq,
                                IsAnchor = false
                            }).ToList();
                    if (list.Count == 0)
                    {
                        list.Add(new TypeDetail
                        {
                            Id = Guid.Empty,
                            ValueId = "",
                            ValueName = "",
                            Seq = 0,
                            IsAnchor = false
                        });
                    }
                    break;
                case (int)CustomMenuType.CMSCategory:
                    var catequery = (from d in baseRepository.GetList<CustomMenu>()
                                     join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                                     join t in baseRepository.GetList<CMSCategory>() on a.Value equals t.Id.ToString()
                                     where d.Id == info.Id
                                     select new
                                     {
                                         Id = a.Id,
                                         ValueId = a.Value,
                                         Seq = a.Seq,
                                         IsAnchor = a.IsAnchor,
                                         t.Name_E,
                                         t.Name_C,
                                         t.Name_S,
                                         t.Name_J,
                                     }).ToList();

                    foreach (var item in catequery)
                    {
                        TypeDetail typeDetail = new TypeDetail();
                        typeDetail.Id = item.Id;
                        typeDetail.ValueId = item.ValueId;
                        typeDetail.Seq = item.Seq;
                        typeDetail.IsAnchor = item.IsAnchor;

                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                typeDetail.ValueName = item.Name_E;
                                break;
                            case Language.C:
                                typeDetail.ValueName = item.Name_C;
                                break;
                            case Language.S:
                                typeDetail.ValueName = item.Name_S;
                                break;
                            case Language.J:
                                typeDetail.ValueName = item.Name_J;
                                break;
                        }
                        list.Add(typeDetail);
                    }
                    break;
                case (int)CustomMenuType.CMSContent:
                    var contentquery = (from d in baseRepository.GetList<CustomMenu>()
                                        join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                                        join t in baseRepository.GetList<CMSContent>() on a.Value equals t.Id.ToString()
                                        where d.Id == info.Id
                                        select new
                                        {
                                            Id = a.Id,
                                            ValueId = a.Value,
                                            Seq = a.Seq,
                                            IsAnchor = a.IsAnchor,
                                            t.Name_E,
                                            t.Name_C,
                                            t.Name_S,
                                            t.Name_J,
                                        }).ToList();

                    foreach (var item in contentquery)
                    {
                        TypeDetail typeDetail = new TypeDetail();
                        typeDetail.Id = item.Id;
                        typeDetail.ValueId = item.ValueId;
                        typeDetail.Seq = item.Seq;
                        typeDetail.IsAnchor = item.IsAnchor;

                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                typeDetail.ValueName = item.Name_E;
                                break;
                            case Language.C:
                                typeDetail.ValueName = item.Name_C;
                                break;
                            case Language.S:
                                typeDetail.ValueName = item.Name_S;
                                break;
                            case Language.J:
                                typeDetail.ValueName = item.Name_J;
                                break;
                        }
                        list.Add(typeDetail);
                    }
                    break;
                case (int)CustomMenuType.RegNPay:
                    list = (from d in baseRepository.GetList<CustomMenu>()
                            join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                            join t in baseRepository.GetList<RnpForm>() on new { a1 = a.Value, a2 = false } equals new { a1 = t.Id.ToString(), a2 = t.IsDeleted }
                            where d.Id == info.Id
                            select new TypeDetail
                            {
                                Id = a.Id,
                                ValueId = a.Value,
                                ValueName = t.Title,
                                Seq = a.Seq,
                                IsAnchor = false
                            }).ToList();
                    break;
                case (int)CustomMenuType.Catalog:
                    list = (from d in baseRepository.GetList<CustomMenu>()
                            join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                            join t in baseRepository.GetList<ProductCatalog>() on a.Value equals t.Id.ToString()
                            join c in baseRepository.GetList<Translation>() on new { a1 = t.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = c.TransId, a2 = c.Lang }
                            where d.Id == info.Id
                            select new TypeDetail
                            {
                                Id = a.Id,
                                ValueId = a.Value,
                                ValueName = c.Value,
                                Seq = a.Seq,
                                IsAnchor = false
                            }).ToList();
                    break;
                case (int)CustomMenuType.Attribute:
                    list = (from d in baseRepository.GetList<CustomMenu>()
                            join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                            join t in baseRepository.GetList<ProductAttribute>() on a.Value equals t.Id.ToString()
                            join c in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = c.TransId, a2 = c.Lang }
                            where d.Id == info.Id
                            select new TypeDetail
                            {
                                Id = a.Id,
                                ValueId = a.Value,
                                ValueName = c.Value,
                                Seq = a.Seq,
                                IsAnchor = false
                            }).ToList();
                    break;
                case (int)CustomMenuType.AttributeValue:
                    list = (from d in baseRepository.GetList<CustomMenu>()
                            join a in baseRepository.GetList<CustomMenuDetail>() on d.Id equals a.MenuId
                            join t in baseRepository.GetList<ProductAttributeValue>() on a.Value equals t.Id.ToString()
                            join c in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = c.TransId, a2 = c.Lang }
                            where d.Id == info.Id
                            select new TypeDetail
                            {
                                Id = a.Id,
                                ValueId = a.Value,
                                ValueName = c.Value,
                                Seq = a.Seq,
                                IsAnchor = false
                            }).ToList();
                    break;
            }
            return list;
        }

        private void GetMenuRedirect(MenuDetailInfo info)
        {
            info.RedirectValue = "";
            info.RedirectName = "";
            switch (info.RedirectType)
            {
                case (int)CustomMenuType.Link:
                    var a = (from d in baseRepository.GetList<CustomMenu>()
                             where d.Id == info.Id
                             select d
                             ).FirstOrDefault();
                    if (a != null)
                    {
                        info.RedirectValue = a.RedirectLinkValue;
                        info.RedirectName = a.RedirectLinkValue;
                    }
                    break;
                case (int)CustomMenuType.CMSCategory:
                    var b = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<CMSCategory>() on d.RedirectLinkValue equals t.Id.ToString()
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 t.Name_E,
                                 t.Name_C,
                                 t.Name_S,
                                 t.Name_J,
                             }
                           ).FirstOrDefault();

                    if (b != null)
                    {
                        info.RedirectValue = b.value;

                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                info.RedirectName = b.Name_E;
                                break;
                            case Language.C:
                                info.RedirectName = b.Name_C;
                                break;
                            case Language.S:
                                info.RedirectName = b.Name_S;
                                break;
                            case Language.J:
                                info.RedirectName = b.Name_J;
                                break;
                        }
                    }
                    break;
                case (int)CustomMenuType.CMSContent:
                    var c = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<CMSContent>() on d.RedirectLinkValue equals t.Id.ToString()
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 t.Name_E,
                                 t.Name_C,
                                 t.Name_S,
                                 t.Name_J,
                             }).FirstOrDefault();

                    if (c != null)
                    {
                        info.RedirectValue = c.value;

                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                info.RedirectName = c.Name_E;
                                break;
                            case Language.C:
                                info.RedirectName = c.Name_C;
                                break;
                            case Language.S:
                                info.RedirectName = c.Name_S;
                                break;
                            case Language.J:
                                info.RedirectName = c.Name_J;
                                break;
                        }
                    }
                    break;
                case (int)CustomMenuType.RegNPay:
                    var e = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<RnpForm>() on d.RedirectLinkValue equals t.Id.ToString()
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 name = t.Title
                             }).FirstOrDefault();

                    if (e != null)
                    {
                        info.RedirectValue = e.value;
                        info.RedirectName = e.name;
                    }
                    break;
                case (int)CustomMenuType.Catalog:
                    var f = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<ProductCatalog>() on d.RedirectLinkValue equals t.Id.ToString()
                             join cc in baseRepository.GetList<Translation>() on new { a1 = t.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = cc.TransId, a2 = cc.Lang }
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 name = cc.Value
                             }).FirstOrDefault();

                    if (f != null)
                    {
                        info.RedirectValue = f.value;
                        info.RedirectName = f.name;
                    }
                    break;
                case (int)CustomMenuType.Attribute:
                    var g = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<ProductAttribute>() on d.RedirectLinkValue equals t.Id.ToString()
                             join cc in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = cc.TransId, a2 = cc.Lang }
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 name = cc.Value
                             }).FirstOrDefault();

                    if (g != null)
                    {
                        info.RedirectValue = g.value;
                        info.RedirectName = g.name;
                    }
                    break;
                case (int)CustomMenuType.AttributeValue:
                    var h = (from d in baseRepository.GetList<CustomMenu>()
                             join t in baseRepository.GetList<ProductAttributeValue>() on d.RedirectLinkValue equals t.Id.ToString()
                             join cc in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = cc.TransId, a2 = cc.Lang }
                             where d.Id == info.Id
                             select new
                             {
                                 value = d.RedirectLinkValue,
                                 name = cc.Value
                             }).FirstOrDefault();

                    if (h != null)
                    {
                        info.RedirectValue = h.value;
                        info.RedirectName = h.name;
                    }
                    break;
            }
        }

        /// <summary>
        /// 根據Type查詢CustomMenu的數據
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public PageData<TypeDetail> SearchResult(TypeDetailCond cond)
        {
            try
            {
                return GetCusMenuDatas(cond);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 獲取頭部、底部菜單
        /// </summary>
        /// <returns></returns>
        public MenuModel GetMenuBar()
        {
            //SystemResult result = new SystemResult();
            try
            {
                MenuModel model = new MenuModel();
                model.HeaderMenus = GetHeaderMenus();
                model.FooterMenus = GetFooterMenus();
                return model;
                //result.Succeeded = true;
                //result.ReturnValue = model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return result;
        }

        private List<FrontMenuDetailModel> GetHeaderMenus()
        {
            return GetMenus((int)CustomMenuPosition.Header);
        }

        private List<FrontMenuDetailModel> GetFooterMenus()
        {
            return GetMenus((int)CustomMenuPosition.Footer);
        }

        private List<FrontMenuDetailModel> GetMenus(int type)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var menus = (from m in baseRepository.GetList<CustomMenu>()
                         where m.IsActive == true && m.IsDeleted == false && m.PositionType == type && m.IsShow == true
                         orderby m.Seq
                         select new MenuDetailModel
                         {
                             Id = m.Id,
                             Name = "",
                             Title = "",
                             Image = "",
                             Level = 0,
                             Type = m.LinkType,
                             Url = "",
                             Value = new ValueModel
                             {
                                 Id = ""
                             },
                             ParentId = m.ParentId.ToString(),
                             PlaceTop = m.PlaceTop,
                             RedirectType = m.RedirectLinkType,
                             RedirectValue = m.RedirectLinkValue,
                             IsNewWin = m.IsNewWin,
                             Seq = m.Seq,
                             NameTransId = m.NameTransId,
                             TitleTransId = m.TitleTransId,
                             ImageTransId = m.ImageTransId,
                         }).ToList();


            var transData = baseRepository.GetList<Translation>().Where(p => p.Lang == CurrentUser.Lang && p.Module == TranslationType.CustomMenu.ToString()).ToList();
            if (transData.Count > 0)
            {
                foreach (var item in menus)
                {
                    item.Name = transData.Where(p => p.TransId == item.NameTransId).FirstOrDefault()?.Value ?? "";
                    item.Title = transData.Where(p => p.TransId == item.TitleTransId).FirstOrDefault()?.Value ?? "";
                    item.Image = transData.Where(p => p.TransId == item.ImageTransId).FirstOrDefault()?.Value ?? "";
                }
            }

            var rootMenus = menus.Where(p => p.ParentId == Guid.Empty.ToString()).ToList();

            var attributes = baseRepository.GetList<ProductAttributeValue>().ToList();

            var result = GenTreeNodes(menus, rootMenus, Guid.Empty, 0, attributes);

            foreach (var item in result)
            {
                var a = new FrontMenuDetailModel();
                a.Childs = item.Childs;
                a.Id = item.Id;
                a.Image = item.Image == "" ? "" : item.Image;//PathUtil.GetPMServer() + item.Image;
                a.Level = item.Level;
                a.Name = item.Name;
                a.ParentId = item.ParentId;
                a.PlaceTop = item.PlaceTop;
                a.Title = item.Title;
                a.Type = item.Type;
                a.Url = item.Url;
                a.Value = item.Value;
                a.IsNewWin = item.IsNewWin;
                a.IsAnchor = item.IsAnchor;
                a.Seq = item.Seq;
                list.Add(a);
            }

            return list;
        }

        private List<FrontMenuDetailModel> GenTreeNodes(List<MenuDetailModel> sources, List<MenuDetailModel> roots, Guid parentId, int level, List<ProductAttributeValue> attributes)
        {
            List<FrontMenuDetailModel> result = new List<FrontMenuDetailModel>();
            level += 1;
            foreach (var item in roots)
            {
                FrontMenuDetailModel node = new FrontMenuDetailModel();
                if (parentId == Guid.Empty || level > 1)
                {
                    node.Id = item.Id;
                    node.Name = item.Name;
                    node.Title = item.Title;
                    node.Level = level;
                    node.Image = item.Image;
                    node.Type = item.Type;
                    node.Url = item.Url;
                    node.Value = item.Value;
                    node.PlaceTop = item.PlaceTop;
                    node.IsNewWin = item.IsNewWin;
                    node.Seq = item.Seq;

                    //node.RedirectType = item.RedirectType;
                    //node.RedirectValue = item.RedirectValue;
                    if (item.RedirectType == (int)CustomMenuType.Link)
                    {
                        node.Url = item.RedirectValue;
                        node.Type = (int)CustomMenuType.Link;
                    }
                    else
                    {
                        node.Type = item.RedirectType;
                        node.Value = new ValueModel { Id = item.RedirectValue };
                        item.Value = new ValueModel { Id = item.RedirectValue };
                    }
                    result.Add(node);
                }
                var childs = sources.Where(p => p.ParentId == item.Id.ToString()).ToList();
                if (childs.Count > 0)
                {
                    node.Childs = GenTreeNodes(sources, childs, item.Id, level, attributes);
                }
                else
                {
                    var childList = GetMenuDetailResult(item, attributes);

                    if (node.PlaceTop == false)//檢查該菜單是否置頂，如果是置頂則將菜單下的第一個數據作為菜單顯示
                    {

                        if (item.Type != (int)CustomMenuType.Link)
                        {
                            node.Childs = childList;
                        }
                        else
                        {
                            var info = childList.FirstOrDefault();
                            node.Url = info?.Url ?? "";
                        }

                        if (item.RedirectType != -1)
                        {
                            node.Type = item.RedirectType;
                            node.Url = item.RedirectValue;
                            node.Value = item.Value;

                            if (item.RedirectType == (int)CustomMenuType.AttributeValue)
                            {
                                node.ParentId = attributes.FirstOrDefault(p => p.Id.ToString() == item.RedirectValue).AttrId.ToString() == "" ? "" : attributes.FirstOrDefault(p => p.Id.ToString() == item.RedirectValue).AttrId.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (childList.Count == 1)
                        {
                            var firstVal = childList.FirstOrDefault();
                            node.Type = firstVal.Type;
                            node.Value = new ValueModel { Id = firstVal.Id.ToString() };
                            node.Name = firstVal.Name;
                            node.Title = firstVal.Title;
                            node.Image = firstVal.Image;
                            node.Value = firstVal.Value;
                            node.PlaceTop = firstVal.PlaceTop;
                            node.IsAnchor = firstVal.IsAnchor;
                            node.Childs = firstVal.Childs;
                        }
                    }
                }
            }
            return result;
        }

        private List<FrontMenuDetailModel> GetMenuDetailResult(MenuDetailModel model, List<ProductAttributeValue> attributes)
        {
            List<FrontMenuDetailModel> List = new List<FrontMenuDetailModel>();
            switch (model.Type)
            {
                case (int)CustomMenuType.Link:
                    List = GetMenuDetailLink(model.Id);
                    break;
                case (int)CustomMenuType.CMSCategory:
                    List = GetMenuCMSCategory(model.Id);
                    break;
                case (int)CustomMenuType.CMSContent:
                    List = GetMenuCMSContent(model.Id);
                    break;
                case (int)CustomMenuType.RegNPay:
                    List = GetMenuRegNPay(model.Id);
                    break;
                case (int)CustomMenuType.Catalog:
                    List = GetMenuCatagory(model.Id);
                    break;
                case (int)CustomMenuType.Attribute:
                    List = GetMenuAttribute(model.Id);
                    break;
                case (int)CustomMenuType.AttributeValue:
                    List = GetMenuAttributeValue(model.Id, attributes);
                    break;
            }
            return List;
        }

        /// <summary>
        /// 獲取菜單下的屬性值
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GetMenuAttributeValue(Guid menuId, List<ProductAttributeValue> attributes)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                           join c in baseRepository.GetList<ProductAttributeValue>() on d.Value equals c.Id.ToString()
                           where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId && c.IsDeleted == false
                           orderby d.Seq
                           select c).ToList();

            if (details.Count > 0)
            {
                foreach (var item in details)
                {
                    var attrId = (attributes.FirstOrDefault(p => p.Id == item.Id).AttrId).ToString();
                    list.Add(new FrontMenuDetailModel
                    {
                        Id = menuId,
                        Image = "",
                        Name = GetMutiLanguage(item.DescTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                        Title = "",
                        Level = 0,
                        ParentId = attrId,
                        Type = (int)CustomMenuType.AttributeValue,
                        Url = "",
                        Value = new ValueModel
                        {
                            Id = item.Id.ToString()
                        },
                        IsAnchor = false
                    });
                }
            }
            else
            {
                var attrs = baseRepository.GetList<ProductAttributeValue>().Where(p => p.IsDeleted == false).ToList();

                foreach (var item in attrs)
                {
                    list.Add(new FrontMenuDetailModel
                    {
                        Id = menuId,
                        Image = "",
                        Name = GetMutiLanguage(item.DescTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                        Title = "",
                        Level = 0,
                        ParentId = item.AttrId.ToString(),
                        Type = (int)CustomMenuType.AttributeValue,
                        Url = "",
                        Value = new ValueModel
                        {
                            Id = item.Id.ToString()
                        },
                    });
                }
            }
            return list;
        }

        private List<FrontMenuDetailModel> GetMenuAttribute(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var menu = baseRepository.GetList<CustomMenu>().FirstOrDefault(p => p.Id == menuId);

            if (menu != null)
            {
                var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                               join c in baseRepository.GetList<ProductAttribute>() on d.Value equals c.Id.ToString()
                               where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId && c.IsDeleted == false
                               && c.IsDeleted == false
                               orderby d.Seq
                               select c).ToList();

                if (details.Count > 0)
                {
                    foreach (var item in details)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = "",
                            Name = GetMutiLanguage(item.DescTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                            Title = "",
                            Level = 0,
                            ParentId = "0",
                            Type = (int)CustomMenuType.Attribute,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.Id.ToString()
                            },
                            IsAnchor = false
                        });
                    }
                }
                else
                {
                    var attrs = baseRepository.GetList<ProductAttribute>().Where(p => p.IsDeleted == false).ToList();

                    foreach (var item in attrs)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = "",
                            Name = GetMutiLanguage(item.DescTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                            Title = "",
                            Level = 0,
                            ParentId = "0",
                            Type = (int)CustomMenuType.Attribute,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.Id.ToString()
                            },
                        });
                    }
                }

                if (menu.ShowNext)
                {
                    var attributes = baseRepository.GetList<ProductAttributeValue>().Where(p => p.IsDeleted == false).ToList();
                    foreach (var item in list)
                    {
                        item.Childs = new List<FrontMenuDetailModel>();
                        var childs = attributes.Where(p => p.AttrId.ToString() == item.Value.Id).ToList();
                        foreach (var val in childs)
                        {
                            item.Childs.Add(new FrontMenuDetailModel
                            {
                                Id = menuId,
                                Image = "",
                                Name = GetMutiLanguage(val.DescTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                                Title = "",
                                Level = 1,
                                ParentId = item.Value?.Id,
                                Type = (int)CustomMenuType.AttributeValue,
                                Url = "",
                                Value = new ValueModel
                                {
                                    Id = val.Id.ToString()
                                },
                            });
                        }
                    }
                }
            }
            return list;
        }

        private List<FrontMenuDetailModel> GetMenuCatagory(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();
            List<ProductCatalog> roots = new List<ProductCatalog>();

            var menu = baseRepository.GetList<CustomMenu>().FirstOrDefault(p => p.Id == menuId);

            if (menu != null)
            {
                var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                               join c in baseRepository.GetList<ProductCatalog>() on d.Value equals c.Id.ToString()
                               where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId
                               && c.IsDeleted == false
                               orderby d.Seq
                               select c).ToList();

                if (details.Count > 0)
                {
                    roots = details;
                    foreach (var item in details)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = "",
                            Name = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                            Title = "",
                            Level = 0,
                            ParentId = "0",
                            Type = (int)CustomMenuType.Catalog,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.Id.ToString()
                            },
                            IsAnchor = false
                        });
                    }
                }
                else
                {
                    roots = baseRepository.GetList<ProductCatalog>().Where(p => p.IsDeleted == false && p.ParentId == Guid.Empty).ToList();

                    foreach (var item in roots)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = "",
                            Name = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc,
                            Title = "",
                            Level = 0,
                            ParentId = "0",
                            Type = (int)CustomMenuType.Catalog,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.Id.ToString()
                            },
                        });
                    }
                }

                if (menu.ShowNext)
                {
                    var catalogs = baseRepository.GetList<ProductCatalog>().Where(p => p.IsDeleted == false).ToList();
                    list = GenCatalogTreeNodes(catalogs, roots, Guid.Empty, 0);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取菜单下的目录的其它层级目录
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="roots"></param>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GenCatalogTreeNodes(List<ProductCatalog> sources, List<ProductCatalog> roots, Guid parentId, int level)
        {
            List<FrontMenuDetailModel> result = new List<FrontMenuDetailModel>();
            level += 1;
            foreach (var item in roots)
            {
                FrontMenuDetailModel node = new FrontMenuDetailModel();
                if (parentId == Guid.Empty || level > 1)
                {
                    node.Name = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc;
                    node.Title = GetMutiLanguage(item.NameTransId).FirstOrDefault(p => p.Language == CurrentUser.Lang).Desc;
                    node.Level = level;
                    node.Image = item.OriginalIcon; //PathUtil.GetPMServer() + item.OriginalIcon;
                    node.Type = (int)CustomMenuType.Catalog;
                    node.Url = "";
                    node.Value = new ValueModel
                    {
                        Id = item.Id.ToString()
                    };
                    result.Add(node);
                }
                var childs = sources.Where(p => p.ParentId == item.Id).ToList();
                if (childs.Count > 0)
                {
                    node.Childs = GenCatalogTreeNodes(sources, childs, item.Id, level);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取菜单下的RegNPay
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GetMenuRegNPay(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                           join c in baseRepository.GetList<RnpForm>() on d.Value equals c.Id.ToString()
                           where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId && c.IsActive == true && c.IsDeleted == false
                           orderby d.Seq
                           select c
                           ).ToList();

            foreach (var item in details)
            {
                list.Add(new FrontMenuDetailModel
                {
                    Id = menuId,
                    Image = "",
                    Name = item.Title,
                    Title = item.Title,
                    Level = 0,
                    ParentId = Guid.Empty.ToString(),
                    Type = (int)CustomMenuType.RegNPay,
                    Url = "",
                    Value = new ValueModel
                    {
                        Id = item.Id.ToString()
                    },
                    IsAnchor = false

                });
            }
            return list;
        }

        /// <summary>
        /// 获取菜单下的CMSContent
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GetMenuCMSContent(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                           join c in baseRepository.GetList<CMSContent>() on d.Value equals c.Id.ToString()
                           where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId
                           orderby d.Seq
                           select new
                           {
                               detail = d,
                               content = c
                           }
                           ).ToList();

            foreach (var item in details)
            {
                list.Add(new FrontMenuDetailModel
                {
                    Id = menuId,
                    Image = "",
                    Name = NameUtil.GetCMSTitle(CurrentUser.Lang, item.content),
                    Title = NameUtil.GetCMSTitle(CurrentUser.Lang, item.content),
                    Level = 0,
                    ParentId = item.content.CategoryId.ToString(),
                    Type = (int)CustomMenuType.CMSContent,
                    Url = "",
                    Value = new ValueModel
                    {
                        Id = item.content.Key.ToString()
                    },
                    IsAnchor = item.detail.IsAnchor
                });
            }
            return list;
        }

        /// <summary>
        /// 获取菜单下的CMSCategory
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GetMenuCMSCategory(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();
            List<CMSCategory> roots = new List<CMSCategory>();

            var menu = baseRepository.GetList<CustomMenu>().FirstOrDefault(p => p.Id == menuId);
            if (menu != null)
            {
                var details = (from d in baseRepository.GetList<CustomMenuDetail>()
                               join c in baseRepository.GetList<CMSCategory>() on d.Value equals c.Id.ToString()
                               where d.IsActive == true && d.IsDeleted == false && d.MenuId == menuId
                               orderby d.Seq
                               select new
                               {
                                   detail = d,
                                   catalog = c,
                               }
                           ).ToList();

                if (details.Count > 0)
                {
                    roots = details.Select(d => d.catalog).ToList();
                    foreach (var item in details)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = item.catalog.Image,
                            Name = NameUtil.GetCategoryName(CurrentUser.Lang, item.catalog),
                            Title = NameUtil.GetCategoryName(CurrentUser.Lang, item.catalog),
                            Level = 0,
                            ParentId = item.catalog.ParentId.ToString(),
                            Type = (int)CustomMenuType.CMSCategory,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.catalog.Id.ToString()
                            },
                            IsAnchor = item.detail.IsAnchor
                        });
                    }
                }
                else
                {
                    roots = baseRepository.GetList<CMSCategory>().Where(p => p.ParentId == Guid.Empty).ToList();
                    foreach (var item in roots)
                    {
                        list.Add(new FrontMenuDetailModel
                        {
                            Id = menuId,
                            Image = item.Image,
                            Name = NameUtil.GetCategoryName(CurrentUser.Lang, item),
                            Title = NameUtil.GetCategoryName(CurrentUser.Lang, item),
                            Level = 0,
                            ParentId = "0",
                            Type = (int)CustomMenuType.CMSCategory,
                            Url = "",
                            Value = new ValueModel
                            {
                                Id = item.Id.ToString()
                            },
                        });
                    }
                }

                if (menu.ShowNext)
                {
                    var categories = baseRepository.GetList<CMSCategory>().ToList();
                    list = GenCategoryTreeNodes(list, categories, roots, Guid.Empty, 0);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取菜单下的CMS目录的其它层级目录
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="roots"></param>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GenCategoryTreeNodes(List<FrontMenuDetailModel> menuList, List<CMSCategory> sources, List<CMSCategory> roots, Guid parentId, int level)
        {

            List<FrontMenuDetailModel> result = new List<FrontMenuDetailModel>();
            level += 1;
            foreach (var item in roots)
            {
                FrontMenuDetailModel node = new FrontMenuDetailModel();
                if (parentId == Guid.Empty || level > 1)
                {
                    var menu = menuList.FirstOrDefault(p => p.Value.Id == item.Id.ToString());
                    node.Name = NameUtil.GetCategoryName(CurrentUser.Lang, item);
                    node.Title = NameUtil.GetCategoryName(CurrentUser.Lang, item);
                    node.Level = level;
                    node.Image = item.Image;
                    node.Type = (int)CustomMenuType.CMSCategory;
                    node.Url = "";
                    node.Value = new ValueModel
                    {
                        Id = item.Id.ToString()
                    };
                    node.IsAnchor = menu?.IsAnchor ?? false;
                    node.ParentId = item.ParentId.ToString();
                    result.Add(node);
                }
                var childs = sources.Where(p => p.ParentId == item.Id).OrderBy(o => o.Seq).ToList();
                if (childs.Count > 0)
                {
                    node.Childs = GenCategoryTreeNodes(menuList, sources, childs, item.Id, level);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取菜单下的自定义链接
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<FrontMenuDetailModel> GetMenuDetailLink(Guid menuId)
        {
            List<FrontMenuDetailModel> list = new List<FrontMenuDetailModel>();

            var details = baseRepository.GetList<CustomMenuDetail>().Where(p => p.MenuId == menuId && p.IsActive == true && p.IsDeleted == false).OrderBy(o => o.Seq).ToList();

            foreach (var item in details)
            {
                list.Add(new FrontMenuDetailModel
                {
                    Id = menuId,
                    Image = "",
                    Name = "",
                    Title = "",
                    Level = 0,
                    ParentId = "0",
                    Type = (int)CustomMenuType.Link,
                    Url = item.Value,
                    Value = new ValueModel
                    {
                        Id = item.Value.ToString()
                    },
                    IsAnchor = false
                });
            }
            return list;
        }

        private PageData<TypeDetail> GetCusMenuDatas(TypeDetailCond cond)
        {
            PageData<TypeDetail> data = new PageData<TypeDetail>();

            switch (cond.Type)
            {
                case (int)CustomMenuType.CMSCategory:
                    var categoryQuery = (from t in baseRepository.GetList<CMSCategory>()
                                         where (cond.Name == "" || t.Name_E.Contains(cond.Name) || t.Name_C.Contains(cond.Name) || t.Name_S.Contains(cond.Name) || t.Name_J.Contains(cond.Name))
                                         select new
                                         {
                                             dataSource = new
                                             {
                                                 Id = Guid.Empty,
                                                 ValueId = t.Id.ToString(),
                                                 t.Name_E,
                                                 t.Name_C,
                                                 t.Name_S,
                                                 t.Name_J,
                                             }
                                         });
                    var catequery = categoryQuery.Select(p => p.dataSource).Distinct();
                    data.TotalRecord = catequery.Count();
                    catequery = catequery.OrderBy(p => p.Id);
                    var cateresult = catequery.Skip(cond.Offset).Take(cond.PageSize).ToList();

                    foreach (var item in cateresult)
                    {
                        TypeDetail typeDetail = new TypeDetail();
                        typeDetail.Id = item.Id;
                        typeDetail.ValueId = item.ValueId;
                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                typeDetail.ValueName = item.Name_E;
                                break;
                            case Language.C:
                                typeDetail.ValueName = item.Name_C;
                                break;
                            case Language.S:
                                typeDetail.ValueName = item.Name_S;
                                break;
                            case Language.J:
                                typeDetail.ValueName = item.Name_J;
                                break;
                        }

                        data.Data.Add(typeDetail);
                    }

                    break;
                case (int)CustomMenuType.CMSContent:
                    var cmsQuery = (from t in baseRepository.GetList<CMSContent>()
                                    where (cond.Name == "" || t.Name_E.Contains(cond.Name) || t.Name_C.Contains(cond.Name) || t.Name_S.Contains(cond.Name) || t.Name_J.Contains(cond.Name))
                                    && t.IsActive == true && t.IsDeleted == false && t.CategoryId != Guid.Empty
                                    select new
                                    {
                                        dataSource = new
                                        {
                                            Id = Guid.Empty,
                                            ValueId = t.Id.ToString(),
                                            t.Name_E,
                                            t.Name_C,
                                            t.Name_S,
                                            t.Name_J,
                                        }
                                    });

                    var cmsquery = cmsQuery.Select(p => p.dataSource).Distinct();
                    data.TotalRecord = cmsquery.Count();
                    cmsquery = cmsquery.OrderBy(p => p.Id);
                    var cmsresult = cmsquery.Skip(cond.Offset).Take(cond.PageSize).ToList();

                    foreach (var item in cmsresult)
                    {
                        TypeDetail typeDetail = new TypeDetail();
                        typeDetail.Id = item.Id;
                        typeDetail.ValueId = item.ValueId;
                        switch (CurrentUser.Lang)
                        {
                            case Language.E:
                                typeDetail.ValueName = item.Name_E;
                                break;
                            case Language.C:
                                typeDetail.ValueName = item.Name_C;
                                break;
                            case Language.S:
                                typeDetail.ValueName = item.Name_S;
                                break;
                            case Language.J:
                                typeDetail.ValueName = item.Name_J;
                                break;
                        }

                        data.Data.Add(typeDetail);
                    }

                    break;
                case (int)CustomMenuType.RegNPay:
                    var cmsRnp = (from t in baseRepository.GetList<RnpForm>()
                                  where t.IsActive == true && t.IsDeleted == false && (cond.Name == "" || t.Title.Contains(cond.Name))
                                  select new TypeDetail
                                  {
                                      Id = Guid.Empty,
                                      ValueId = t.Id.ToString(),
                                      ValueName = t.Title
                                  }).ToList();
                    data.TotalRecord = cmsRnp.Select(d => d.ValueId).Count();
                    data.Data = cmsRnp.Skip(cond.Offset).Take(cond.PageSize).ToList();
                    break;
                case (int)CustomMenuType.Catalog:
                    var catQuery = (from t in baseRepository.GetList<ProductCatalog>()
                                    join b in baseRepository.GetList<Translation>() on new { a1 = t.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = b.TransId, a2 = b.Lang }
                                    where t.IsDeleted == false && t.ParentId == Guid.Empty
                                    && (cond.Name == "" || b.Value.Contains(cond.Name))
                                    select new TypeDetail
                                    {
                                        Id = Guid.Empty,
                                        ValueId = t.Id.ToString(),
                                        ValueName = b.Value
                                    }).ToList();
                    data.TotalRecord = catQuery.Select(d => d.ValueId).Count();
                    data.Data = catQuery.Skip(cond.Offset).Take(cond.PageSize).ToList();
                    break;
                case (int)CustomMenuType.Attribute:
                    var invAttrQuery = (from t in baseRepository.GetList<ProductAttribute>()
                                        join b in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = b.TransId, a2 = b.Lang }
                                        where t.IsDeleted == false
                                        && (cond.Name == "" || b.Value.Contains(cond.Name))
                                        select new TypeDetail
                                        {
                                            Id = Guid.Empty,
                                            ValueId = t.Id.ToString(),
                                            ValueName = b.Value
                                        }).ToList();
                    data.TotalRecord = invAttrQuery.Select(d => d.ValueId).Count();
                    data.Data = invAttrQuery.Skip(cond.Offset).Take(cond.PageSize).ToList();
                    break;
                case (int)CustomMenuType.AttributeValue:
                    var nonInvAttrQuery = (from t in baseRepository.GetList<ProductAttributeValue>()
                                           join b in baseRepository.GetList<Translation>() on new { a1 = t.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = b.TransId, a2 = b.Lang }
                                           where t.IsDeleted == false
                                           && (cond.Name == "" || b.Value.Contains(cond.Name))
                                           select new TypeDetail
                                           {
                                               Id = Guid.Empty,
                                               ValueId = t.AttrId.ToString(),
                                               ValueName = b.Value
                                           }).ToList();
                    data.TotalRecord = nonInvAttrQuery.Select(d => d.ValueId).Count();
                    data.Data = nonInvAttrQuery.Skip(cond.Offset).Take(cond.PageSize).ToList();
                    break;
            }
            return data;
        }

        public CustomMenuDto GetCustomMenuById(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = new CustomMenuDto();
                var query = baseRepository.GetList<CustomMenu>().Where(p => p.Id == id && !p.IsDeleted && p.IsActive).FirstOrDefault();
                if (query != null)
                {
                    result = AutoMapperExt.MapTo<CustomMenuDto>(query);
                    return result;
                }
            }
            return null;
        }

        public List<CustomMenuDetailDto> GetCustomMenuDetailByMenuId(Guid menuId)
        {
            if (menuId != Guid.Empty)
            {
                var result = new List<CustomMenuDetailDto>();
                var query = baseRepository.GetList<CustomMenuDetail>().Where(p => p.MenuId == menuId && !p.IsDeleted && p.IsActive).ToList();
                if (query.Count > 0)
                {
                    result = AutoMapperExt.MapToList<CustomMenuDetail, CustomMenuDetailDto>(query);
                    return result;
                }
            }
            return null;
        }


        public List<MutiLanguage> GetMutiLanguage(Guid transId)
        {
            var data = new List<MutiLanguage>();
            var supportLangs = GetSupportLanguage();
            if (transId == Guid.Empty)
            {
                foreach (var supportLang in supportLangs)
                {
                    data.Add(new MutiLanguage { Desc = "", Lang = supportLang });
                }
                return data;
            }
            else
            {
                var translates = baseRepository.GetList<Translation>().Where(d => d.TransId == transId).Select(d => d).ToList();

                bool exist = false;
                foreach (var supportLang in supportLangs)
                {
                    exist = false;
                    foreach (var tran in translates)
                    {
                        if (supportLang.Code.Trim() == tran.Lang.ToString().Trim())
                        {
                            exist = true;
                            data.Add(new MutiLanguage { Desc = tran.Value, Lang = supportLang });
                        }
                    }
                    if (!exist)
                    {
                        data.Add(new MutiLanguage { Desc = "", Lang = supportLang });
                    }
                }
                return data;
            }
        }

        public bool VerifySeq(MenuDetailInfo info)
        {
            bool result = false;
            try
            {
                var data = baseRepository.GetList<CustomMenu>().Where(p => p.Seq == info.Seq && p.IsActive && !p.IsDeleted).ToList();

                if (info.ParentId == Guid.Empty)
                {
                    var item = data.Where(p => p.ParentId == Guid.Empty).FirstOrDefault();
                    if (item == null)
                    {
                        result = true;
                    }
                }
                else
                {
                    var item = data.Where(p => p.ParentId == info.ParentId).FirstOrDefault();
                    if (item == null)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetCustomMenuSeq(MenuDetailInfo info)
        {
            int seq = 0;
            try
            {
                var data = baseRepository.GetList<CustomMenu>().Where(p => p.IsActive == true && !p.IsDeleted).OrderByDescending(p => p.CreateDate).ToList();

                if (info.ParentId == Guid.Empty)
                {
                    var item = data.Where(p => p.ParentId == Guid.Empty).OrderByDescending(p => p.CreateDate).Select(p => p.Seq).Take(1).FirstOrDefault();
                    seq = item + 1;
                }
                else
                {
                    var item = data.Where(p => p.ParentId == info.ParentId).OrderByDescending(p => p.CreateDate).Select(p => p.Seq).Take(1).FirstOrDefault();
                    seq = item + 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return seq;
        }
    }
}
