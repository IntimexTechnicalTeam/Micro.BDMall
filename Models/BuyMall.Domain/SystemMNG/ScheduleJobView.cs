namespace BDMall.Domain
{
    public class ScheduleJobView
    {
        public string Service { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
        public ScheduleIntervalType? Type { get; set; }
        /// <summary>
        /// 月份頻率
        /// </summary>
        public int? MonthValue { get; set; }
        /// <summary>
        /// 周份頻率
        /// </summary>
        public int? WeekValue { get; set; }
        /// <summary>
        /// 日頻率
        /// </summary>
        public int? DayValue { get; set; }
        /// <summary>
        /// 時頻率
        /// </summary>
        public int? HourValue { get; set; }
        /// <summary>
        /// 分頻率
        /// </summary>
        public int? MinuteValue { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }
    }
}
