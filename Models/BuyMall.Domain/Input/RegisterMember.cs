﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    [DataContract]
    public class RegisterMember
    {
        /// <summary>
        /// 账号
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Account不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Password不能为空")]
        public string Password { get; set; }

        public virtual void Validate()
        {
            if (this.Password.Length < 6 || this.Password.Length > 20)
                throw new InvalidInputException("密码必须是6-20位");
        }
    }
}
