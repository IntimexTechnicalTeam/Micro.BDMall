namespace BDMall.Domain
{
    [Serializable]
    [DataContract]
    public class MbrSearchCond : PageInfo
    {
        /// <summary>
        /// 會員電郵地址
        /// </summary>
        [DataMember]
        public string EmailAdd { get; set; }
        /// <summary>
        /// 會員編號
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// 會員名字
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// 會員姓氏
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// 會員電話
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// 會員組別
        /// </summary>
        [DataMember]
        public Guid BuyerGroup { get; set; }
        /// <summary>
        /// 語言
        /// </summary>
        [DataMember]
        public List<string> Language { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        [DataMember]
        public bool? Active { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [DataMember]
        public bool? Approve { get; set; }

        /// <summary>
        /// 是否刪除
        /// </summary>
        [DataMember]
        public bool? Deleted { get; set; }

        /// <summary>
        /// 是否停用
        /// </summary>
        //public bool Suspended { get; set; }
        /// <summary>
        /// 會員註冊時間範圍的起始時間
        /// </summary>
        [DataMember]
        public DateTime? RegDateFrom { get; set; }
        /// <summary>
        /// 會員註冊時間範圍的結束時間
        /// </summary>
        [DataMember]
        public DateTime? RegDateTo { get; set; }
        /// <summary>
        /// 是否不接受推廣郵件
        /// </summary>
        [DataMember]
        public bool? OutForReceiving { get; set; }

        //public MemberSearchType Type { get; set; }
        // public List<string> SelectedLanguageChoose { get; set; }
    }
}
