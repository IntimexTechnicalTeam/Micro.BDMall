namespace BDMall.BLL
{
    public class ScheduleJobBLL : BaseBLL, IScheduleJobBLL
    {
        IScheduleJobRepository ScheduleJobRepository;
        ISettingBLL SettingBLL;
        public ScheduleJobBLL(IServiceProvider services) : base(services)
        {
            ScheduleJobRepository = services.Resolve<IScheduleJobRepository>();
            SettingBLL = services.Resolve<ISettingBLL>();
        }



        public List<ScheduleJobView> GetScheduleJobs()
        {
            var jobViewList = new List<ScheduleJobView>();

            var cmList = SettingBLL.GetScheduleJobs();
            if (cmList.Count > 0)
            {
                var jobList = baseRepository.GetList<ScheduleJob>().Where(x => x.IsActive && !x.IsDeleted).OrderBy(x => x.Service).ToList();

                foreach (var job in cmList)
                {
                    var jobView = new ScheduleJobView();
                    var cm = jobList.FirstOrDefault(p => p.Service == job.Key + "Service");
                    if (cm != null)
                    {
                        jobView.Service = cm.Service;
                        jobView.Name = job?.Description ?? cm.Service;
                        jobView.Remarks = job?.Remark ?? string.Empty;
                        jobView.MonthValue = cm.MonthValue;
                        jobView.DayValue = cm.DayValue;
                        jobView.WeekValue = cm.WeekValue;
                        jobView.HourValue = cm.HourValue;
                        jobView.MinuteValue = cm.MinuteValue;
                    }
                    else
                    {

                        jobView.Service = job.Key + "Service";
                        jobView.Name = job?.Description ?? cm?.Service??"";
                        jobView.Remarks = job?.Remark ?? string.Empty;
                        jobView.MonthValue = null;
                        jobView.DayValue = null;
                        jobView.WeekValue = null;
                        jobView.HourValue = null;
                        jobView.MinuteValue = 1;
                    }

                    if (jobView.MinuteValue != null)
                    {
                        jobView.Value = jobView.MinuteValue;
                        jobView.Type = ScheduleIntervalType.Minute;
                    }
                    else if (jobView.HourValue != null)
                    {
                        jobView.Value = jobView.HourValue;
                        jobView.Type = ScheduleIntervalType.Hour;
                    }
                    else if (jobView.DayValue != null)
                    {
                        jobView.Value = jobView.DayValue;
                        jobView.Type = ScheduleIntervalType.Day;
                    }
                    else if (jobView.WeekValue != null)
                    {
                        jobView.Value = jobView.WeekValue;
                        jobView.Type = ScheduleIntervalType.Week;
                    }
                    else if (jobView.MonthValue != null)
                    {
                        jobView.Value = jobView.MonthValue;
                        jobView.Type = ScheduleIntervalType.Month;
                    }

                    jobViewList.Add(jobView);
                }
            }


            return jobViewList;
        }

        public SystemResult UpdateScheduleJob(ScheduleJobView jobView)
        {
            var sysRslt = new SystemResult();


            if (jobView != null)
            {
                var job = baseRepository.GetList<ScheduleJob>().FirstOrDefault(x => x.Service == jobView.Service && x.IsActive && !x.IsDeleted);
                if (job != null)
                {
                    job.MonthValue = null;
                    job.WeekValue = null;
                    job.DayValue = null;
                    job.HourValue = null;
                    job.MinuteValue = null;

                    if (jobView.Value != null && jobView.Type != null)
                    {
                        switch (jobView.Type.Value)
                        {
                            case ScheduleIntervalType.Minute:
                                job.MinuteValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Hour:
                                job.HourValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Day:
                                job.DayValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Week:
                                job.WeekValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Month:
                                job.MonthValue = jobView.Value;
                                break;
                            default:
                                break;
                        }
                    }

                    baseRepository.Update(job);
                    sysRslt.Succeeded = true;
                }
                else
                {
                    ScheduleJob dbJob = new ScheduleJob();
                    dbJob.Id = Guid.NewGuid();
                    dbJob.Service = jobView.Service;
                    dbJob.MonthValue = null;
                    dbJob.WeekValue = null;
                    dbJob.DayValue = null;
                    dbJob.HourValue = null;
                    dbJob.MinuteValue = null;

                    if (jobView.Value != null && jobView.Type != null)
                    {
                        switch (jobView.Type.Value)
                        {
                            case ScheduleIntervalType.Minute:
                                dbJob.MinuteValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Hour:
                                dbJob.HourValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Day:
                                dbJob.DayValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Week:
                                dbJob.WeekValue = jobView.Value;
                                break;
                            case ScheduleIntervalType.Month:
                                dbJob.MonthValue = jobView.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    baseRepository.Insert(dbJob);
                    sysRslt.Succeeded = true;
                }
            }


            return sysRslt;
        }

    }
}

