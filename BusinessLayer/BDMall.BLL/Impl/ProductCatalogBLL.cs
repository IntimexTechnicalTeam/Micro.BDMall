using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
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
    public class ProductCatalogBLL : BaseBLL, IProductCatalogBLL
    {
        IProductCatalogRepository productCatalogRepository;
        ITranslationRepository translationRepository;

        public ProductCatalogBLL(IServiceProvider services) : base(services)
        {
            productCatalogRepository = Services.Resolve<IProductCatalogRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
        }

        public List<ProductCatalogSummaryView> GetAllCatalog()
        {
            var catalogs = productCatalogRepository.GetAllActiveCatalog();
            var result = AutoMapperExt.MapTo<List<ProductCatalogSummaryView>>(catalogs);
            return result;
        }

        public List<ProductCatalogEditModel> GetCatalogTree(bool isActive)
        {
            List<ProductCatalogEditModel> result = new List<ProductCatalogEditModel>();

            var catalogs = isActive ? productCatalogRepository.GetAllActiveCatalog() : productCatalogRepository.GetAllCatalog();

            var catalogEditModels = AutoMapperExt.MapTo<List<ProductCatalogEditModel>>(catalogs);

            foreach (var item in catalogEditModels)
            {
                GenProductCatalogEditModel(item);
            }

            var parents = catalogEditModels.Where(p => p.ParentId == Guid.Empty).ToList();
            result = GenCatalogTree(catalogEditModels, parents, Guid.Empty);

            return result;
        }

        public ProductCatalogEditModel GetCatalog(Guid catId)
        {
            ProductCatalogEditModel result = new ProductCatalogEditModel();

            if (catId == Guid.Empty)
            {
                result.Descs = LangUtil.GetMutiLangFromTranslation(null, GetSupportLanguage());
                return result;
            }

            var catalog = productCatalogRepository.GetById(catId);
            if (catalog != null)
            {
                result = AutoMapperExt.MapTo<ProductCatalogEditModel>(catalog);
                GenProductCatalogEditModel(result);
            }
            return result;
        }

        public void DeleteCatalog(Guid id)
        {
            var flag = baseRepository.Any<Product>(x => x.CatalogId == id && x.IsActive && !x.IsDeleted);
            if (flag)
                throw new ServiceException(Resources.Message.CatalogHasUse);

            var catalog = baseRepository.GetModelById<ProductCatalog>(id);
            if (catalog != null)
            {
                catalog.IsDeleted = true;

                baseRepository.Update(catalog);

                var catalogParents = baseRepository.GetList<ProductCatalogParent>(x => x.CatalogId == id);
                baseRepository.Delete(catalogParents);
            }
        }

        public async Task UpdateCatalogSeqAsync(List<ProductCatalogEditModel> list)
        {
            await UpdateCatalogSeq(list);
            //更新缓存，调用PreHeatProductCatalogService.SetDataToHashCache方法
        }

        /// <summary>
        /// 保存catalog
        /// </summary>
        /// <param name="productCatalog"></param>
        public async Task<SystemResult> SaveCatalog(ProductCatalogEditModel productCatalog)
        {
            SystemResult result = new SystemResult();
            var catalog = GenProductCatalog(productCatalog);

            if (productCatalog.Action == ActionTypeEnum.Add)
            {
                if (baseRepository.Any<ProductCatalog>(x => x.Code == productCatalog.Code && x.IsActive && !x.IsDeleted))
                    throw new BLException(Resources.Message.CatalogCodeIsExist);

                result = InsertCatalog(catalog);
            }
            else        
                result = UpdateCatalog(catalog);
            
            
            return result;
        }

        public async Task<SystemResult> DisActiveCatalogAsync(Guid catId)
        {
            var sysRslt = DisActiveCatalog(catId);
            return sysRslt;
        }

        public async Task<SystemResult> ActiveCatalogAsync(Guid catId)
        {
            var sysRslt = ActiveCatalog(catId);         
            return sysRslt;
        }

        public string GetCatalogPath(Guid catID)
        {
            string result = "";

            var dbCatalog = baseRepository.GetModelById<ProductCatalog>(catID);
            var catalog = AutoMapperExt.MapTo<ProductCatalogDto>(dbCatalog);
            catalog.Descs = translationRepository.GetMutiLanguage(catalog.NameTransId);
            catalog.Desc = catalog.Descs?.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
            var catalogView = AutoMapperExt.MapTo<ProductCatalogSummaryView>(catalog);
            var dbCatalogPaths = productCatalogRepository.GetCatalogUrlByCatalogId(catalog.Id);
            if (catalog != null)
            {
                if (dbCatalogPaths != null && dbCatalogPaths.Any())
                {
                    var catalogPaths = AutoMapperExt.MapTo<List<ProductCatalogEditModel>>(dbCatalogPaths);
                    foreach (var item in catalogPaths)
                    {
                        //var parentCatalog = _productCatalogRepository.GetByKey(item.ParentCatalogId);
                        var parentCatalogView = GenProductCatalogEditModel(item);
                        if (result == "")
                        {
                            result = parentCatalogView.Desc;
                        }
                        else
                        {
                            result += "->" + parentCatalogView.Desc;
                        }

                    }
                }
                if (result == "")
                {
                    result = catalogView.Desc;
                }
                else
                {
                    result += "->" + catalogView.Desc;
                }
            }
            return result;
        }

        public List<ProdCatatogInfo> GetActiveCatalogTree()
        {
            var allCatalogList = productCatalogRepository.GetAllActiveCatalog();

            var catalogInfos = allCatalogList.Select(s => GenProductCatalogInfo(s)).ToList();
            var parents = catalogInfos.Where(p => p.ParentId == Guid.Empty).ToList();
            var result = GenCatalogTree(catalogInfos, parents, Guid.Empty);

            return result;
        }

        public async Task<SystemResult> GetCatalogAsync()
        {
            var result =new SystemResult();
            string key = CacheKey.MenuCatalog.ToString();
            string field = $"{CacheField.SubCatalog}_{CurrentUser.Lang}";

            var data = await RedisHelper.HGetAsync<List<ProdCatatogInfo>>(key, field);
            if (data == null)
            {
                data = GetActiveCatalogTree();
                await RedisHelper.HSetAsync(key,field,data);
            }

            result.ReturnValue = AutoMapperExt.MapToList<ProdCatatogInfo,Catalog>(data);
            result.Succeeded = true;
            return result;
        }

        private ProdCatatogInfo GenProductCatalogInfo(ProductCatalogDto catalog)
        {
            var fileServer = string.Empty;
            ProdCatatogInfo info = new ProdCatatogInfo();
            info.Id = catalog.Id;
            info.Children = null;
            info.Img = fileServer + catalog.OriginalIcon;
            info.ImgS = fileServer + catalog.SmallIcon;
            info.ImgB = fileServer + catalog.BigIcon;
            info.ImgM = fileServer + catalog.MOriginalIcon;
            info.ImgSM = fileServer + catalog.MSmallIcon;
            info.ImgBM = fileServer + catalog.MBigIcon;
            info.Name = translationRepository.GetDescForLang(catalog.NameTransId, CurrentUser.Lang);
            info.ParentId = catalog.ParentId;
            //info.PathId = catalog.PathId;
            info.Level = catalog.Level;

            return info;
        }

        private List<ProdCatatogInfo> GenCatalogTree(List<ProdCatatogInfo> data, List<ProdCatatogInfo> nodes, Guid parentId)
        {
            List<ProdCatatogInfo> result = new List<ProdCatatogInfo>();

            foreach (ProdCatatogInfo item in nodes)
            {
                ProdCatatogInfo node = new ProdCatatogInfo();
                node.Id = item.Id;
                node.ParentId = item.ParentId;
                node.PathId = item.PathId;
                node.Img = item.Img;
                node.ImgS = item.ImgS;
                node.ImgB = item.ImgB;
                node.ImgM = item.ImgM;
                node.ImgSM = item.ImgSM;
                node.ImgBM = item.ImgBM;
                node.Name = item.Name;
                result.Add(node);
                var childs = data.Where(p => p.ParentId == item.Id && p.ParentId != p.Id).ToList();
                if (childs.Count > 0)
                {
                    node.Children = GenCatalogTree(data, childs, item.Id);
                }
            }
            return result;
        }

        private async Task UpdateCatalogSeq(List<ProductCatalogEditModel> list)
        {
            var catalogList = await baseRepository.GetListAsync<ProductCatalog>(x => list.Select(s => s.Id).Contains(x.Id));
            var query = catalogList.ToList();
            foreach (var item in query)
            {
                item.Seq = list.FirstOrDefault(x => x.Id == item.Id).Seq;
                item.UpdateDate = DateTime.Now;
            }

            if (query.Any())
                await baseRepository.UpdateAsync(query);
        }

        private SystemResult InsertCatalog(Tuple<ProductCatalogDto, List<ProductCatalogAttr>> model)
        {
            SystemResult result = new SystemResult();
            var productCatalog = model.Item1;
            var catalogAttrs = model.Item2;

            //生成catalog目录名称的多语言
            var catNameTransId = Guid.NewGuid();
            var cataLogTrans = translationRepository.GenTranslations(productCatalog.Descs, TranslationType.Catalog, catNameTransId);

            //var catalogId = Guid.NewGuid();
            //if (productCatalog.Id == Guid.Empty) productCatalog.Id = Guid.NewGuid();
            //productCatalog.Id = catalogId;
            productCatalog.NameTransId = catNameTransId;
            productCatalog.Level = productCatalog.ParentId == Guid.Empty ? 1 : productCatalog.Level;
            productCatalog.CreateDate = DateTime.Now;
            productCatalog.CreateBy = Guid.Parse(CurrentUser.UserId);
            productCatalog.IsActive = true;
            var Catalog = AutoMapperExt.MapTo<ProductCatalog>(productCatalog);
           

            //生成catalog的父级
            List<ProductCatalogParent> catalogUrls = new List<ProductCatalogParent>();
            if (productCatalog.ParentId != Guid.Empty)//如果父目录存在，则继承父目录的url
            {
                var parentCatalogUrls = baseRepository.GetList<ProductCatalogParent>(x => x.CatalogId == productCatalog.ParentId);
                if (parentCatalogUrls != null && parentCatalogUrls.Any())
                {
                    foreach (var item in parentCatalogUrls)//继承父目录的url
                    {
                        ProductCatalogParent url = new ProductCatalogParent();

                        url.Id = Guid.NewGuid();
                        url.CatalogId = productCatalog.Id;
                        url.ParentCatalogId = item.ParentCatalogId;
                        url.Level = item.Level;
                        catalogUrls.Add(url);
                    }
                    ProductCatalogParent selfUrl = new ProductCatalogParent();
                    selfUrl.Id = Guid.NewGuid();
                    selfUrl.CatalogId = productCatalog.Id;
                    selfUrl.ParentCatalogId = productCatalog.ParentId;
                    selfUrl.Level = productCatalog.Level;
                    catalogUrls.Add(selfUrl);
                }
            }
            else
            {
                ProductCatalogParent url = new ProductCatalogParent();
                url.Id = Guid.NewGuid();
                url.CatalogId = productCatalog.Id;
                url.ParentCatalogId = Guid.Empty;
                url.Level = 1;
                catalogUrls.Add(url);
            }

            //生成catalog的属性
            //if (CatalogAttrs.Any())
            //{
            //    //foreach (var item in CatalogAttrs)
            //    //{
            //    //    item.CatalogId = item.CatalogId == Guid.Empty ? catalogId : item.CatalogId;
            //    //}
            //}

            using var tran = baseRepository.CreateTransation();
            baseRepository.Insert(cataLogTrans);
            baseRepository.Insert(Catalog);
            baseRepository.Insert(catalogUrls);
            baseRepository.Insert(catalogAttrs);
            tran.Commit();

            result.Succeeded = true;
            return result;
        }

        private SystemResult UpdateCatalog(Tuple<ProductCatalogDto, List<ProductCatalogAttr>> model)
        {
            SystemResult result = new SystemResult();
            var productCatalog = model.Item1;
            var CatalogAttrs = model.Item2;
            var catalog = baseRepository.GetModelById<ProductCatalog>(productCatalog.Id);
            if (catalog == null)
                throw new BLException("找不到ProductCatalog");

            CheckCatalogRemoveByUsed(CatalogAttrs, productCatalog.Id);
            var dbCatalog = AutoMapperExt.MapTo<ProductCatalog>(catalog);
            dbCatalog.UpdateDate = DateTime.Now;
            dbCatalog.BigIcon = productCatalog.BigIcon;
            dbCatalog.SmallIcon = productCatalog.SmallIcon;
            dbCatalog.MSmallIcon = productCatalog.MSmallIcon;
            dbCatalog.OriginalIcon = productCatalog.OriginalIcon;
            dbCatalog.MBigIcon = productCatalog.MBigIcon;
            dbCatalog.MOriginalIcon = productCatalog.MOriginalIcon;
            dbCatalog.Code = productCatalog.Code;

            //处理Catalog的属性
            var dbCatalogAttrLst = baseRepository.GetList<ProductCatalogAttr>(x => x.CatalogId == productCatalog.Id).ToList();
            var updateAttrLst = new List<ProductCatalogAttr>();
            var insertAttrLst = new List<ProductCatalogAttr>();
            if (CatalogAttrs != null && CatalogAttrs.Any())
            {
                foreach (var item in dbCatalogAttrLst)//删除catalog的属性
                {
                    var existAttr = CatalogAttrs.Any(p => p.AttrId == item.AttrId);
                    if (!existAttr)
                    {
                        item.IsDeleted = true; item.UpdateDate = DateTime.Now;
                        updateAttrLst.Add(item);
                    }
                }

                foreach (var item in CatalogAttrs)
                {
                    var existAttr = dbCatalogAttrLst.FirstOrDefault(p => p.AttrId == item.AttrId);

                    if (existAttr != null)
                    {
                        if (existAttr.IsDeleted == true)
                        {
                            existAttr.IsDeleted = false;
                            item.UpdateDate = DateTime.Now;
                            updateAttrLst.Add(item);
                        }
                    }
                    else
                    {                      
                        item.IsDeleted = false;
                        insertAttrLst.Add(item);
                    }
                }

            }
            else
            {
                // 表示界面没有选择属性,数据库的catalog对应的属性全部删除
                foreach (var item in dbCatalogAttrLst)
                {
                    item.IsDeleted = true;
                }
            }

            //处理Catalog的多语言
            var catalogTrans = translationRepository.GetTranslation(dbCatalog.NameTransId);
            foreach (var item in catalogTrans)
            {
                item.Value = productCatalog.Descs.FirstOrDefault(x => x.Lang.Code == item.Lang.ToString())?.Desc ?? "";
                item.UpdateDate = DateTime.Now;
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Update(catalogTrans);
            baseRepository.Update(dbCatalog);
            baseRepository.Update(dbCatalogAttrLst);
            baseRepository.Update(updateAttrLst);
            baseRepository.Insert(insertAttrLst);
            baseRepository.Insert(ReMappingProductAttrs(dbCatalog));

            tran.Commit();

            return result;
        }

        private ProductCatalogEditModel GenProductCatalogEditModel(ProductCatalogEditModel item)
        {
            var fileServer = string.Empty;

            item.Code = item?.Code ?? "";
            item.ParentId = item?.ParentId ?? Guid.Empty;
            item.NameTransId = item?.NameTransId ?? Guid.Empty;
            item.InvAttributes = baseRepository.GetList<ProductCatalogAttr>(p => p.IsInvAttribute && !p.IsDeleted && p.CatalogId == item.Id).Select(d => d.AttrId).ToList();
            item.NotInvAttributes = baseRepository.GetList<ProductCatalogAttr>(p => !p.IsInvAttribute && !p.IsDeleted && p.CatalogId == item.Id).Select(d => d.AttrId).ToList();
            item.Children = new List<ProductCatalogEditModel>();
            item.Collapse = false;
            item.IsChange = false;
            item.Level = item?.Level ?? 0;
            item.Seq = item?.Seq ?? 0;
            item.SmallIcon = fileServer + item?.SmallIcon ?? "";
            item.BigIcon = fileServer + item?.BigIcon ?? "";
            item.OriginalIcon = fileServer + item?.OriginalIcon ?? "";
            item.IconPath = fileServer + item?.SmallIcon ?? "";
            item.MSmallIcon = fileServer + item?.MSmallIcon ?? "";
            item.MBigIcon = fileServer + item?.MBigIcon ?? "";
            item.MOriginalIcon = fileServer + item?.MOriginalIcon ?? "";
            item.IconPathM = fileServer + item?.MSmallIcon ?? "";
            item.Descs = item?.Descs ?? LangUtil.GetMutiLangFromTranslation(null, GetSupportLanguage());
            item.Desc = item?.Desc ?? "";
            item.IsMappingProduct = baseRepository.Any<Product>(x => x.CatalogId == item.Id);
            item.IsActive = item?.IsActive ?? false;

            return item;
        }

        /// <summary>
        /// 检查是否删除了已经使用的属性
        /// </summary>
        /// <param name="viewCatalogs"></param>
        /// <param name="catalogId"></param>
        /// <exception cref="BLException"></exception>
        private void CheckCatalogRemoveByUsed(List<ProductCatalogAttr> viewCatalogs, Guid catalogId)
        {
            if (viewCatalogs !=null && viewCatalogs.Any())
            {
                var attrInfos = (from a in baseRepository.GetList<ProductAttribute>()
                                 join t in baseRepository.GetList<Translation>() on new { a1 = a.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                 from tt in tc.DefaultIfEmpty()
                                 select new
                                 {
                                     Id = a.Id,
                                     Name = tt.Value
                                 });

                //原来catalog的配对属性列表
                var catalogDBAttrs = baseRepository.GetList<ProductCatalogAttr>().Where(p => p.CatalogId == catalogId).Select(d => d.AttrId).ToList();

                foreach (var dbAttrId in catalogDBAttrs)
                {
                    var isExist = viewCatalogs.Any(x=>x.AttrId == dbAttrId);
                    if (!isExist)//如果原来存在的AttrId不在传入的清单中就判断该值是否已经被使用
                    {                      
                        var productUsedAttr = baseRepository.Any<ProductAttr>(p => p.AttrId == dbAttrId && p.CatalogID == catalogId);
                        //如果被使用了则不能删除了
                        if (productUsedAttr)
                        {
                            var attrInfo = attrInfos.FirstOrDefault(p => p.Id == dbAttrId);
                            throw new BLException(string.Format(Resources.Message.AttributeInUsed, (attrInfo?.Name ?? "")));
                        }
                    }
                }
            }
        }


        private List<ProductAttr> ReMappingProductAttrs(ProductCatalog productCatalog)
        {
            //当前目录的非库存属性，用于补齐已经配对了目录的产品的非库存、库存属性
            var viewInvAttrs = baseRepository.GetList<ProductCatalogAttr>(x => x.CatalogId == productCatalog.Id && x.IsInvAttribute).OrderBy(o => o.Seq).ToList();
            var viewNonInvAttrs = baseRepository.GetList<ProductCatalogAttr>(x => x.CatalogId == productCatalog.Id && !x.IsInvAttribute).OrderBy(o => o.Seq).ToList();

            var catalogProcucts = baseRepository.GetList<Product>(x => x.CatalogId == productCatalog.Id);
            var catalogAttrs = baseRepository.GetList<ProductAttr>().Where(p => p.CatalogID == productCatalog.Id).ToList();

            List<ProductAttr> productAttrs = new List<ProductAttr>();
            foreach (var item in catalogProcucts)
            {
                if (catalogAttrs.Any(c=>c.ProductId == item.Id))
                {
                    var productInvs = catalogAttrs.Where(p => p.ProductId ==item.Id && p.IsInv == true).ToList();
                    var productNonInvs = catalogAttrs.Where(p => p.ProductId == item.Id && p.IsInv == false).ToList();

                    var invStartIndex = productInvs.Count();
                    foreach (var newAttr in viewInvAttrs)//如果有新的InvAttr，则分配入产品
                    {
                        var isNew = true;
                        foreach (var prodAttr in productInvs)
                        {
                            if (prodAttr.AttrId == newAttr.AttrId)
                            {
                                isNew = false;
                            }
                        }
                        if (isNew)
                        {
                            invStartIndex += 1;
                            productAttrs.Add(new ProductAttr
                            {
                                Id = Guid.NewGuid(),
                                AttrId = newAttr.AttrId,
                                CatalogID = productCatalog.Id,
                                ProductId = item.Id,
                                IsInv = newAttr.IsInvAttribute,
                                Seq = invStartIndex
                            });
                        }
                    }
                    var nonInvStartIndex = productNonInvs.Count();
                    foreach (var newAttr in viewNonInvAttrs)//如果有新的NonInvAttr，则分配入产品
                    {
                        var isNew = true;
                        foreach (var prodAttr in productNonInvs)
                        {
                            if (prodAttr.AttrId == newAttr.AttrId)
                            {
                                isNew = false;
                            }
                        }
                        if (isNew)
                        {
                            nonInvStartIndex += 1;
                            productAttrs.Add(new ProductAttr
                            {
                                Id = Guid.NewGuid(),
                                AttrId = newAttr.AttrId,
                                CatalogID = productCatalog.Id,
                                ProductId = item.Id,
                                IsInv = newAttr.IsInvAttribute,
                                Seq = nonInvStartIndex
                            });
                        }
                    }
                }
                else
                {
                    foreach (var invAttr in viewInvAttrs)
                    {
                        productAttrs.Add(new ProductAttr
                        {
                            Id = Guid.NewGuid(),
                            AttrId = invAttr.AttrId,
                            CatalogID = productCatalog.Id,
                            ProductId = item.Id,
                            IsInv = invAttr.IsInvAttribute,
                            Seq = invAttr.Seq
                        });
                    }
                    foreach (var nonInvAttr in viewNonInvAttrs)
                    {
                        productAttrs.Add(new ProductAttr
                        {
                            Id = Guid.NewGuid(),
                            AttrId = nonInvAttr.AttrId,
                            CatalogID = productCatalog.Id,
                            ProductId = item.Id,
                            IsInv = nonInvAttr.IsInvAttribute,
                            Seq = nonInvAttr.Seq
                        });
                    }
                }
            }
            return productAttrs;
        }

        private Tuple<ProductCatalogDto, List<ProductCatalogAttr>> GenProductCatalog(ProductCatalogEditModel catalog)
        {          
            var productCatalog = new ProductCatalogDto();
            productCatalog.Id = catalog.Id;
            productCatalog.Code = catalog.Code;
            productCatalog.ParentId = catalog.ParentId;
            productCatalog.NameTransId = catalog.NameTransId;
            productCatalog.Level = catalog.Level;
            productCatalog.Seq = 0;
            productCatalog.SmallIcon = catalog.SmallIcon;
            productCatalog.BigIcon = catalog.BigIcon;
            productCatalog.OriginalIcon = catalog.OriginalIcon;
            productCatalog.MSmallIcon = catalog.MSmallIcon;
            productCatalog.MBigIcon = catalog.BigIcon;
            productCatalog.MOriginalIcon = catalog.MOriginalIcon;
            productCatalog.Desc = catalog.Desc;
            productCatalog.Descs = catalog.Descs;

            var catalogAttrList = new List<ProductCatalogAttr>();
            foreach (var item in catalog.InvAttributes)
            {
                catalogAttrList.Add(new ProductCatalogAttr {Id = Guid.NewGuid(), AttrId = item,CatalogId = catalog.Id, IsInvAttribute = true});
            }

            foreach (var item in catalog.NotInvAttributes)
            {
                catalogAttrList.Add(new ProductCatalogAttr { Id = Guid.NewGuid(), AttrId = item, CatalogId = catalog.Id, IsInvAttribute = false });
            }

            var result = new Tuple<ProductCatalogDto, List<ProductCatalogAttr>>( productCatalog, catalogAttrList);
            return result;
        }

        private List<ProductCatalogEditModel> GenCatalogTree(List<ProductCatalogEditModel> data, List<ProductCatalogEditModel> nodes , Guid parentId)
        {
            List<ProductCatalogEditModel> result = new List<ProductCatalogEditModel>();
            
            foreach (ProductCatalogEditModel item in nodes)
            {
                ProductCatalogEditModel node = new ProductCatalogEditModel();


                node.Id = item.Id;
                node.Code = item.Code;
                node.Desc = item.Desc;
                node.SmallIcon = item.SmallIcon;
                //node.IconPath = PathUtil.GetFileServer() + item.Icon;
                node.Level = item.Level;
                //node.Url = item.Url;
                node.ParentId = item.ParentId;
                //node.PathId = item.PathId;
                node.Collapse = item.ParentId == Guid.Empty ? true : false;
                //node.Attrs = item.Attrs;
                node.Seq = item.Seq;
                node.IsChange = false;
                node.IsActive = item.IsActive;
                result.Add(node);


                var childs = data.Where(p => p.ParentId == item.Id && p.Id != p.ParentId).OrderBy(o => o.Seq).ToList();
                if (childs.Count > 0)
                {
                    node.Collapse = true;
                    node.Children = GenCatalogTree(data, childs, item.Id);
                }
                else
                {
                    node.Collapse = false;
                }
            }

            return result;
        }

        private SystemResult DisActiveCatalog(Guid catId)
        {
            SystemResult result = new SystemResult();

          
            var catalogs = productCatalogRepository.GetAllCatalogChilds(catId);
            var products = productCatalogRepository.GetAllCatalogChildProducts(catId);

            foreach (var catalog in catalogs)
            {
                catalog.IsActive = false;
            }

            foreach (var product in products)
            {
                product.IsActive = false;
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Update(products);
            baseRepository.Update(catalogs);
            tran.Commit();

            result.Succeeded = true;

            return result;
        }

        private SystemResult ActiveCatalog(Guid catId)
        {
            SystemResult result = new SystemResult();

            var catalogs = productCatalogRepository.GetAllCatalogChilds(catId);

            foreach (var catalog in catalogs)
            {
                catalog.IsActive = true;
            }

            baseRepository.Update(catalogs);
            result.Succeeded = true;

            return result;
        }
    }
}
