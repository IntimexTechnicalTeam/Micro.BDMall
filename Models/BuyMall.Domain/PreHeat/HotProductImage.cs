namespace BDMall.Domain
{
    public class HotProductImage
    {
        /// <summary>
        /// 取ProductImageLists.ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 关联HotProduct.DefaultImageId
        /// </summary>
        public Guid ImageId { get; set; }

        public string ImagePath { get; set; }

        public Guid ProductId { get; set; }

        public string ProductCode { get; set; }

        public ImageSizeType Type { get; set; }
    }
}
