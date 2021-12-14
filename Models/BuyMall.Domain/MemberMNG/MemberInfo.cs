using BDMall.Enums;
using System;
using System.Collections.Generic;

namespace BDMall.Domain
{
    public class MemberInfo : PersonalInfo
    {
        public string Password { get; set; }

        public string IsActivated { get; set; }

        public string PermissionId { get; set; }

        public string IsSuspend { get; set; }
        /// <summary>
        /// 会员组别ID
        /// </summary>
        public Guid GroupId { get; set; }

        public DateTime? BirthDate { get; set; }

        public string BirthDateString { get; set; }

        public DateTime? LastLogin { get; set; }

        public string LastLoginString { get; set; }



        public string CreateDateString { get; set; }



        public string UpdateDateString { get; set; }

        public Language Language { get; set; }

        /// <summary>
        /// 语言的描述
        /// </summary>
        public string LanguageName { get; set; }

        public string ClientCode { get; set; }

        public string MemberComment { get; set; }

        public int? Salutation { get; set; }

        public string JobTitle { get; set; }

        public string State { get; set; }

        public string Fax { get; set; }

        public string Website { get; set; }

        public string ShippingInfo { get; set; }

        public string Remarks { get; set; }

        public bool OptOut { get; set; }

        //public AppTypeEnum ComeFrom { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set { }
        }
        public string ThirdPartyUserId { get; set; }
        public int ThirdPartyType { get; set; }

        /// <summary>
        /// 是否临时用户
        /// </summary>
        public bool IsTempUser { get; set; }

        //public SimpleCurrency Currency { get; set; }

        /// <summary>
        /// 是否已綁定Transin帳戶
        /// </summary>
        public bool TransinLinkuped { get; set; }

        /// <summary>
        /// Transin帳戶詳細
        /// </summary>
        //public TransinMbrDetail TransinAcctDetail { get; set; } = new TransinMbrDetail();

       
    }
}
