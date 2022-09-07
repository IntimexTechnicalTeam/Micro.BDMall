namespace BDMall.Domain
{
    public class ProductSpecificationView
    {
        public Guid ProdID { get; set; }

        /// <summary>
        /// 产品长
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// 产品宽
        /// </summary>
        public decimal Heigth { get; set; }
        /// <summary>
        /// 产品长
        /// </summary>
        public decimal Length { get; set; }
        /// <summary>
        /// 产品单位
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// 包裝描述
        /// </summary>
        public string PackageDescription { get; set; }

        /// <summary>
        /// 包装高
        /// </summary>
        public decimal PackageHeight { get; set; }
        /// <summary>
        /// 包装宽
        /// </summary>
        public decimal PackageWidth { get; set; }
        /// <summary>
        /// 包装长
        /// </summary>
        public decimal PackageLength { get; set; }
        /// <summary>
        ///包装单位
        /// </summary>
        public int PackageUnit { get; set; }


        /// <summary>
        /// 毛重
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// 淨重
        /// </summary>
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 重量單位
        /// </summary>
        public WeightUnit WeightUnit { get; set; }
    }
}
