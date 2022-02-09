using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class SalesReturnOrderDto:BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 單號
        /// </summary>
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 銷售單ID
        /// </summary>
      
        public Guid SOId { get; set; }

        /// <summary>
        /// 銷售退回時間
        /// </summary>

        public DateTime ReturnDate { get; set; }

        public List<SalesReturnOrderDetailDto> SalesReturnItemList { get; set; } = new List<SalesReturnOrderDetailDto>();

        public virtual void Validate()
        {          
            if (SalesReturnItemList == null || !SalesReturnItemList.Any()) throw new BLException(Resources.Message.OneSelect);
            foreach (var item in SalesReturnItemList)
            {
                if (item.ReturnQty < 0) throw new BLException(Resources.Message.SalesReturnQtySufficient);
                if (item.ReturnQty > item.OrderQty)  throw new BLException(Resources.Message.SalesReturnQtyOverrun);
            }
        }
    }
}
