
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PaymentMethodDto : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称多语言ID
        /// </summary>
        public Guid NameTransId { get; set; }
        /// <summary>
        /// 备注ID
        /// </summary>
        public Guid RemarkTransId { get; set; }
        /// <summary>
        /// 圖片
        /// </summary>
        public string Image { get; set; }
        public string BankAccount { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 支付服務手續費率
        /// </summary>
        public decimal ServRate { get; set; }

        public List<MutiLanguage> Names { get; set; }
        public List<MutiLanguage> Remarks { get; set; }
        public string ImgPath { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 支付服務手續費率描述
        /// </summary>
        public string ServRateDesc
        {
            get
            {
                return (ServRate * 100).ToString() + "%";
            }
        }
    }
}