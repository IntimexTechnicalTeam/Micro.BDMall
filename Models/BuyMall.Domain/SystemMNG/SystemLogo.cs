using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class SystemLogo
    {
        /// <summary>
        /// 商店Logo
        /// </summary>
        public Image StoreLogo { get; set; }
        /// <summary>
        /// 報表Logo
        /// </summary>
        public Image ReportLogo { get; set; }
        /// <summary>
        /// 郵件Logo
        /// </summary>
        public Image EmailLogo { get; set; }
    }
}
