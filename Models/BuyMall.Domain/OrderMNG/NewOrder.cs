using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class NewOrder
    {

        public List<CheckoutItem> Items { get; set; } = new List<CheckoutItem>();

        public Guid PaymentMethodId { get; set; } = Guid.Empty;

        public Currency Currency { get; set; } = new Currency();

        /// <summary>
        /// 积分
        /// </summary>
        public decimal MallFun { get; set; }

        /// <summary>
        /// e-coupon :货价券，运费券，會員折扣等
        /// </summary>
        public List<DiscountView> Discounts { get; set; } = new List<DiscountView>();


        /// <summary>
        /// 創建訂單成功返回訂單編號
        /// </summary>
        public string OrderNO { get; set; } = "";
    }
}
