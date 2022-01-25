using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ExpressCompanyView
    {
        public Guid Id { get; set; }
        public string CCode { get; set; }
        public string TCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
