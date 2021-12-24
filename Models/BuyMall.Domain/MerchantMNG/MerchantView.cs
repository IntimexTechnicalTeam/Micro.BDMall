using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{

    public class MerchantView
    {      
        /// <summary>
        /// 商家主檔記錄ID
        /// </summary>
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        /// <summary>
        /// M BuyDong No.
        /// </summary>
        public string MerchNo { get; set; }
        /// <summary>
        /// 商家公司名稱
        /// </summary>
        public string Name { get; set; }
        public Guid NameTransId { get; set; }
        public List<MutiLanguage> NameList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 商家聯繫人
        /// </summary>
        public string Contact { get; set; }
        public Guid ContactTransId { get; set; }
        public List<MutiLanguage> ContactList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string ContactPhoneNum { get; set; }
        /// <summary>
        /// 傳真號碼
        /// </summary>
        public string FaxNum { get; set; }
        /// <summary>
        /// 聯繫地址
        /// </summary>
        public string ContactAddress { get; set; }
        public Guid ContactAddrTransId { get; set; }
        public List<MutiLanguage> ContactAddrList { get; set; } = new List<MutiLanguage>();
        public string ContactAddress2 { get; set; }
        public Guid ContactAddr2TransId { get; set; }
        public List<MutiLanguage> ContactAddr2List { get; set; } = new List<MutiLanguage> { };
        public string ContactAddress3 { get; set; }
        public Guid ContactAddr3TransId { get; set; }
        public List<MutiLanguage> ContactAddr3List { get; set; } = new List<MutiLanguage>();
        public string ContactAddress4 { get; set; }
        public Guid ContactAddr4TransId { get; set; }
        public List<MutiLanguage> ContactAddr4List { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 聯繫電郵地址
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 其他電郵地址
        /// </summary>
        public string OrderEmail { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }
        public Guid RemarksTransId { get; set; }
        public List<MutiLanguage> RemarksList { get; set; } = new List<MutiLanguage>();
        /// <summary>
        /// 是否修改記錄
        /// </summary>
        //public bool IsModify { get; set; }
        /// <summary>
        /// 記錄更改狀態
        /// </summary>
        public RecordStatus RecStatus { get; set; }
        /// <summary>
        /// 記錄是否有效
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 記錄是否已被邏輯刪除
        /// </summary>
        //public bool IsDeleted { get; set; }

        /// <summary>
        /// 小商家Logo
        /// </summary>
        public string SmallLogo { get; set; }

        /// <summary>
        /// 大商家Logo
        /// </summary>
        public string BigLogo { get; set; }

        /// <summary>
        /// 銀行賬號
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 系統賬戶是否已經創建
        /// </summary>
        public bool IsAccountCreated { get; set; }

        /// <summary>
        /// 推廣描述
        /// </summary>
        public string PromotionDesc { get; set; }

        /// <summary>
        /// 累計銷售量
        /// </summary>
        public int SalesQty { get; set; }

        /// <summary>
        /// 推廣資料
        /// </summary>
        //public MerchantPromotion Promotion { get; set; }

        /// <summary>
        /// 產品列表
        /// </summary>
        public List<ProductSummary> ProductList { get; set; } = new List<ProductSummary>();

        public MerchantType MerchantType { get; set; }

        public ApproveType ApproveStatus { get; set; }


        public bool IsExternal { get; set; }
        /// <summary>
        /// 是否Transin商家
        /// </summary>
        public bool IsTransin { get; set; }

        public string BankAccount { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }
        /// <summary>
        /// 自定义Url
        /// </summary>
        public List<string> CustomUrls { get; set; } = new List<string>();

        /// <summary>
        /// 是否香港品牌
        /// </summary>
        public bool IsHongKong { get; set; }
        

        public string ApproveStatusString
        {
            get
            {
                string result = "";
                switch (ApproveStatus)
                {
                    case ApproveType.WaitingApprove:
                        result = Resources.Value.WaitingApprove;
                        break;
                    case ApproveType.Pass:
                        result = Resources.Value.Pass;
                        break;
                    case ApproveType.Reject:
                        result = Resources.Value.Reject;
                        break;
                    case ApproveType.Editing:
                        result = Resources.Value.Editing;
                        break;
                }
                return result;
            }
            set { }
        }

        public string GCP { get; set; }

        public decimal CommissionRate { get; set; }

        public Language Language { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Score { get; set; }

        public MerchantECShipInfoDto ECShipInfo { get; set; } = new MerchantECShipInfoDto ();

        public int TranSinId { get; set; }

        public Language Lang { get; set; }


        public virtual void Validate()
        {
            var pattern = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+$";

            if (NameList==null || !NameList.Any())
                throw new InvalidInputException($"[{Resources.Label.MerchantName}]: { Resources.Message.RequiredField}");

            if (!ContactEmail.IsEmpty())
            {
                var emailRegx = new Regex(pattern);
                if (!emailRegx.IsMatch(ContactEmail))
                    throw new InvalidInputException($"[{Resources.Label.MerchantContactEmail}]: { Resources.Message.EmailAddrFormatIncorrect}");
            }

            if (OrderEmail.IsEmpty())           
                throw new InvalidInputException($"[{Resources.Label.MerchantOrderEmail}]: { Resources.Message.RequiredField}");

            if (ToolUtil.CheckHasHTMLTag(this))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            //检查多语言值中是否有HTML标签
            if (ToolUtil.CheckMultLangListHasHTMLTag(NameList.Select(s=>s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactList.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactAddrList.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactAddr2List.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactAddr3List.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(ContactAddr4List.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");

            if (ToolUtil.CheckMultLangListHasHTMLTag(RemarksList.Select(s => s.Desc).ToList()))
                throw new InvalidInputException($"{ Resources.Message.ExistHTMLLabel}");
        }
    }
}
