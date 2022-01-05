using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MailServerView
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String MailServer { get; set; }
        public String Port { get; set; }
        public bool IsSSL { get; set; }
    }
}
