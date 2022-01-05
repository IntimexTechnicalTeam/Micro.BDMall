using BDMall.Enums;
using BDMall.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class ProductAttributeDto
    {

        public Guid Id { get; set; }
       
        public Guid ClientId { get; set; }

        public string Code { get; set; }

        public Guid DescTransId { get; set; } 

     
        public bool IsInvAttribute { get; set; } = false;


        public AttrLayout Layout { get; set; } = AttrLayout.Select;
          
        public string Desc { get; set; }

        public string Value { get; set; }

        public List<MutiLanguage> Descs { get; set; } = new List<MutiLanguage>();

        public List<ProductAttributeValueDto> AttributeValues { get; set; } = new List<ProductAttributeValueDto>();

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;


        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        public Guid CreateBy { get; set; } 

        public Guid? UpdateBy { get; set; } = Guid.Empty;

        public virtual void Validate() {

            var pattern = "<\\s*(img|br|p|b|/p|a|div|iframe|button|script|i|html|form|input|frameset|body|table|br|label|link|li|style).*?>";
            var mateches = Regex.Matches(this.Code, pattern);
            if (mateches.Count > 0)
                throw new InvalidInputException(Message.ExistHTMLLabel);

            foreach (var item in Descs)
            {
                mateches = Regex.Matches(item.Desc, pattern);
                if (mateches.Count > 0)
                    throw new InvalidInputException(Message.ExistHTMLLabel);
            }

        }
    }
}
