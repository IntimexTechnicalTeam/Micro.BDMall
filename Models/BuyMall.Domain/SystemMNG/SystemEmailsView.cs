using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class SystemEmailsView
    {
        public Guid Id { get; set; }
        public string SendFrom { get; set; }

        public string SendTo { get; set; }

        public string Subject { get; set; }

        public bool IsSucceeded { get; set; }

        public EmailerStatus Status { get; set; }

        public int FailCount { get; set; }
        public string StatuString
        {
            get
            {
                var result = "";
                switch (Status)
                {
                    case EmailerStatus.Editting:
                        result = EmailerStatus.Editting.ToString();
                        break;
                    case EmailerStatus.Processing:
                        result = EmailerStatus.Processing.ToString();
                        break;
                    case EmailerStatus.Finish:
                        result = EmailerStatus.Finish.ToString();
                        break;
                }
                return result;
            }
            set { }
        }

        public string Type { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
