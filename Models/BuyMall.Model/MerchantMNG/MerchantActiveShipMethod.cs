namespace BDMall.Model
{
    public class MerchantActiveShipMethod : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 付運方法Code
        /// </summary>
        [MaxLength(20)]
        [Column(Order = 4, TypeName = "varchar")]
        public string ShipCode { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column(Order = 5)]
        public bool IsEffect { get; set; }


    }
}
