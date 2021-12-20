using BDMall.Enums;
using BDMall.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Framework;

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

        public virtual void Validate()
        {          
            var pattern = "<\\s*(img|br|p|b|/p|a|div|iframe|button|script|i|html|form|input|frameset|body|table|br|label|link|li|style).*?>";
            var mateches = Regex.Matches(this.Code, pattern);
            if (mateches.Count > 0)
                throw new InvalidInputException(Message.ExistHTMLLabel);

            foreach (var item in Descs)
            {
                mateches = Regex.Matches(item.Desc, pattern);
                if (mateches.Count > 0)
                    throw new InvalidInputException(Message.ExistHTMLLabel);
            }
        }

    }
}
