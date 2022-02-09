using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class CancelInventoryQtyHandler : BaseBLL, INotificationHandler<OrderCancleRequest<OrderDto>>
    {
        public CancelInventoryQtyHandler(IServiceProvider services) : base(services)
        {
        }

        public Task Handle(OrderCancleRequest<OrderDto> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
