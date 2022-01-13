using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Model
{
    /// <summary>
    /// 銷售退回可選項資料
    /// </summary>
    public class SalesReturnOrderDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 所屬銷售退回單記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid SROId { get; set; }
        /// <summary>
        /// 所屬銷售退回單信息
        /// </summary>
        [ForeignKey("SROId")]
        public virtual SalesReturnOrder SROInfo { get; set; }
        /// <summary>
        /// 庫存產品記錄ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid Sku { get; set; }
        [ForeignKey("Sku")]
        public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 退回數量
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public int ReturnQty { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [DecimalPrecision(18, 2)]
        [Column(Order = 6)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 送貨單Id
        /// </summary>
        [Column(Order = 7)]
        public Guid DeliveryId { get; set; }

        /// <summary>
        /// 發出倉庫ID
        /// </summary>
        [Column(Order = 8)]
        public Guid WHId { get; set; }

    }
}
