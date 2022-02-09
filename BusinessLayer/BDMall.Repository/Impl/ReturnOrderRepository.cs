using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class ReturnOrderRepository : PublicBaseRepository, IReturnOrderRepository
    {
        public ReturnOrderRepository(IServiceProvider service) : base(service)
        {
        }

        public List<ReturnOrder> GetReturnOrders(ReturnOrderCondition cond)
        {
            List<ReturnOrder> rOrderList = new List<ReturnOrder>();
            DateTime? createDateFrom = null;
            DateTime? createDateTo = null;

            if (!string.IsNullOrEmpty(cond.CreateDateFrom))
            {
                createDateFrom = DateTime.Parse(cond.CreateDateFrom);
            }
            if (!string.IsNullOrEmpty(cond.CreateDateTo))
            {
                createDateTo = DateTime.Parse(cond.CreateDateTo).AddDays(1);
            }
     
            var query = from ro in baseRepository.GetList<ReturnOrder>()
                        join rod in baseRepository.GetList<ReturnOrderDetail>() on ro.Id equals rod.ROrderId
                        join mb in baseRepository.GetList<Member>() on ro.MemberId equals mb.Id
                        join o in baseRepository.GetList <Order>() on ro.OrderId equals o.Id
                        where ro.IsActive && !ro.IsDeleted 
                        select new
                        {
                            ReturnOrder = ro,
                            ReturnOrderDtl = rod,
                            Order = o,
                        };

            if (CurrentUser.IsMerchant)
            {
                query = query.Where(p => p.ReturnOrderDtl.MerchantId == CurrentUser.MerchantId);
            }
            if (cond.MemberId != Guid.Empty)
            {
                query = query.Where(p => p.ReturnOrder.MemberId == cond.MemberId);
            }
            if (cond.MerchantId != Guid.Empty)
            {
                query = query.Where(p => p.ReturnOrderDtl.MerchantId == cond.MerchantId);
            }
            if (cond.OrderId != Guid.Empty)
            {
                query = query.Where(p => p.ReturnOrder.OrderId == cond.OrderId);
            }
            if ((int)cond.StatusCode != -1)
            {
                query = query.Where(p => p.ReturnOrder.Status == cond.StatusCode);
            }
            if (createDateFrom != null)
            {
                query = query.Where(p => p.ReturnOrder.CreateDate >= createDateFrom);
            }
            if (createDateTo != null)
            {
                query = query.Where(p => p.ReturnOrder.CreateDate < createDateTo);
            }
            if (!string.IsNullOrEmpty(cond.RONoFrom))
            {
                query = query.Where(p => p.ReturnOrder.RONo.CompareTo(cond.RONoFrom) >= 0);
            }
            if (!string.IsNullOrEmpty(cond.RONoTo))
            {
                query = query.Where(p => p.ReturnOrder.RONo.CompareTo(cond.RONoTo) <= 0);
            }
         
            if (!string.IsNullOrEmpty(cond.OrderNo))
            {
                query = query.Where(p => p.Order.OrderNO.Contains(cond.OrderNo));
            }

            var query2 = query.Select(d => d.ReturnOrder);
            query2 = query2.OrderByDescending(o => o.CreateDate);

            rOrderList = query2.Distinct().ToList();
            return rOrderList;
        }
    }
}
