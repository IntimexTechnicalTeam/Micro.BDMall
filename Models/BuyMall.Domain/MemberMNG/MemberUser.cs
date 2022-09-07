namespace BDMall.Domain
{
    public class MemberUser
    {
        /// <summary>
        ///  积分数
        /// </summary>
        public decimal Fun { get; set; }

        /// <summary>
        /// 每天最大兑换积分数
        /// </summary>
        public decimal MaxLimitDayFun { get; set; }

        /// <summary>
        /// 月度最大兑换积分数
        /// </summary>
        public decimal MaxLimitMonthFun { get; set; }

        /// <summary>
        /// 年度最大兑换积分数
        /// </summary>
        public decimal MaxLimitYearFun { get; set; }

        /// <summary>
        /// 累计每天兑换了的积分数,每兑换一次进行累计，每天凌晨重置为0
        /// </summary>
        public decimal TotalDayFun { get; set; }

        /// <summary>
        /// 累计每月兑换了的积分数,每兑换一次进行累计，每月1号凌晨重置为0
        /// </summary>
        public decimal TotalMonthFun { get; set; }

        /// <summary>
        /// 累计每年兑换了的积分数,每兑换一次进行累计，每年最后一天凌晨重置为0
        /// </summary>
        public decimal TotalYearFun { get; set; }
    }
}
