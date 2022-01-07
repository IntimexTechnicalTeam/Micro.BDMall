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
    public class EmailTemplateDto : BaseDto
    {
        public Guid Id { get; set; }
        public MailType EmailType { get; set; }
        public string EmailTypeDesc { get; set; }
        /// <summary>
        /// 邮件模板编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 邮件模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        public string LangText { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        //[MaxLength(4000)]
        public string EmailContent { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        //[MaxLength(4000)]
        public string MessageContent { get; set; }


        /// <summary>
        /// 使用語言
        /// </summary>
        public Language Lang { get; set; }
        /// <summary>
        /// WhatsApp消息内容
        /// </summary>
        public string WhatsAppContent { get; set; }


    }
}
