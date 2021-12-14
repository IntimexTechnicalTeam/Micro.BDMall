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
    /// 產品點擊、搜尋點擊匯總
    /// </summary>
    public class ProductClickRateSummry : BaseEntity<int>
    {
        [Required]
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }
        [Required]
        [Column(Order = 4)]
        public string ProductCode { get; set; }
        [Required]
        [Column(Order = 5)]
        public int Year { get; set; }
        [Required]
        [Column(Order = 6)]
        public int Month { get; set; }
        [Required]
        [Column(Order = 7)]
        public int Day { get; set; }
        [Required]
        [Column(Order = 8)]
        public int ClickCounter { get; set; }

        [Required]
        [Column(Order = 9)]
        public int SearchClickCounter { get; set; }
    }
}
