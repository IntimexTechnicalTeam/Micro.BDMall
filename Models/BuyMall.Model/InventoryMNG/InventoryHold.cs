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
    /// 庫存保留記錄（Hold貨）
    /// </summary>
    public class InventoryHold : BaseEntity<Guid>
    {
        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid SkuId { get; set; }
        /// <summary>
        /// 會員ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid MemberId { get; set; }
        /// <summary>
        /// 保留數量
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public int Qty { get; set; }
    }
}
