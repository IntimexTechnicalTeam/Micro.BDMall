namespace BDMall.Model
{
    public class Member : BaseAccount<Guid>
    {
        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar", Order = 5)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar", Order = 6)]
        public string LastName { get; set; }


        [StringLength(200)]
        [Column(TypeName = "nvarchar", Order = 8)]
        public string Mobile { get; set; }


        /// <summary>
        /// 使用语言
        /// </summary>
        [Column(Order = 12)]
        public Language Language { get; set; }

        [Column(Order = 13)]
        public bool? Gender { get; set; }

        [Column(Order = 14)]
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 不接收推广信息
        /// </summary>
        [Column(Order = 15)]
        public bool OptOutPromotion { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Column(Order = 16)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 最后一次登入日期
        /// </summary>
        [Column(Order = 17)]
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// 会员编码
        /// </summary>
        [MaxLength(30)]
        [Column(TypeName = "varchar", Order = 18)]
        public string Code { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        [Column(Order = 19)]
        public Guid GroupId { get; set; }


        [Column(Order = 20)]
        [StringLength(5)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 是否激活，认证
        /// </summary>
        [Column(Order = 21)]
        public bool IsApprove { get; set; }

        /// <summary>
        /// 是否transin标识
        /// </summary>
        [Column(Order = 24)]
        public bool IsTransin { get; set; }


        [NotMapped]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        [NotMapped]
        public decimal MallFun { get; set; }

        //[NotMapped]
        //public PersonalInfo PersonalInfo { get; set; }

        /// <summary>
        /// 第三方帳戶ID
        /// </summary>
        [NotMapped]
        public string ThirdPartyUserId { get; set; }
        /// <summary>
        /// 第三方帳戶類型
        /// </summary>
        [NotMapped]
        public int? ThirdPartyType { get; set; }
    }
}
