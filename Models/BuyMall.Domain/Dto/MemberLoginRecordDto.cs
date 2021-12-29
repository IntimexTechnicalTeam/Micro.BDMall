using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MemberLoginRecordDto
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }


        public DateTime LoginTime { get; set; }


        public AppTypeEnum LoginFrom { get; set; }



        public DateTime? LogoutTime { get; set; }

        /// <summary>
        /// 持续的时间(秒)
        /// </summary>
        public int Duration { get; set; }


        /// <summary>
        ///  登出類型
        /// </summary>
        public LogoutType LogoutType { get; set; }

        public string MemberName { get; set; }

        public string LoginFromDisplay { get; set; }
        public string LogoutTypeDisplay { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
