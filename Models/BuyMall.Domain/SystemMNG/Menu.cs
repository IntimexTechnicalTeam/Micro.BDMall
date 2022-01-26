using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class Menu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }    

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
    }
}
