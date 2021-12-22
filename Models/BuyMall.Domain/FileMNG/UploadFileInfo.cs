using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public  class UploadFileInfo
    {
        /// <summary>
        /// 文件名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件原名称
        /// </summary>
        public string OriginalName { get; set; }
        /// <summary>
        /// 縮略圖
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// 縮略圖相對路徑
        /// </summary>
        public string ThumbnailPath { get; set; }
        /// <summary>
        /// 文件後綴
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 文件所在路徑
        /// </summary>
        public string Path { get; set; }

        public string ContentType { get; set; }

        /// <summary>
        ///  文件字节
        /// </summary>
        public byte[] Bytes { get; set; }
        /// <summary>
        /// 是否目錄（否則為文件）
        /// </summary>
        public bool IsDirectory { get; set; }
    }
}
