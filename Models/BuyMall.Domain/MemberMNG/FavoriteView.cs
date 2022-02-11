using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public  class FavoriteMchView
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }

        public string ImgPath { get; set; }
 
        public string CurrencyCode { get; set; }
    }

    public class FavoriteProductView
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        public string ImgPath { get; set; }

        public decimal SalePrice { get; set; }

        public string CurrencyCode { get; set; }
    }

}
