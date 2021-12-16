using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum ActionTypeEnum
    {
        Add,
        Modify,
        Copy,
        Delete,
        Read,
        /// <summary>
        /// 創建新版本產品
        /// </summary>
        NewVer
    }
}
