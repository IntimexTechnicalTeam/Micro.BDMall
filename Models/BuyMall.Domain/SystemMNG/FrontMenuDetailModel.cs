using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class FrontMenuDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public ValueModel Value { get; set; }

        public int Level { get; set; }

        public string ParentId { get; set; }


        /// <summary>
        /// 是否置頂
        /// </summary>
        public bool PlaceTop { get; set; }
        public bool IsNewWin { get; set; }

        public bool IsAnchor { get; set; }
        public List<FrontMenuDetailModel> Childs { get; set; }
        public int Seq { get; set; }
    }
}
