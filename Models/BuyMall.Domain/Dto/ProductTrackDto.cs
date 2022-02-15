using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductTrackDto:BaseDto
    {
         
        public Guid MemberId { get; set; }

        public string ProductCode { get; set; }

        public Guid MerchantId { get; set; }

        public string MerchantName { get; set; }
    }

  
}
