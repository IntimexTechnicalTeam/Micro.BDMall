using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class LowQtyEmailRequest : INotification
    {
        public List<Guid> skuList { get; set; }= new List<Guid>();
    }
}
