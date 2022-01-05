using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CustomMenuDto: BaseDto
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string Key { get; set; }

        public Guid NameTransId { get; set; }

        public Guid TitleTransId { get; set; }

        public Guid ImageTransId { get; set; }

        public int Seq { get; set; }

        public int PositionType { get; set; }

        public int LinkType { get; set; }

        public bool ShowNext { get; set; }

        public bool IsShow { get; set; }

        public bool PlaceTop { get; set; }

        public int RedirectLinkType { get; set; }

        public string RedirectLinkValue { get; set; }

        public bool IsNewWin { get; set; }


    }
}
