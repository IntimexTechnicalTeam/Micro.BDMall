using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// 計劃任務
    /// </summary>
    public class ScheduleJobController : BaseApiController
    {
        
        IScheduleJobBLL ScheduleJobBLL;
        ISettingBLL SettingBLL;
        public ScheduleJobController(IComponentContext services) : base(services)
        {
            ScheduleJobBLL = services.Resolve<IScheduleJobBLL>();
            SettingBLL = services.Resolve<ISettingBLL>();
        }
        /// <summary>
        /// 獲取計劃任務列表
        /// </summary>
        [HttpGet]
        public async Task<List<ScheduleJobView>> GetScheduleJobList()
        {
            var jobViewList = ScheduleJobBLL.GetScheduleJobs();
            return jobViewList;
        }

        /// <summary>
        /// 更新計劃任務資料
        /// </summary>
        /// <param name="jobView">計劃任務資料</param>
        [HttpPost]
        public async Task<SystemResult> UpdateScheduleJob([FromBody]ScheduleJobView jobView)
        {
            var sysRslt = ScheduleJobBLL.UpdateScheduleJob(jobView);
            return sysRslt;
        }

        /// <summary>
        /// 獲取服務時間間隔類型列表
        /// </summary>
        [HttpGet]
        public async Task<List<KeyValue>> GerServiceIntervalTypes()
        {
            var keyValList = new List<KeyValue>();

            var cmIntervalList = SettingBLL.GetServiceIntervalUnits();
            if (cmIntervalList?.Count > 0)
            {
                foreach (ScheduleIntervalType type in Enum.GetValues(typeof(ScheduleIntervalType)))
                {
                    var cmInterval = cmIntervalList.FirstOrDefault(x => x.Key == type.ToString());
                    if (cmInterval != null)
                    {
                        var keyVal = new KeyValue()
                        {
                            Id = cmInterval.Value,
                            Text = cmInterval.Description,
                        };
                        keyValList.Add(keyVal);
                    }
                }

                keyValList = keyValList.OrderBy(x => x.Id).ToList();
            }

            return keyValList;
        }
    }

}
