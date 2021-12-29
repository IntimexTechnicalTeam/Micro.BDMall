using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using BDMall.Enums;
using System.Net;
using Intimex.Common;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IAuditTrailBLL : IDependency
    {
        PageData<MemberLoginRecordDto> GetMemAuditTrail(MemLoginRecPager pageInfo);
    }
}
