using System;

namespace BDMall.Runtime
{
    public class Setting
    {
        /// <summary>
        /// 默認時間值格式
        /// </summary>
        public static readonly string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 無限制數量內容的字數上限
        /// </summary>
        public static int UnlimitedContentWordQty = 200000;

        public static readonly string DefaultDateTimeFormat2 = "yyyy-MM-dd HH:mm";

        public static readonly string DefaultDateTimeFormat3 = "yyyy-MM-dd HH:mm:ss.SSS";
        /// <summary>
        /// 短日期格式
        /// </summary>
        public static readonly string ShortDateFormat = "yyyy-MM-dd";
    }
}
