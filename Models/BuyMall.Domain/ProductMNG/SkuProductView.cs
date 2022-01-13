using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 產品信息
    /// </summary>
    /// <remarks>單個產品信息，</remarks>
    public class SkuProductView
    {
        public Guid SkuId { get; set; }
        public Guid ProductId { get; set; }
        public Guid MerchanId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid Attr1 { get; set; }
        public Guid Attr2 { get; set; }
        public Guid Attr3 { get; set; }
        public Guid AttrValue1 { get; set; }
        public Guid AttrValue2 { get; set; }
        public Guid AttrValue3 { get; set; }
        public string Attr1Name { get; set; }
        public string Attr2Name { get; set; }
        public string Attr3Name { get; set; }
        public string AttrValue1Name { get; set; }
        public string AttrValue2Name { get; set; }
        public string AttrValue3Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string Image { get; set; }
    }
}
