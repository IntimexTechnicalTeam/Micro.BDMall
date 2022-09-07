namespace BDMall.Domain
{
    public class SalesReturnOrderDetailDto:BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 所屬銷售退回單記錄ID
        /// </summary>
        public Guid SROId { get; set; }
        /// <summary>
        /// 所屬銷售退回單信息
        /// </summary>
      
        public Guid Sku { get; set; }

        /// <summary>
        /// 退回數量
        /// </summary>
        public int ReturnQty { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 送貨單Id
        /// </summary>

        public Guid DeliveryId { get; set; }

        /// <summary>
        /// 發出倉庫ID
        /// </summary>

        public Guid WHId { get; set; }

        public int OrderQty { get; set; }

        public string ProdCode { get; set; }
 
        public string ProdName { get; set; }
  
        public Guid Attr1 { get; set; }
      
        public Guid Attr2 { get; set; }

        public Guid Attr3 { get; set; }

        public string Attr1Desc { get; set; }

        public string Attr2Desc { get; set; }

        public string Attr3Desc { get; set; }
        
        public Guid SOId { get; set; }
        
        public string SalesOrderNo { get; set; }
        
        public Guid MemberId { get; set; }
        
        public string MemberName { get; set; }
    }
}
