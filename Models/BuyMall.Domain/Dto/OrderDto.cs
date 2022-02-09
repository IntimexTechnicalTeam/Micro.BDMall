using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class OrderDto: BaseDto
    {
        public Guid Id { get; set; }
      
        public string OrderNO { get; set; }

        public string InvoiceNO { get; set; }

        public Guid MemberId { get; set; }
     
        public string MemberName { get; set; }

        public Guid PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; }
       
        public string CurrencyCode { get; set; }

      
        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 兑换率
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal TotalPrice { get; set; }
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
        /// 折扣后的運費
        /// </summary>
        public decimal DiscountFreight { get; set; }

        public decimal MallFun { get; set; }

        /// <summary>
        /// 運費折扣
        /// </summary>
        public decimal FreightDiscount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>    
        public string Remark { get; set; }

        /// <summary>
        ///  订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        public string StatusName { get; set; }

        /// <summary>
        /// 是否已經支付
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 計算免運費后的運費
        /// </summary>
        public decimal FreeShippingFreight { get; set; }

        /// <summary>
        /// 訂單詳細鏈接
        /// </summary>      
        public string OrderUrl { get; set; }

        //[ForeignKey("PaymentMethodId")]
        //public virtual PaymentMethod PaymentMethod { get; set; }

        //[ForeignKey("MemberId")]
        //public virtual Member Member { get; set; }

        /// <summary>
        /// 送货单
        /// </summary> 
        public List<OrderDeliveryDto> OrderDeliverys { get; set; } = new List<OrderDeliveryDto>();

        /// <summary>
        /// 订单明细
        /// </summary> 
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        /// <summary>
        /// 订单时段价格明细
        /// </summary> 
        public List<OrderPriceDetail> OrderPriceDetails { get; set; } = new List<OrderPriceDetail>();


        /// <summary>
        /// bank authorization code
        /// </summary>
        public string BankAuthCode { get; set; }

        /// <summary>
        /// 银行交易id
        /// </summary>      
        public string TransactionId { get; set; }

        /// <summary>
        /// 卡种类      
        ///  AMEX - American Express
        ///  CHINA_UNIONPAY - China UnionPay
        ///  DINERS_CLUB - Diners Club
        ///  DISCOVER - Discover
        ///  JCB - JCB(Japan Credit Bureau)
        ///  MASTERCARD - MasterCard
        ///  OTHER - The scheme of the card used in the transaction could not be identified.
        ///  UATP - UATP (Universal Air Travel Plan)
        ///  VISA - Visa
        /// </summary>     
        public string Acquirer { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        public List<Guid> skuList { get; set; } = new List<Guid>();
    }
}
