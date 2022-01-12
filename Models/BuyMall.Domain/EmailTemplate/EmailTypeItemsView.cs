using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class EmailTypeItemsView : BaseDto
    {
        public MailType EmailType { get; set; }

        public string EmailTypeId { get; set; }

        public string Description { get; set; }

        public List<Guid> EmailItems { get; set; }
    }
}
