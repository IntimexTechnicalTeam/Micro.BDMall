using BDMall.Enums;
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
    /// 郵件模板
    /// </summary>
    public class EmailTempItem : BaseEntity<Guid>
    {
        public Guid DescId { get; set; }


        /// <summary>
        /// 占位符号
        /// 用双大括号开始结尾，例如{{Name}}
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PlaceHolder { get; set; }

        /// <summary>
        /// 目标对象的类型
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ObjectType { get; set; }



        /// <summary>
        /// 目标对象的属性
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Propertity { get; set; }

        [MaxLength(1000)]
        public string Remark { get; set; }



    }
}