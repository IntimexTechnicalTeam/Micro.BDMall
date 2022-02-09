using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MicroProductDetail
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商品Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属目录
        /// </summary>
        public Guid CatalogId { get; set; }

        /// <summary>
        /// 是否喜爱
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal SalePrice { get; set; }

        public decimal SalePrice2 { get; set; }

        /// <summary>
        /// 貨幣
        /// </summary>
        public SimpleCurrency Currency { get; set; }
        public SimpleCurrency Currency2 { get; set; }

        /// <summary>
        /// 原價
        /// </summary>
        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrice2 { get; set; }

        /// <summary>
        /// 附件价
        /// </summary>
        public decimal MarkupPrice { get; set; }
        public decimal MarkupPrice2 { get; set; }

        /// <summary>
        /// 產品圖片
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// 附加圖片
        /// </summary> 
        public List<List<string>> AdditionalImages { get; set; }

        /// <summary>
        /// 产品属性列表
        /// </summary>        
        public List<ProdAtt> AttrList { get; set; }

        /// <summary>
        /// 产品详细信息，描述
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 评分数
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 拒送国家列表
        /// </summary>
        public List<string> RefuseCountry { get; set; }

        /// <summary>
        /// 可配送國家列表
        /// </summary>
        public List<string> SupportCountry { get; set; }
    }
}
