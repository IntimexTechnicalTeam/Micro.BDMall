using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantPromotionView
    {
        public Guid Id { get; set; }

        public Guid NameTranId { get; set; }

        public Guid DescTransId { get; set; }

        public Guid MerchantId { get; set; }
        public Guid CoverId { get; set; }

        public List<MutiLanguage> Covers { get; set; } = new List<MutiLanguage>();

        public Guid LogoId { get; set; }

        public List<MutiLanguage> Logos { get; set; } = new List<MutiLanguage>();

        public Guid BigLogoId { get; set; }

        public List<MutiLanguage> BigLogos { get; set; } = new List<MutiLanguage>();

        public List<MerchantPromotionBannerView> Banners { get; set; } = new List<MerchantPromotionBannerView>();

        public List<MutiLanguage> PromotionNames { get; set; } = new List<MutiLanguage>();
        public string PromotionName { get; set; }
        public List<MutiLanguage> Descriptions { get; set; } = new List<MutiLanguage>();
        public string Description { get; set; }

        public Guid OrderTranId { get; set; }


        public string ExpCompleteDay { get; set; }
        public List<MutiLanguage> ExpCompleteDays { get; set; } = new List<MutiLanguage>();

        public int LocalCoolDownDay { get; set; }
        public int OverSeaCoolDownDay { get; set; }

        public Guid TAndCTranId { get; set; }
        public List<MutiLanguage> TAndCs { get; set; } = new List<MutiLanguage>();
        public string TAndC { get; set; }

        public Guid NoticeTranId { get; set; }
        public List<MutiLanguage> Notices { get; set; } = new List<MutiLanguage>();
        public string Notice { get; set; }

        public Guid ReturnTermsTranId { get; set; }
        public List<MutiLanguage> ReturnTermses { get; set; } = new List<MutiLanguage>();
        public string ReturnTerms { get; set; }

        public Guid MobileCoverId { get; set; }
        public List<MutiLanguage> MobileCovers { get; set; } = new List<MutiLanguage>();

        public List<MerchantPromotionProductView> ProductList { get; set; } = new List<MerchantPromotionProductView>();

        public List<MutiLanguage> PromotionIntroductions { get; set; }

        public string PromotionIntroduction { get; set; }

        public ApproveType ApproveStatus { get; set; }

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

        public Guid IntorductionTranId { get; set; }

        public virtual void Validate()
        {
            string overFlowFmt = Resources.Message.DataLengthOverFlow;
            int wordQtyLimited = 10000;

            foreach (var item in Descriptions)
            {
                if (item.Desc?.Length > wordQtyLimited)
                    throw new InvalidOperationException(string.Format(overFlowFmt, BDMall.Resources.Label.Introduction, wordQtyLimited.ToString()));                
            }   
        }
    }

}