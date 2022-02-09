using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IInventoryBLL:IDependency
    {
        List<WarehouseDto> GetWarehouseLstByCond(WarehouseDto cond);

        SystemResult LogicDeleteWarehouses(string recIDsList);

        WarehouseDto GetWarehouseById(Guid recId);

        SystemResult Save(WhseView saveInfo);

        PageData<InvSummaryView> GetInvSummaryByPage(InvSrchCond cond);

        List<InvSummaryDetlView> GetInvSummaryDetlLst(string prodCode);

        SkuProductView GetSkuProductView(Guid skuId, Language lang);

        List<KeyValue> GetSupplierComboSrc();

        List<KeyValue> GetInvFlowTypeLstComboSrc();

        List<KeyValue> GetInvTransTypeComboSrc();

        List<KeyValue> GetWhseComboSrc(Guid merchantId);

        PageData<InvFlowView> GetInvFlowLstByCond(InvFlowSrchCond condition);

        List<InvTransItemView> GetPurchaseItmLst(InvTransSrchCond condition);

        List<InvTransItemView> GetPurReturnItmLst(InvTransSrchCond condition);

        Task<SystemResult> SaveInvTransRec(InvTransView transView);

        SystemResult IsExsitBathNum(InvTransactionDtlDto cond);

        SystemResult InsertInventoryHold(InventoryHold insRec);

        SystemResult DeleteInventoryHold(InventoryHold delRec);

        decimal GetTotAvailableInvQty(InventoryReservedDto uniqueProp);

        SystemResult AddInvReserved(InventoryReservedDto reserve, Guid memberId);

        SystemResult IsExsitInventoryHold(InventoryHold curRec);

        /// <summary>
        /// 使用預留記錄扣除庫存數量
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        SystemResult DeductInvQtyWithReserve(InventoryReservedDto reserve);

        /// <summary>
        /// 取消庫存預留
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        SystemResult CancelInvReserved(InventoryReservedDto reserve);

        void InventoryChangeCheckAndNotify(InventoryReservedDto reserve);

        SystemResult SaveSalesReturnRec(SalesReturnOrderDto salesReturnIns);
    }
}
