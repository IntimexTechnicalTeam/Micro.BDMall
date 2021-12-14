using BDMall.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    /// <summary>
    /// 第三方連結
    /// </summary>
    public class ThirdpartyLinkup : BaseEntity<Guid>
    {
        /// <summary>
        /// 倉庫記錄ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public ThridpartyType Type { get; set; }
        /// <summary>
        /// 會員ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid MemberId { get; set; }
        /// <summary>
        /// 外部連結ID
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column(TypeName = ("varchar"), Order = 5)]
        public string ExternalAccId { get; set; }
    }
}
