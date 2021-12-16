using BDMall.Domain;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class ProductCatalogBLL : BaseBLL, IProductCatalogBLL
    {
        IProductCatalogRepository productCatalogRepository;

        public ProductCatalogBLL(IServiceProvider services) : base(services)
        {
            productCatalogRepository = Services.Resolve<IProductCatalogRepository>();
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
            var flag = baseRepository.Any<Product>(x=>x.CatalogId == id && x.IsActive && !x.IsDeleted);
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

        private async Task UpdateCatalogSeq(List<ProductCatalogEditModel> list)
        {         
            var catalogList =await baseRepository.GetListAsync<ProductCatalog>(x => list.Select(s => s.Id).Contains(x.Id));
            foreach (var item in catalogList)
            {
                item.Seq = list.FirstOrDefault(x => x.Id == item.Id).Seq;
                item.UpdateDate = DateTime.Now;
            }
            await baseRepository.UpdateAsync(catalogList);
        }

        private void GenProductCatalogEditModel(ProductCatalogEditModel item)
        {
            var fileServer = string.Empty;
            
            item.Code = item?.Code ?? "";
            item.ParentId = item?.ParentId ?? Guid.Empty;
            item.NameTransId = item?.NameTransId ?? Guid.Empty;
            item.InvAttributes = baseRepository.GetList<ProductCatalogAttr>(p => p.IsInvAttribute == true && p.IsDeleted == false).Select(d => d.AttrId).ToList();
            item.NotInvAttributes = baseRepository.GetList<ProductCatalogAttr>(p => p.IsInvAttribute == false && p.IsDeleted == false).Select(d => d.AttrId).ToList();
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
    }
}
