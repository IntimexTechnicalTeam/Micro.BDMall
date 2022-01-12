using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class CodeMasterBLL : BaseBLL, ICodeMasterBLL
    {
        ICodeMasterRepository _codeMasterRepo;
        ITranslationRepository _translationRepo;
        ISettingBLL _settingBLL;

        public CodeMasterBLL(IServiceProvider services) : base(services)
        {
            _codeMasterRepo = Services.Resolve<ICodeMasterRepository>();
            _translationRepo = Services.Resolve<ITranslationRepository>();
            _settingBLL = Services.Resolve<ISettingBLL>();
        }

        public bool ActualDelete(int id)
        {

            var codeMaster = baseRepository.GetModel<CodeMaster>(p => p.Id == id);
            if (codeMaster != null)
            {
                _translationRepo.DeleteByTransId(codeMaster.DescTransId);
                baseRepository.Delete(codeMaster);
                if (codeMaster.Remark?.Length == 36)
                {
                    Guid remarkTransId;
                    if (Guid.TryParse(codeMaster.Remark, out remarkTransId))
                    {
                        _translationRepo.DeleteByTransId(remarkTransId);
                    }
                }
            }
            return true;

        }

        public bool DeleteCodeMaster(int id)
        {
            var codeMaster = baseRepository.GetModel<CodeMaster>(p => p.Id == id);
            if (codeMaster != null)
            {
                codeMaster.IsDeleted = true;
                baseRepository.Update(codeMaster);
            }
            return true;

        }

        public CodeMasterDto GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key)
        {
            return GetCodeMasters(module.ToString(), function.ToString(), key).FirstOrDefault();
        }

        public CodeMasterDto GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key, string value)
        {
            CodeMasterDto model = null;

            model = _codeMasterRepo.GetCodeMaster(module.ToString(), function.ToString(), key, value);

            return model;
        }

        public CodeMasterDto GetCodeMasterById(int id)
        {
            CodeMasterDto model = null;
            //ISettingBLL settingBLL = new SettingBLL(UnitOfWork, _codeMasterRepo, _translationRepo);

            var langs = _settingBLL.GetSupportLanguages();

            if (id == 0)
            {
                model = new CodeMasterDto();
                model.Descriptions = LangUtil.GetMutiLangFromTranslation(new List<Translation>(), langs);

            }
            else
            {
                var dbModel = baseRepository.GetModel<CodeMaster>(p => p.Id == id);

                if (dbModel != null)
                {
                    model = AutoMapperExt.MapTo<CodeMasterDto>(dbModel);
                    if (model.DescTransId != null)
                    {
                        var translates = _translationRepo.GetTranslation(model.DescTransId);

                        model.Descriptions = LangUtil.GetMutiLangFromTranslation(translates, langs);

                    }
                    else
                    {
                        model.Descriptions = LangUtil.GetMutiLangFromTranslation(null, langs);
                    }

                }
            }
            return model;
        }

        public CodeMasterDto GetCodeMasterByKey(string module, string function, string key)
        {
            return _codeMasterRepo.GetCodeMaster(module, function, key);

        }

        public List<CodeMasterDto> GetCodeMasters(CodeMasterModule module, CodeMasterFunction function)
        {
            return GetCodeMasters(module.ToString(), function.ToString(), null);
        }

        public List<CodeMasterDto> GetCodeMasters(string module, string function, string key)
        {
            return _codeMasterRepo.GetCodeMasters(Guid.Empty, module, function, key);
        }

        public PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition cond)
        {
            return _codeMasterRepo.GetCodeMastersByPage(cond);
        }

        public MailBox GetMailBox()
        {
            string MailSender = "MailSender";
            string MailSenderAccount = "MailSenderAccount";
            string MailServer = "MailServer";
            string MailServerPort = "MailServerPort";
            string MailServerSSL = "MailServerSSL";
            string MailSenderPwd = "MailSenderPwd";
            string MailSenderDisplay = "MailSenderDisplay";


            MailBox box = new MailBox();
            box.Sender = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailSender)?.Value;
            box.Account = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailSenderAccount)?.Value;

            if (string.IsNullOrEmpty(box.Sender))
            {
                throw new BLException("MailSender is null or empty.");
            }
            if (string.IsNullOrEmpty(box.Account))
            {
                throw new BLException("MailSenderAccount is null or empty.");
            }
            box.Display = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailSenderDisplay)?.Value;
            box.Server = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailServer)?.Value;
            if (string.IsNullOrEmpty(box.Server))
            {
                throw new BLException("MailServer IsNullOrEmpty");
            }
            string port = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailServerPort)?.Value;
            if (string.IsNullOrEmpty(port))
            {
                throw new BLException("port IsNullOrEmpty");
            }
            box.Port = string.IsNullOrEmpty(port) ? 25 : int.Parse(port);
            box.Password = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailSenderPwd)?.Value;

            string ssl = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), MailServerSSL)?.Value;
            if (ssl != null)
            {
                if (ssl == "1" || ssl.ToLower() == "true")
                {
                    box.SSL = true;
                }
            }
            return box;

        }

        public List<CodeMasterDto> GetMallInfo()
        {
            string MallInfoName = "MallInfoName";
            string MallInfoAddress = "MallInfoAddress";
            string MallInfoContact = "MallInfoContact";
            string MallInfoFax = "MallInfoFax";
            string MallInfoPhone = "MallInfoPhone";
            string MallInfoEmail = "MallInfoEmail";
            string MailSender = "MailSender";
            string MailSenderPwd = "MailSenderPwd";
            string MailSenderDisplay = "MailSenderDisplay";
            string MallInfoWebSiteUrl = "MallInfoWebSiteUrl";
            string MallAdminUrl = "MallAdminUrl";

            string[] mallInfoList = { MallInfoName, MallInfoAddress, MallInfoContact, MallInfoFax, MallInfoPhone, MallInfoEmail, MallInfoWebSiteUrl, MallAdminUrl };
            string[] emailList = { MailSender, MailSenderPwd, MailSenderDisplay };

            List<CodeMasterDto> list = new List<CodeMasterDto>();
            foreach (var item in mallInfoList)
            {
                var info = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), item);
                list.Add(info);
            }
            foreach (var item in emailList)
            {
                var info = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Email.ToString(), item);
                list.Add(info);
            }
            return list;

        }

        public StoreInfo GetStoreInfo(Language lang)
        {
            StoreInfo storeInfo = new StoreInfo();
            storeInfo.Name = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoName")?.Descriptions.FirstOrDefault(d => d.Language == lang)?.Desc ?? "";
            storeInfo.Address = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoAddress")?.Descriptions.FirstOrDefault(d => d.Language == lang)?.Desc ?? "";
            storeInfo.Phone = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoPhone")?.Value ?? "";
            storeInfo.Email = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoEmail")?.Value ?? "";
            storeInfo.Fax = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoFax")?.Value ?? "";
            storeInfo.WebSiteUrl = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallInfoWebSiteUrl")?.Value ?? "";
            storeInfo.AdminUrl = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.MallInfo.ToString(), "MallAdminUrl")?.Value ?? "";
            var emailLogo = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.SystemLogo.ToString(), "EmailLogo")?.Value ?? "";
            var storeLogo = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.SystemLogo.ToString(), "StoreLogo")?.Value ?? "";
            storeInfo.WhatsAppChannelId = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.WhatsAppSetting.ToString(), "Channel_ID")?.Value ?? "";
            storeInfo.WhatsAppApiKey = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.WhatsAppSetting.ToString(), "API_Key")?.Value ?? "";
            storeInfo.WhatsAppDelayMs = GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.WhatsAppSetting.ToString(), "DelayMs")?.Value ?? "";

            storeInfo.EmailLogo = PathUtil.GetRelativePath(Guid.Empty.ToString(), FileFolderEnum.StoreLogo) + emailLogo;

            storeInfo.StoreLogo = PathUtil.GetRelativePath(Guid.Empty.ToString(), FileFolderEnum.StoreLogo) + storeLogo;
            //if (CurrentUser.Type == AccountType.User)//后台用户
            //{

            //    storeInfo.EmailLogo = PathUtil.GetStoreLogoImagePath(AppTypeEnum.AdminSite, CurrentWebStore.ClientId, emailLogo);
            //    storeInfo.StoreLogo = PathUtil.GetStoreLogoImagePath(AppTypeEnum.AdminSite, CurrentWebStore.ClientId, storeLogo);
            //}
            //else
            //{
            //    storeInfo.EmailLogo = PathUtil.GetStoreLogoImagePath(AppTypeEnum.MobileApp, CurrentWebStore.ClientId, emailLogo);
            //    storeInfo.StoreLogo = PathUtil.GetStoreLogoImagePath(AppTypeEnum.MobileApp, CurrentWebStore.ClientId, storeLogo);
            //}

            return storeInfo;
        }



        public SystemLogo GetSystemLogos()
        {
            SystemLogo logo = new SystemLogo();

            var logos = GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.SystemLogo);
            var storeLogo = logos.FirstOrDefault(p => p.Key == "StoreLogo");
            var emailLogo = logos.FirstOrDefault(p => p.Key == "EmailLogo");
            var reportLogo = logos.FirstOrDefault(p => p.Key == "ReportLogo");

            logo.StoreLogo = storeLogo == null ? new Image() : new Image { ImageName = storeLogo.Value, ImagePath = storeLogo.Value };
            logo.EmailLogo = emailLogo == null ? new Image() : new Image { ImageName = emailLogo.Value, ImagePath = emailLogo.Value };
            logo.ReportLogo = reportLogo == null ? new Image() : new Image { ImageName = reportLogo.Value, ImagePath = reportLogo.Value };

            return logo;
        }

        public void InsertCodeMaster(CodeMasterDto codeMaster)
        {

            if (StringHtmlContentCheck(codeMaster))
            {
                throw new BLException(Resources.Message.ExistHTMLLabel);
            }
            UnitOfWork.IsUnitSubmit = true;

            Guid translateId = _translationRepo.InsertMutiLanguage(codeMaster.Descriptions, TranslationType.CodeMaster);
            CodeMaster master = new CodeMaster();
            master.ClientId = Guid.Empty;
            master.Module = codeMaster.Module;
            master.Function = codeMaster.Function;
            master.DescTransId = translateId;
            master.Key = codeMaster.Key;
            master.Value = codeMaster.Value;
            master.IsActive = true;
            master.IsDeleted = false;
            master.Remark = codeMaster.Remark;
            baseRepository.Insert(master);

            UnitOfWork.Submit();
        }
        public bool StringHtmlContentCheck(CodeMasterDto model)
        {
            if (model != null)
            {
                if (StringUtil.CheckHasHTMLTag(model.Value))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Description))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Remark))
                {
                    return true;
                }

                if (CheckMultLangListHasHTMLTag(model.Descriptions))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckMultLangListHasHTMLTag(List<MutiLanguage> multList)
        {
            if (multList?.Count > 0)
            {
                foreach (var item in multList)
                {
                    if (StringUtil.CheckHasHTMLTag(item.Desc))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public SystemResult SaveSystemLogo(SystemLogo logo)
        {
            SystemResult result = new SystemResult();
            SaveStoreLogo(logo.StoreLogo);
            SaveEmailLogo(logo.EmailLogo);
            SaveReportLogo(logo.ReportLogo);
            result.Succeeded = true;
            return result;
        }
        private void SaveStoreLogo(Image storeLogo)
        {
            try
            {


                var dbStoreLogo = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(p => p.Key == "StoreLogo");
                if (dbStoreLogo == null)
                {
                    var master = new CodeMaster
                    {
                        ClientId = Guid.Empty,
                        Module = CodeMasterModule.Setting.ToString(),
                        Function = CodeMasterFunction.SystemLogo.ToString(),
                        DescTransId = Guid.NewGuid(),
                        Key = "StoreLogo",
                        Value = storeLogo.ImageName,
                        Remark = ""
                    };

                    baseRepository.Insert(master);
                }
                else
                {
                    dbStoreLogo.Value = storeLogo.ImageName;
                    baseRepository.Update(dbStoreLogo);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("保存商店Logo失败 " + ex.Message);
            }
        }

        private void SaveEmailLogo(Image emailLogo)
        {
            try
            {


                var dbEmailLogo = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(p => p.Key == "EmailLogo");
                if (dbEmailLogo == null)
                {
                    var master = new CodeMaster
                    {
                        ClientId = Guid.Empty,
                        Module = CodeMasterModule.Setting.ToString(),
                        Function = CodeMasterFunction.SystemLogo.ToString(),
                        DescTransId = Guid.NewGuid(),
                        Key = "EmailLogo",
                        Value = emailLogo.ImageName,
                        Remark = ""
                    };

                    baseRepository.Insert(master);
                }
                else
                {
                    dbEmailLogo.Value = emailLogo.ImageName;
                    baseRepository.Update(dbEmailLogo);
                }

                UnitOfWork.Submit();


            }
            catch (Exception ex)
            {
                throw new Exception("保存邮件Logo失败 " + ex.Message);
            }
        }

        private void SaveReportLogo(Image reportLogo)
        {
            try
            {

                var dbReportLogo = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(p => p.Key == "ReportLogo");
                if (dbReportLogo == null)
                {
                    var master = new CodeMaster
                    {
                        ClientId = Guid.Empty,
                        Module = CodeMasterModule.Setting.ToString(),
                        Function = CodeMasterFunction.SystemLogo.ToString(),
                        DescTransId = Guid.NewGuid(),
                        Key = "ReportLogo",
                        Value = reportLogo.ImageName,
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        CreateDate = DateTime.Now,
                        Remark = ""
                    };

                    baseRepository.Insert(master);
                }
                else
                {
                    dbReportLogo.Value = reportLogo.ImageName;
                    baseRepository.Update(dbReportLogo);
                }

                UnitOfWork.Submit();

            }
            catch (Exception ex)
            {
                throw new Exception("保存邮件Logo失败 " + ex.Message);
            }
        }

        public void UpdateCodeMaster(CodeMasterDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (StringHtmlContentCheck(model))
            {
                throw new BLException(Resources.Message.ExistHTMLLabel);
            }

            var dbModel = baseRepository.GetModel<CodeMaster>(p => p.Id == model.Id);
            UnitOfWork.IsUnitSubmit = true;
            if (dbModel != null)
            {
                dbModel.Module = model.Module;
                dbModel.Function = model.Function;
                dbModel.Key = model.Key;
                dbModel.Value = model.Value;
                dbModel.IsActive = model.IsActive;
                dbModel.Remark = model.Remark;
                dbModel.UpdateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.UpdateDate = DateTime.Now;
                baseRepository.Update(dbModel);
            }

            //更新翻譯表
            _translationRepo.UpdateMutiLanguage(dbModel.DescTransId, model.Descriptions, TranslationType.CodeMaster);

            UnitOfWork.Submit();


        }

        public void UpdateMallInfo(List<CodeMasterDto> info)
        {
            if (info?.Count() > 0)
            {
                foreach (var item in info)
                {
                    UpdateCodeMaster(item);
                }
            }
        }

        #region  系統定制化功能

        /// <summary>
        /// 獲取系統定制化功能清單
        /// </summary>
        public SysCustomization GetSysCustomization()
        {

            var funcCustom = new SysCustomization();

            //CouponSwitch	
            #region CouponSwitch
            CodeMasterFunction funcEnm = CodeMasterFunction.CouponSetting;
            var codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "CouponSwitch");
            if (codeMaster != null)
            {
                funcCustom.CouponSwitch = codeMaster.Value.ToUpper() == "TRUE" ? true : false;
            }
            #endregion

            //MailServerSSL
            #region MailServerSSL
            funcEnm = CodeMasterFunction.Email;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "MailServerSSL");
            if (codeMaster != null)
            {
                funcCustom.MailServerSSLSwitch = codeMaster.Value == "1" ? true : false;
            }
            #endregion

            //Email
            #region Email
            funcEnm = CodeMasterFunction.Email;
            var codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm);
            if (codeMasterList?.Count > 0)
            {
                codeMasterList = codeMasterList.Where(x => x.Key != "MailServerSSL").ToList();
                foreach (var item in codeMasterList)
                {
                    var cusVal = new CustomizationValue()
                    {
                        Id = item.Key,
                        Value = item.Value,
                        Desc = item.Description
                    };
                    funcCustom.EmailSettings.Add(cusVal);
                }
            }
            #endregion



            //DefaultLanguage
            #region DefaultLanguage
            funcEnm = CodeMasterFunction.Language;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "DefaultLanguage");
            if (codeMaster != null)
            {
                funcCustom.DefaultLanguage = codeMaster.Value;
            }
            #endregion

            //OrderNoSetting
            #region OrderNoSetting
            funcEnm = CodeMasterFunction.OrderNoSetting;
            codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm);
            if (codeMasterList?.Count > 0)
            {
                funcCustom.OrderNoAutoGenSwitch = codeMasterList.FirstOrDefault(x => x.Key == "IsAuto").Value == "1" ? true : false;

                codeMasterList = codeMasterList.Where(x => x.Key != "IsAuto").ToList();
                foreach (var item in codeMasterList)
                {
                    var keyVal = new CustomizationValue()
                    {
                        Id = item.Key,
                        Value = item.Value,
                        Desc = item.Description
                    };
                    funcCustom.OrderNoAutoGenSettings.Add(keyVal);
                }
            }
            #endregion

            //SupportLanguage
            #region SupportLanguage
            funcEnm = CodeMasterFunction.SupportLanguage;
            codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm).OrderBy(o => o.Key).ToList();
            if (codeMasterList?.Count > 0)
            {
                foreach (var item in codeMasterList)
                {
                    var keyVal = new CustomizationValue()
                    {
                        Id = item.Key,
                        BooleanVal = item.Value == "1" ? true : false,
                        Desc = item.Description
                    };
                    funcCustom.SupportLanguageSettings.Add(keyVal);
                }
            }
            #endregion

            //ShipMethod
            #region ShipMethod
            //funcEnm = CodeMasterFunction.ActiveShippingMethod;
            //codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm);
            //if (codeMasterList?.Count > 0)
            //{
            //    foreach (var item in codeMasterList)
            //    {
            //        var keyVal = new CustomizationValue()
            //        {
            //            Id = item.Key,
            //            BooleanVal = item.Value == "1" ? true : false,
            //            Desc = item.Description
            //        };
            //        funcCustom.MerchantShipMethods.Add(keyVal);
            //    }
            //}
            #endregion

            //Report Running Setting
            #region Report Running Setting

            funcEnm = CodeMasterFunction.DLReportSetting;
            codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm);
            if (codeMasterList?.Count > 0)
            {
                var reptCMRefund = codeMasterList.FirstOrDefault(x => x.Key == "RefundRepoDLTime");
                if (reptCMRefund != null)
                {
                    funcCustom.CR2ReportSetting = new CustomizationValue()
                    {
                        Id = reptCMRefund.Key,
                        Value = reptCMRefund.Value,
                        Desc = reptCMRefund.Description
                    };
                }
                var reptCInvoiceDay = codeMasterList.FirstOrDefault(x => x.Key == "InvoiceRepoDLDay");
                var reptCInvoiceTm = codeMasterList.FirstOrDefault(x => x.Key == "InvoiceRepoDLTime");
                if (reptCInvoiceDay != null)
                {
                    var cr3Value = new CustomizationValue()
                    {
                        Id = reptCInvoiceDay.Key,
                        Value = reptCInvoiceDay.Value,
                        Desc = reptCInvoiceDay.Description,
                        Unit = BDMall.Resources.Label.Day,
                    };
                    funcCustom.CR3ReportSettings.Add(cr3Value);
                }
                if (reptCInvoiceTm != null)
                {
                    var cr3Value = new CustomizationValue()
                    {
                        Id = reptCInvoiceTm.Key,
                        Value = reptCInvoiceTm.Value,
                        Desc = reptCInvoiceTm.Description,
                        Unit = BDMall.Resources.Label.Hour,
                    };
                    funcCustom.CR3ReportSettings.Add(cr3Value);
                }
                var reptDCA = codeMasterList.FirstOrDefault(x => x.Key == "DCARepoDLTime");
                if (reptDCA != null)
                {
                    funcCustom.DCAReportSetting = new CustomizationValue()
                    {
                        Id = reptDCA.Key,
                        Value = reptDCA.Value,
                        Desc = reptDCA.Description
                    };
                }
                var reptISU = codeMasterList.FirstOrDefault(x => x.Key == "ISURepoDLTime");
                if (reptISU != null)
                {
                    funcCustom.ISUReportSetting = new CustomizationValue()
                    {
                        Id = reptISU.Key,
                        Value = reptISU.Value,
                        Desc = reptISU.Description
                    };
                }
            }

            #endregion

            //Session Expires // 單位（分鐘）
            #region Session Expires

            funcEnm = CodeMasterFunction.Time;
            codeMasterList = GetCodeMasters(CodeMasterModule.Setting, funcEnm);
            if (codeMasterList?.Count > 0)
            {
                foreach (var item in codeMasterList)
                {
                    if (item.Key == "MemSessionTimeout" || item.Key == "UserSessionTimeout")
                    {
                        var expireVal = new CustomizationValue()
                        {
                            Id = item.Key,
                            Value = item.Value,
                            Desc = item.Description,
                            //Unit = BDMall.Resources.Label.Second,
                        };
                        funcCustom.SessionControlSettings.Add(expireVal);
                    }
                }
            }

            codeMasterList = GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.MallSetting);
            if (codeMasterList?.Count > 0)
            {
                foreach (var item in codeMasterList)
                {
                    if (item.Key == "MaxClinetOLQty")
                    {
                        var expireVal = new CustomizationValue()
                        {
                            Id = item.Key,
                            Value = item.Value,
                            Desc = item.Description
                        };
                        funcCustom.SessionControlSettings.Add(expireVal);
                    }
                }
            }
            #endregion

            //Grace Peroid
            #region Grace Peroid 冷靜期 單位（日）
            funcEnm = CodeMasterFunction.GracePeriod;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "OrderComepleted");
            if (codeMaster != null)
            {
                funcCustom.GracePeroidSetting = new CustomizationValue()
                {
                    Id = codeMaster.Key,
                    Value = codeMaster.Value,
                    Desc = codeMaster.Description
                };
            }
            #endregion

            //Hold Product Cancel
            #region Hold貨取消 單位（分鐘）
            funcEnm = CodeMasterFunction.Order;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "ShopcartTimeout");
            if (codeMaster != null)
            {
                funcCustom.ShopCartTimeOutSetting = new CustomizationValue()
                {
                    Id = codeMaster.Key,
                    Value = codeMaster.Value,
                    Desc = codeMaster.Description
                };
            }
            #endregion
            //Order No Pay Cancel 
            #region 購物車貨物過期 單位（分鐘）
            funcEnm = CodeMasterFunction.Order;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "OrderPayTimeout");
            if (codeMaster != null)
            {
                funcCustom.PayTimeOutSetting = new CustomizationValue()
                {
                    Id = codeMaster.Key,
                    Value = codeMaster.Value,
                    Desc = codeMaster.Description
                };
            }
            #endregion
            //GS1Setting
            #region GS1執行時間 單位（小時）
            funcEnm = CodeMasterFunction.GS1;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "RunHour");
            if (codeMaster != null)
            {
                funcCustom.GS1RunHourSetting = new CustomizationValue()
                {
                    Id = codeMaster.Key,
                    Value = codeMaster.Value,
                    Desc = codeMaster.Description
                };
            }
            #endregion

            //維護模式標識
            #region 維護模式標識

            funcEnm = CodeMasterFunction.Common;
            codeMaster = GetCodeMaster(CodeMasterModule.Setting, funcEnm, "MaintainSwitch");
            if (codeMaster != null)
            {
                funcCustom.MaintainModeSwitch = codeMaster.Value.ToUpper() == "TRUE" ? true : false;
            }

            #endregion

            return funcCustom;
        }

        /// <summary>
        /// 更新系統定制化功能
        /// </summary>
        /// <param name="funcCustom">系統定制功能清單</param>
        public SystemResult UpdateSysCustomization(SysCustomization funcCustom)
        {
            SystemResult sysRslt = new SystemResult();
            if (funcCustom != null)
            {
                UnitOfWork.IsUnitSubmit = true;

                //CouponSwitch
                #region CouponSwitch
                var codeMaster = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.CouponSetting.ToString()
                && x.Key == "CouponSwitch"
                && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    codeMaster.Value = funcCustom.CouponSwitch ? "true" : "false";
                    baseRepository.Update(codeMaster);
                }
                #endregion

                //MailServerSSL
                #region MailServerSSL
                codeMaster = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.Email.ToString()
                && x.Key == "MailServerSSL"
                && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    codeMaster.Value = funcCustom.MailServerSSLSwitch ? "1" : "0";
                    baseRepository.Update(codeMaster);
                }
                #endregion

                //Email
                #region Email
                if (funcCustom.EmailSettings?.Count > 0)
                {
                    var emailSettings = UnitOfWork.DataContext.CodeMasters.Where(x => x.Module == CodeMasterModule.Setting.ToString()
                    && x.Function == CodeMasterFunction.Email.ToString()
                    && x.Key != "MailServerSSL"
                    && x.IsActive && !x.IsDeleted).ToList();

                    if (emailSettings?.Count > 0)
                    {
                        foreach (var item in funcCustom.EmailSettings)
                        {
                            var emailSetting = emailSettings.FirstOrDefault(x => x.Key == item.Id);
                            emailSetting.Value = item.Value;
                        }
                        baseRepository.Update(emailSettings);
                    }
                }
                #endregion

                //DefaultLanguage
                #region DefaultLanguage
                var defaultLanguage = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.Language.ToString()
                && x.Key == "DefaultLanguage"
                && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    defaultLanguage.Value = funcCustom.DefaultLanguage;
                    baseRepository.Update(defaultLanguage);
                }
                #endregion

                //OrderNoSetting
                #region OrderNoSetting
                var orderNoSettings = UnitOfWork.DataContext.CodeMasters.Where(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.OrderNoSetting.ToString()
                && x.IsActive && !x.IsDeleted).ToList();

                var orderNoAutoGenSwitch = orderNoSettings.FirstOrDefault(x => x.Key == "IsAuto");
                if (orderNoAutoGenSwitch != null)
                {
                    orderNoAutoGenSwitch.Value = funcCustom.OrderNoAutoGenSwitch ? "1" : "0";
                    baseRepository.Update(orderNoAutoGenSwitch);
                }
                if (funcCustom.OrderNoAutoGenSwitch && funcCustom.OrderNoAutoGenSettings?.Count > 0)
                {
                    foreach (var item in funcCustom.OrderNoAutoGenSettings)
                    {
                        var orderNoSetting = orderNoSettings.FirstOrDefault(x => x.Key == item.Id);
                        orderNoSetting.Value = item.Value;
                    }
                    baseRepository.Update(orderNoSettings);
                }
                #endregion

                //SupportLanguage
                #region SupportLanguage
                if (funcCustom.SupportLanguageSettings?.Count > 0)
                {
                    var supportLanguageSettings = UnitOfWork.DataContext.CodeMasters.Where(x => x.Module == CodeMasterModule.Setting.ToString()
                    && x.Function == CodeMasterFunction.SupportLanguage.ToString()
                    && x.IsActive && !x.IsDeleted).ToList();

                    if (supportLanguageSettings?.Count > 0)
                    {
                        bool exsitEnableLang = funcCustom.SupportLanguageSettings.Exists(x => x.BooleanVal);
                        if (exsitEnableLang)
                        {
                            foreach (var item in funcCustom.SupportLanguageSettings)
                            {
                                var supportLanguageSetting = supportLanguageSettings.FirstOrDefault(x => x.Key == item.Id);
                                supportLanguageSetting.Value = item.BooleanVal ? "1" : "0";
                            }
                            baseRepository.Update(supportLanguageSettings);
                        }
                        else
                        {
                            throw new BLException(BDMall.Resources.Message.EnableSysLangRequire);
                        }
                    }
                }
                #endregion

                //ShipMethod
                #region ShipMethod
                //if (funcCustom.MerchantShipMethods?.Count > 0)
                //{
                //    var shipmethodSettings = _codeMasterRepo.Entities.Where(x => x.Module == CodeMasterModule.Setting.ToString()
                //    && x.Function == CodeMasterFunction.ActiveShippingMethod.ToString()
                //    && x.IsActive && !x.IsDeleted && x.ClientId == CurrentUser.ClientId).ToList();

                //    if (shipmethodSettings?.Count > 0)
                //    {

                //        foreach (var item in funcCustom.MerchantShipMethods)
                //        {
                //            var shipmethodSetting = shipmethodSettings.FirstOrDefault(x => x.Key == item.Id);
                //            shipmethodSetting.Value = item.BooleanVal ? "1" : "0";
                //        }

                //        _codeMasterRepo.Update(shipmethodSettings);

                //    }
                //}
                #endregion

                //Report Running Setting
                #region Report Running Setting

                if (funcCustom.CR2ReportSetting != null)
                {
                    var cr2CM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                    && x.Function == CodeMasterFunction.DLReportSetting.ToString()
                    && x.Key == funcCustom.CR2ReportSetting.Id
                    && x.IsActive && !x.IsDeleted);
                    if (cr2CM != null)
                    {
                        cr2CM.Value = funcCustom.CR2ReportSetting.Value;
                        baseRepository.Update(cr2CM);
                    }
                }

                if (funcCustom.CR3ReportSettings?.Count > 1)
                {
                    foreach (var cr3Setting in funcCustom.CR3ReportSettings)
                    {
                        var cr3CM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                            && x.Function == CodeMasterFunction.DLReportSetting.ToString()
                            && x.Key == cr3Setting.Id
                            && x.IsActive && !x.IsDeleted);
                        if (cr3CM != null)
                        {
                            cr3CM.Value = cr3Setting.Value;
                            baseRepository.Update(cr3CM);
                        }
                    }
                }

                if (funcCustom.DCAReportSetting != null)
                {
                    var dcaCM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                    && x.Function == CodeMasterFunction.DLReportSetting.ToString()
                    && x.Key == funcCustom.DCAReportSetting.Id
                    && x.IsActive && !x.IsDeleted);
                    if (dcaCM != null)
                    {
                        dcaCM.Value = funcCustom.DCAReportSetting.Value;
                        baseRepository.Update(dcaCM);
                    }
                }

                if (funcCustom.ISUReportSetting != null)
                {
                    var isuCM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                    && x.Function == CodeMasterFunction.DLReportSetting.ToString()
                    && x.Key == funcCustom.ISUReportSetting.Id
                    && x.IsActive && !x.IsDeleted);
                    if (isuCM != null)
                    {
                        isuCM.Value = funcCustom.ISUReportSetting.Value;
                        baseRepository.Update(isuCM);
                    }
                }

                #endregion

                //Session Expires // 單位（分鐘）
                #region Session Expires // 單位（分鐘）

                if (funcCustom.SessionControlSettings?.Count > 1)
                {
                    foreach (var tokenSetting in funcCustom.SessionControlSettings)
                    {
                        var sessionCM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                            && x.Function == CodeMasterFunction.Time.ToString()
                            && x.Key == tokenSetting.Id
                            && x.IsActive && !x.IsDeleted);
                        if (sessionCM != null)
                        {
                            sessionCM.Value = tokenSetting.Value;
                            baseRepository.Update(sessionCM);
                        }
                        else
                        {
                            //最大用户数
                            sessionCM = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                            && x.Function == CodeMasterFunction.MallSetting.ToString()
                            && x.Key == tokenSetting.Id
                            && x.IsActive && !x.IsDeleted);
                            if (sessionCM != null)
                            {
                                sessionCM.Value = tokenSetting.Value;
                                baseRepository.Update(sessionCM);
                            }
                        }
                    }
                }

                #endregion

                //Grace Peroid
                #region Grace Peroid 冷靜期 單位（日）
                var gracePeriod = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
               && x.Function == CodeMasterFunction.GracePeriod.ToString()
               && x.Key == "OrderComepleted"
               && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    gracePeriod.Value = funcCustom.GracePeroidSetting.Value.ToString();
                    baseRepository.Update(gracePeriod);
                }
                #endregion


                //Hold Product Cancel
                #region Hold貨取消 單位（分鐘）
                var hold = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
               && x.Function == CodeMasterFunction.Order.ToString()
               && x.Key == "ShopcartTimeout"
               && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    hold.Value = funcCustom.ShopCartTimeOutSetting.Value.ToString();
                    baseRepository.Update(hold);
                }
                #endregion
                //Order No Pay Cancel 
                #region 購物車貨物過期 單位（分鐘）
                var pay = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.Order.ToString()
                && x.Key == "OrderPayTimeout"
                && x.IsActive && !x.IsDeleted);

                if (codeMaster != null)
                {
                    pay.Value = funcCustom.PayTimeOutSetting.Value.ToString();
                    baseRepository.Update(pay);
                }
                #endregion

                //GS1 RunHour Setting
                #region GS1運行時間 單位（分鐘）
                var gs1 = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.GS1.ToString()
                && x.Key == "RunHour"
                && x.IsActive && !x.IsDeleted);

                if (gs1 != null)
                {
                    gs1.Value = funcCustom.GS1RunHourSetting.Value.ToString();
                    baseRepository.Update(gs1);
                }
                #endregion

                //維護模式標識
                #region 維護模式標識

                var sosMaintain = UnitOfWork.DataContext.CodeMasters.FirstOrDefault(x => x.Module == CodeMasterModule.Setting.ToString()
                && x.Function == CodeMasterFunction.Common.ToString()
                && x.Key == "MaintainSwitch"
                && x.IsActive && !x.IsDeleted);

                if (sosMaintain != null)
                {
                    string sosMaintainVal = funcCustom.MaintainModeSwitch ? "true" : "false";
                    sosMaintain.Value = sosMaintainVal;
                    baseRepository.Update(sosMaintain);
                }

                #endregion

                UnitOfWork.Submit();
                sysRslt.Succeeded = true;

                //UpdateCacheDependencyFile(new SystemCodeMaster(), CurrentUser.ClientId.ToString());
                //UpdateCacheDependencyFile(new Translations(), CurrentUser.ClientId.ToString());
            }
            else
            {
                throw new BLException("傳入的數據為空");
            }
            return sysRslt;
        }


        #endregion
    }
}
