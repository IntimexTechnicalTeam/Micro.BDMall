using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    /// <summary>
    /// 採購單明細
    /// </summary>
    public class PurchaseOrderDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 所屬採購單記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid POId { get; set; }
        /// <summary>
        /// 所屬採購單信息
        /// </summary>
        [ForeignKey("POId")]
        public virtual PurchaseOrder POInfo { get; set; }

        /// <summary>
        /// 庫存產品記錄ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid Sku { get; set; }
        [ForeignKey("Sku")]
        public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 採購數量
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public int OrderQty { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [DecimalPrecision(18, 2)]
        [Column(Order = 6)]
        public decimal UnitPrice { get; set; }
    }
}