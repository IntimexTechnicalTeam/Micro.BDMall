namespace BDMall.Domain
{
    public class ProductImageItemView
    {
        public Guid Id { get; set; }

        public Guid ImageID { get; set; }

        /// <summary>
        /// 原圖片路徑
        /// </summary>
        public string OriginalPath { get; set; }

        /// <summary>
        /// 縮略圖路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图片字节
        /// </summary>
        public long Size { get; set; }

        public string Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Length { get; set; }
        public ImageSizeType Type { get; set; }
        public string ImageType { get; set; }

    }
}
