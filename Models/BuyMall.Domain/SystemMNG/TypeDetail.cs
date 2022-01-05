using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class TypeDetail
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 配對的Id
        /// </summary>
        public string ValueId { get; set; }

        public string ValueName { get; set; }

        public bool IsAnchor { get; set; }
        public int Seq { get; set; }


    }
}
