using System.Runtime.Serialization;

namespace BDMall.Domain
{
    public class MemberDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [DataMember]
        
        public string Account { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        [DataMember]
        public string Email { get; set; }
       
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }


        /// <summary>
        /// 使用语言
        /// </summary>
       
        public Language Language { get; set; }

      
        public bool? Gender { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 不接收推广信息
        /// </summary>        
        public bool OptOutPromotion { get; set; }

        /// <summary>
        /// 生日
        /// </summary>       
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 最后一次登入日期
        /// </summary>     
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// 会员编码
        /// </summary>     
        public string Code { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
      
        public Guid GroupId { get; set; }


        public string CurrencyCode { get; set; }

        /// <summary>
        /// 是否激活，认证
        /// </summary>     
        public bool IsApprove { get; set; }

        /// <summary>
        /// 是否transin标识
        /// </summary>
       
        public bool IsTransin { get; set; }


    
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

      
        public decimal MallFun { get; set; }

        //[NotMapped]
        //public PersonalInfo PersonalInfo { get; set; }

        /// <summary>
        /// 第三方帳戶ID
        /// </summary>
   
        public string ThirdPartyUserId { get; set; }
        /// <summary>
        /// 第三方帳戶類型
        /// </summary>
       
        public int? ThirdPartyType { get; set; }

    
    }
}
