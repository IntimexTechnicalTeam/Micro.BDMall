using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    /// <summary>
    /// 会员注册成功，发送邮件
    /// </summary>
    [Dependency]
    public class SendMailWhenSuccessHandler : BaseBLL, INotificationHandler<MemberCreate<MemberDto>>
    {
        public SendMailWhenSuccessHandler(IServiceProvider services) : base(services)
        {
                
        }

        public  Task Handle(MemberCreate<MemberDto> notification, CancellationToken cancellationToken)
        {           
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 会员注册成功，送积分
    /// </summary>
    [Dependency]
    public class AwardPointsHandler : BaseBLL, INotificationHandler<MemberCreate<MemberDto>>
    {
        public AwardPointsHandler(IServiceProvider services) : base(services)
        {

        }

        public Task Handle(MemberCreate<MemberDto> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
