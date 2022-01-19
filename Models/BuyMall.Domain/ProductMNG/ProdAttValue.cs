using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProdAttValue
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// 附加價錢
        /// </summary>
        public decimal AddPrice { get; set; }
        public decimal AddPrice2 { get; set; }
    }
}
