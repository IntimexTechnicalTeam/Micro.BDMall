using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class BaseDto
    {
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
