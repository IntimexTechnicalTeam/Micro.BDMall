using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Model
{
    /// <summary>
    /// 角色\用户组别
    /// </summary>
  
    public class Role : BaseEntity<Guid>
    {
        public Role()
        {
            
        }

        [StringLength(20)]
        [Column(TypeName = "varchar", Order = 3)]
        public string Name { get; set; }


        /// <summary>
        /// 名字多语言Id
        /// </summary>
        [Column(Order = 9)]
        public Guid FullNameTransId { get; set; }

        /// <summary>
        /// 备注多语言Id
        /// </summary>
        [Column(Order = 10)]
        public Guid RemarkTransId { get; set; }

        /// <summary>
        /// 是否系統默認角色
        /// </summary>
        [Column(Order = 97)]
        public bool IsSystem { get; set; }


        /// <summary>
        /// 權限
        /// </summary>
        [NotMapped]
        public List<Permission> PermissionList { get; set; }
        ////virtual 增加此标记会使用延迟加载
        //public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}
