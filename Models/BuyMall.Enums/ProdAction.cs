using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum ProdAction
    {
        /// <summary>
        /// 审批通过
        /// </summary>
        Apporve,

        /// <summary>
        /// 上架
        /// </summary>
        OnSale,

        /// <summary>
        /// 删除
        /// </summary>
        Delete,

        /// <summary>
        /// 下架
        /// </summary>
        OffSale,
    }
}
