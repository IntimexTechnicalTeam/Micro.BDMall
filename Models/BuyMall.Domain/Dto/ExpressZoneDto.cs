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
    public class ExpressZoneDto : BaseDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 快递公司ID
        /// </summary>
        public Guid ExpressId { get; set; }
        public string Code { get; set; }
        public Guid NameTransId { get; set; }
        public Guid RemarkTransId { get; set; }
        /// <summary>
        /// 燃油附加費
        /// </summary>
        public decimal FuelSurcharge { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string ExpressCompanyName { get; set; }

        public List<CountryDto> Countrys { get; set; }
        public List<MutiLanguage> Names { get; set; }
        public List<MutiLanguage> Remarks { get; set; }

    }
}
