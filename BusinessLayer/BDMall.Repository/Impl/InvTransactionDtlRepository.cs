using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository.Impl
{
    public class InvTransactionDtlRepository : PublicBaseRepository, IInvTransactionDtlRepository
    {
        private List<KeyValue> _TransTypeDescList;

        public ICodeMasterRepository codeMasterRepository;

        public InvTransactionDtlRepository(IServiceProvider service) : base(service)
        {
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
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
                TransTypeDesc = _TransTypeDescList?.FirstOrDefault(x => x.Id == transDtl.TransType.ToString())?.Text,
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
                        invFlow.RefNo = poDtl.POInfo?.OrderNo;
                        invFlow.BatchNo = poDtl.POInfo?.BatchNum;
                        invFlow.Remarks = poDtl.POInfo?.Remarks;
                    }
                    break;
                case InvTransType.Relocation:
                    var roDtl = baseRepository.GetModel<RelocationOrderDetail>(x => x.Id == transDtl.BizId);
                    if (roDtl != null)
                    {
                        invFlow.RefNo = roDtl.ROInfo?.OrderNo;
                        //invFlow.BatchNo = roDtl.ROInfo?.BatchNum;
                        invFlow.Remarks = roDtl.ROInfo?.Remarks;
                    }
                    break;
                case InvTransType.PurchaseReturn:
                    var proDtl = baseRepository.GetModel<PurchaseReturnOrderDetail>(x => x.Id == transDtl.BizId);
                    if (proDtl != null)
                    {
                        invFlow.RefNo = proDtl.PROInfo?.OrderNo;
                        invFlow.BatchNo = proDtl.PROInfo?.BatchNum;
                        invFlow.Remarks = proDtl.PROInfo?.Remarks;
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
                        invFlow.RefNo = sroDtl.SROInfo?.OrderNo;
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
