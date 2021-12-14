using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class MemberCreate<T> : INotification
    {
        public string Id;

        public T Param { get; set; } = default(T);
    }
}
