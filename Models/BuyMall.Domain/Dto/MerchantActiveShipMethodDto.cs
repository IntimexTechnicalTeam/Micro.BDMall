using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public  class MerchantActiveShipMethodDto
    {
       public Guid Id { get; set; }

        public string ShipMethodName { get; set; }

        public Guid MerchantId { get; set; }

        /// <summary>
        /// 付運方法Code
        /// </summary>
        
        public string ShipCode { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
       
        public bool IsEffect { get; set; }


        public bool IsActive { get; set; }


        public bool IsDeleted { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

      
        public Guid? UpdateBy { get; set; }
    }
}
