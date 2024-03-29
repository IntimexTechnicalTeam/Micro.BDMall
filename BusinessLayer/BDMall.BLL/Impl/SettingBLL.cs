﻿namespace BDMall.BLL
{
    public class SettingBLL : BaseBLL, ISettingBLL
    {
        ICodeMasterRepository _codeMasterRepo;
        //private ITranslationRepository _translationRepo;

        public SettingBLL(IServiceProvider services) : base(services)
        {
            _codeMasterRepo = Services.Resolve<ICodeMasterRepository>();
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


        /// <summary>
        /// 獲取計劃任務資料列表
        /// </summary>
        /// <returns></returns>
        public List<CodeMasterDto> GetScheduleJobs()
        {
            var settingLst = _codeMasterRepo.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.Schedule);
            if (settingLst != null)
            {
                return settingLst;
            }
            return new List<CodeMasterDto>();
        }

        /// <summary>
        /// 獲取服務時間間隔單位列表
        /// </summary>
        /// <returns></returns>
        public List<CodeMasterDto> GetServiceIntervalUnits()
        {
            var settingLst = _codeMasterRepo.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.ServiceIntervalUnit);
            if (settingLst != null)
            {
                return settingLst;
            }
            return new List<CodeMasterDto>();
        }


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

        public List<ImageSize> GetProductImageSize()
        {
            List<ImageSize> list = new List<ImageSize>();

            var vals = _codeMasterRepo.GetCodeMasters(CodeMasterModule.Setting.ToString(), CodeMasterFunction.ProductImgSize.ToString())
                                    .OrderBy(o => o.Key).ThenBy(o => int.Parse(o.Value)).ToList();

            if (vals == null || !vals.Any())
            {
                for (var i = 1; i <= 8; i++)
                {
                    list.Add(new ImageSize { Width = i * 100, Length = i * 100 });
                }
                return list;
            }

            list = vals.Select(s => new ImageSize { Width = int.Parse(s.Value), Length = int.Parse(s.Value) }).ToList();
            return list;
        }

        public List<ImageSize> GetProductAdditionalImageSize()
        {
            List<ImageSize> list = new List<ImageSize>();
            var vals = _codeMasterRepo.GetCodeMasters(CodeMasterModule.Setting.ToString(), CodeMasterFunction.ProductAdditionalImgSize.ToString()).OrderBy(o => o.Key).ToList();
            if (vals == null || !vals.Any())
            {
                list.Add(new ImageSize { Width = 100, Length = 100 });
                list.Add(new ImageSize { Width = 400, Length = 400 });
                list.Add(new ImageSize { Width = 800, Length = 800 });
                return list;
            }

            list = vals.Select(item => new ImageSize
            {
                Width = int.Parse(item.Value),
                Length = int.Parse(item.Value)
            }).ToList();

            return list;
        }

        public List<KeyValue> GetCMCalculateTypes()
        {
            List<KeyValue> typeList = new List<KeyValue>();
            foreach (ProdCommissionType typeItm in Enum.GetValues(typeof(ProdCommissionType)))
            {
                var type = new KeyValue()
                {
                    Id = ((int)typeItm).ToString(),
                };
                switch (typeItm)
                {
                    case ProdCommissionType.MerchInheriting:
                        type.Text = Resources.Label.CMType_MerchInheriting;
                        break;
                    case ProdCommissionType.FixedValue:
                        type.Text = Resources.Label.CMType_FixedValue;
                        break;
                    case ProdCommissionType.FixedRate:
                        type.Text = Resources.Label.CMType_FixedRate;
                        break;
                    default:
                        type.Text = Resources.Value.Unknown;
                        break;
                }

                typeList.Add(type);
            }
            return typeList;
        }
        public ImageSize GetSmallProductImageSize()
        {
            ImageSize size = new ImageSize();

            var val = _codeMasterRepo.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.ProductImgSize.ToString(), "S");
            size.Width = int.Parse(val?.Value ?? "100");
            size.Length = int.Parse(val?.Value ?? "100");

            return size;
        }

        /// <summary>
        /// 獲取庫存交易類型列表
        /// </summary>
        /// <returns></returns>
        public List<CodeMasterDto> GetInvTransTypeLst()
        {
            var settingLst = _codeMasterRepo.GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.InvtTransType);
            if (settingLst != null)
            {
                return settingLst;
            }
            return new List<CodeMasterDto>();
        }

        public ECShipAccountInfo GetECShipAccountInfo()
        {

            ECShipAccountInfo result = new ECShipAccountInfo();
            var ecshipInfos = _codeMasterRepo.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.ECShip);
            //var merchantShipCount = MerchantShipInfoRepository.GetByKey(merchId);


            result.ECShipEmail = ecshipInfos.FirstOrDefault(p => p.Key == "MerchantShipCount")?.Value ?? "";// string.IsNullOrEmpty(merchantShipCount?.ECShipEmail) ? "eric_ml_chan@hkpo.gov.hk" : merchantShipCount?.ECShipEmail.Trim();
            result.IntegratorUserName = ecshipInfos.FirstOrDefault(p => p.Key == "IntegratorUserName")?.Value ?? "";// string.IsNullOrEmpty(merchantShipCount?.ECShipIntegraterName) ? ecshipInfos.FirstOrDefault(p => p.Key == "IntegratorUserName")?.Value ?? "" : merchantShipCount?.ECShipIntegraterName.Trim();// "techptxhk";// 
            result.ECShipName = ecshipInfos.FirstOrDefault(p => p.Key == "ECShipName")?.Value ?? "";// string.IsNullOrEmpty(merchantShipCount?.ECShipName) ? ecshipInfos.FirstOrDefault(p => p.Key == "ECShipName")?.Value ?? "" : merchantShipCount?.ECShipName.Trim();//"ttechptxhk";// 
            result.Password = ecshipInfos.FirstOrDefault(p => p.Key == "Password")?.Value ?? "";// string.IsNullOrEmpty(merchantShipCount?.ECShipPassword) ? ecshipInfos.FirstOrDefault(p => p.Key == "Password")?.Value ?? "" : merchantShipCount?.ECShipPassword.Trim();//"17924f27-5009-47a7-92b4-434b056ce50b";//
            result.Url = ecshipInfos.FirstOrDefault(p => p.Key == "Url")?.Value ?? "";//"17924f27-5009-47a7-92b4-434b056ce50b";//
            return result;


        }

        public ECShipAccountInfo GetECSSoazAccountInfo()
        {

            ECShipAccountInfo result = new ECShipAccountInfo();
            var ecshipInfos = _codeMasterRepo.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.ECShip);

            //var merchantShipCount = MerchantShipInfoRepository.GetByKey(merchId);


            result.ECShipEmail = "";
            result.IntegratorUserName = ecshipInfos.FirstOrDefault(p => p.Key == "SPIntegratorUserName")?.Value ?? "";// string.IsNullOrEmpty(merchantShipCount?.SPIntegraterName) ? ecshipInfos.FirstOrDefault(p => p.Key == "IntegratorUserName")?.Value ?? "" : merchantShipCount?.SPIntegraterName.Trim();// "techptxhk";// 
            result.ECShipName = ecshipInfos.FirstOrDefault(p => p.Key == "SPECShipName")?.Value ?? ""; //string.IsNullOrEmpty(merchantShipCount?.SPName) ? ecshipInfos.FirstOrDefault(p => p.Key == "ECShipName")?.Value ?? "" : merchantShipCount?.SPName.Trim();//"ttechptxhk";// 
            result.Password = ecshipInfos.FirstOrDefault(p => p.Key == "SPPassword")?.Value ?? ""; //string.IsNullOrEmpty(merchantShipCount?.SPPassword) ? ecshipInfos.FirstOrDefault(p => p.Key == "Password")?.Value ?? "" : merchantShipCount?.SPPassword.Trim();//"17924f27-5009-47a7-92b4-434b056ce50b";//
            result.Url = ecshipInfos.FirstOrDefault(p => p.Key == "Url")?.Value ?? "";//"17924f27-5009-47a7-92b4-434b056ce50b";//
            return result;


        }

        public MailTrackingAccountInfo GetMailTrackingAccountInfo()
        {
            //ICodeMasterBLL masterBLL = BLLFactory.Create(CurrentWebStore).CreateCodeMasterBLL();

            MailTrackingAccountInfo result = new MailTrackingAccountInfo();
            var ecshipInfos = _codeMasterRepo.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.MailTracking);
            result.ACId = ecshipInfos.FirstOrDefault(p => p.Key == "ACId")?.Value ?? "";// "techptxhk";// 
            result.ACPassword = ecshipInfos.FirstOrDefault(p => p.Key == "Password")?.Value ?? "";//"ttechptxhk";// 
            result.SystemCode = ecshipInfos.FirstOrDefault(p => p.Key == "SystemCode")?.Value ?? "";//"17924f27-5009-47a7-92b4-434b056ce50b";//
            result.Url = ecshipInfos.FirstOrDefault(p => p.Key == "Url")?.Value ?? "";//"17924f27-5009-47a7-92b4-434b056ce50b";//
            return result;
        }

        public InvTransIOType? GetInvTransIOType(InvTransType type)
        {
            switch (type)
            {
                case InvTransType.Purchase:
                    return InvTransIOType.I;
                case InvTransType.Relocation:
                    return InvTransIOType.O;
                case InvTransType.PurchaseReturn:
                    return InvTransIOType.O;
                case InvTransType.SalesShipment:
                    return InvTransIOType.O;
                case InvTransType.SalesReturn:
                    return InvTransIOType.I;
                case InvTransType.DeliveryReturn:
                    return InvTransIOType.I;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 獲取系統所設置的訂單冷靜期（全局）
        /// </summary>
        /// <returns></returns>
        public int GetOrderGracePeriodValue()
        {
            var setting = _codeMasterRepo.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.GracePeriod.ToString(), "OrderComepleted");
            if (setting != null)
            {
                return int.Parse(setting.Value);
            }
            return 14;
        }
    }
}
