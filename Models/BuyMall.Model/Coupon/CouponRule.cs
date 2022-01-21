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
    /// 優惠券規則
    /// </summary>
    public class CouponRule : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid MerchantId { get; set; }


        /// <summary>
        /// 優惠券規則類型
        /// </summary>
        [Column(Order = 4)]
        public CouponRuleType Type { get; set; }

        /// <summary>
        /// 送出的限制數量
        /// </summary>
        [Column(Order = 5)]

        public int Limit { get; set; }

        /// <summary>
        /// 滿足優惠金額
        /// </summary>
        [Column(Order = 6)]
        public decimal MeetAmount { get; set; }

        /// <summary>
        /// 是否折扣券
        /// </summary>
        [Column(Order = 7)]
        public bool IsDiscount { get; set; }

        /// <summary>
        /// 優惠金額(如果IsDiscount==true則是折扣，反之則是現金優惠)
        /// </summary>
        [Column(Order = 8)]
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// 是否設置特定日期
        /// </summary>
        [Column(Order = 9)]
        public bool IsSpecialDate { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 10)]
        public DateTime? EffectDateFrom { get; set; }


        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 11)]
        public DateTime? EffectDateTo { get; set; }

        /// <summary>
        /// 有效天數
        /// </summary>
        [Column(Order = 12)]
        public int EffectDates { get; set; }


        /// <summary>
        /// 優惠券圖片
        /// </summary>
        [MaxLength(500)]
        [Column(TypeName = "varchar", Order = 13)]
        public string Image { get; set; }


        /// <summary>
        /// 是否自動有效
        /// </summary>
        [Column(Order = 14)]
        public bool IsAutoActive { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 15)]
        public DateTime? ActiveDateFrom { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        [Column(Order = 16)]
        public DateTime? ActiveDateTo { get; set; }


        /// <summary>
        /// 買滿多少送
        /// </summary>
        [Column(Order = 17)]
        public decimal ShoppingAmount { get; set; }

        /// <summary>
        /// 備註
        /// </summary>

        [Column(Order = 18)]
        public Guid RemarkTransId { get; set; }


        [Column(Order = 19)]
        public Guid TitleTransId { get; set; }

        //[Column(Order = 19)]
        //public bool IsSuperimposed { get; set; }

        [Column(Order = 20)]
        public Guid CopyId { get; set; }


        [Column(Order = 21)]
        public Guid RootId { get; set; }

        [Column(Order = 22)]
        public bool IsLimit { get; set; }

        [Column(Order = 23)]
        public CouponUsage CouponUsage { get; set; }



    }
}
