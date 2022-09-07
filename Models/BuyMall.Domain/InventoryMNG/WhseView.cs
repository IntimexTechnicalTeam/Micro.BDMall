namespace BDMall.Domain
{
    /// <summary>
    /// 倉庫資料
    /// </summary>
    /// <remarks>前端交互專用</remarks>
    public class WhseView
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所屬商家ID
        /// </summary>
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 聯繫地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 聯繫人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 倉庫名稱列表
        /// </summary>
        /// <remarks>多語言版本</remarks>
        public List<MutiLanguage> NameList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 倉庫地址列表
        /// </summary>
        /// <remarks>多語言版本</remarks>
        public List<MutiLanguage> AddressList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 倉庫聯繫人列表
        /// </summary>
        /// <remarks>多語言版本</remarks>
        public List<MutiLanguage> ContactList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 郵政編號
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 成本中心
        /// </summary>
        public string CostCenter { get; set; }
        /// <summary>
        /// 銀行賬戶
        /// </summary>
        public string AccountCode { get; set; }
        /// <summary>
        /// 是否修改記錄
        /// </summary>
        public bool IsModify { get; set; }
        /// <summary>
        /// 商家名稱
        /// </summary>
        public string MerchantName { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual void Validate()
        {
            var pattern = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+$";

            if (NameList == null || !NameList.Any())
                throw new InvalidInputException($"[{Resources.Message.WarehouseNameMultiRequire}]: { Resources.Message.RequiredField}");

            if (ToolUtil.CheckHasHTMLTag(this))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            //检查多语言值中是否有HTML标签
            if (ToolUtil.CheckMultLangListHasHTMLTag(NameList.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactList.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

           
        }
    }
}
