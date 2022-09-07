namespace BDMall.Domain
{
    public class InvTransactionDtlDto:BaseDto
    {
        public Guid Id { get; set; }

        public InvTransType TransType { get; set; }
        /// <summary>
        /// 交易進出類型
        /// </summary>
       
        public InvTransIOType IOType { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
      
        public DateTime TransDate { get; set; }
        /// <summary>
        /// 業務記錄ID
        /// </summary>
       
        public Guid BizId { get; set; }
        /// <summary>
        /// 庫存單元ID
        /// </summary>
        
        public Guid Sku { get; set; }
       
        /// <summary>
        /// 交易數量
        /// </summary>
        
        public int TransQty { get; set; }
        /// <summary>
        /// 倉庫ID
        /// </summary>
        
        public Guid WHId { get; set; }


        /// <summary>
        /// 倉庫ID
        /// </summary>
      
        public Guid FromId { get; set; }
        
        public Guid ToId { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
  
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        
        public string BatchNum { get; set; }
        /// <summary>
        /// 銷售訂單記錄ID
        /// </summary>
       
        public Guid? SOId { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        
        public string Remarks { get; set; }
    }
}
