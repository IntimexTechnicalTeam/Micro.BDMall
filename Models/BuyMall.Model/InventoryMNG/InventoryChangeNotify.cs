using BDMall.Enums;
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
    /// 庫存變動通知記錄
    /// </summary>
    public class InventoryChangeNotify : BaseEntity<Guid>
    {
        /// <summary>
        /// Sku
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid SkuId { get; set; }
        /// <summary>
        /// 是否已通知商家
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public bool IsMerchNotified { get; set; }
        /// <summary>
        /// 通知商家時間
        /// </summary>
        [Column(Order = 5)]
        public DateTime? MerchNotifyDate { get; set; }
        /// <summary>
        /// 通知類型
        /// </summary>
        [Column(Order = 6)]
        public InvChangeNotifyType Type { get; set; }
        /// <summary>
        /// 是否處理完畢
        /// </summary>
        [Column(Order = 7)]
        public bool IsProcessed { get; set; }
        ///// <summary>
        ///// 當前庫存數量
        ///// </summary>
        //[NotMapped]
        //public int CurStockQty { get; set; }
    }
}
