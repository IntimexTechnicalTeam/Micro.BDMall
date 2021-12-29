using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MemberLoginRecord : BaseEntity<Guid>
    {
        public Guid MemberId { get; set; }

        [Column(TypeName = "datetime2")]

        public DateTime LoginTime { get; set; }


        public AppTypeEnum LoginFrom { get; set; }
        

        [Column(TypeName = "datetime2")]

        public DateTime? LogoutTime { get; set; }

        /// <summary>
        /// 持续的时间(秒)
        /// </summary>
        public int Duration { get; set; }


        /// <summary>
        ///  登出類型
        /// </summary>
        public LogoutType LogoutType { get; set; }
    }
}
