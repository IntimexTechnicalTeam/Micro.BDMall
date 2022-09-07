using System.Threading;
using MemberInfo = BDMall.Domain.MemberInfo;

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

