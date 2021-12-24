using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class SettingBLL : BaseBLL, ISettingBLL
    {
        private ICodeMasterRepository _codeMasterRepo;
        //private ITranslationRepository _translationRepo;

        public SettingBLL(IServiceProvider services) : base(services)
        {
        }

        public List<KeyValue> GetApproveStatuses()
        {
            var statusList = new List<KeyValue>();
            foreach (ApproveType statusItm in Enum.GetValues(typeof(ApproveType)))
            {
                var status = new KeyValue()
                {
                    Id = ((int)statusItm).ToString(),
                };
                switch (statusItm)
                {
                    case ApproveType.WaitingApprove:
                        status.Text = Resources.Value.WaitingApprove;
                        break;
                    case ApproveType.Pass:
                        status.Text = Resources.Value.Pass;
                        break;
                    case ApproveType.Reject:
                        status.Text = Resources.Value.Reject;
                        break;
                    case ApproveType.Editing:
                        status.Text = Resources.Value.Editing;
                        break;
                    default:
                        status.Text = string.Empty;
                        break;
                }

                statusList.Add(status);
            }
            return statusList;
        }

        //public List<KeyValue> GetSupportLanguages()
        //{
        //    var langs = GetSupportLanguage();
        //    var list = langs.Select(s => new KeyValue
        //    {
        //        Text = s.Text,
        //        Id = ((Language)Enum.Parse(typeof(Language), s.Code)).ToInt().ToString(),
        //    });

        //    return list.ToList();
        //}



        public double GetProductImageLimtSize()
        {
            double result = GetImgDefaultLimitSize();//默認為2MB
            var master = _codeMasterRepo.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.ImageLimitSize.ToString(), "ProductLimitSize");

            if (master != null)
            {
                result = double.Parse(master.Value) * 1024;
            }
            return result;
        }

        private double GetImgDefaultLimitSize()
        {
            return 2048;
        }

        /// <summary>
        /// 獲取店家支持的語言
        /// </summary>
        public List<SystemLang> GetSupportLanguages(Language rtnLang)
        {
            var langs = GetSupportLanguage(rtnLang);
            return langs;
        }

        /// <summary>
        /// 獲取店家支持的語言
        /// </summary>
        public List<SystemLang> GetSupportLanguages()
        {

            //注意，此缓存在repo内实现
            //string cacheKey = "SupportLanguages";
            //List<SystemLang> langs = CacheHelper.Get<List<SystemLang>>(cacheKey);
            //if (langs == null)
            //{
            //    langs = _codeMasterRepo.GetSupportLanguage();
            //    CacheHelper.AddFTClear(cacheKey, langs);
            //}

            var langs = GetSupportLanguage();
            return langs;

        }
    }
}
