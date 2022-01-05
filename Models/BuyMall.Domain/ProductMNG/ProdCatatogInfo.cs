using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProdCatatogInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public List<ProdCatatogInfo> Children { get; set; } = new List<ProdCatatogInfo>();

        public string Img { get; set; }
        public string ImgM { get; set; }

        /// <summary>
        /// 目录上级经过的树节点
        /// </summary>
        public List<ProdCatatogInfo> Nodes { get; set; } = new List<ProdCatatogInfo>();

        /// <summary>
        /// catalog細圖
        /// </summary>
        public string ImgS { get; set; }

        /// <summary>
        /// catalog大圖
        /// </summary>
        public string ImgBM { get; set; }
        /// <summary>
        /// catalog細圖
        /// </summary>
        public string ImgSM { get; set; }

        /// <summary>
        /// catalog大圖
        /// </summary>
        public string ImgB { get; set; }

        /// <summary>
        /// 路徑ID
        /// </summary>
        public Guid PathId { get; set; }
        /// <summary>
        /// 層級
        /// </summary>
        public int Level { get; set; }

    }
}
