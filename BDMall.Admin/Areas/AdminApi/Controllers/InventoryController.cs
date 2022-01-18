using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.App.Areas.AdminAPI.Controllers
{
    /// <summary>
    /// 倉存管理API
    /// </summary>
    /// 
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.IventoryModule, Modules = new string[] { })]
    [ApiController] 
    public class InventoryController : BaseApiController
    {
        public IUserBLL UserBLL;
        public IInventoryBLL InventoryBLL;
        public IMerchantBLL MerchantBLL;
        public IAttributeBLL AttributeBLL;
        public IProductBLL ProductBLL;
        public IDealProductQtyCacheBLL DealProductQtyCacheBLL;

        public InventoryController(IComponentContext services) : base(services)
        {
            MerchantBLL = Services.Resolve<IMerchantBLL>();
            AttributeBLL = Services.Resolve<IAttributeBLL>();
            ProductBLL = Services.Resolve<IProductBLL>();
            DealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            InventoryBLL = Services.Resolve<IInventoryBLL>();
            UserBLL = Services.Resolve<IUserBLL>();
        }

        /// <summary>
        /// 搜尋倉庫信息列表
        /// </summary>
        /// <param name="pageInfo">搜尋條件對象</param>
        /// <returns>倉庫信息列表</returns>
        [HttpPost]
        public List<WhseView> SearchWhseLst(WhsePageInfo pageInfo)
        {
            List<WhseView> whseViewLst = new List<WhseView>();

            if (pageInfo != null && pageInfo.Condition != null)
            {
                var condition = new WarehouseDto
                {
                    Name = pageInfo.Condition.WarehouseName,
                };
                if (!pageInfo.Condition.MerchantId.IsEmpty())
                {
                    condition.MerchantId = new Guid(pageInfo.Condition.MerchantId);
                }

                var whseList = InventoryBLL.GetWarehouseLstByCond(condition);

                whseViewLst = whseList.Select(s => new WhseView
                {

                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Contact = s.Contact,
                    PhoneNum = s.PhoneNum,
                    PostalCode = s.PostalCode,
                    Remarks = s.Remarks,
                    CostCenter = s.CostCenter,
                    AccountCode = s.AccountCode,
                    MerchantName = s.MerchantName,
                    CreateDate = s.CreateDate,

                }).ToList();
                if (whseViewLst != null && whseViewLst.Any())
                {
                    string sortName = pageInfo.SortName;
                    string sortOrder = pageInfo.SortOrder;                   
                    var sortType = SortType.DESC;
                    if (!string.IsNullOrEmpty(sortName))
                    {
                        if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC")
                        {
                            sortType = SortType.ASC;
                        }
                        whseViewLst = whseViewLst.AsQueryable().SortBy(sortName, sortType).ToList();
                    }
                }
            }
           
            return whseViewLst;
        }

        /// <summary>
        /// 獲取指定記錄ID的倉庫信息
        /// </summary>
        /// <param name="recID">記錄ID</param>
        /// <returns>倉庫信息</returns>
        [HttpGet]
        public WhseView GetWhseInfo(Guid recID)
        {
            WhseView whseView = new WhseView();
            var warehouse = InventoryBLL.GetWarehouseById(recID);
          
            whseView = AutoMapperExt.MapTo<WhseView>(warehouse);
            
            return whseView;
        }

        /// <summary>
        /// 刪除倉庫信息
        /// </summary>
        /// <param name="recIDList">倉庫記錄ID列表</param>
        /// <returns>操作結果</returns>
        [HttpGet]
        public SystemResult DeleteWhseRecs(string recIDList)
        {
            SystemResult sysRslt = new SystemResult();

            sysRslt = InventoryBLL.LogicDeleteWarehouses(recIDList);
          
            return sysRslt;
        }

        /// <summary>
        /// 保存倉庫信息
        /// </summary>
        /// <param name="saveInfo"></param>
        /// <returns>操作結果</returns>
        [HttpPost]
        public SystemResult SaveWhseRec([FromForm]WhseView saveInfo)
        {
            var sysRslt = new SystemResult();

            saveInfo.Validate();
            if (!CurrentUser.IsMerchant && saveInfo.MerchantId == Guid.Empty)
                throw new BLException(Resources.Message.PleaseSelectMerchant);

            sysRslt = InventoryBLL.Save(saveInfo);
            return sysRslt;
        }

        /// <summary>
        /// 搜尋庫存匯總信息列表
        /// </summary>
        /// <param name="pageInfo">搜尋條件</param>
        /// <returns>庫存匯總信息列表</returns>
        [HttpPost]
        public PageData<InvSummaryView> SearchInvSummaryLst(InvSummaryPageInfo pageInfo)
        {
            var result = InventoryBLL.GetInvSummaryByPage(pageInfo.Condition);
            return result;
        }

        /// <summary>
        /// 獲取指定產品編號的庫存詳細信息列表
        /// </summary>
        /// <param name="pageInfo">搜尋條件</param>
        /// <returns>庫存詳細信息列表</returns>
        [HttpPost]
        public List<InvSummaryDetlView> GetInvDetlLst(InvSummaryPageInfo pageInfo)
        {
            var invSummaryDetlVwLst = InventoryBLL.GetInvSummaryDetlLst(pageInfo.Condition.ProductCode);          
            return invSummaryDetlVwLst;
        }

        /// <summary>
        /// 獲取庫存搜尋界面的屬性選項值緩存
        /// </summary>
        /// <param name="CatID">目錄ID</param>
        /// <returns>屬性選項值</returns>
        [HttpGet]
        public object GetInvAttrLstCaches(string CatID)
        {
            if (CatID == "-1" || CatID == Guid.Empty.ToString()) return new InvAttributeLst();

            var attrItmLst = AttributeBLL.GetInvAttributeItemsByCatID(Guid.Parse(CatID));
            if (attrItmLst == null) return new InvAttributeLst();

            var obj = new
            {
                attrItmLst.AttrIList,
                attrItmLst.AttrIIList,
                attrItmLst.AttrIIIList
            };

            return obj;
        }

        /// <summary>
        /// 獲取庫存流動報告界面的數據緩存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetInvFlowRptPageCaches()
        {
            var supplierList = InventoryBLL.GetSupplierComboSrc();
            var flowTypeList = InventoryBLL.GetInvFlowTypeLstComboSrc();

            var obj = new
            {
                SupplierList = supplierList,
                FlowTypeList = flowTypeList
            };

            return obj;
        }

        /// <summary>
        /// 搜尋庫存流動數據列表
        /// </summary>
        /// <param name="pageInfo">搜尋條件</param>
        /// <returns>庫存流動數據列表</returns>
        [HttpPost]
        public PageData<InvFlowView> SearchInvFlowLst(InvFlowPageInfo pageInfo)
        {
            var flowViewList = InventoryBLL.GetInvFlowLstByCond(pageInfo.Condition);
            return flowViewList;
        }

        /// <summary>
        /// 读取商家仓库信息
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetInvTransPageCaches(Guid merchantId)
        {
            var obj = new object();

            var warehouseList = InventoryBLL.GetWhseComboSrc(merchantId); 
            //var supplierList = InventoryBLL.GetSupplierComboSrc();
            var transTypeList = InventoryBLL.GetInvTransTypeComboSrc();
            string userName = UserBLL.GetUserInfoById(CurrentUser?.UserId)?.Name ?? "";

            obj = new
            {
                CurrentUser = userName,
                WarehouseList = warehouseList,
                //SupplierList = supplierList,
                TransTypeList = transTypeList
            };


            return obj;
        }

        [HttpPost]
        public List<InvTransItemView> GetPurchaseItmLst([FromForm]InvTransPageInfo pageInfo)
        {     
            var condition = pageInfo.Condition;
            var invTransItmVwLst = InventoryBLL.GetPurchaseItmLst(condition);
            return invTransItmVwLst;
        }

        [HttpPost]
        public List<InvTransItemView> GetPurReturnTransItmLst([FromForm]InvTransPageInfo pageInfo)
        {            
            var condition = pageInfo.Condition;
            var invTransItmViewList = InventoryBLL.GetPurReturnItmLst(condition);
           
            return invTransItmViewList;
        }

        /// <summary>
        /// 采购，采购退回，调拨
        /// </summary>
        /// <param name="transView"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SystemResult> SaveInvTransRec([FromForm]InvTransView transView)
        {
            var result  = new SystemResult();
            result =await InventoryBLL.SaveInvTransRec(transView);

            //if (result.Succeeded)
            //{
            //    var list = result.ReturnValue as List<InvTransactionDtlDto>;
            //    if (list != null && list.Any())
            //    {
            //        ///只处理采购，采购退回
            //        int[] cond = new int[] { InvTransType.Purchase.ToInt(), InvTransType.PurchaseReturn.ToInt() };    
            //        if (cond.Any(x => x == transView.TransType.ToInt()))
            //            result = await DealProductQtyCacheBLL.UpdateQtyWhenPurchaseOrReturn(list);
            //    }
            //}

            return result;
        }

    }
}