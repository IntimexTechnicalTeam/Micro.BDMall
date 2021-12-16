using BDMall.Domain;
using BDMall.Model;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class ProductCatalogRepository : PublicBaseRepository, IProductCatalogRepository
    {
        public ProductCatalogRepository(IServiceProvider service) : base(service)
        {

        }

        public  IQueryable<ProductCatalogDto> baseQuery()
        {
            var query = (from c in baseRepository.GetList<ProductCatalog>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = c.NameTransId, a2 = CurrentUser.Lang }
                         equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         select new ProductCatalogDto
                         {
                             Id = c.Id,
                             Code = c.Code,
                             NameTransId = c.NameTransId,
                             BigIcon = c.BigIcon,
                             Level = c.Level,
                             MBigIcon = c.MBigIcon,
                             MOriginalIcon = c.MOriginalIcon,
                             MSmallIcon = c.MSmallIcon,
                             OriginalIcon = c.OriginalIcon,
                             SmallIcon = c.SmallIcon,
                             Seq = c.Seq,
                             ParentId = c.ParentId,
                             Desc = tt.Value ?? "",
                             IsActive = c.IsActive,
                             IsDeleted = c.IsDeleted,
                             CreateBy = c.CreateBy,
                             CreateDate = c.CreateDate,
                             UpdateBy = c.UpdateBy,
                             UpdateDate = c.UpdateDate,
                         });
            return query;    
        }

        public List<ProductCatalogDto> GetAllActiveCatalog()
        {
            var query =  baseQuery().Where(c=>c.IsActive && !c.IsDeleted).OrderBy(o => o.Seq).ToList();
            return query;
        }

        public List<ProductCatalogDto> GetAllCatalog()
        {
            List<ProductCatalog> result = new List<ProductCatalog>();
            var query = baseQuery().Where(c => !c.IsDeleted).OrderBy(o => o.Seq).ToList();
            return query;
        }

        public ProductCatalogDto GetById(Guid id)
        {
            var result = new ProductCatalogDto();

            var query = (from c in baseRepository.GetList<ProductCatalog>()
                         join t in baseRepository.GetList <Translation>() on c.NameTransId equals t.TransId into tc
                         from tt in tc.DefaultIfEmpty()
                         where c.IsActive && !c.IsDeleted && c.Id == id
                         select new { catalog = c, name = tt }
                         );

            var groupInfo = query.ToList().GroupBy(g => g.catalog).Select(d => new
            {
                Catalog = d.Key,
                Names = d.Select(a => a.name).ToList()
            }).FirstOrDefault();

            result = AutoMapperExt.MapTo<ProductCatalogDto>(groupInfo.Catalog);

            var supportLang = GetSupportLanguage();

            result.Descs = LangUtil.GetMutiLangFromTranslation(groupInfo.Names, supportLang);
            result.Desc = result.Descs.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc.Trim() ?? "";

            return result;
        }

    }
}
