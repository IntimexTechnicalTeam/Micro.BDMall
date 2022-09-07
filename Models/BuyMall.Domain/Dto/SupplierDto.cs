namespace BDMall.Domain
{
    public class SupplierDto:BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 供應商名稱
        /// </summary>
      
        public string Name { get; set; }
      
        public Guid NameTransId { get; set; }
       
        public List<MutiLanguage> NameList { get; set; } = new List<MutiLanguage> { };  
        /// <summary>
        /// 聯繫人
        /// </summary>
       
        public string Contact { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        
        public string PhoneNum { get; set; }
        /// <summary>
        /// 傳真電話
        /// </summary>
        
        public string FaxNum { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
       
        public string Remarks { get; set; }
        /// <summary>
        /// 所屬商家記錄ID
        /// </summary>
       
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 商家名稱
        /// </summary>
       
        public string MerchantName { get; set; }
    }
}
