using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyExchangeRateDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 基本货币，需要转换的货币
        /// </summary>

        public string FromCurCode { get; set; }

        /// <summary>
        /// 转换货币，需要转换为此货币
        /// </summary>
        public string ToCurCode { get; set; }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ToName { get; set; }

    }
}
