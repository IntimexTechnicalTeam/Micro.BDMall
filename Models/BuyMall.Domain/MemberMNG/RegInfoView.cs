namespace BDMall.Domain
{
    public class RegInfoView : SimpleMember
    {
       
        public string Password { get; set; }
        public bool Gender { get; set; }

        public Language Language { get; set; }

        public SimpleCurrency Currency { get; set; }

        public bool IsLogin { get; set; }

        /// <summary>
        /// LoginSerialNO
        /// </summary>
        public string SNO { get; set; }

        /// <summary>
        /// 最后一次访问时间
        /// </summary> 
        public DateTime LastAccessTime { get; set; }
 

        public bool IsTempUser { get; set; }
 
    }
}
