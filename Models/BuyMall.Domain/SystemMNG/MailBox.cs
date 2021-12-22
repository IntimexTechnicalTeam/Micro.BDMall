using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MailBox
    {
        /// <summary>
        /// 郵件服務器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        public bool SSL { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 發送賬號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 賬號密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发送邮件账号的显示内容
        /// </summary>
        public string Display { get; set; }
    }
}
