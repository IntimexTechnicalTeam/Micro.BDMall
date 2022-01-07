using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    /// <summary>
    /// 郵件模板
    /// </summary>
    public class EmailTypeTempItem : BaseEntity<Guid>
    {
        /// <summary>
        /// 邮件类型
        /// </summary>
        public MailType MailType { get; set; }
        public Guid ItemId { get; set; }


    }
}
