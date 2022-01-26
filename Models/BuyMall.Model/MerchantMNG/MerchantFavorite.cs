using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MerchantFavorite : BaseEntity<Guid>
    {
        /// <summary>
        /// 會員ID
        /// </summary>
        [Column(Order = 3)]
        public Guid MemberId { get; set; }

        /// <summary>
        /// 收藏時的商家ID
        /// </summary>
        [Column(Order = 4)]
        public Guid MerchId { get; set; }
    }
}
