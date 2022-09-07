namespace BDMall.Model
{
    public class ShoppingCartItemDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 关联ShoppingCartItem.Id
        /// </summary>
        [Column(Order = 3)]
        public Guid ShoppingCartItemId { get; set; }

        /// <summary>
        ///  会员ID/临时ID
        /// </summary>
        [Column(Order = 4)]
        public Guid MemberId { get; set; }

        [Column(Order = 5)]
        public Guid ProductId { get; set; }

        [Column(Order = 6)]
        public string ProductCode { get; set; }

        [Column(Order = 7)]
        public Guid MerchantId { get; set; }

        [Column(Order = 8)]
        public Guid SkuId { get; set; }

        [Column(Order = 9)]
        public int Qty { get; set; }

        [Column(Order = 10)]
        public Guid AttrId1 { get; set; }

        [Column(Order = 11)]
        public Guid AttrId2 { get; set; }

        [Column(Order = 12)]
        public Guid AttrId3 { get; set; }

        [Column(Order = 13)]
        public Guid AttrValue1 { get; set; }

        [Column(Order =14)]
        public Guid AttrValue2 { get; set; }

        [Column(Order = 15)]
        public Guid AttrValue3 { get; set; }

        [Column(Order = 16)]
        public Guid AttrName1 { get; set; }

        [Column(Order = 17)]
        public Guid AttrName2 { get; set; }

        [Column(Order = 18)]
        public Guid AttrName3 { get; set; }

        [Column(Order = 19)]
        public Guid AttrValueName1 { get; set; }

        [Column(Order = 20)]
        public Guid AttrValueName2 { get; set; }

        [Column(Order = 21)]
        public Guid AttrValueName3 { get; set; }

        [Column(Order = 22)]
        public decimal AttrValue1Price { get; set; }

        [Column(Order = 23)]
        public decimal AttrValue2Price { get; set; }

        [Column(Order = 24)]
        public decimal AttrValue3Price { get; set; }

    }
}
