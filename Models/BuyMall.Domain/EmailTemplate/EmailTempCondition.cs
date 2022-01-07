using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class EmailTempCondition
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        //public string Type { get; set; }
        public MailType? Type { get; set; }
        public Language? Language { get; set; }
    }
}
