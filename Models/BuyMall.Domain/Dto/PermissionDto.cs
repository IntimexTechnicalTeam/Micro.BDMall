using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{

    public class PermissionDto:BaseDto
    {
        public PermissionDto()
        {

        }

        public Guid Id { get; set; }

        /// <summary>
        /// 模塊
        /// </summary>
       
        public string Module { get; set; }

        /// <summary>
        /// 功能
        /// </summary>

        public string Function { get; set; }

        /// <summary>
        /// 描述
        /// </summary>       
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 描述
        /// </summary>       
        [MaxLength(1000)]
        public string DescriptionTC { get; set; }

        [MaxLength(1000)]
        public string DescriptionSC { get; set; }

        public int Seq { get; set; }
        //public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
