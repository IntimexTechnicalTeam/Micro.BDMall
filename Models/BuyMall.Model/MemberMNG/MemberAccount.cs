using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MemberAccount : BaseEntity<Guid>
    {
        public MemberAccount()
        {

        }

        /// <summary>
        /// memberId
        /// </summary>
        [Column(Order = 3)]
        public Guid MemberId { get; set; }

        /// <summary>
        ///  B-Points积分,真实积分,兑换成功后，更新这个值
        /// </summary>
        [Column(Order = 4)]
        public decimal Fun { get; set; }

        /// <summary>
        /// T-Points总分数，每次同步必须传过来
        /// </summary>
        [Column(Order = 5)]
        public decimal TPoints { get; set; } = 0;

        /// <summary>
        /// T-Marks总数，每次同步必须传过来
        /// </summary>
        [Column(Order = 6)]
        public decimal TMarks { get; set; } = 0;

        /// <summary>
        /// 每天最大兑换积分数
        /// </summary>
        [Column(Order = 7)]
        [DefaultValue(1000)]
        public decimal MaxLimitDayFun { get; set; }

        /// <summary>
        /// 月度最大兑换积分数
        /// </summary>
        [Column(Order = 8)]
        [DefaultValue(5000)]
        public decimal MaxLimitMonthFun { get; set; }

        /// <summary>
        /// 年度最大兑换积分数
        /// </summary>
        [Column(Order = 9)]
        [DefaultValue(10000)]
        public decimal MaxLimitYearFun { get; set; }

        /// <summary>
        /// 兑换前的T-Points积分，每同步一次，累计一次
        /// </summary>
        [Column(Order = 10)]
        [DefaultValue(0)]
        public decimal BeforeFun { get; set; }

        /// <summary>
        /// 累计每天兑换了的积分数,每兑换一次进行累计，每天凌晨重置为0
        /// </summary>
        [Column(Order = 11)]
        [DefaultValue(0)]
        public decimal TotalDayFun { get; set; }

        /// <summary>
        /// 累计每月兑换了的积分数,每兑换一次进行累计，每月1号凌晨重置为0
        /// </summary>
        [Column(Order = 12)]
        [DefaultValue(0)]
        public decimal TotalMonthFun { get; set; }

        /// <summary>
        /// 累计每年兑换了的积分数,每兑换一次进行累计，每年最后一天凌晨重置为0
        /// </summary>
        [Column(Order = 13)]
        [DefaultValue(0)]
        public decimal TotalYearFun { get; set; }     
    }
}
