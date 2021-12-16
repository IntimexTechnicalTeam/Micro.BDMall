using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductCatalogSummaryView
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid MerchantId { get; set; }
        public Guid ParentId { get; set; }

        public Guid PathId { get; set; }
        public string Icon { get; set; }
        public int Seq { get; set; }
        public int Level { get; set; }
        public string Desc { get; set; }

        public List<MutiLanguage> Descs { get; set; } = new List<MutiLanguage>();

    }
}

