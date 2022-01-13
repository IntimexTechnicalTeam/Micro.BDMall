using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class InvFlowDetail
    {
        public InvTransactionDtl Detail { get; set;} = new InvTransactionDtl();

        public ProductSku Sku { get; set; } = new ProductSku();

        public User User { get; set; }= new User();

        public Product Product { get; set; }   =new Product();


        public string WhName { get; set; }=string.Empty ;

        public string Attr1Desc { get; set; } = string.Empty;

        public string Attr2Desc { get; set; } = string.Empty;

        public string Attr3Desc { get; set; } = string.Empty;

    }
}
