using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class OrderDeliveryInfo
    {
        /// <summary>
        /// 送货单ID
        /// </summary>
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        /// <summary>
        /// 优惠
        /// </summary>
        public List<CouponDiscountInfo> Coupon { get; set; } = new List<CouponDiscountInfo>();
        public string OrderNo { get; set; }
        /// <summary>
        /// 送货单单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 送货单产品
        /// </summary>
        public List<OrderItem> DeliveryItems { get; set; } = new List<OrderItem> { new OrderItem() };

        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 送货单产品的总价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 送货单的总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 折扣后的订单总价
        /// </summary>
        public decimal DiscountTotalAmount { get; set; }

        /// <summary>
        /// 折扣后的货物总价
        /// </summary>
        public decimal DiscountTotalPrice { get; set; }

        /// <summary>
        ///  优惠券、Promotion code优惠后的运费
        /// </summary>
        public decimal DiscountDeliveryCharge { get; set; }

        /// <summary>
        /// 送货单的产品数量
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 送货单运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 送货单运费折扣
        /// </summary>
        public decimal FreightDiscount { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        public string ConactName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ConactPhone { get; set; }

        /// <summary>
        /// 聯繫郵箱
        /// </summary>
        public string ConactMail { get; set; }


        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostCode { get; set; }

        public string CountryCode { get; set; }
        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }


        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public string Remark { get; set; }

        public string UpdateDate { get; set; }


        /// <summary>
        /// 快递类型（邮递、邮政局柜位、智邮站）
        /// </summary>
        public DeliveryType DeliveryType { get; set; }
        /// <summary>
        /// 快遞類型名稱
        /// </summary>
        public string DeliveryTypeName { get; set;  }

        /// <summary>
        /// 
        /// </summary>
        public string DeliveryTypeCode => DeliveryType.ToString();

        /// <summary>
        /// 快递ID
        /// </summary>
        public Guid ExpressId { get; set; }

        public string ExpressCode { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        public string ExpressName { get; set; }

        /// <summary>
        /// 快递服务(ECShip的服务)
        /// </summary>
        public string ExpressServiceCode { get; set; }

        public string ExpressServiceName { get; set; }

        /// <summary>
        /// 快递派送类型
        /// </summary>
        public DeliveryMailType MailType { get; set; }

        /// <summary>
        /// 智邮站的Code
        /// </summary>
        public string PLCode { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string PLName { get; set; }

        /// <summary>
        /// 智邮站类型（邮件编号、手机）
        /// </summary>
        public IPSType PLType { get; set; }

        /// <summary>
        /// 柜台编号
        /// </summary>
        public string COCode { get; set; }
        /// <summary>
        /// 柜名称
        /// </summary>
        public string CollectionOfficeName { get; set; }

        /// <summary>
        /// 送貨單的詳細地址信息
        /// </summary>
        public string FullAddress { get; set; }


        /// <summary>
        /// 創庫Id
        /// </summary>
        public Guid LocationId { get; set; }
        /// <summary>
        /// 快遞單單號
        /// </summary>
        public string TrackingNo { get; set; }

        public string ECShipNo { get; set; }

        public string ECShipDocNo { get; set; }

        /// <summary>
        /// 實際送貨地址（根據快遞類型判斷）
        /// </summary>
        public string DeliveryAddress
        {
            get
            {
                string address = string.Empty;
                switch (DeliveryType)
                {
                    case DeliveryType.D:
                        address = Country + " " + Province + " " + City + " " + Address + " " + Address1 + " " + Address2 + " " + Address3;
                        break;
                    case DeliveryType.P:
                        address = CollectionOfficeName;
                        break;
                    case DeliveryType.Z:
                        address = PLName;
                        break;
                    default:
                        break;
                }
                return address;
            }
            set
            {
                //DeliveryAddress = "";
            }
        }

        public string StatusCode { get; set; }

        public string StatusName { get; set; }

        public string MCNCode { get; set; }

        /// <summary>
        /// 是否冷静期
        /// </summary>
        public bool IsCalmeDate { get; set; }

        public string CalmeDateCaption { get; set; }

        /// <summary>
        /// 子訂單使用的優惠券
        /// </summary>
        public List<DiscountView> Discounts { get; set; } = new List<DiscountView>();

        public bool CanComment { get; set; }

        public bool IsEdit { get; set; }

        public int SendBackCount { get; set; }


        /// <summary>
        /// 是否显示投寄易状态
        /// </summary>
        public bool IsShowECShipStatus { get; set; }
        public string ECShipMessage { get; set; }

        /// <summary>
        /// 投寄易狀態
        /// </summary>
        public MassProcessStatusType ECShipStatus { get; set; }
        public string ECShipStatusString
        {
            get
            {
                return ECShipStatus.ToString();
            }
            set { }
        }

        /// <summary>
        /// 商品類型
        /// </summary>
        public ShoppingCartItemType GoodsType { get; set; } = ShoppingCartItemType.BUYDONG;

        //public RouteResp RouteResp { get; set; }
    }
}

