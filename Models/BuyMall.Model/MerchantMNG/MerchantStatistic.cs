using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MerchantStatistic : BaseEntity<Guid>
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid MerchId { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        [Column(Order = 4)]
        public decimal Score { get; set; }
        /// <summary>
        /// 被評分次數
        /// </summary>
        [Column(Order = 5)]
        public int ScoreQty { get; set; }
        /// <summary>
        /// 被購買次数
        /// </summary>
        [Column(Order = 6)]
        public int PurchaseQty { get; set; }
        /// <summary>
        /// 被瀏覽次数
        /// </summary>
        [Column(Order = 7)]
        public int BrowseQty { get; set; }
        /// <summary>
        /// 被搜尋次数
        /// </summary>
        [Column(Order = 10)]
        public int SearchQty { get; set; }
    }
}
