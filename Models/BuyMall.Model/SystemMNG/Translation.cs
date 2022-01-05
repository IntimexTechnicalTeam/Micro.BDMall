using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BDMall.Enums;


namespace BDMall.Model
{
    /// <summary>
    /// 翻译
    /// </summary>
    public class Translation : BaseEntity<Guid>
    {

        public Translation()
        {
            Module = string.Empty;
        }

        [Required]
        [Column(Order = 3)]
        public Guid TransId { get; set; }

        [Required]
        [Column(Order = 4)]
        public Language Lang { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Column(Order = 5)]
        public string Value { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Column(Order = 6)]
        [MaxLength(20)]
        public string Module { get; set; }

        //[ForeignKey("TransId")]
        //public virtual ProductAttribute ProductAttribute { get; set; }


    }
}
