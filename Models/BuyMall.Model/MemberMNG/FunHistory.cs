using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class FunHistory : BaseEntity<Guid>
    {
        /// <summary>
        /// 用戶賬號Id
        /// </summary>
        [Column(Order = 3)]
        public Guid AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column(Order = 4)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 加定減
        /// </summary>
        [Column(Order = 5)]
        public InOut Type { get; set; }

        [ForeignKey("AccountId")]
        public virtual MemberAccount MemberAccount { get; set; }

    }
}
