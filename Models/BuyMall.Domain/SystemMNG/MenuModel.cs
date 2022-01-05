using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class MenuModel
    {
        public List<FrontMenuDetailModel> HeaderMenus { get; set; }

        public List<FrontMenuDetailModel> FooterMenus { get; set; }

    }
}
