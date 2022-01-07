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
    public class EmailTypeTempItemDto : BaseDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 邮件类型
        /// </summary>
        public MailType MailType { get; set; }
        public Guid ItemId { get; set; }


    }
}
