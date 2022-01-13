using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace BDMall.Model
{
    /// <summary>
    /// 送货单
    /// </summary>
    public class OrderDelivery : BaseEntity<Guid>
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId
        {
            get;
            set;
        }

        //[Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        [Display(Description = "送貨單號")]
        public string DeliveryNO { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq
        {
            get;
            set;
        }

        /// <summary>
        /// 商家ID
        /// </summary>
        public Guid MerchantId
        {
            get;
            set;
        }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }

        public int ItemQty { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [StringLength(200)]
        [Column(TypeName = "varchar")]
        public string TrackingNo { get; set; }

        /// <summary>
        /// 快递公司id
        /// </summary>
        public Guid ExpressCompanyId { get; set; }

    
        [MaxLength(5)]
        [Column(TypeName = "varchar")]
        public string CountryCode { get; set; }
        /// <summary>
        /// 快遞類型
        /// </summary>
        //[StringLength(5)]
        //[Column(TypeName = "varchar")]
        public DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// 快遞服務類型
        /// </summary>
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string ServiceType { get; set; }

        /// <summary>
        /// 快递派送类型
        /// </summary>
        public DeliveryMailType MailType { get; set; }


        /// <summary>
        /// MCNCode
        /// </summary>
        public string MCNCode { get; set; }

        /// <summary>
        /// 智邮站取件类型（MCN、PHone）
        /// </summary>
        public IPSType PLType { get; set; }

        /// <summary>
        /// 智邮站站点编号
        /// </summary>
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string PLCode { get; set; }

        /// <summary>
        /// 柜台编号
        /// </summary>
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string COCode { get; set; }


        /// <summary>
        /// 国家
        /// </summary>
        [StringLength(100)]
        public string Country { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        [StringLength(30)]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(100)]
        public string City { get; set; }

        /// <summary>
        /// 郵政編碼
        /// </summary>
        [StringLength(15)]
        public string PostalCode { get; set; }


        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight
        {
            get;
            set;
        }

        /// <summary>
        /// 運費折扣
        /// </summary>
        public decimal FreightDiscount
        {
            get;
            set;
        }

        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal TotalPrice
        {
            get;
            set;
        }

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

        /// <summary>
        /// 收货人
        /// </summary>
        [StringLength(200)]
        public string Recipients
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(200)]
        public string ContactPhone
        {
            get;
            set;
        }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        /// <summary>
        /// 后备联系电话
        /// </summary>
        [StringLength(20)]
        public string BackupPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址
        /// </summary>
        [StringLength(200)]
        public string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址1
        /// </summary>
        [StringLength(200)]
        public string Address1
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址2
        /// </summary>
        [StringLength(200)]
        public string Address2
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址3
        /// </summary>
        [StringLength(200)]
        public string Address3
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CoolDownDay { get; set; }

        /// <summary>
        ///  订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// ECShipNo
        /// </summary>
        [StringLength(50)]
        public string ECShipNo { get; set; }
        /// <summary>
        /// ECship文档No
        /// </summary>
        [StringLength(50)]
        public string ECShipDocNo { get; set; }

        public int SendBackCount { get; set; }

        /// <summary>
        /// 計算免運費后的運費
        /// </summary>
        public decimal FreeShippingFreight { get; set; }

        /// <summary>
        /// 實際費用
        /// </summary>
        public decimal ActualAmount { get; set; }

        [StringLength(2000)]
        [Column(TypeName = "nvarchar")]
        public string Remark { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}