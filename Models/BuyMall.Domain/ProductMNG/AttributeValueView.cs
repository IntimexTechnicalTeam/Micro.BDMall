using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class AttributeValueView
    {
        public string Id { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 属性值附加价钱
        /// </summary>
        public decimal Price { get; set; } = 0;

    }
}
