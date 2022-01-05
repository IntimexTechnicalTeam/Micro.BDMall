using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MailServer : BaseEntity<Guid>
    {
        public string Code { get; set; }

        public string Server { get; set; }

        public string Port { get; set; }

        public bool IsSSL { get; set; }
    }
}
