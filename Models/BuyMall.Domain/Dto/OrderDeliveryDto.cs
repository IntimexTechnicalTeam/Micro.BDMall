namespace BDMall.Domain
{
    public class OrderDeliveryDto:BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId
        {
            get;
            set;
        }

       
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
     
        public string TrackingNo { get; set; }

        /// <summary>
        /// 快递公司id
        /// </summary>
        public Guid ExpressCompanyId { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary> 
        
        //[ForeignKey("CourierId")]
        //public ExpressCompany Courier { get; set; }

    
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
     
        public string PLCode { get; set; }

        /// <summary>
        /// 柜台编号
        /// </summary>
      
        public string COCode { get; set; }


        /// <summary>
        /// 国家
        /// </summary>
       
        public string Country { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
       
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
       
        public string City { get; set; }

        /// <summary>
        /// 郵政編碼
        /// </summary>
      
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
       
        public string Recipients
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
      
        public string ContactPhone
        {
            get;
            set;
        }

       
        public string Email { get; set; }

        /// <summary>
        /// 后备联系电话
        /// </summary>
      
        public string BackupPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址
        /// </summary>
       
        public string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址1
        /// </summary>
        
        public string Address1
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址2
        /// </summary>
       
        public string Address2
        {
            get;
            set;
        }
        /// <summary>
        /// 送货地址3
        /// </summary>
      
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

        public string ECShipNo { get; set; }
        /// <summary>
        /// ECship文档No
        /// </summary>
 
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

      
        public string Remark { get; set; }

      
        public string StatusName { get; set; }

        /// <summary>
        /// 货品明细
        /// </summary>
        public  List<OrderDeliveryDetailDto> DeliveryDetails { get; set; } = new List<OrderDeliveryDetailDto>();

      
    }
}
