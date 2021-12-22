using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class StoreInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string EmailSender { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }

        public string Domain { get; set; }

        public string WebSiteUrl { get; set; }

        public string AdminUrl { get; set; }

        public string StoreLogo { get; set; }

        /// <summary>
        /// 郵件商標
        /// </summary>
        public string EmailLogo { get; set; }
        public string WhatsAppChannelId { get; set; }
        public string WhatsAppApiKey { get; set; }
        public string WhatsAppDelayMs { get; set; }
    }
}
