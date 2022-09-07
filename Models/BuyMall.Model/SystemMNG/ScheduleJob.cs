namespace BDMall.Model
{
    /// <summary>
    /// 程式服務任務計劃
    /// </summary>
    public class ScheduleJob : BaseEntity<Guid>
    {
        /// <summary>
        /// 服務類型
        /// </summary>
        [Required]
        [StringLength(50)]
        [Column(Order = 3, TypeName = "varchar")]
        public string Service { get; set; }
        /// <summary>
        /// 月份頻率
        /// </summary>
        [Column(Order = 4)]
        public int? MonthValue { get; set; }
        /// <summary>
        /// 周份頻率
        /// </summary>
        [Column(Order = 5)]
        public int? WeekValue { get; set; }
        /// <summary>
        /// 日頻率
        /// </summary>
        [Column(Order = 6)]
        public int? DayValue { get; set; }
        /// <summary>
        /// 時頻率
        /// </summary>
        [Column(Order = 7)]
        public int? HourValue { get; set; }
        /// <summary>
        /// 分頻率
        /// </summary>
        [Column(Order = 8)]
        public int? MinuteValue { get; set; }
    }
}
