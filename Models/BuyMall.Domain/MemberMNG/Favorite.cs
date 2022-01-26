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
        /// 用户喜欢的产品
        /// </summary>
        public List<string> ProductList { get; set; }

        /// <summary>
        /// 用户喜欢的商家
        /// </summary>
        public List<Guid> MchList { get; set; }
    }
}
