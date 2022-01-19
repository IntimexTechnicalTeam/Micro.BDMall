using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class PriceUtil
    {
        /// <summary>
        /// 系統價格，統一轉化爲兩位小數的價錢 
        /// </summary>
        /// <param name="price">一个双精度浮点数</param>
        /// <remarks></remarks>
        /// <returns>保留兩位小數的双精度浮点数</returns>
        public static double SystemPrice(double price)
        {
            return Math.Ceiling(price * 100) / 100;
        }

        /// <summary>
        /// 系統價格，統一轉化爲兩位小數的價錢 
        /// </summary>
        /// <param name="price">一个双精度浮点数</param>
        /// <remarks></remarks>
        /// <returns>保留兩位小數的双精度浮点数</returns>
        public static decimal SystemPrice(decimal price)
        {
            return Math.Ceiling(price * 100) / 100;
        }
    }
}
