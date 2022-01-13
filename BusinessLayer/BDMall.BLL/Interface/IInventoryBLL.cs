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

        PageData<InvFlowView> GetInvFlowLstByCond(InvFlowSrchCond condition);
    }
}
