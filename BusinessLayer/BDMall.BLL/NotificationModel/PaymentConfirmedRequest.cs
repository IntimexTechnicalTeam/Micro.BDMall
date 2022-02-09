using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class PaymentConfirmedRequest<T> : INotification
    {
        public UpdateStatusCondition cond { get; set; } = new UpdateStatusCondition();
         
        public T Param { get; set; } = default(T);
    }
}
