using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class TmpProductQty
    {
        /// <summary>
        /// PushMessage.Id
        /// </summary>
        public Guid Id { get; set; }

        public Guid SkuId { get; set; }

        public int InvtActualQty { get; set; }

        public int SalesQty { get; set; }

        public int InvtReservedQty { get; set; }

        public int InvtHoldQty { get; set; }

        public QtyType QtyType { get; set; }

    }

    public enum QtyType
    {

        WhenPurchasing,
        /// <summary>
        /// 采购退回，销售退回，發貨退回
        /// </summary>
        WhenReturn,
        WhenAddToCart,
        WhenDeleteCart,
        WhenModifyCart,
        WhenPay,
        WhenDeliveryArranged,
        WhenOrderCancel,
        /// <summary>
        /// 支付超时，回滚Hold货数量
        /// </summary>
        WhenPayTimeOut,
    }
}
