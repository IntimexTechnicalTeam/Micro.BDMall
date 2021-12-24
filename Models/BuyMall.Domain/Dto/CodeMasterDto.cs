using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CodeMasterDto
    {
        public int Id { get; set; }
        public string Module { get; set; }

        public string Function { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public Guid DescTransId { get; set; }

        public string Remark { get; set; }
        public string Description
        {
            get; set;

        }
        public List<MutiLanguage> Descriptions { get; set; }
    }
}
