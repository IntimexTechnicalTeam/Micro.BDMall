using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class EmailTempItemCondition
    {
        public Guid ItemId { get; set; }
        public Guid DescId { get; set; }
        public string Value { get; set; }
        public string Propertity { get; set; }
        public string Remark { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
