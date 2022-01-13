using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class WarehouseDto :BaseDto
    {
        /// <summary>
        /// 倉庫名稱
        /// </summary>
       
        public Guid Id { get; set; }

        public string Name { get; set; }
       
        public Guid NameTransId { get; set; }
        
        public List<MutiLanguage> NameList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫地址
        /// </summary>

        public string Address { get; set; }
      
        public Guid AddressTransId { get; set; }
       
        public List<MutiLanguage> AddressList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫人
        /// </summary>
      
        public string Contact { get; set; } = "";

        public Guid ContactTransId { get; set; }
       
        public List<MutiLanguage> ContactList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫電話
        /// </summary>      
        public string PhoneNum { get; set; } = "";
        /// <summary>
        /// 郵政編號
        /// </summary>     
        public string PostalCode { get; set; } = "";
        /// <summary>
        /// 備註
        /// </summary>

        public string Remarks { get; set; } = "";
        /// <summary>
        /// 所屬商家記錄ID
        /// </summary>

        public Guid MerchantId { get; set; }
        
        
        public string CostCenter { get; set; } = "";

        public string AccountCode { get; set; } = "";
        /// <summary>
        /// 商家名稱
        /// </summary>

        public string MerchantName { get; set; }
    }
}
