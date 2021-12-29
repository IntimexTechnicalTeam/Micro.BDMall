using BDMall.Domain;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

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
