namespace BDMall.Model
{
    public class Customization : BaseEntity<int>
    {

        [StringLength(50)]
        [Column(TypeName = "varchar", Order = 3)]
        public string Type { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar", Order = 4)]
        public string Key { get; set; }


        [StringLength(1000)]
        [Column(Order = 5)]
        public string Value { get; set; }

    }
}
