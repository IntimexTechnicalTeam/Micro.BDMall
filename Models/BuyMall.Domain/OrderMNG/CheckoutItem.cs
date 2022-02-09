using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CheckoutItem
    {

        public ShoppingCartItemType CartType { get; set; } = ShoppingCartItemType.BUYDONG;

        public Guid DeliveryId { get; set; } = Guid.Empty;

        /// <summary>
        /// 物品清单
        /// </summary>
        [Required]
        public List<BuyItem> Detail { get; set; } = new List<BuyItem>();

        /// <summary>
        /// 送貨方式
        /// </summary>
        public DeliveryType DeliveryType { get; set; } = DeliveryType.D;

        /// <summary>
        /// 邮递地址
        /// </summary>
        [Required(ErrorMessage = "邮递地址不能为空")]
        public Guid AddressId { get; set; } = Guid.Empty;

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

        /// <summary>
        /// 快递相关参数
        /// </summary>
        public ExpressChargeInfo ChargeInfo { get; set; } = new ExpressChargeInfo();

        //public CollectionOfficeChargeInfo COChargeInfo { get; set; }

        public string ChargeGroupId { get; set; }=string.Empty;

        /// <summary>
        /// 快递公司ID
        /// </summary>
        public Guid ExpressCompanyId { get; set; } = Guid.Empty;

        /// <summary>
        /// 柜台Code
        /// </summary>
        public string COCode { get; set; } = string.Empty;

        /// <summary>
        /// 智邮站领取编号
        /// </summary>
        public string MCNCode { get; set; } = string.Empty;

        /// <summary>
        /// 智邮站类型MCN、Phone
        /// </summary>
        public IPSType IPSType { get; set; }

        /// <summary>
        /// 智邮站站点Code
        /// </summary>
        public string IPSCode { get; set; } = string.Empty;

        /// <summary>
        /// 取件人联系电话
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;
        /// <summary>
        /// 取件人名称
        /// </summary>
        public string ContactName { get; set; } = string.Empty;
        /// <summary>
        /// 取件人郵箱
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;

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

        public string PickupDate { get; set; } = string.Empty;

        public string PickupTime { get; set; } = string.Empty;

        /// <summary>
        /// 送货方式
        /// </summary>
        public string ServiceType { get; set; } = string.Empty;

        /// <summary>
        /// 国家代码
        /// </summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public decimal TotalWeight { get; set; }


        /// <summary>
        /// e-coupon :货价券，运费券，Rule，promotionCode等
        /// </summary>
        public List<DiscountView> Discounts { get; set; } = new List<DiscountView>();

        public string Remark { get; set; } = string.Empty;

    }
}
