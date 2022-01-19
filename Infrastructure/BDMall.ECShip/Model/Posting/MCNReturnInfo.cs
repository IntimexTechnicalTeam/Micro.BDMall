using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.ECShip.Model.Posting
{
    public class MCNReturnInfo
    {
        public string CCCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// MCN对应的智邮站站点编号
        /// </summary>
        public string PLCode { get; set; }
    }
}
