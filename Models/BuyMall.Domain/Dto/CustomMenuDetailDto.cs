using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CustomMenuDetailDto: BaseDto
    {
        public Guid Id { get; set; }    
        public Guid MenuId { get; set; }

        public string Value { get; set; }

        public int Seq { get; set; }

        public bool IsAnchor { get; set; }

    }
}
