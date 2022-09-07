namespace BDMall.Repository.Impl
{
    public class InvTransactionDtlRepository : PublicBaseRepository, IInvTransactionDtlRepository
    {
        private List<KeyValue> _TransTypeDescList;

        public ICodeMasterRepository codeMasterRepository;
        public ITranslationRepository translationRepository;
        public IProductRepository productRepository;


        public InvTransactionDtlRepository(IServiceProvider service) : base(service)
        {
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            productRepository = Services.Resolve<IProductRepository>();
        }

        /// <summary>
        /// 獲取指定條件的庫存交易記錄
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        public PageData<InvFlow> GetInvTransDtlLst(InvFlowSrchCond cond)
        {
            PageData<InvFlow> result = new PageData<InvFlow>();

            List<Guid> tranIds = new List<Guid>();
            if (cond != null && !string.IsNullOrEmpty(cond.ProductName))
            {
                tranIds = (from m in baseRepository.GetList<ProductStatistics>()
                           join t in baseRepository.GetList<Translation>() on m.InternalNameTransId equals t.TransId into tc
                           from tt in tc.DefaultIfEmpty()
                           where tt.Value.Contains(cond.ProductName)
                           select m.InternalNameTransId).ToList();
            }

            DateTime? dtBegin = null;
            DateTime? dtEnd = null;

            if (!string.IsNullOrEmpty(cond.TransBeginDate))
            {
                dtBegin = DateUtil.ConvertoDateTime(cond.TransBeginDate, "yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(cond.TransEndDate))
            {
                dtEnd = DateUtil.ConvertoDateTime(cond.TransEndDate, "yyyy-MM-dd").AddDays(1);
            }
            var transTypeList = new List<InvTransType>();
            if (!string.IsNullOrEmpty(cond.TransTypeList))
            {
                var arr = cond.TransTypeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var transType in arr)
                {
                    int typeVal;
                    if (int.TryParse(transType, out typeVal))
                    {
                        if (typeVal != -1)
                        {
                            transTypeList.Add((InvTransType)typeVal);
                        }
                    }
                }
            }

            #region 组装查询
            var query = (from d in baseRepository.GetList<InvTransactionDtl>()
                             join s in baseRepository.GetList<ProductSku>() on d.Sku equals s.Id into skuTemp
                             from st in skuTemp.DefaultIfEmpty()
                             join u in baseRepository.GetList<User>() on d.CreateBy equals u.Id into uTemp
                             from ut in uTemp.DefaultIfEmpty()
                             join ps in baseRepository.GetList<ProductStatistics>() on st.ProductCode equals ps.Code into psTemp
                             from pst in psTemp.DefaultIfEmpty()
                             join w in baseRepository.GetList<Warehouse>() on d.WHId equals w.Id into wc
                             from ww in wc.DefaultIfEmpty()
                             join wt in baseRepository.GetList<Translation>() on new { a1 = ww.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = wt.TransId, a2 = wt.Lang } into wtc
                             from wtt in wtc.DefaultIfEmpty()
                             join p in baseRepository.GetList<Product>() on new { a1 = pst.Code, a2 = pst.InternalNameTransId } equals new
                             { a1 = p.Code, a2 = p.NameTransId } into pTemp
                             from pt in pTemp.DefaultIfEmpty()
                             join a in baseRepository.GetList<ProductAttributeValue>() on st.AttrValue1 equals a.Id into ac
                             from aa in ac.DefaultIfEmpty()
                             join at in baseRepository.GetList<Translation>() on new { a1 = aa.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = at.TransId, a2 = at.Lang } into atc
                             from att in atc.DefaultIfEmpty()
                             join b in baseRepository.GetList<ProductAttributeValue>() on st.AttrValue2 equals b.Id into bc
                             from bb in bc.DefaultIfEmpty()
                             join bt in baseRepository.GetList<Translation>() on new { a1 = bb.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = bt.TransId, a2 = bt.Lang } into btc
                             from btt in btc.DefaultIfEmpty()
                             join c in baseRepository.GetList<ProductAttributeValue>() on st.AttrValue3 equals c.Id into dc
                             from cc in dc.DefaultIfEmpty()
                             join ct in baseRepository.GetList<Translation>() on new { a1 = cc.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = ct.TransId, a2 = ct.Lang } into ctc
                             from ctt in ctc.DefaultIfEmpty()

                             where d.IsActive && d.IsDeleted == false
                             && (ut == null || (ut != null && ut.IsActive && !ut.IsDeleted))
                             && (string.IsNullOrEmpty(cond.ProductCode) || st.ProductCode == cond.ProductCode.Trim())
                             && (tranIds.Count == 0 || tranIds.Contains(pst.InternalNameTransId))
                             && (dtBegin == null || d.TransDate >= dtBegin)
                             && (dtEnd == null || d.TransDate < dtEnd)
                             && (transTypeList.Count == 0 || transTypeList.Contains(d.TransType))
                             && (cond.Attr1 == Guid.Empty || st.AttrValue1 == cond.Attr1)
                             && (cond.Attr2 == Guid.Empty || st.AttrValue2 == cond.Attr2)
                             && (cond.Attr3 == Guid.Empty || st.AttrValue3 == cond.Attr3)
                             && (cond.MerchantId == Guid.Empty || pt.MerchantId == cond.MerchantId)

                             select new
                             {
                                 Detail = d,
                                 Sku = st,
                                 User = ut,
                                 Product = pt,
                                 WhName = wtt.Value,
                                 Attr1Desc = att.Value,
                                 Attr2Desc = btt.Value,
                                 Attr3Desc = ctt.Value
                             }).Distinct();
            #endregion

            cond.SortName = cond.SortName ?? string.Empty;
            cond.SortBy = cond.SortBy ?? string.Empty;
            #region 排序
            string descStr = "DESC";
            if (!string.IsNullOrEmpty(cond.SortName))
            {
                if (cond.SortName == "ProductCode")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Sku.ProductCode);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Sku.ProductCode);
                    }
                }
                else if (cond.SortName == "ActionType")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Detail.TransType);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Detail.TransType);
                    }
                }
                else if (cond.SortName == "TransDate")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Detail.TransDate);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Detail.TransDate);
                    }
                }
                else if (cond.SortName == "TransQty")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Detail.TransQty);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Detail.TransQty);
                    }
                }
                else if (cond.SortName == "Attr1Desc")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Attr1Desc);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Attr1Desc);
                    }
                }
                else if (cond.SortName == "Attr2Desc")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Attr2Desc);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Attr2Desc);
                    }
                }
                else if (cond.SortName == "Attr3Desc")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.Attr3Desc);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Attr3Desc);
                    }
                }
                else if (cond.SortName == "WhName")
                {
                    if (cond.SortBy.ToUpper() == descStr)
                    {
                        query = query.OrderByDescending(x => x.WhName);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.WhName);
                    }
                }                
            }
            else
            {
                query = query.OrderByDescending(x => x.Detail.TransDate).OrderByDescending(x => x.Detail.CreateDate);
            }
            #endregion

            result.TotalRecord = query.Count();
            var infos = query.Skip(cond.Offset).Take(cond.PageSize).ToList();
            List<InvFlow> invFlowList = new List<InvFlow>();

            InitTransTypeList();          
            invFlowList = infos.Select(item => GenInvFlow(item.Detail, item.Sku, item.Attr1Desc, item.Attr2Desc, item.Attr3Desc, item.WhName,
                                                                    (item.Product?.Id ?? Guid.Empty),(item.User?.Name ?? ""))).ToList();

            result.Data = invFlowList;
            return result;           
        }

        /// <summary>
        /// 獲取交易項資料列表
        /// </summary>
        /// <param name="condition">搜尋條件</param>
        public List<InvTransItemView> GetPurchaseItmLst(InvTransSrchCond condition)
        {
            var itemVwList = new List<InvTransItemView>();

            if (condition != null && condition.ProdCodeList.Any())
            {              
                var merchId = condition.MerchantId;

                condition.ProdCodeList = condition.ProdCodeList.Distinct().ToList();

                foreach (string prodCodeItem in condition.ProdCodeList)
                {
                    if (!string.IsNullOrEmpty(prodCodeItem))
                    {
                        string prodCode = prodCodeItem.Trim();

                        var productList = productRepository.GetLastVersionProductLstByCode(new List<string>() { prodCode })?.Where(x => x.MerchantId == merchId).ToList();
                        if (productList != null && productList.Any())
                        {
                            var productSkuList = (from ps in baseRepository.GetList<ProductSku>()
                                                  where ps.IsActive && !ps.IsDeleted
                                                  && ps.ProductCode.ToUpper() == prodCode.Trim().ToUpper()
                                                  select ps).ToList();

                            foreach (var prodSku in productSkuList)
                            {
                                //按條件篩選屬性值
                                if (condition.Attr1 != Guid.Empty)
                                {
                                    if (prodSku.AttrValue1 != condition.Attr1) continue;
                                }
                                if (condition.Attr2 != Guid.Empty)
                                {
                                    if (prodSku.AttrValue2 != condition.Attr2) continue;
                                }
                                if (condition.Attr3 != Guid.Empty)
                                {
                                    if (prodSku.AttrValue3 != condition.Attr3) continue;
                                }

                                var transItem = new InvTransItemView()
                                {
                                    Sku = prodSku.Id,
                                    ProdCode = prodSku.ProductCode,
                                    Attr1 = prodSku.AttrValue1,
                                    Attr1Desc = GetAttrValDesc(prodSku.AttrValue1),
                                    Attr2 = prodSku.AttrValue2,
                                    Attr2Desc = GetAttrValDesc(prodSku.AttrValue2),
                                    Attr3 = prodSku.AttrValue3,
                                    Attr3Desc = GetAttrValDesc(prodSku.AttrValue3)
                                };

                                var product = productList.FirstOrDefault(x => x.Code.Trim().ToUpper() == prodCode.Trim().ToUpper());
                                if (product != null)
                                {
                                    transItem.CatalogId = product.CatalogId;
                                    transItem.ProdName = product.Name;
                                }
                                itemVwList.Add(transItem);
                            }

                        }
                    }
                }
            }

            return itemVwList;
        }

        public List<InvTransItemView> GetPurchaseReturnItmLst(InvTransSrchCond condition)
        {
            var itemVwList = new List<InvTransItemView>();
            if (condition != null && !string.IsNullOrEmpty(condition.BatchNum))
            {
                bool isMerchant = CurrentUser.IsMerchant;
                var merchId = condition.MerchantId;
                var curLang = CurrentUser.Lang;

                //篩選出產品
                var queryProd = (from po in baseRepository.GetList<PurchaseOrder>()
                                 join pod in baseRepository.GetList<PurchaseOrderDetail>()
                                 on po.Id equals pod.POId
                                 join ps in baseRepository.GetList<ProductSku>()
                                 on pod.Sku equals ps.Id
                                 join pq in baseRepository.GetList<ProductQty>()
                                 on pod.Sku equals pq.SkuId
                                 where po.IsActive && !po.IsDeleted && pod.IsActive && !pod.IsDeleted && ps.IsActive && !ps.IsDeleted
                                 && po.BatchNum.ToUpper() == condition.BatchNum.Trim().ToUpper() && (condition.WhId == Guid.Empty || po.WHId == condition.WhId)
                                 select new { po, pod, ps, pq }).ToList();

                foreach (var prodItem in queryProd)
                {
                    if (prodItem != null && prodItem.ps != null)
                    {
                        //按條件篩選屬性值
                        if (condition.Attr1 != Guid.Empty)
                        {
                            if (prodItem.ps.AttrValue1 != condition.Attr1) continue;
                        }
                        if (condition.Attr2 != Guid.Empty)
                        {
                            if (prodItem.ps.AttrValue2 != condition.Attr2) continue;
                        }
                        if (condition.Attr3 != Guid.Empty)
                        {
                            if (prodItem.ps.AttrValue3 != condition.Attr3) continue;
                        }

                        var transItem = new InvTransItemView()
                        {
                            Sku = prodItem.ps.Id,
                            ProdCode = prodItem.ps.ProductCode,
                            Attr1 = prodItem.ps.AttrValue1,
                            Attr1Desc = GetAttrValDesc(prodItem.ps.AttrValue1),
                            Attr2 = prodItem.ps.AttrValue2,
                            Attr2Desc = GetAttrValDesc(prodItem.ps.AttrValue2),
                            Attr3 = prodItem.ps.AttrValue3,
                            Attr3Desc = GetAttrValDesc(prodItem.ps.AttrValue3),
                            TransFrom = prodItem.po.WHId,
                            TransFromDesc = GetWarehouseName(prodItem.po.WHId),
                            TransTo = prodItem.po.SupplierId,
                            TransToDesc = GetSupplierName(prodItem.po.SupplierId),
                            ReturnQty = 0,
                            TransQty = prodItem.pod.OrderQty,
                            UnitPrice = prodItem.pod.UnitPrice,
                            SalesQty = prodItem.pq.SalesQty,
                        };

                        var productList = productRepository.GetLastVersionProductLstByCode(new List<string>() { prodItem.ps.ProductCode });
                        if (productList != null)
                        {
                            var product = productList.FirstOrDefault(x => x.MerchantId == merchId);
                            if (product != null)
                            {
                                transItem.CatalogId = product.CatalogId;
                                transItem.ProdName = product.Name;
                            }
                        }
                        itemVwList.Add(transItem);
                    }
                }
            }

            return itemVwList;
        }

        /// <summary>
        /// 獲取指定庫存屬性的屬性值列表
        /// </summary>
        /// <param name="attrId">屬性ID</param>
        private List<ProductAttributeValue> GetProductAttributeValues(Guid attrId)
        {
            var merchId = CurrentUser.MerchantId;
            bool isMerchant = CurrentUser.IsMerchant;
            var attrValList = new List<ProductAttributeValue> { new ProductAttributeValue {
                AttrId = attrId,             
                MerchantId = merchId,
                IsActive = true,
                IsDeleted = false 
            }};

            if (attrValList == null || !attrValList.Any())
                return attrValList;

            attrValList = baseRepository.GetList<ProductAttributeValue>().Where(x => x.AttrId == attrId
                                    && (isMerchant || (!isMerchant && x.MerchantId == merchId)) && x.IsActive && !x.IsDeleted).ToList();
            return attrValList;
        }

        private string GetAttrValDesc(Guid attrValId)
        {
            string desc = string.Empty;
            var attr = baseRepository.GetModel<ProductAttributeValue>(x => x.Id == attrValId);
            if (attr != null) desc = translationRepository.GetDescForLang(attr.DescTransId, CurrentUser.Lang);         
            return desc;
        }

        /// <summary>
        /// 獲取供應商名稱
        /// </summary>
        /// <param name="suppId">Id</param>
        private string GetSupplierName(Guid suppId)
        {
            string name = string.Empty;
            var supplier = baseRepository.GetModel<Supplier>(x => x.Id == suppId);
            if (supplier != null)
            {
                name = translationRepository.GetDescForLang(supplier.NameTransId ,CurrentUser.Lang);
            }
            return name;
        }

        /// <summary>
        /// 獲取倉庫名稱
        /// </summary>
        /// <param name="suppId">Id</param>
        private string GetWarehouseName(Guid whId)
        {
            string name = string.Empty;
            var warehouse = baseRepository.GetModel<Warehouse>(x => x.Id == whId);
            if (warehouse != null)
            {
                name = translationRepository.GetDescForLang(warehouse.NameTransId, CurrentUser.Lang);
            }
            return name;
        }

        /// <summary>
        /// 初始化庫存交易描述列表緩存
        /// </summary>
        private void InitTransTypeList()
        {
            var invTransTypeLst = codeMasterRepository.GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.InvtTransType);
            _TransTypeDescList  = invTransTypeLst.Select(transType => new KeyValue
                    {
                        Id = transType.Value.ToString(),
                        Text = transType.Description
                    }).ToList();
        }

        private InvFlow GenInvFlow(InvTransactionDtl transDtl, ProductSku skuInfo, string attr1Desc, string attr2Desc, string attr3Desc, string wName,Guid ProductId,string UserName)
        {
            InvFlow invFlow = null;

            if (transDtl == null)   return null;

            invFlow = new InvFlow()
            {
                Sku = transDtl.Sku,
                ProductCode = skuInfo.ProductCode,// transDtl.ProductSkuInfo.ProductCode,
                Attr1 = skuInfo.AttrValue1,// transDtl.ProductSkuInfo.AttrValue1,
                Attr1Desc = attr1Desc ?? "", //GetAttrValDesc(transDtl.ProductSkuInfo.AttrValue1),
                Attr2 = skuInfo.AttrValue2,//transDtl.ProductSkuInfo.AttrValue2,
                Attr2Desc = attr2Desc ?? "",
                Attr3 = skuInfo.AttrValue3,//transDtl.ProductSkuInfo.AttrValue3,
                Attr3Desc = attr3Desc ?? "",
                //GetAttrValDesc(transDtl.ProductSkuInfo.AttrValue3),
                TransType = transDtl.TransType,
                TransTypeDesc = _TransTypeDescList?.FirstOrDefault(x => x.Id == transDtl.TransType.ToInt().ToString())?.Text,
                TransDate = transDtl.TransDate,
                TransQty = transDtl.TransQty,
                CreateDate = transDtl.CreateDate,
                ProductId = ProductId,
                Handler = UserName,
            };

            string refNum = string.Empty;
            Language lang = CurrentUser.Lang;
            switch (transDtl.TransType)
            {
                case InvTransType.Purchase:
                    var poDtl = baseRepository.GetModel<PurchaseOrderDetail>(x => x.Id == transDtl.BizId);
                    if (poDtl != null)
                    {
                        var poOder = baseRepository.GetModelById<PurchaseOrder>(poDtl.POId);
                        invFlow.RefNo = poOder?.OrderNo ?? "";
                        invFlow.BatchNo = poOder?.BatchNum ?? "";
                        invFlow.Remarks = poOder?.Remarks ?? "";
                    }
                    break;
                case InvTransType.Relocation:
                    var roDtl = baseRepository.GetModel<RelocationOrderDetail>(x => x.Id == transDtl.BizId);
                    if (roDtl != null)
                    {
                        var roOder = baseRepository.GetModelById<RelocationOrder>(roDtl.ROId);
                        invFlow.RefNo = roOder?.OrderNo;
                        //invFlow.BatchNo = roDtl.ROInfo?.BatchNum;
                        invFlow.Remarks = roOder?.Remarks;
                    }
                    break;
                case InvTransType.PurchaseReturn:
                    var proDtl = baseRepository.GetModel<PurchaseReturnOrderDetail>(x => x.Id == transDtl.BizId);
                    if (proDtl != null)
                    {
                        var proOder = baseRepository.GetModelById<PurchaseReturnOrder>(proDtl.PROId);
                        invFlow.RefNo = proOder?.OrderNo ??  "";
                        invFlow.BatchNo = proOder?.BatchNum ?? "";
                        invFlow.Remarks = proOder?.Remarks ?? "";
                    }
                    break;
                case InvTransType.SalesShipment:
                    var soDtl = baseRepository.GetModel<OrderDelivery>(x => x.Id == transDtl.BizId);
                    if (soDtl != null)
                    {
                        invFlow.RefNo = soDtl.DeliveryNO;
                    }
                    break;
                case InvTransType.SalesReturn:
                    var sroDtl = baseRepository.GetModel<SalesReturnOrderDetail>(x => x.Id == transDtl.BizId);
                    if (sroDtl != null)
                    {
                        var sroOder = baseRepository.GetModelById<SalesReturnOrder>(sroDtl.SROId);
                        invFlow.RefNo = sroOder?.OrderNo ?? "";
                    }
                    break;
                default:
                    refNum = string.Empty;
                    break;
            }

            invFlow.WhName = wName ?? "";
            invFlow.IOTypeDesc = transDtl.IOType == InvTransIOType.I ? BDMall.Resources.Label.ImportWarehouse : BDMall.Resources.Label.ExportWarehouse;

            return invFlow;
        }
    }
}
