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
    /// 
    /// </summary>
    [Dependency]
    public class TestHandler : INotificationHandler<MemberCreate<MemberInfo>>
    {
        public Task Handle(MemberCreate<MemberInfo> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

