using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class Image
    {
        public Image()
        {
            this.ImageName = "";
            this.ImagePath = "";
        }
        /// <summary>
        /// 圖片名稱
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; set; }

    }
}
