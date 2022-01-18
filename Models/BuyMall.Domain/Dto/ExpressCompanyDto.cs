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
    public class ExpressCompanyDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid NameTransId { get; set; }
        public string CCode { get; set; }
        public string TCode { get; set; }
        public string Code { get; set; }
        public bool UseApi { get; set; }
        public string Name { get; set; }
        public List<MutiLanguage> Names { get; set; }
        public List<int> CountryIds { get; set; }


    }
}
