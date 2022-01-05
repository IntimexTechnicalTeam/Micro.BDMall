using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class MenuDetailInfo
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Position { get; set; }

        public int Type { get; set; }

        public Guid NameTransId { get; set; }
        public List<MutiLanguage> Names { get; set; }

        public Guid TitleTransId { get; set; }
        public List<MutiLanguage> Titles { get; set; }

        public Guid ImageTransId { get; set; }
        public List<MutiLanguage> Images { get; set; }

        public bool IsShow { get; set; }

        public int Seq { get; set; }

        /// <summary>
        /// 是否顯子級的值，例如catalog的子級目錄
        /// </summary>
        public bool ShowSub { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        public bool PlacedTop { get; set; }


        public int RedirectType { get; set; }

        public string RedirectValue { get; set; }

        public string RedirectName { get; set; }

        /// <summary>
        /// 是否打開新窗體
        /// </summary>
        public bool IsNewWin { get; set; }

        public List<TypeDetail> Details { get; set; }
    }
}
