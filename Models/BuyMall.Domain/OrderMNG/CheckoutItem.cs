using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CheckoutItem
    {
        public CheckoutItem()
        {
            this.DeliveryId = Guid.NewGuid();
        }

        public ShoppingCartItemType CartType { get; set; }

        public Guid DeliveryId { get; set; }

        public List<BuyItem> Detail { get; set; } = new List<BuyItem>();

        /// <summary>
        /// 送貨方式
        /// </summary>
        public string DeliveryType { get; set; }

        public Guid AddressId { get; set; }

        /// <summary>
        /// 扣除了免运费产品重量得出的运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 没扣除免运费产品重量得出的运费
        /// </summary>
        public decimal OriginalFreight { get; set; }

        public decimal FreightDiscount { get; set; }

        public Guid ChargeId { get; set; }

        public ExpressChargeInfo ChargeInfo { get; set; }

        //public CollectionOfficeChargeInfo COChargeInfo { get; set; }

        public string ChargeGroupId { get; set; }

        public Guid ExpressCompanyId { get; set; }

        /// <summary>
        /// 柜台Code
        /// </summary>
        public string COCode { get; set; }

        /// <summary>
        /// 智邮站领取编号
        /// </summary>
        public string MCNCode { get; set; }

        /// <summary>
        /// 智邮站类型MCN、Phone
        /// </summary>
        public IPSType IPSType { get; set; }

        /// <summary>
        /// 智邮站站点Code
        /// </summary>
        public string IPSCode { get; set; }

        /// <summary>
        /// 取件人联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 取件人名称
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 取件人郵箱
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// 智邮站运费
        /// </summary>
        public decimal IPSDeliveryCharge { get; set; }

        /// <summary>
        /// 智油站原運費
        /// </summary>
        public decimal IPSDeliveryChargeOld { get; set; }

        /// <summary>
        /// 没扣减免运费产品得出的运费
        /// </summary>
        public decimal IPSOriginalDeliveryCharge { get; set; }

        /// <summary>
        /// 是否与寄件人资料相同
        /// </summary>
        public bool IsSameAsSender { get; set; }

        public string PickupDate { get; set; }

        public string PickupTime { get; set; }

        /// <summary>
        /// 送货方式
        /// </summary>
        public string ServiceType { get; set; }

        public string CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal TotalWeight { get; set; }


        /// <summary>
        /// e-coupon :货价券，运费券，Rule，promotionCode等
        /// </summary>
        public List<DiscountView> Discounts { get; set; }

        public string Remark { get; set; }

    }
}
