using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class InventoryBLL : BaseBLL, IInventoryBLL
    {
        public ITranslationRepository TranslationRepository;
        public IWarehouseRepository WarehouseRepository;
        public IInventoryRepository InvRepository;
        public IProductRepository ProductRepository;
        public IAttributeBLL AttributeBLL;
        public ISupplierRepository SupplierRepository;
        public ISettingBLL SettingBLL;
        public IInvTransactionDtlRepository InvTransactionDtlRepository;
        public IProductBLL ProductBLL;
        public IUpdateInventoryBLL UpdateInventoryBLL;
        public IDealProductQtyCacheBLL DealProductQtyCacheBLL;
        public IInvReservedRepository InvReservedRepository;
        public ICodeMasterRepository CodeMasterRepository;
        public IUpdateInventoryBLL updateInventoryBLL;
        public IInventoryChangeNotifyBLL inventoryChangeNotifyBLL;

        public InventoryBLL(IServiceProvider services) : base(services)
        {
            TranslationRepository = Services.Resolve<ITranslationRepository>();
            WarehouseRepository = Services.Resolve<IWarehouseRepository>();
            InvRepository = Services.Resolve<IInventoryRepository>();
            ProductRepository = Services.Resolve<IProductRepository>();
            AttributeBLL = Services.Resolve<IAttributeBLL>();
            SettingBLL = Services.Resolve<ISettingBLL>();

            SupplierRepository = Services.Resolve<ISupplierRepository>();
            InvTransactionDtlRepository = Services.Resolve<IInvTransactionDtlRepository>();

            ProductBLL = Services.Resolve<IProductBLL>();
            UpdateInventoryBLL = Services.Resolve<IUpdateInventoryBLL>();
            DealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            InvReservedRepository = Services.Resolve<IInvReservedRepository>();
            CodeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            updateInventoryBLL = Services.Resolve<IUpdateInventoryBLL>();
            inventoryChangeNotifyBLL = Services.Resolve<IInventoryChangeNotifyBLL>();
        }

        public List<WarehouseDto> GetWarehouseLstByCond(WarehouseDto cond)
        {
            var dbWareHouse = WarehouseRepository.GetWarehouseList(cond);
            var warehouseFullLst = AutoMapperExt.MapTo<List<WarehouseDto>>(dbWareHouse);
            if (warehouseFullLst != null && warehouseFullLst.Any())
            {
                foreach (var warehouse in warehouseFullLst)
                {
                    var names = TranslationRepository.GetTranslation(warehouse.NameTransId);
                    //warehouse.NameList = names;
                    warehouse.Name = names?.FirstOrDefault(x => x.Lang == CurrentUser.Lang)?.Value ?? "";

                    var contacts = TranslationRepository.GetTranslation(warehouse.ContactTransId);
                    //warehouse.ContactList = contacts;
                    warehouse.Contact = contacts?.FirstOrDefault(x => x.Lang == CurrentUser.Lang)?.Value ?? "";

                    var addresses = TranslationRepository.GetTranslation(warehouse.AddressTransId);
                    //warehouse.AddressList = addresses;
                    warehouse.Address = addresses?.FirstOrDefault(x => x.Lang == CurrentUser.Lang)?.Value ?? "";

                    bool nameChk = string.IsNullOrEmpty(cond?.Name) || (names != null && names.Any(x => string.Equals(x.Value, cond?.Name, StringComparison.CurrentCultureIgnoreCase)));
                    bool contactChk = string.IsNullOrEmpty(cond?.Contact) || (contacts != null && contacts.Any(x => string.Equals(x.Value, cond?.Contact, StringComparison.CurrentCultureIgnoreCase)));
                    bool addressChk = string.IsNullOrEmpty(cond?.Address) || (addresses != null && addresses.Any(x => string.Equals(x.Value, cond?.Address, StringComparison.CurrentCultureIgnoreCase)));

                    if (nameChk && contactChk && addressChk)
                    {
                        var merchant = baseRepository.GetModelById<Merchant>(warehouse.MerchantId);
                        if (merchant != null)
                        {
                            warehouse.MerchantName = TranslationRepository.GetDescForLang(merchant.NameTransId, CurrentUser.Lang);
                        }
                    }
                }
            }
            warehouseFullLst = warehouseFullLst.OrderBy(x => x.Name).ToList();

            return warehouseFullLst;
        }

        /// <summary>
        /// 批量刪除倉庫記錄
        /// </summary>
        /// <param name="recIDsList">待刪除的記錄ID列表</param>
        /// <returns>操作結果信息</returns>
        public SystemResult LogicDeleteWarehouses(string recIDsList)
        {
            SystemResult sysRslt = new SystemResult();
            if (!recIDsList.IsEmpty())
            {
                //將接收到的待刪除記錄ID字符串轉換成int數據列表               
                var recIdList = recIDsList.Split(',').Where(x => !x.IsEmpty()).Select(s => Guid.Parse(s)).ToList();
                if (recIdList.Any())
                {
                    List<Warehouse> warehouseLst = new List<Warehouse>();
                    foreach (var recId in recIdList)
                    {
                        var checkRes = WarehouseInventoryQtyCheck(recId);
                        if (!checkRes.Succeeded)
                        {
                            var whShow = GetWarehouseById(recId);
                            whShow.Name = whShow.NameList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty;
                            checkRes.Message = BDMall.Resources.Message.InventoryQtyNotNull + "(" + whShow.Name + ")";
                            return checkRes;
                        }

                        var warehouse = baseRepository.GetModelById<Warehouse>(recId);
                        warehouse.IsDeleted = true;
                        warehouse.UpdateDate = DateTime.Now;
                        warehouseLst.Add(warehouse);
                    }

                    baseRepository.Update(warehouseLst);
                    sysRslt.Succeeded = true;
                    return sysRslt;
                }
            }

            return sysRslt;
        }

        /// <summary>
        /// 獲取指定記錄ID的倉庫資料
        /// </summary>
        /// <param name="recId">記錄ID</param>
        /// <returns>倉庫資料</returns>
        public WarehouseDto GetWarehouseById(Guid recId)
        {
            WarehouseDto warehouse = null;
            warehouse = FindWarehouseByIdWithMerchId(recId);
            var supportLang = GetSupportLanguage();
            var emptyLangList = LangUtil.GetMutiLangFromTranslation(null, supportLang);
            if (warehouse == null)
            {
                warehouse = new WarehouseDto();
                warehouse.NameList = emptyLangList;
                warehouse.MerchantId = CurrentUser.MerchantId;
                warehouse.ContactList = emptyLangList;
                warehouse.AddressList = emptyLangList;
                return warehouse;
            }

            warehouse.NameList = TranslationRepository.GetMutiLanguage(warehouse.NameTransId);
            warehouse.Name = warehouse.NameList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty;
            warehouse.ContactList = TranslationRepository.GetMutiLanguage(warehouse.ContactTransId);
            if (warehouse.ContactList == null || !warehouse.ContactList.Any())
            {
                warehouse.ContactList = emptyLangList;
            }
            warehouse.Contact = warehouse.ContactList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty;
            warehouse.AddressList = TranslationRepository.GetMutiLanguage(warehouse.AddressTransId);
            if (warehouse.AddressList == null || !warehouse.AddressList.Any())
            {
                warehouse.AddressList = emptyLangList;
            }

            return warehouse;
        }

        public SystemResult Save(WhseView saveInfo)
        {
            var result = new SystemResult();
            var warehouse = new WarehouseDto
            {
                Id = saveInfo.Id,
                MerchantId = saveInfo.MerchantId,
                NameList = saveInfo.NameList,
                AddressList = saveInfo.AddressList,
                ContactList = saveInfo.ContactList,
                PhoneNum = saveInfo.PhoneNum,
                PostalCode = saveInfo.PostalCode,
                Remarks = saveInfo.Remarks,
                CostCenter = saveInfo.CostCenter,
                AccountCode = saveInfo.AccountCode,
            };

            if (!saveInfo.IsModify)
            {
                result = InsertWarehouse(warehouse);
            }
            else
            {
                result = UpdateWarehouse(warehouse);
            }

            return result;
        }

        public PageData<InvSummaryView> GetInvSummaryByPage(InvSrchCond cond)
        {
            var invtSummaryLst = new PageData<InvSummaryView>();

            var page = InvRepository.GetInvSummaryByPage(cond);
            if (page.Data.Any())
            {
                var views = page.Data.Select(invSummary => new InvSummaryView {

                    ProdImage = invSummary.ProdImage,
                    ProdCode = invSummary.ProdCode,
                    ProdName = invSummary.ProdName,
                    InventoryTotalQty = invSummary.InventoryTotalQty,
                    ReservedTotalQty = invSummary.ReservedTotalQty,
                    HoldingTotalQty = invSummary.HoldTotalQty,
                    SalesTotalQty = invSummary.SalesTotalQty,

                }).ToList();

                invtSummaryLst.TotalRecord = page.TotalRecord;
                invtSummaryLst.Data = views;
            }

            return invtSummaryLst;
        }

        public List<InvSummaryDetlView> GetInvSummaryDetlLst(string prodCode)
        {
            var detailList = new List<InvSummaryDetlView>();

            //var clientId = CurrentUser.ClientId;
            Language lang = CurrentUser.Lang;

            var queryList = (from i in baseRepository.GetList<Inventory>()
                             join s in baseRepository.GetList<ProductSku>()
                             on i.Sku equals s.Id
                             join w in baseRepository.GetList<Warehouse>()
                             on i.WHId equals w.Id
                             where s.ProductCode == prodCode
                             && i.IsActive && !i.IsDeleted
                             && s.IsActive && !s.IsDeleted
                              && w.IsActive && !w.IsDeleted
                             && i.MerchantId == w.MerchantId
                             select new
                             {
                                 i,
                                 s,
                                 w
                             }).ToList();

            var skuViewList = new Dictionary<Guid, SkuProductView>();

            foreach (var query in queryList)
            {
                Guid skuId = query.s.Id;
                SkuProductView skuView = null;
                if (!skuViewList.Keys.Contains(skuId))
                {
                    skuView = GetSkuProductView(skuId, lang);
                    if (skuView != null)
                    {
                        skuViewList.Add(skuId, skuView);
                    }
                }
                else
                {
                    skuView = skuViewList[skuId];
                }

                if (query.w.MerchantId != skuView.MerchanId)
                {
                    continue;
                }

                var invTransDtl = new InvSummaryDetlView()
                {
                    ProductCode = skuView.ProductCode,
                    ProductName = skuView.ProductName,
                    Sku = skuView.SkuId,
                    ImgPath = skuView.Image,
                    AttrValue1Desc = skuView.AttrValue1Name,
                    AttrValue2Desc = skuView.AttrValue2Name,
                    AttrValue3Desc = skuView.AttrValue3Name,
                    InventoryQty = query.i.Quantity,
                    WarehouseName = TranslationRepository.GetDescForLang(query.w.NameTransId, CurrentUser.Lang),
                };

                detailList.Add(invTransDtl);
            }

            return detailList;
        }

        public SkuProductView GetSkuProductView(Guid skuId, Language lang)
        {
            SkuProductView skuView = null;
            var dbsku = baseRepository.GetModel<ProductSku>(x => x.Id == skuId && x.IsActive && !x.IsDeleted);
            if (dbsku != null)
            {
                var sku = AutoMapperExt.MapTo<ProductSkuDto>(dbsku);
                var attr1 = AttributeBLL.GetProductAttribute(sku.Attr1, lang);
                var attr2 = AttributeBLL.GetProductAttribute(sku.Attr2, lang);
                var attr3 = AttributeBLL.GetProductAttribute(sku.Attr3, lang);
                sku.Attr1Name = attr1?.Desc ?? string.Empty;
                sku.Attr2Name = attr2?.Desc ?? string.Empty;
                sku.Attr3Name = attr3?.Desc ?? string.Empty;
                sku.AttrValue1Name = attr1?.AttributeValues == null ? string.Empty : attr1?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue1)?.Desc ?? string.Empty;
                sku.AttrValue2Name = attr2?.AttributeValues == null ? string.Empty : attr2?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue2)?.Desc ?? string.Empty;
                sku.AttrValue3Name = attr3?.AttributeValues == null ? string.Empty : attr3?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue3)?.Desc ?? string.Empty;

                var lvProduct = ProductRepository.GetLastVersionProductByCode(sku.ProductCode);
                if (lvProduct != null)
                {
                    var product = baseRepository.GetModelById<Product>(lvProduct.Id);
                    ProductSummary summary = new ProductSummary
                    {
                        Code = product.Code,
                        CurrencyCode = product.CurrencyCode,
                        Name = lvProduct.Name,
                        OriginalPrice = product.OriginalPrice,
                        SalePrice = product.SalePrice,
                        ProductId = product.Id,
                        MerchantId = product.MerchantId,
                        Imgs = new List<string>(),
                    };

                    if (!string.IsNullOrEmpty(lvProduct.SmallImage))
                    {
                        summary.Imgs.Add(lvProduct.SmallImage);
                    }

                    if (summary != null)
                    {
                        decimal addictionPrice = 0;
                        var prodAttrVals = (from d in baseRepository.GetList<ProductAttr>()
                                            join a in baseRepository.GetList<ProductAttrValue>() on d.Id equals a.ProdAttrId
                                            where a.IsActive && a.IsDeleted == false && d.ProductId == summary.ProductId && d.IsInv
                                            select a).ToList();
                        if (prodAttrVals != null && prodAttrVals.Any())
                        {
                            var prodAttrVal1 = prodAttrVals.FirstOrDefault(x => x.AttrValueId == sku.AttrValue1);
                            if (prodAttrVal1 != null)
                            {
                                addictionPrice += prodAttrVal1.AdditionalPrice;
                            }

                            var prodAttrVal2 = prodAttrVals.FirstOrDefault(x => x.AttrValueId == sku.AttrValue2);
                            if (prodAttrVal2 != null)
                            {
                                addictionPrice += prodAttrVal2.AdditionalPrice;
                            }

                            var prodAttrVal3 = prodAttrVals.FirstOrDefault(x => x.AttrValueId == sku.AttrValue3);
                            if (prodAttrVal3 != null)
                            {
                                addictionPrice += prodAttrVal3.AdditionalPrice;
                            }
                        }

                        skuView = new SkuProductView()
                        {
                            SkuId = sku.Id,
                            ProductId = summary.ProductId,
                            MerchanId = summary.MerchantId,
                            ProductCode = sku.ProductCode,
                            ProductName = summary.Name,
                            Attr1 = sku.Attr1,
                            Attr2 = sku.Attr2,
                            Attr3 = sku.Attr3,
                            AttrValue1 = sku.AttrValue1,
                            AttrValue2 = sku.AttrValue2,
                            AttrValue3 = sku.AttrValue3,
                            Attr1Name = sku.Attr1Name,
                            Attr2Name = sku.Attr2Name,
                            Attr3Name = sku.Attr3Name,
                            AttrValue1Name = sku.AttrValue1Name,
                            AttrValue2Name = sku.AttrValue2Name,
                            AttrValue3Name = sku.AttrValue3Name,
                            SalePrice = summary.SalePrice + addictionPrice,
                            OriginalPrice = summary.OriginalPrice + addictionPrice,
                            CurrencyCode = summary.CurrencyCode,
                        };

                        if (summary.Imgs.Any())
                        {
                            skuView.Image = summary.Imgs[0];
                        }
                    }

                }
                return skuView;
            }
            return skuView;
        }

        /// <summary>
        /// 獲取供應商列表的下拉框資源
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetSupplierComboSrc()
        {
            List<KeyValue> sourceList = new List<KeyValue>();
            var supplierList = GetSupplierLstByCond(new SupplierDto());
            sourceList = supplierList.Select(supplier => new KeyValue {
                Id = supplier.Id.ToString(),
                Text = supplier.Name
            }).ToList();

            return sourceList;
        }

        /// <summary>
        /// 獲取庫存流動類型列表的下拉框資源
        /// </summary>
        /// <returns>下拉框資源</returns>
        public List<KeyValue> GetInvFlowTypeLstComboSrc()
        {
            var transTypeList = SettingBLL.GetInvTransTypeLst();
            var result = transTypeList.Select(transType => new KeyValue {

                Id = transType.Value.ToString(),
                Text = transType.Description

            }).ToList();

            return result;
        }

        public List<KeyValue> GetInvTransTypeComboSrc()
        {
            var Ids = new string[] { InvTransType.Purchase.ToInt().ToString(), InvTransType.Relocation.ToInt().ToString(), InvTransType.PurchaseReturn.ToInt().ToString() };

            var transTypeList = GetInvFlowTypeLstComboSrc().Where(x => Ids.Contains(x.Id)).ToList();
            return transTypeList;
        }

        public List<KeyValue> GetWhseComboSrc(Guid merchantId)
        {
            var warehouseLst = GetWarehouseLstByCond(new WarehouseDto()).Where(p => p.MerchantId == merchantId).ToList();
            var keyValList = warehouseLst.Select(s => new KeyValue { Id = s.Id.ToString(), Text = s.Name }).ToList();
            return keyValList;
        }

        /// <summary>
        /// 獲取指定條件的庫存流動數據列表
        /// </summary>
        /// <param name="condition">搜尋條件</param>
        /// <returns>庫存流動數據列表</returns>
        public PageData<InvFlowView> GetInvFlowLstByCond(InvFlowSrchCond condition)
        {
            PageData<InvFlowView> result = new PageData<InvFlowView>();

            var flowList = InvTransactionDtlRepository.GetInvTransDtlLst(condition);
            result.TotalRecord = flowList.TotalRecord;
            result.Data = flowList.Data.Select(flow => new InvFlowView
            {
                SKU = flow.Sku,
                ProductCode = flow.ProductCode,
                ProductId = flow.ProductId,
                Attr1Desc = flow.Attr1Desc,
                Attr2Desc = flow.Attr2Desc,
                Attr3Desc = flow.Attr3Desc,
                ActionType = flow.TransTypeDesc,
                TransQty = flow.TransQty,
                RefNo = flow.RefNo,
                BatchNo = flow.BatchNo,
                Remarks = flow.Remarks,
                Handler = flow.Handler,

                WhName = flow.WhName,
                IOTypeDesc = flow.IOTypeDesc,
                TransDate = flow.TransDate != DateTime.MinValue ? flow.TransDate.ToString(Runtime.Setting.ShortDateFormat) : String.Empty,
                ImgPath = GetImage(flow.ProductId),

            }).ToList();

            return result;
        }

        /// <summary>
        /// 獲取指定條件的供應商信息列表
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        /// <returns>供應商信息列表</returns>
        public List<SupplierDto> GetSupplierLstByCond(SupplierDto cond)
        {
            var supplierLst = new List<SupplierDto>();

            var searchData = AutoMapperExt.MapTo<Supplier>(cond);

            var supplierFullLst = SupplierRepository.GetSupplierList(searchData);
            if (supplierFullLst != null && supplierFullLst.Any())
            {
                supplierLst = AutoMapperExt.MapTo<List<SupplierDto>>(supplierFullLst);

                foreach (var item in supplierLst)
                {
                    item.NameList = TranslationRepository.GetMutiLanguage(item.NameTransId);
                    item.Name = item.NameList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? "";
                }

                if (!cond.Name.IsEmpty())
                    supplierLst = supplierLst.Where(x => x.NameList.Select(s => s.Desc).Contains(cond.Name)).ToList();
            }

            supplierLst = supplierLst.OrderBy(x => x.Name).ToList();
            return supplierLst;
        }

        public List<InvTransItemView> GetPurchaseItmLst(InvTransSrchCond condition)
        {
            var invtTransItemVwList = InvTransactionDtlRepository.GetPurchaseItmLst(condition);
            return invtTransItemVwList;
        }

        public List<InvTransItemView> GetPurReturnItmLst(InvTransSrchCond condition)
        {
            var invtTransItemVwList = InvTransactionDtlRepository.GetPurchaseReturnItmLst(condition);
            return invtTransItemVwList;

        }

        public async Task<SystemResult> SaveInvTransRec(InvTransView transView)
        {
            var result = new SystemResult();

            transView.TransactionItemList = transView.TransactionItemList.Where(x => x.IsChecked).ToList();

            #region 检查采购批号
            if (transView.TransType == InvTransType.Purchase)
            {
                foreach (var item in transView.TransactionItemList)
                {
                    var batchNumChkCond = new InvTransactionDtlDto()
                    {
                        BatchNum = transView.BatchNum,
                        WHId = transView.TransTo,
                        Sku = item.Sku,
                    };
                    var chkRslt = IsExsitBathNum(batchNumChkCond);
                    if (chkRslt.Succeeded)
                    {
                        bool res = (bool)chkRslt.ReturnValue;
                        if (res)
                        {
                            result.Message = chkRslt.Message + " [" + item.ProdCode + "]";
                            return result;
                        }
                    }
                }
            }

            if (!transView.TransactionItemList.Any())
            {
                result.ReturnValue = false;
                result.Message = "";
                return result;
            }

            #endregion

            var transDtlList = transView.TransactionItemList.Select(transItemVw => new InvTransactionDtlDto
            {
                Sku = transItemVw.Sku,
                TransDate = transView.TransDate,
                BatchNum = transView.BatchNum,
                UnitPrice = transItemVw.UnitPrice,
                TransType = transView.TransType,
                Remarks = transView.Remarks,
                TransQty = transView.TransType == InvTransType.PurchaseReturn ? transItemVw.ReturnQty : transItemVw.TransQty,
                FromId = transView.TransType == InvTransType.PurchaseReturn ? transView.TransTo : transView.TransFrom,
                ToId = transView.TransType == InvTransType.PurchaseReturn ? transView.TransFrom : transView.TransTo,
            }).ToList();

            //var dbTranDtlList = AutoMapperExt.MapTo<List<InvTransactionDtl>>(transDtlList);
            result = await InsertInvTransList(transView.TransType, transDtlList);
            return result;
        }

        /// <summary>
        /// 新增庫存交易記錄
        /// </summary>
        /// <param name="transTyp">庫存交易類型</param>
        /// <param name="insertLst">庫存交易記錄</param>
        /// <returns>操作結果</returns>
        public async Task<SystemResult> InsertInvTransList(InvTransType transTyp, List<InvTransactionDtlDto> insertLst)
        {
            InvTransIOType? transIOTyp = SettingBLL.GetInvTransIOType(transTyp);
            if (transIOTyp == null) throw new BLException(InventoryErrorEnum.InvTransIOTypeNotExsit.ToString());

            var sysRslt = new SystemResult();
            sysRslt = InsertInvTransListWithSign(transTyp, insertLst, true);

            //处理Inventory表
            sysRslt = UpdateInventoryBLL.DealProductInventory(insertLst, transIOTyp, transTyp);
            UnitOfWork.Submit();

            //到货通知，消息处理请在这里实现
            if (sysRslt.Succeeded)
            {
                ///只处理采购，采购退回
                int[] cond = new int[] { InvTransType.Purchase.ToInt(), InvTransType.PurchaseReturn.ToInt() };
                if (cond.Any(x => x == transTyp.ToInt()))
                    sysRslt = await DealProductQtyCacheBLL.UpdateQtyWhenPurchaseOrReturn(insertLst);
                sysRslt.Succeeded = true;
            }

            return sysRslt;
        }

        /// <summary>
        /// 检查采购批号
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public SystemResult IsExsitBathNum(InvTransactionDtlDto cond)
        {
            var sysRslt = new SystemResult();

            var query = from po in baseRepository.GetList<PurchaseOrder>()
                        join pod in baseRepository.GetList<PurchaseOrderDetail>() on po.Id equals pod.POId
                        where po.IsActive && !po.IsDeleted && po.BatchNum == cond.BatchNum
                        && po.WHId == cond.WHId && pod.Sku == cond.Sku
                        select 1;

            if (query != null && query.Any())
            {
                sysRslt.ReturnValue = true;
                sysRslt.Message = BDMall.Resources.Message.BatchNumExsit;
            }
            else
            {
                sysRslt.ReturnValue = false;
                sysRslt.Message = string.Empty;
            }
            sysRslt.Succeeded = true;

            return sysRslt;
        }

        /// <summary>
        /// 使用預留記錄扣除庫存數量
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        public SystemResult DeductInvQtyWithReserve(InventoryReservedDto reserve)
        {
            SystemResult sysRslt = DeductInvQtyWithReservePreprocess(reserve);

            if (sysRslt.Succeeded)
            {
                //int qty = int.Parse(sysRslt.ReturnValue.ToString());
                var returnObj = sysRslt.ReturnValue as InventoryReservedDto;
                if (returnObj != null)
                {
                    reserve.ReservedQty = returnObj.ReservedQty;
                    reserve.WHId = returnObj.WHId;
                }
                var transDtlLst = GenSalesShipmentTransDtlLst(new List<InventoryReservedDto>() { reserve });
                sysRslt = updateInventoryBLL.DealProductInventory(transDtlLst, InvTransIOType.O, InvTransType.SalesShipment);
                sysRslt.ReturnValue = returnObj;

                //檢查是否售罄并發出通知
                ///InventoryChangeCheckAndNotify(reserve);
            }

            return sysRslt;
        }

        /// <summary>
        /// 檢查產品庫存量變化，并判斷需要觸發的通知類型
        /// </summary>
        /// <param name="reserve"></param>
        public void InventoryChangeCheckAndNotify(InventoryReservedDto reserve)
        {

            int invTotalQty = (int)GetActualInvQty(reserve);

            var productSku = baseRepository.GetModelById<ProductSku>(reserve.Sku);
            string prodCode = productSku?.ProductCode;
            var lvProduct = ProductRepository.GetLastVersionProductByCode(prodCode);
            if (lvProduct != null)
            {
                var product = baseRepository.GetModelById<Product>(lvProduct.Id);
                var prduxtExt = baseRepository.GetModel<ProductExtension>(x => x.Id == product.Id && x.IsActive && !x.IsDeleted);
                if (prduxtExt != null)
                {
                    //低於安全庫存
                    if (invTotalQty < prduxtExt.SafetyStock)
                    {
                        var notify = new InventoryChangeNotify()
                        {
                            SkuId = reserve.Sku,
                            Type = InvChangeNotifyType.LowThanSaftey,
                            //CurStockQty = invTotalQty,
                        };
                        inventoryChangeNotifyBLL.AddInventoryChangeNotify(notify);
                    }
                }
            }

            if (invTotalQty <= 0)
            {
                //售罄
                var notify = new InventoryChangeNotify()
                {
                    SkuId = reserve.Sku,
                    Type = InvChangeNotifyType.SoldOut,
                    //CurStockQty = invTotalQty,
                };
                inventoryChangeNotifyBLL.AddInventoryChangeNotify(notify);
            }
        }

        /// <summary>
        /// 取消庫存預留
        /// </summary>
        /// <param name="reserve">庫存預留</param>
        /// <param name="dbContext">數據庫操作對象</param>
        /// <returns>操作結果</returns>
        public SystemResult CancelInvReserved(InventoryReservedDto reserve)
        {
            SystemResult sysRslt = CancelInvReservedWithSign(reserve, false);
            return sysRslt;
        }

        /// <summary>
        /// 新增或更新庫存保留記錄
        /// </summary>
        /// <param name="insRec">庫存保留資料</param>
        public SystemResult InsertInventoryHold(InventoryHold insRec)
        {
            var sysRslt = new SystemResult();

            Guid skuId = insRec.SkuId;
            if (skuId == Guid.Empty)
            {
                throw new BLException("skuId不能為空值");
            }
            Guid memberId = insRec.MemberId;
            if (memberId == Guid.Empty)
            {
                throw new BLException("memberId不能為空值");
            }

            var invtHold = baseRepository.GetModel<InventoryHold>(x => x.SkuId == skuId && x.MemberId == memberId && x.IsActive && !x.IsDeleted);

            if (invtHold != null)
            {
                //存在原記錄，更新
                invtHold.Qty = insRec.Qty;
                baseRepository.Update(invtHold);
                sysRslt.Succeeded = true;
            }
            else
            {
                //不存在原記錄，新增
                invtHold = new InventoryHold()
                {
                    Id = Guid.NewGuid(),
                    SkuId = insRec.SkuId,
                    MemberId = insRec.MemberId,
                    Qty = insRec.Qty,
                };
                baseRepository.Insert(invtHold);

                sysRslt.Succeeded = true;
            }

            return sysRslt;
        }

        public SystemResult DeleteInventoryHold(InventoryHold delRec)
        {
            var sysRslt = new SystemResult();

            Guid skuId = delRec.SkuId;
            Guid memberId = delRec.MemberId;

            var invtHolds = baseRepository.GetList<InventoryHold>(x => x.SkuId == skuId && x.MemberId == memberId && x.IsActive && !x.IsDeleted).ToList();
            baseRepository.Delete(invtHolds);
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        /// <summary>
        /// 獲取總的可用庫存數量
        /// </summary>
        /// <param name="uniqueProp">庫存產品唯一標識</param>
        /// <returns>可用的庫存數量</returns>
        public decimal GetTotAvailableInvQty(InventoryReservedDto uniqueProp)
        {
            decimal invtQty = decimal.Zero;

            //產品庫存記錄（各個倉庫）
            var currentInvtList = InvRepository.GetInventoryList(new InventoryDto()
            {
                Sku = uniqueProp.Sku,
            });

            //庫存預留記錄
            var reservedDtlList = InvReservedRepository.GetInvReservedLst(new InventoryReserved()
            {
                Sku = uniqueProp.Sku,
                ProcessState = InvReservedState.RESERVED
            });

            //庫存留貨記錄
            var holdDtlList = baseRepository.GetList<InventoryHold>(x => x.SkuId == uniqueProp.Sku && x.IsActive && !x.IsDeleted).ToList();

            decimal actualInvtQty = decimal.Zero;
            decimal reservedQty = decimal.Zero;
            decimal holdQty = decimal.Zero;
            if (currentInvtList != null)
            {
                actualInvtQty = currentInvtList.Sum(x => x.Quantity);
            }
            if (reservedDtlList != null)
            {
                reservedQty = reservedDtlList.Sum(x => x.ReservedQty);
            }
            if (holdDtlList != null)
            {
                holdQty = holdDtlList.Sum(x => x.Qty);
            }

            invtQty = actualInvtQty - reservedQty - holdQty;
            return invtQty;
        }

        /// <summary>
        /// 根据用户Id添加庫存預留
        /// </summary>
        /// <param name="reserve"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public SystemResult AddInvReserved(InventoryReservedDto reserve, Guid memberId)
        {
            SystemResult sysRslt = AddInvReservedWithSign(reserve, false, memberId);
            return sysRslt;
        }

        /// <summary>
        /// 檢查是否存在指定的庫存保留記錄
        /// </summary>
        /// <param name="curRec">庫存保留資料</param>
        /// <returns></returns>
        public SystemResult IsExsitInventoryHold(InventoryHold curRec)
        {
            var sysRslt = new SystemResult();
            Guid skuId = curRec.SkuId;
            Guid memberId = curRec.MemberId;

            var invtHold = baseRepository.Any<InventoryHold>(x => x.SkuId == skuId && x.MemberId == memberId && x.IsActive && !x.IsDeleted);
            if (invtHold)
            {
                sysRslt.Succeeded = true;
            }
            return sysRslt;
        }

        /// <summary>
        /// 保存銷售退回數據
        /// </summary>
        /// <param name="saveInfo">保存數據</param>
        /// <returns>操作結果</returns>
        public SystemResult SaveSalesReturnRec(SalesReturnOrderDto salesReturnIns)
        {
            var sysRslt = new SystemResult();

            if (salesReturnIns == null || !salesReturnIns.SalesReturnItemList.Any()) throw new BLException(Resources.Message.DataVerifyFail);
            salesReturnIns.Validate();

            var transList = salesReturnIns.SalesReturnItemList.Select(rtnItem => new InvTransactionDtlDto
            {
                Sku = rtnItem.Sku,
                TransQty = rtnItem.ReturnQty,
                TransDate = DateTime.Now,
                TransType = InvTransType.SalesReturn,
                ToId = rtnItem.WHId,
                SOId = salesReturnIns.SOId,
                UnitPrice = rtnItem.UnitPrice

            }).ToList();
           
            sysRslt = InsertInvTransListWithSign(InvTransType.SalesReturn, transList, true);
            return sysRslt;    
        }

        /// <summary>
        /// 獲取總的實際庫存數量
        /// </summary>
        private decimal GetActualInvQty(InventoryReservedDto uniqueProp)
        {
            decimal invtQty = decimal.Zero;
            //產品庫存記錄（各個倉庫）
            var currentInvtList = InvRepository.GetInventoryList(new InventoryDto()
            {
                Sku = uniqueProp.Sku
            });

            if (currentInvtList != null)
            {
                invtQty = currentInvtList.Sum(x => x.Quantity);
            }
            return invtQty;
        }

        /// <summary>
        /// 使用是否批量標識取消庫存預留
        /// </summary>
        /// <param name="reserve">預留信息</param>
        /// <param name="isBatch">是否批量處理</param>
        private SystemResult CancelInvReservedWithSign(InventoryReservedDto reserve, bool isBatch)
        {
            var sysRslt = new SystemResult();

            InventoryReserved currentReserved = null;//當前預留記錄

            #region 獲取當前預留記錄

            var currentInvReservedLst = InvReservedRepository.GetInvReservedLst(new InventoryReserved()
            {
                OrderId = reserve.OrderId,
                Sku = reserve.Sku,
                ProcessState = InvReservedState.RESERVED
            });

            if (currentInvReservedLst == null) throw new BLException($"订单[{reserve.OrderId}]的库存预留记录不存在");
            currentReserved = currentInvReservedLst.FirstOrDefault();
            if (currentReserved.ProcessState != InvReservedState.RESERVED) throw new BLException($"订单[{reserve.OrderId}]的库存预留记录状态不是预留中");
            #endregion

            currentReserved.ProcessState = InvReservedState.CANCEL;
            baseRepository.Update(currentReserved);

            if (!isBatch) UnitOfWork.Submit();
            sysRslt.Succeeded = true;       
            return sysRslt;
        }

        /// <summary>
        /// 使用預留記錄扣除庫存前的預處理
        /// </summary>
        /// <param name="reserve">預留記錄</param>
        private SystemResult DeductInvQtyWithReservePreprocess(InventoryReservedDto reserve)
        {
            SystemResult sysRslt = new SystemResult();
            decimal availableInvTotQty = GetTotAvailableInvQty(reserve);//可用庫存總數量

            //指定倉庫庫存記錄
            var currentInvtLst = InvRepository.GetInventoryList(new InventoryDto() { Sku = reserve.Sku, WHId = reserve.WHId.Value });
            var currentInvt = currentInvtLst.FirstOrDefault();

            if (currentInvt != null && currentInvt.Quantity < 1)//如果選擇的倉庫冇貨，就自動選擇有該產品的倉庫
            {
                currentInvtLst = InvRepository.GetInventoryList(new InventoryDto() { Sku = reserve.Sku, WHId = Guid.Empty });
                currentInvtLst = currentInvtLst.Where(x => x.Quantity > 0).ToList();
            }

            //庫存預留記錄
            var currentInvReservedLst = InvReservedRepository.GetInvReservedLst(new InventoryReserved()
            {
                OrderId = reserve.OrderId,
                SubOrderId = reserve.SubOrderId,
                Sku = reserve.Sku,
                ProcessState = InvReservedState.RESERVED//預留中狀態
            });

            if ((!currentInvtLst?.Any() ?? false) || (!currentInvReservedLst?.Any() ?? false )) throw new BLException($"找不到库存记录或库存预留记录,信息,订单[{reserve.OrderId}],Sku[{reserve.Sku}],仓位[{reserve.WHId}]");

            currentInvt = currentInvtLst.FirstOrDefault();
            var currentInvReserved = currentInvReservedLst.FirstOrDefault();

            reserve.WHId = currentInvt.WHId;

            decimal availableWHQty = currentInvt?.Quantity ?? 0;//指定倉庫的庫存數量
            decimal reservedQty = currentInvReserved.ReservedQty;
            decimal diffQty = availableWHQty - reservedQty;

            if (diffQty < 0) throw new BLException($"检测到库存预留记录的库存不足，无法进行扣减,信息:订单[{reserve.OrderId}],仓位[{reserve.WHId}],预留记录[{currentInvReserved.Id}]");

            //更新庫存預存記錄的狀態
            currentInvReserved.ProcessState = InvReservedState.FINISH;
            currentInvReserved.UpdateDate = DateTime.Now;
            baseRepository.Update(currentInvReserved);

            sysRslt.ReturnValue = new InventoryReservedDto() { ReservedQty = (int)reservedQty, WHId = reserve.WHId };
            sysRslt.Succeeded = true;

            return sysRslt;
        }

        /// <summary>
        /// 生成銷售出貨的交易記錄列表
        /// </summary>
        /// <param name="reservedLst"></param>
        private List<InvTransactionDtlDto> GenSalesShipmentTransDtlLst(List<InventoryReservedDto> reservedLst)
        {
            var transDtlLst = reservedLst.Select(reserved => new InvTransactionDtlDto
            {
                Sku = reserved.Sku,
                FromId = reserved.WHId.Value,
                BizId = reserved.SubOrderId,
                TransQty = reserved.ReservedQty,
                TransDate = DateTime.Now,
                TransType = InvTransType.SalesShipment,
                WHId = reserved.WHId.Value,

            }).ToList();
            return transDtlLst;
        }

        string GetImage(Guid ProductId)
        {
            var images = ProductBLL.GetProductImages(ProductId);
            if (images==null && !images.Any())
            {
                return "";
            }
            return images.FirstOrDefault();
        }

        /// <summary>
        /// 插入倉庫資料
        /// </summary>
        /// <param name="recInsert">待插入信息</param>
        /// <returns>結果</returns>
        SystemResult InsertWarehouse(WarehouseDto recInsert)
        {
            SystemResult sysRslt = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;
            recInsert.Id = Guid.NewGuid();
            recInsert.NameTransId = TranslationRepository.InsertMutiLanguage(recInsert.NameList, TranslationType.WareHouse);
            recInsert.ContactTransId = TranslationRepository.InsertMutiLanguage(recInsert.ContactList, TranslationType.WareHouse);
            recInsert.AddressTransId = TranslationRepository.InsertMutiLanguage(recInsert.AddressList, TranslationType.WareHouse);

            var dbModel = AutoMapperExt.MapTo<Warehouse>(recInsert);
            dbModel.UpdateBy = Guid.Parse(CurrentUser.UserId);
            dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
            dbModel.PostalCode = "";
            dbModel.Remarks = "";
            dbModel.PhoneNum = "";
            dbModel.CostCenter = "";
            dbModel.AccountCode = "";
            dbModel.IsActive = true;

            baseRepository.Insert(dbModel);
            UnitOfWork.Submit();

            sysRslt.ReturnValue = recInsert.Id.ToString();
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        SystemResult UpdateWarehouse(WarehouseDto recUpdate)
        {
            var result = new SystemResult();
            //獲取存在數據中的舊數據記錄
            var warehouse = baseRepository.GetModel<Warehouse>(x => x.Id == recUpdate.Id);
            if (warehouse != null)
            {
                UnitOfWork.IsUnitSubmit = true;
                warehouse.NameTransId = TranslationRepository.UpdateMutiLanguage(recUpdate.NameTransId, recUpdate.NameList, TranslationType.WareHouse);
                warehouse.ContactTransId = TranslationRepository.UpdateMutiLanguage(recUpdate.ContactTransId, recUpdate.ContactList, TranslationType.WareHouse);
                warehouse.AddressTransId = TranslationRepository.UpdateMutiLanguage(recUpdate.AddressTransId, recUpdate.AddressList, TranslationType.WareHouse);
                warehouse.UpdateDate = DateTime.Now;
                baseRepository.Update(warehouse);
                UnitOfWork.Submit();

                result.Succeeded = true;
            }
            return result;
        }

        /// <summary>
        /// 檢查指定倉庫是否存在庫存記錄,没有记录返回true,有记录返回false
        /// </summary>
        /// <param name="whId"></param>
        /// <returns></returns>
        private SystemResult WarehouseInventoryQtyCheck(Guid whId)
        {
            SystemResult sysRslt = new SystemResult();

            var inentoryLst = InvRepository.GetInventoryList(new InventoryDto() { WHId = whId });
            if (inentoryLst == null || !inentoryLst.Any())
            {
                sysRslt.Succeeded = true;//沒有庫存記錄時可刪除
            }

            return sysRslt;
        }

        private WarehouseDto FindWarehouseByIdWithMerchId(Guid recId)
        {
            WarehouseDto warehouse = null;

            var dbWareHouse = baseRepository.GetModelById<Warehouse>(recId);
            if (dbWareHouse == null) return warehouse;

            warehouse = AutoMapperExt.MapTo<WarehouseDto>(dbWareHouse);
            var merchant = baseRepository.GetModelById<Merchant>(warehouse.MerchantId);
            if (merchant != null)
            {
                warehouse.MerchantName = TranslationRepository.GetDescForLang(merchant.NameTransId, CurrentUser.Lang);
            }
            //非 BuyDong User需要檢查是否匹配商家ID
            if (CurrentUser.IsMerchant)
            {
                warehouse = warehouse.MerchantId == CurrentUser.MerchantId ? warehouse : null;
            }
            return warehouse;
        }

        private SystemResult InsertInvTransListWithSign(InvTransType transTyp, List<InvTransactionDtlDto> insertLst, bool whetherCommit)
        {
            var sysRslt = new SystemResult();

            //InvTransIOType? transIOTyp = SettingBLL.GetInvTransIOType(transTyp);
            //if (transIOTyp == null) throw new BLException(InventoryErrorEnum.InvTransIOTypeNotExsit.ToString());
             
            #region 生成單據

            UnitOfWork.IsUnitSubmit = true;
            switch (transTyp)
            {
                case InvTransType.Purchase:sysRslt = CreatePurchaseOrder(insertLst); break;
                case InvTransType.Relocation:sysRslt= CreateRelocationOrder(insertLst); break;                  
                case InvTransType.PurchaseReturn:sysRslt = CreatePurchaseReturnOrder(insertLst); break;                    
                case InvTransType.SalesReturn:sysRslt = CreateSalesReturnOrder(insertLst);break;                 
                default: break;
            }
            #endregion
            return sysRslt;
        }

        /// <summary>
        /// 创建采购单
        /// </summary>
        /// <param name="insertLst"></param>
        /// <returns></returns>
        SystemResult CreatePurchaseOrder(List<InvTransactionDtlDto> insertLst)
        {
            var sysRslt = new SystemResult();

            var order = new PurchaseOrder()
            {
                Id = Guid.NewGuid(),
                InDate = DateTime.Now,
                OrderNo = AutoGenerateNumber("PO"),
                SupplierId = insertLst.FirstOrDefault().ToId,
                WHId = insertLst.FirstOrDefault().ToId,
                Remarks = insertLst.FirstOrDefault().Remarks ?? string.Empty,
                BatchNum = insertLst.FirstOrDefault().BatchNum ?? string.Empty
            };
            baseRepository.Insert(order);

            var poDtlList = new List<PurchaseOrderDetail>();
            foreach (var item in insertLst)
            {
                PurchaseOrderDetail orderDtl = new PurchaseOrderDetail()
                {
                    Id = Guid.NewGuid(),
                    POId = order.Id,
                    Sku = item.Sku,
                    OrderQty = item.TransQty,
                    UnitPrice = item.UnitPrice
                };
                poDtlList.Add(orderDtl);

                item.BizId = orderDtl.Id;
            }
            baseRepository.Insert(poDtlList);
            sysRslt.ReturnValue = insertLst;
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        /// <summary>
        /// 创建调拨单
        /// </summary>
        /// <param name="insertLst"></param>
        /// <returns></returns>
        SystemResult CreateRelocationOrder(List<InvTransactionDtlDto> insertLst)
        {
            var sysRslt = new SystemResult();
            Guid fromId = insertLst.FirstOrDefault().FromId;
            Guid toId = insertLst.FirstOrDefault().ToId;
            if (fromId == toId)//出入庫不能相同
            {              
                sysRslt.Message = Resources.Message.PleaseSelectDiffWarehouse;
                return sysRslt;
            }

            var order = new RelocationOrder()
            {
                Id = Guid.NewGuid(),
                OrderNo = AutoGenerateNumber("RO"),
                ExportWHId = fromId,
                ImportWHId = toId,
                RelocateDate = DateTime.Now,
                Remarks = insertLst.FirstOrDefault().Remarks ?? string.Empty
            };
            baseRepository.Insert(order);

            var roDtlList = new List<RelocationOrderDetail>();
            foreach (var item in insertLst)
            {
                var orderDtl = new RelocationOrderDetail()
                {
                    Id = Guid.NewGuid(),                   
                    ROId = order.Id,
                    Sku = item.Sku,
                    OrderQty = item.TransQty
                };
                roDtlList.Add(orderDtl);

                item.BizId = orderDtl.Id;
            }
            baseRepository.Insert(roDtlList);
            sysRslt.ReturnValue = insertLst;
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        /// <summary>
        /// 创建采购退回单
        /// </summary>
        /// <param name="insertLst"></param>
        /// <returns></returns>
        SystemResult CreatePurchaseReturnOrder(List<InvTransactionDtlDto> insertLst)
        {
            var sysRslt = new SystemResult();

            var order = new PurchaseReturnOrder()
            {
                Id = Guid.NewGuid(),            
                OutDate = DateTime.Now,
                OrderNo = AutoGenerateNumber("PRO"),
                WHId = insertLst.FirstOrDefault().FromId,
                SupplierId = insertLst.FirstOrDefault().ToId,
                BatchNum = insertLst.FirstOrDefault().BatchNum ?? string.Empty,
                Remarks = insertLst.FirstOrDefault().Remarks ?? string.Empty,
            };
            baseRepository.Insert(order);

            var dtlList = new List<PurchaseReturnOrderDetail>();
            foreach (var item in insertLst)
            {
                PurchaseReturnOrderDetail orderDtl = new PurchaseReturnOrderDetail()
                {
                    Id = Guid.NewGuid(),
                   
                    OrderQty = item.TransQty,
                    PROId = order.Id,
                    Sku = item.Sku,
                    UnitPrice = item.UnitPrice
                };
                dtlList.Add(orderDtl);

                item.BizId = orderDtl.Id;
            }
            baseRepository.Insert(dtlList);
            sysRslt.ReturnValue = insertLst;
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        /// <summary>
        /// 创建销售退回单
        /// </summary>
        /// <param name="insertLst"></param>
        /// <returns></returns>
        SystemResult CreateSalesReturnOrder(List<InvTransactionDtlDto> insertLst)
        {
            var sysRslt = new SystemResult();

            Guid fromId = insertLst[0].FromId;
            Guid toId = insertLst[0].ToId;
            Guid soId = insertLst[0].SOId ?? Guid.Empty;

            var order = new SalesReturnOrder()
            {
                Id = Guid.NewGuid(),
                ReturnDate = DateTime.Now,
                OrderNo = AutoGenerateNumber("SRO"),
                //WHId = toId,
                SOId = soId
            };
            baseRepository.Insert(order);

            var dtlList = new List<SalesReturnOrderDetail>();
            foreach (var item in insertLst)
            {
                var orderDtl = new SalesReturnOrderDetail()
                {
                    Id = Guid.NewGuid(),                
                    SROId = order.Id,                  
                    Sku = item.Sku,
                    UnitPrice = item.UnitPrice,
                    ReturnQty = item.TransQty,
                    //WHId =toId,
                };
                dtlList.Add(orderDtl);

                item.BizId = orderDtl.Id;
            }
            baseRepository.Insert(dtlList);

            sysRslt.ReturnValue = insertLst;
            sysRslt.Succeeded = true;
            return sysRslt;
        }

        /// <summary>
        /// 使用是否批量標識添加庫存預留
        /// </summary>
        /// <param name="reserve">預留信息</param>
        /// <param name="isBatch">是否批量處理</param>
        /// <param name="memberId">用户Id</param>
        /// <returns></returns>
        private SystemResult AddInvReservedWithSign(InventoryReservedDto reserve, bool isBatch, Guid memberId)
        {
            SystemResult sysRslt = new SystemResult();

            int reservedQty = reserve.ReservedQty;//需要預留的數量
            InvReservedType invtReservedTyp = InvReservedType.PRESELL;//需要預留的類型  

            /*新增預留記錄時，先判斷Hold貨記錄是否存在并數量相等，如果不存在，則檢查庫存是否足夠扣除預留數量，
             * 是則新增正常預留記錄，
             * 否則新增預訂預留記錄*/
            var invtHold =  baseRepository.GetModel<InventoryHold>(x => x.SkuId == reserve.Sku && x.MemberId == memberId && x.IsActive && !x.IsDeleted);
            if (invtHold != null)
            {
                if (invtHold.Qty < reservedQty)
                {                  
                    throw new BLException("库存预留数据不正确");
                    
                }
                else
                {
                    invtReservedTyp = InvReservedType.NORMAL;
                }
            }
            else
            {
                decimal availableQty = GetTotAvailableInvQty(reserve);//可用庫存總數量                 

                if (availableQty > 0 && reservedQty <= availableQty)
                {
                    //庫存足夠扣除預留數量
                    invtReservedTyp = InvReservedType.NORMAL;
                }
                else
                {
                    //庫存不足，獲取是夠可以預訂的標識
                    var preSwitch = CodeMasterRepository.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.InvtReserved.ToString(), "PresellSwitch");
                    bool canPresell = bool.Parse(preSwitch.Value);
                    if (!canPresell)
                    {
                        if (availableQty <= 0) throw new BLException("库存为零");                        
                        else  throw new BLException("库存不足");                      
                       
                    }
                }
            }

            var reservedLst = InvReservedRepository.GetInvReservedLst(new InventoryReserved()
            {
                OrderId = reserve.OrderId,
                SubOrderId = reserve.SubOrderId,
                Sku = reserve.Sku,
                ProcessState = InvReservedState.RESERVED
            });
            if (reservedLst?.Any() ?? false) throw new BLException("已存在的预留记录，无法新增");

            var reservedDetail = new InventoryReserved()
            {
                Id = Guid.NewGuid(),
                OrderId = reserve.OrderId,
                SubOrderId = reserve.SubOrderId,
                Sku = reserve.Sku,
                ProcessState = InvReservedState.RESERVED,
                ReservedType = invtReservedTyp,
                ReservedQty = reservedQty
            };

            baseRepository.Insert(reservedDetail);
            sysRslt.Succeeded = true;
            return sysRslt;
        }
    }
}
