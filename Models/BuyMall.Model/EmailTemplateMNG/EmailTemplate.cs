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
    public class EmailTemplate : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        [Required]
        public MailType EmailType { get; set; }

        /// <summary>
        /// 邮件模板编号
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 4)]
        [MaxLength(20)]
        public string Code { get; set; }
        /// <summary>
        /// 邮件模板名称
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 5)]
        [MaxLength(40)]
        public string Name { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 6)]
        [MaxLength(80)]
        public string Subject { get; set; }


        /// <summary>
        /// 邮件内容
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 7)]
        //[MaxLength(4000)]
        public string EmailContent { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 8)]
        //[MaxLength(4000)]
        public string MessageContent { get; set; }


        /// <summary>
        /// 使用語言
        /// </summary>
        [Column(Order = 9)]
        public Language Lang { get; set; }
        /// <summary>
        /// WhatsApp消息内容
        /// </summary>
        [Column(TypeName = "nvarchar", Order = 10)]
        public string WhatsAppContent { get; set; }
    }
}