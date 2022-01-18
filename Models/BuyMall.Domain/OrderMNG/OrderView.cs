using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class OrderView
    {
        public Guid Id { get; set; }
      
        public string OrderNO { get; set; }
  
        public string InvoiceNO { get; set; }

        public Guid MemberId { get; set; }

        public Guid PaymentMethodId { get; set; }

        public string CurrencyCode { get; set; }

       
        /// <summary>
        ///  订单合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 數量合計
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// 订单商品总重量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 折扣后後的商品總價
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 折扣后的訂單合計金額
        /// </summary>
        public decimal DiscountAmount { get; set; }

      
        /// <summary>
        ///  订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 是否已經支付
        /// </summary>
        public bool IsPaid { get; set; }

        public string FirstName { get; set; }   

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }  

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string OrderDiscount { get; set; }

        public string MemberName => this.FirstName;

        public string StatusName =>Enum.GetName(typeof(OrderStatus), Status);

        public decimal DiscountTotalAmount => DiscountAmount;
    }
}
