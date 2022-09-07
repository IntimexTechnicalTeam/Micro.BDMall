namespace BDMall.Repository
{
    public class InventoryRepository : PublicBaseRepository, IInventoryRepository
    {
        public InventoryRepository(IServiceProvider service) : base(service)
        {
        }

        /// <summary>
        /// 獲取指定條件的庫存資料列表
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        /// <returns></returns>
        public List<Inventory> GetInventoryList(InventoryDto cond)
        {
            List<Inventory> invtLst = new List<Inventory>();

            Guid merchId = Guid.Empty;
            var productSku = baseRepository.GetModelById<ProductSku>(cond.Sku);
            if (productSku != null)
            {
                var product = baseRepository.GetModel<Product>(x => x.IsActive && !x.IsDeleted && x.Code == productSku.ProductCode);
                if (product != null)
                {
                    merchId = product.MerchantId;
                }
            }

            var invtQuery = from inv in baseRepository.GetList<Inventory>()
                            join w in baseRepository.GetList<Warehouse>() on inv.WHId equals w.Id
                            where inv.IsActive && !inv.IsDeleted && w.IsActive && !w.IsDeleted
                            select new { inv, w };

            if (cond.WHId != Guid.Empty)
            {
                invtQuery = invtQuery.Where(x => x.inv.WHId == cond.WHId);
            }
            if (cond.Sku != Guid.Empty)
            {
                invtQuery = invtQuery.Where(x => x.inv.Sku == cond.Sku);
            }
            if (CurrentUser.IsMerchant)
            {
                invtQuery = invtQuery.Where(x => x.w.MerchantId == CurrentUser.MerchantId);
            }
            if (merchId != Guid.Empty)
            {
                invtQuery = invtQuery.Where(x => x.w.MerchantId == merchId);
            }

            invtLst = invtQuery.Select(x => x.inv).ToList();
            return invtLst;
        }

        public PageData<InvSummary> GetInvSummaryByPage(InvSrchCond cond)
        {
            PageData<InvSummary> result = new PageData<InvSummary>();
            //var invSummaryLst = new PageData<InvSummary>();

            //Guid clientIdStr = _unitWork.Operator.ClientId;
            Guid merchIdStr = cond == null ? Guid.Empty : cond.MerchantId;
            string prodCode = string.IsNullOrEmpty(cond?.ProductCode) ? string.Empty : cond?.ProductCode;
            Guid catalogId = cond == null ? Guid.Empty : cond.CategoryId;
            Guid attrVal1 = cond == null ? Guid.Empty : cond.AttributeI;
            Guid attrVal2 = cond == null ? Guid.Empty : cond.AttributeII;
            Guid attrVal3 = cond == null ? Guid.Empty : cond.AttributeIII;
            int salesQtyLower = cond?.SalesQtyLowerLimit == null ? 0 : cond.SalesQtyLowerLimit.Value;
            int salesQtyUpper = cond?.SalesQtyUpperLimit == null ? 0 : cond.SalesQtyUpperLimit.Value;

            List<Guid> tranIds = new List<Guid>();
            if (cond != null && !string.IsNullOrEmpty(cond.ProductName))
            {
                tranIds = (from m in  baseRepository.GetList<ProductStatistics>()
                           join t in baseRepository.GetList<Translation>() on m.InternalNameTransId equals t.TransId into tc
                           from tt in tc.DefaultIfEmpty()
                           where tt.Value.Contains(cond.ProductName)
                           select m.InternalNameTransId).ToList();
            }

            var query = (from q in baseRepository.GetList<ProductQty>()
                         join s in baseRepository.GetList<ProductSku>() on q.SkuId equals s.Id
                         join p in baseRepository.GetList<Product>() on s.ProductCode equals p.Code
                         join ps in baseRepository.GetList<ProductStatistics>() on s.ProductCode equals ps.Code into ProductStatisticTemp
                         from pst in ProductStatisticTemp.DefaultIfEmpty()
                         join t in baseRepository.GetList<Translation>() on new { a1 = pst.InternalNameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         where 1==1
                         && (merchIdStr == Guid.Empty || p.MerchantId == merchIdStr)
                         && (string.IsNullOrEmpty(prodCode) || p.Code.Contains(prodCode))
                         && (catalogId == Guid.Empty || p.CatalogId == catalogId)
                         && (attrVal1 == Guid.Empty || s.AttrValue1 == attrVal1)
                         && (attrVal2 == Guid.Empty || s.AttrValue2 == attrVal2)
                         && (attrVal3 == Guid.Empty || s.AttrValue3 == attrVal3)
                         && (salesQtyLower == 0 || q.SalesQty >= salesQtyLower)
                         && (salesQtyUpper == 0 || q.SalesQty <= salesQtyUpper)
                         && (tranIds.Count == 0 || (tranIds.Count > 0 && tranIds.Contains(pst.InternalNameTransId)))
                         && (pst == null || (pst != null && (pst.InternalNameTransId == Guid.Empty || pst.InternalNameTransId == p.NameTransId)))
                         select new InvSummary
                         {
                             HoldTotalQty = q.InvtHoldQty,
                             InventoryTotalQty = q.InvtActualQty,
                             ProdCode = s.ProductCode,
                             ProdName = (tt != null ? tt.Value : string.Empty),
                             ReservedTotalQty = q.InvtReservedQty,
                             SalesTotalQty = q.SalesQty,
                             //ProdImageId = p.DefaultImage
                         }).ToList();

            var groupQuery = query.GroupBy(g => new { g.ProdCode, g.ProdName }).Select(d => new InvSummary
            {
                HoldTotalQty = d.Sum(s => s.HoldTotalQty),
                InventoryTotalQty = d.Sum(s => s.InventoryTotalQty),
                ReservedTotalQty = d.Sum(s => s.ReservedTotalQty),
                SalesTotalQty = d.Sum(s => s.SalesTotalQty),
                ProdCode = d.Key.ProdCode,
                ProdName = d.Key.ProdName,
            }).ToList();

            if (cond != null && !cond.SortName.IsEmpty())
            {
                var sortBy = cond.SortOrder.ToUpper().ToEnum<SortType>();
                groupQuery = groupQuery.AsQueryable().SortBy(cond.SortName, sortBy).ToList();
            }
            else
            {
                groupQuery = groupQuery.OrderBy(o => o.ProdCode).ToList();
            }

            result.TotalRecord = groupQuery.Count;
            result.Data = groupQuery.Skip(cond.Offset).Take(cond.PageSize).ToList();
            return result;
        }
    }
}
