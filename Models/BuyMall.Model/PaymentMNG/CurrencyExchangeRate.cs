using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class CurrencyExchangeRate : BaseEntity<Guid>
    {
        /// <summary>
        /// 基本货币，需要转换的货币
        /// </summary>

        [MaxLength(10)]
        [Column(TypeName = "varchar", Order = 5)]
        public string FromCurCode { get; set; }

        /// <summary>
        /// 转换货币，需要转换为此货币
        /// </summary>
        [MaxLength(10)]
        [Column(TypeName = "varchar", Order = 6)]
        public string ToCurCode { get; set; }

        /// <summary>
        /// 转换率
        /// </summary>
        [Column(Order = 7)]
        public decimal Rate { get; set; }
        //[ForeignKey("FromCurId")]
        //public virtual Currency BaseCurrency { get; set; }

        //[ForeignKey("ToCurId")]
        //public virtual Currency ExchangeCurrency { get; set; }


    }
}
