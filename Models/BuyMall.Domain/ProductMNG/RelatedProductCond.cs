using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class RelatedProductCond
    {
        public PageInfo PageInfo { get; set; }

        public RelatedProductCond()
        {
            this.ProductCode = "";
            this.PageInfo = new PageInfo();
            this.ProductId = Guid.Empty;
            this.CategoryID = Guid.Empty;
        }


        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public Guid CategoryID { get; set; }

        //public bool IsDeleted { get; set; }

    }
}
