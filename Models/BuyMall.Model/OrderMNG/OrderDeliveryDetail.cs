using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace BDMall.Model
{
    /// <summary>
    /// 送货单明细
    /// </summary>
    public class OrderDeliveryDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId
        {
            get; set;
        }

        /// <summary>
        /// 送货单ID
        /// </summary>
        public Guid DeliveryId
        {
            get; set;
        }

        /// <summary>
        /// 单品ID
        /// </summary>
        public Guid SkuId
        {
            get; set;
        }
        /// <summary>
        /// 產品Id
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get; set;
        }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal OriginalPrice
        {
            get; set;
        }

        /// <summary>
        /// 购买价格
        /// </summary>
        public decimal SalePrice
        {
            get; set;
        }

        /// <summary>
        /// 是否贈品
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 多件購買優惠價錢
        /// </summary>
        public decimal SetPrice { get; set; }

        /// <summary>
        /// 每件產品實際支付價錢
        /// </summary>
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 推廣規則ID
        /// </summary>
        public Guid RuleId { get; set; }

        /// <summary>
        /// 是否有赠品
        /// </summary>
        public bool HasFree { get; set; }

        /// <summary>
        /// 倉庫Id
        /// </summary>
        public Guid LocationId { get; set; }
        

        /// <summary>
        /// 產品對應的快遞單號
        /// </summary>
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string TrackingNo { get; set; }

        [ForeignKey("SkuId")]
        public virtual ProductSku Sku { get; set; }

        [ForeignKey("DeliveryId")]
        public virtual OrderDelivery OrderDelivery { get; set; }

    }
}