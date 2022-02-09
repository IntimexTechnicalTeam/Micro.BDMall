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
    /// 退換單詳細
    /// </summary>
    public class ReturnOrderDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 退換單ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid ROrderId { get; set; }
        
        /// <summary>
        /// Sku Id
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid SkuId { get; set; }

        /// <summary>
        /// 產品Id
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 商家Id
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public int Qty { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [Column(Order = 8)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 快遞單號
        /// </summary>
        [MaxLength(30)]
        [Column(Order = 9, TypeName = ("varchar"))]
        public string TrackingNo { get; set; }

        /// <summary>
        /// 送貨單ID
        /// </summary>
        [Required]
        [Column(Order = 10)]
        public Guid OrderDeliveryId { get; set; }
    }
}
