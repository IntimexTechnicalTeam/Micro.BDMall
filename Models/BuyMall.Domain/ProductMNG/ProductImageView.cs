using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductImageView
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Guid AttrValue1 { get; set; }
        public Guid AttrValue2 { get; set; }
        public Guid AttrValue3 { get; set; }
        public string AttrValues1Name { get; set; }
        public string AttrValues2Name { get; set; }
        public string AttrValues3Name { get; set; }


        public Guid Sku { get; set; }

        public ImageType Type { get; set; }

        /// <summary>
        /// 用于默认显示的图片(默认为Items中的第一张图)
        /// </summary>
        public string Image { get; set; }

        public List<ProductImageItemView> Items { get; set; }

        /// <summary>
        /// 是否默認圖片
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsDefaultName { get; set; }

        //public string SmallImage { get; set; }
        //public string MiddleImage { get; set; }
        //public string BigImage { get; set; }
        //public string LargeImage { get; set; }



    }
}
