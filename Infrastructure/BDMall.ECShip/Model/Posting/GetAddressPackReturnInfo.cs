using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Posting
{
    public class GetAddressPackReturnInfo
    {
        /// <summary>
        /// 64位的PDF文件流
        /// </summary>
        public string FileString { get; set; }

        public string ErrMessage { get; set; }

        public string Status { get; set; }
        
    }
}
