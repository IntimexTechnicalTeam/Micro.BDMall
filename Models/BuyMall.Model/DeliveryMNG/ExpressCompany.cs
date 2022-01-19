using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressCompany : BaseEntity<Guid>
    {

        public Guid NameTransId { get; set; }
        [StringLength(10)]
        public string CCode { get; set; }
        [StringLength(10)]
        public string TCode { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        // public decimal Discount { get; set; }
        public bool UseApi { get; set; }
        //public List<Country> countryItem { get; set; }
        //public string Language { get; set; }


    }
}