using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CurrencyView
    {
        public int Id { get; set; }
        /// <summary>
        /// 匯率記錄ID
        /// </summary>
        public Guid RateId { get; set; }
        /// <summary>
        /// 匯率
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 基準匯率編碼
        /// </summary>
        public string FromCurCode { get; set; }
        /// <summary>
        /// 目標匯率編碼
        /// </summary>
        public string ToCurCode { get; set; }
        /// <summary>
        /// 匯率編碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 描述列表
        /// </summary>
        public List<MutiLanguage> Descriptions { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        public string CreateDateStr { get; set; }
        public string UpdateDateStr { get; set; }
        public bool IsModify { get; set; }
    }
}
