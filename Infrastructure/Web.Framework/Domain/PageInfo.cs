namespace Web.Framework
{
    public class PageInfo
    {       
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int Offset
        {
            get
            {
                return (Page - 1) * PageSize;
            }
        }

        /// <summary>
        /// 排序名称
        /// </summary>
        public string SortName { get; set; } = "";

        /// <summary>
        /// 正序(Asc)，反序(Desc)
        /// </summary>
        public string SortOrder { get; set; } = "Desc";

    }
}
