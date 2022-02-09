using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class DeliveryArrangedRequest<T> : INotification
    {
        public UpdateStatusCondition cond { get; set; } = new UpdateStatusCondition();

        public List<string> trackingNoList { get; set; } = new List<string>();

        public T Param { get; set; } = default(T);
    }
}
