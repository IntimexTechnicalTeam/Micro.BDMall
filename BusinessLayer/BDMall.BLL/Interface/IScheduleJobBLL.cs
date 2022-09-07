namespace BDMall.BLL
{
    public interface IScheduleJobBLL : IDependency
    {

        /// <summary>
        /// 獲取計劃任務列表
        /// </summary>
        /// <returns></returns>
        List<ScheduleJobView> GetScheduleJobs();

        /// <summary>
        /// 更新計劃任務資料
        /// </summary>
        /// <param name="jobView">計劃任務資料</param>
        /// <returns></returns>
        SystemResult UpdateScheduleJob(ScheduleJobView jobView);
    }
}
