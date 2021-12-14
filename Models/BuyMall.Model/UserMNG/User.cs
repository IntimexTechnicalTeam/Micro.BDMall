using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BDMall.Model
{
    /// <summary>
    /// 系统用户
    /// </summary> 
    public class User : BaseAccount<Guid>
    {
        public User()
        {
           
            Language = Language.E;
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar", Order = 3)]
        //[Display(Name = "UserName", ResourceType = typeof(Resources.Model))]
        public string Name { get; set; }

        /// <summary>
        /// 设定的版面语言
        /// </summary> 
        [Column(Order = 5)]
        [DefaultValue(Language.E)]
        public Language Language { get; set; }

        /// <summary>
        /// 時間顯示格式
        /// </summary>
        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 6)]
        public string DateTimeFormat { get; set; }


        [Required]
        [Column(Order = 7)]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 最后一次登入时间
        /// </summary>
        [Column(Order = 8, TypeName = "datetime2")]
        public DateTime LastLogin { get; set; }

    }
}
