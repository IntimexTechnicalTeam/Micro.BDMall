using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class Favorite
    {
        /// <summary>
        /// 用户喜欢的产品,ProductCode
        /// </summary>
        public List<string> ProductList { get; set; }= new List<string>();

        /// <summary>
        /// 用户喜欢的商家
        /// </summary>
        public List<Guid> MchList { get; set; } = new List<Guid>();
    }
}
