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
    public class DeliveryAddressDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Remark { get; set; }
        public bool Default { get; set; }

        public Guid MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Address1 { get; set; }


        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        public string City { get; set; }
        public int CityId { get; set; }

        public string PostalCode { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }
    }
}
