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
    /// 調撥單明細
    /// </summary>
    public class RelocationOrderDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 所屬調撥單記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid ROId { get; set; }
        /// <summary>
        /// 所屬調撥單信息
        /// </summary>
        [ForeignKey("ROId")]
        public virtual RelocationOrder ROInfo { get; set; }
        /// <summary>
        /// 庫存產品記錄ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid Sku { get; set; }
        [ForeignKey("Sku")]
        public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 調撥數量
        /// </summary>
        [Required]
        [DecimalPrecision(18, 2)]
        [Column(Order = 5)]
        public decimal OrderQty { get; set; }
    }
}
