using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductAttributeDto
    {

        public Guid Id { get; set; }
       
        public Guid ClientId { get; set; }

        public string Code { get; set; }
       
        public Guid DescTransId { get; set; }

     
        public bool IsInvAttribute { get; set; }

     
        public AttrLayout Layout { get; set; }
          
        public string Desc { get; set; }

        public string Value { get; set; }

        public List<MutiLanguage> Descs { get; set; } = new List<MutiLanguage>();

        public List<ProductAttributeValueDto> AttributeValues = new List<ProductAttributeValueDto>();

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

     
        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
