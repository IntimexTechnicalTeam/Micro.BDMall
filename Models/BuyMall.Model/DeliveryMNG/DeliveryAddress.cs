namespace BDMall.Model
{
    public class DeliveryAddress : BaseEntity<Guid>
    {
        public string Remark { get; set; }
        public bool Default { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string Address { get; set; }


        [StringLength(200)]
        public string Address1 { get; set; }


        [StringLength(200)]
        public string Address2 { get; set; }

        [StringLength(200)]
        public string Address3 { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        [StringLength(100)]
        public string City { get; set; }
        [NotMapped]
        public int CityId { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Mobile { get; set; }
        public bool? Gender { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]

        public string Email { get; set; }
    }
}