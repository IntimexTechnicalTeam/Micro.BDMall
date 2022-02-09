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
    /// 退貨單留言
    /// </summary>
    public class ReturnOrderMessage : BaseEntity<Guid>
    {
        /// <summary>
        /// 退換單ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid ROrderId { get; set; }
        
        /// <summary>
        /// 消息
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        [Column(Order = 4, TypeName = "nvarchar")]
        public string Message { get; set; }
        /// <summary>
        /// 用戶ID
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public Guid UserId { get; set; }
        /// <summary>
        /// 用戶類型
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public AccountType UserType { get; set; }
    }
}
