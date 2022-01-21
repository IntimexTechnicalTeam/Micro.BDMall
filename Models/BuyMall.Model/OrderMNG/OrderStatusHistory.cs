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
    public class OrderStatusHistory : BaseEntity<Guid>
    {

        /// <summary>
        /// 訂單ID
        /// </summary>
        [Column(Order = 3)]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 4)]
        public string Operator { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        [Column(Order = 5)]
        public OrderStatus Status { get; set; }




    }
}
