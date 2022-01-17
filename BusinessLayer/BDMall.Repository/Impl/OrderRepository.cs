using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository.Impl
{
    public  class OrderRepository: PublicBaseRepository, IOrderRepository
    {
        public OrderRepository(IServiceProvider service) : base(service)
        {
        }

        public PageData<OrderView> GetSimpleOrderByPage(OrderCondition cond)
        {
            PageData<OrderView> pageData = new PageData<OrderView>();
            DateTime? createDateFrom = null;
            DateTime? createDateTo = null;


            if (cond.CreateDateFrom != "")
            {
                createDateFrom = DateTime.Parse(cond.CreateDateFrom);
            }
            if (cond.CreateDateTo != "")
            {
                createDateTo = DateTime.Parse(cond.CreateDateTo).AddDays(1);
            }
            var query = from o in baseRepository.GetList<Order>()
                        join p in baseRepository.GetList<OrderDelivery>() on o.Id equals p.OrderId
                        join m in baseRepository.GetList<PaymentMethod>() on o.PaymentMethodId equals m.Id
                        join mb in baseRepository.GetList<Member>() on o.MemberId equals mb.Id                                              
                        join od in baseRepository.GetList<OrderDiscount>() on o.Id equals od.OrderId into odd
                        from odt in odd.DefaultIfEmpty()
                        where o.IsActive && !o.IsDeleted 
                        && (cond.PaymentMethod == Guid.Empty || o.PaymentMethodId == cond.PaymentMethod)
                        && (cond.ExpressId == Guid.Empty || p.ExpressCompanyId == cond.ExpressId)
                        select new
                        {
                            order = new OrderView
                            {
                                Id = o.Id,
                                MemberId = o.MemberId,
                                FirstName = mb.FirstName,
                                LastName = mb.LastName,
                                OrderNO = o.OrderNO,
                                InvoiceNO = o.InvoiceNO,
                                IsPaid = o.IsPaid,
                                CurrencyCode = o.CurrencyCode,
                                Status = o.Status,
                                TotalAmount = o.TotalAmount,
                                TotalWeight = o.TotalWeight,
                                DiscountAmount = o.DiscountAmount,
                                DiscountPrice = o.DiscountPrice,
                                ItemQty = o.ItemQty,
                                PaymentMethodId = o.PaymentMethodId,
                                CreateDate = o.CreateDate,
                                UpdateDate = o.UpdateDate.Value,
                                IsActive = o.IsActive,
                                IsDeleted = o.IsDeleted,                              
                                OrderDiscount = odt.Code
                            },
                            deliveries = p
                        };

            #region 查询条件

            if (CurrentUser.IsMerchant)
            {
                query = query.Where(p => p.deliveries.MerchantId == CurrentUser.MerchantId);
            }

            if (cond.MemberId != Guid.Empty)
            {
                query = query.Where(p => p.order.MemberId == cond.MemberId);
            }

            if (cond.MerchantId != Guid.Empty)
            {
                query = query.Where(p => p.deliveries.MerchantId == cond.MerchantId);
            }

            if ((int)cond.StatusCode != -1)
            {
                query = query.Where(p => p.order.Status == cond.StatusCode);
            }

            if (createDateFrom != null)
            {
                query = query.Where(p => p.order.CreateDate >= createDateFrom);
            }

            if (createDateTo != null)
            {
                query = query.Where(p => p.order.CreateDate < createDateTo);
            }

            if (!string.IsNullOrEmpty(cond.InvoiceNoFrom))
            {
                query = query.Where(p => p.order.InvoiceNO.CompareTo(cond.InvoiceNoFrom) >= 0);
            }
            if (!string.IsNullOrEmpty(cond.InvoiceNoTo))
            {
                query = query.Where(p => p.order.InvoiceNO.CompareTo(cond.InvoiceNoTo) <= 0);
            }
            if (!string.IsNullOrEmpty(cond.OrderNo))
            {
                query = query.Where(p => p.order.OrderNO.Contains(cond.OrderNo));
            }
           
            if (!string.IsNullOrEmpty(cond.OrderDiscount))
            {
                query = query.Where(p => p.order.OrderDiscount.Contains(cond.OrderDiscount));
            }

            #endregion

            var query2 = query.Select(d => d.order).Distinct();
            pageData.TotalRecord = query2.Count();

            #region 排序

            if (!string.IsNullOrEmpty(cond.PageInfo.SortName))
            {
                var sortBy = cond.PageInfo.SortOrder.ToUpper().ToEnum<SortType>();
                query2 = query2.SortBy(cond.PageInfo.SortName, sortBy);                
            }
            else            
                query2 = query2.OrderByDescending(o => o.UpdateDate);

            #endregion

            pageData.Data = query2.Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();
             
            return pageData;

        }
    }
}
