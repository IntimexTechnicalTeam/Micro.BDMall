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
    /// 採購退回單明細
    /// </summary>
    public class PurchaseReturnOrderDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 所屬採購退回單記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid PROId { get; set; }
        /// <summary>
        /// 所屬採購退回單信息
        /// </summary>
        [ForeignKey("PROId")]
        public virtual PurchaseReturnOrder PROInfo { get; set; }
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
        [DecimalPrecision(18, 2)]
        [Column(Order = 5)]
        public decimal OrderQty { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [DecimalPrecision(18, 2)]
        [Column(Order = 6)]
        public decimal UnitPrice { get; set; }
    }
}
