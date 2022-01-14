using BDMall.Domain;
using BDMall.Enums;
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
    }
}
