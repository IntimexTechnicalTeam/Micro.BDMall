using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    public class ProductCatalogParent : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid CatalogId { get; set; }
        [Column(Order = 4)]
        public Guid ParentCatalogId { get; set; }

        [Column(Order = 5)]
        public int Level { get; set; }
        /// <summary>
        /// 一条完整路径的ID
        /// </summary>
        //[Column(Order = 6)]
        //public Guid PathId { get; set; }

    }
}
