using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductCatalogEditModel : ProductCatalogDto
    {


        /// <summary>
        /// 库存属性
        /// </summary>
        public List<Guid> InvAttributes { get; set; } = new List<Guid> { Guid.Empty };

        /// <summary>
        /// 非库存属性
        /// </summary>
        public List<Guid> NotInvAttributes { get; set; } = new List<Guid> { Guid.Empty };



        /// <summary>
        /// 状态-用于表示记录是否变动。用于排序
        /// </summary>
        public bool IsChange { get; set; } = false;
        public bool Collapse { get; set; } = false;


        public List<ProductCatalogEditModel> Children { get; set; } = new List<ProductCatalogEditModel>();

        ///// <summary>
        ///// 图片显示路径
        ///// </summary>
        //public string IconPath { get; set; }

        public string IconPath { get; set; }
        public string IconPathM { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionTypeEnum Action { get; set; }

        public bool IsMappingProduct { get; set; } = false;

        public string Text => Desc;
    }
}
