using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductAtrtValueModel
    {
        public Guid Id { get; set; }

        public string AttrName { get; set; }

        public string Desc { get; set; }

        public decimal AddPrice { get; set; }
    }
}
