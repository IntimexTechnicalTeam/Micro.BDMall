using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class AttributeObjectView
    {
        /// <summary>
        /// 下拉框属性的ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 下拉框属性的名称
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 下拉框选项
        /// </summary>
        public List<AttributeValueView> SubItems { get; set; } = new List<AttributeValueView>();
    }
}
