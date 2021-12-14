using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IUpdateProductQtyBLL:IDependency
    {
        Task<SystemResult> DealProductQty(Guid Id);

        /// <summary>
        /// 补偿回写Qty
        /// </summary>
        /// <returns></returns>
        Task<SystemResult> HandleQtyAsync();
    }
}
