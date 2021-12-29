using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain 
{
    public class ExpressCompanyDto
    {
        public Guid Id { get; set; }
        public Guid NameTransId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public string Name { get; set; }
        

    }
}
