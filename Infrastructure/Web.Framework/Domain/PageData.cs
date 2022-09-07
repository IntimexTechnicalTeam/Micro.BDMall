namespace Web.Framework
{
    public class PageData<T> : PageInfo
    {
        public PageData()
        {
            this.PageSize = 10;
            this.Page = 1;
            this.Data = new List<T>();
        }

        public PageData(PageInfo pageInfo)
        {
            if (pageInfo != null)
            {
                this.PageSize = pageInfo.PageSize;
                this.Page = pageInfo.Page;
                this.Data = new List<T>();
            }
        }
        //private int _totalPage;
        public int TotalPage
        {
            get
            {
                if (TotalRecord != 0 && Data != null)
                {
                    return (int)Math.Ceiling((decimal)TotalRecord / PageSize);
                }
                //else
                //{
                //    return _totalPage;
                //}
                return 0;

            }
            //set
            //{
            //    _totalPage = value;
            //}
        }


        private int _totalRecord;
        public int TotalRecord
        {
            get
            {

                return _totalRecord;

            }
            set
            {
                _totalRecord = value;

            }
        }

        public List<T> Data { get; set; }
    }
}
