using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class CodeMasterCondition
    {
        public CodeMasterCondition()
        {
            this.Module = "";
            this.Function = "";
            this.Key = "";
            this.Value = "";
            this.IsActive = -1;
            this.IsDeleted = -1;
        }

        public PageInfo PageInfo { get; set; }
        public Guid ClientId { get; set; }
        public string Module { get; set; }
        public string Function { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int IsActive { get; set; }

        public int IsDeleted { get; set; }
    }
}
