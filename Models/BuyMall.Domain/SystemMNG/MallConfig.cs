using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MallConfig
    {
        public string Description { get; set; }
        public string Keywords { get; set; }

        public string MallName { get; set; }

        public string Image { get; set; }
        public Language DefaultLanguage { get; set; }
    }
}
