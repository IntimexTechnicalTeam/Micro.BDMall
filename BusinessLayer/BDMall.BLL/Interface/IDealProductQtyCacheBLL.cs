using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IDealProductQtyCacheBLL : IDependency
    {
        /// <summary>
        /// 当订单状态改变时，更新库存
        /// </summary>
        /// <param name="orderStatusInfo"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenOrderStateChange(UpdateStatusCondition orderStatusInfo);

        /// <summary>
        /// 采购入库,采购退回,采购调拨，銷售退回，發貨退回时更新库存（ProductQty）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenPurchaseOrReturn(List<InvTransactionDtlDto> list);

        /// <summary>
        /// 加入购物车，进行Hold货时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        Task UpdateQtyWhenAddToCart(Guid SkuId, int Qty);

        /// <summary>
        /// 当移除购物车上的物品时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        Task UpdateQtyWhenDeleteCart(Guid SkuId, int Qty);

        /// <summary>
        /// 当修改购物车上的物品数量时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="NewQty">变化后的</param>
        /// <param name="OldQty">变化前的</param>
        Task UpdateQtyWhenModifyCart(Guid SkuId, int NewQty, int OldQty);

        /// <summary>
        /// 支付超时，支付失败，或者后台在待付款状态时手动取消订单，恢复Hold货数
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenPayTimeOut(Guid OrderId);
    }
}