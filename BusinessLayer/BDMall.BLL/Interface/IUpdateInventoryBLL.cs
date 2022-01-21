using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IUpdateInventoryBLL:IDependency
    {
        /// <summary>
        /// 更新Inverntory表
        /// </summary>
        /// <param name="insertLst"></param>
        /// <param name="transIOTyp"></param>
        /// <param name="transType"></param>
        /// <returns></returns>
        SystemResult DealProductInventory(List<InvTransactionDtlDto> insertLst, InvTransIOType? transIOTyp, InvTransType transType);
    }
}
