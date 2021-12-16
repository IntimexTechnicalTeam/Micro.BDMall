using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductAttributeValueDto
    {        
        public Guid AttrId { get; set; }
        
        public string Code { get; set; }
       
        public Guid DescTransId { get; set; }

        public string Image { get; set; }
       
        public Guid MerchantId { get; set; }

        public Guid Id { get; set; }
     
        public Guid ClientId { get; set; }

        public string Desc { get; set; }

        public string MerchantName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }


        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
