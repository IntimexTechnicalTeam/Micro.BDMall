using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BDMall.Enums;

namespace BDMall.Model
{
    public class OrderDiscount : BaseEntity<Guid>
    {

        /// <summary>
        /// 訂單Id
        /// </summary>
        [Column(Order = 3)]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 送貨單Id
        /// </summary>
        [Column(Order = 4)]
        public Guid SubOrderId { get; set; }

        /// <summary>
        /// 折扣Id包括，優惠券、會員優惠、Promotion Code、Promotion Rule
        /// </summary>
        [Column(Order = 5)]
        public Guid DiscountId { get; set; }

        /// <summary>
        /// 折扣的類型
        /// </summary>
        [Column(Order = 6)]
        public DiscountType DiscountType { get; set; }

        /// <summary>
        /// 是否百分比
        /// </summary>
        [Column(Order = 7)]
        public bool IsPercent { get; set; }

        /// <summary>
        /// 優惠值
        /// </summary>
        [Column(Order = 8)]
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// 優惠的價錢
        /// </summary>
        [Column(Order = 9)]
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 優惠的類型、價錢或運費
        /// </summary>
        [Column(Order = 10)]
        public CouponUsage DiscountUsage { get; set; }

        [Column(Order = 11)]
        public Guid ProductId { get; set; }

        [Column(Order = 12)]
        public Guid MemberId { get; set; }

        /// <summary>
        /// 保存推广码
        /// </summary>
        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 13)]
        public string Code { get; set; }


    }
}