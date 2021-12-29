using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class AuditTrailBLL : BaseBLL, IAuditTrailBLL
    {
        private ICodeMasterRepository _codeMasterRepo;
        //private ITranslationRepository _translationRepo;

        public AuditTrailBLL(IServiceProvider services) : base(services)
        {
        }
        public PageData<MemberLoginRecord> GetMemAuditTrail(MemLoginRecPager pageInfo)
        {

            PageData<MemberLoginRecordDto> result = new PageData<MemberLoginRecordDto>(pageInfo);
            var dbctxt = UnitOfWork.DataContext;
            var query = from l in baseRepository.GetList<MemberLoginRecord>()
                        join m in baseRepository.GetList<Member>() on l.MemberId equals m.Id into lms
                        from d in lms.DefaultIfEmpty()
                        select new
                        {
                            MemberId = l.MemberId,
                            Email = d.Email,
                            LoginTime = l.LoginTime,
                            LoginFrom = l.LoginFrom,
                            Duration = l.Duration,
                            LogoutTime = l.LogoutTime,
                            LogoutType = l.LogoutType,
                            CreateDate = l.CreateDate
                        };
            
            if (!string.IsNullOrEmpty(pageInfo.Email))
            {
                query = query.Where(d => d.Email == pageInfo.Email);
            }
            result.TotalRecord = query.Count();
            if (!string.IsNullOrEmpty(pageInfo.SortName))
            {
                if (pageInfo.SortName == "LoginFromDisplay")
                {
                    pageInfo.SortName = "LoginFrom";
                    query = query.SortBy("LoginFrom", SortType.ASC);
                }
                else
                {
                    query = query.OrderBy2(pageInfo.SortName,  pageInfo.SortOrder.ToUpper() == "DESC", date);

                    if (pageInfo.SortName == "LogoutTypeDisplay")
                    {
                        pageInfo.SortName = "LogoutType";
                        query = query.OrderBy2(pageInfo.SortName, pageInfo.SortOrder.ToUpper() == "DESC", LogoutType.UserLogout);
                    }
                    if (pageInfo.SortName == "LogoutTime")
                    {
                        pageInfo.SortName = "LogoutTime";
                        DateTime? date = null;
                        
                    }
                    else if (pageInfo.SortName == "MemberName")
                    {
                        pageInfo.SortName = "Email";
                        query = query.OrderBy(pageInfo.SortName, pageInfo.SortOrder.ToUpper() == "DESC");
                    }
                    else
                    {
                        query = query.OrderBy(pageInfo.SortName, pageInfo.SortOrder.ToUpper() == "DESC");
                    }
                }
            }
            else
            {

                query = query.OrderByDescending(d => d.CreateDate);
            }
            var data = query.Skip(pageInfo.Offset).Take(pageInfo.PageSize).ToList();
            result.Data = new List<MemberLoginRecordDto>();
            foreach (var item in data)
            {
                string logoutTypeDisplay = string.Empty;
                switch (item.LogoutType)
                {
                    case LogoutType.UserLogout:
                        logoutTypeDisplay = Resources.Value.UserLogout;
                        break;
                    case LogoutType.TokenExpire:
                        logoutTypeDisplay = Resources.Value.TokenExpire;
                        break;
                    case LogoutType.TimeOut:
                        logoutTypeDisplay = Resources.Value.LoginTimeout;
                        break;
                }
                MemberLoginRecordDto rec = new MemberLoginRecordDto()
                {
                    MemberId = item.MemberId,
                    MemberName = item.Email,
                    LoginTime = item.LoginTime,
                    LoginFrom = item.LoginFrom,
                    LoginFromDisplay = item.LoginFrom.ToString(),
                    Duration = item.Duration,
                    LogoutTime = item.LogoutTime,
                    LogoutTypeDisplay = logoutTypeDisplay,
                    LogoutType = item.LogoutType,
                    CreateDate = item.CreateDate
                };
                result.Data.Add(rec);
            }

            return result;
        }

    }
}
