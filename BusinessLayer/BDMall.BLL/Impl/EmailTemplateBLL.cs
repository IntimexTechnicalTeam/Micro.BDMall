using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class EmailTemplateBLL : BaseBLL, IEmailTemplateBLL
    {

        ISettingBLL settingBLL;
        ITranslationRepository _translationRepo;
        IEmailTempRepository _emailTempRepo;
        IEmailTempItemRepository _emailTempItemRepo;
        ICodeMasterRepository _codeMasterRepo;
        IEmailTypeTempItemRepository _emailTypeTempItemRepo;

        public EmailTemplateBLL(IServiceProvider services) : base(services)
        {
            settingBLL = Services.Resolve<ISettingBLL>();
            _translationRepo = Services.Resolve<ITranslationRepository>();
            _codeMasterRepo = Services.Resolve<ICodeMasterRepository>();
            _emailTempRepo = Services.Resolve<IEmailTempRepository>();
            _emailTempItemRepo = Services.Resolve<IEmailTempItemRepository>();
            _emailTypeTempItemRepo = Services.Resolve<IEmailTypeTempItemRepository>();
        }
        public SystemResult AddTempItem(EmailTempItemDto model)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;
            EmailTempItem dbModel = new EmailTempItem();
            dbModel = AutoMapperExt.MapTo<EmailTempItem>(model);
            dbModel.Id = Guid.NewGuid();
            dbModel.DescId = _translationRepo.InsertMutiLanguage(model.Descriptions, TranslationType.EmailTempItem);
            dbModel.IsActive = true;
            dbModel.IsDeleted = false;
            dbModel.CreateDate = DateTime.Now;
            dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
            baseRepository.Insert(model);

            UnitOfWork.Submit();
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 檢查郵件模板內容的數字是否有超過限制數值
        /// </summary>
        /// <param name="model">郵件模板內容</param>
        private SystemResult ContentWordQtyCheck(EmailTemplateDto model)
        {
            var sysRslt = new SystemResult();
            sysRslt.Succeeded = true;

            if (model != null)
            {
                string overFlowFmt = Resources.Message.DataLengthOverFlow;
                int wordQtyLimited = Runtime.Setting.UnlimitedContentWordQty;

                if (model.EmailContent?.Length > 0)
                {
                    if (model.EmailContent.Length > wordQtyLimited)
                    {
                        sysRslt.Message = string.Format(overFlowFmt, BDMall.Resources.Label.EmailContent, wordQtyLimited.ToString());
                        sysRslt.Succeeded = false;
                        return sysRslt;
                    }
                }
            }

            return sysRslt;
        }

        public SystemResult AddTemplate(EmailTemplateDto model)
        {
            SystemResult result = new SystemResult();

            var wordQtyChk = ContentWordQtyCheck(model);
            if (!wordQtyChk.Succeeded)
            {
                throw new Exception(wordQtyChk.Message);
            }
            var dbModel = AutoMapperExt.MapTo<EmailTemplate>(model);
            dbModel.Id = Guid.NewGuid();
            dbModel.IsActive = true;
            dbModel.IsDeleted = false;
            dbModel.CreateDate = DateTime.Now;
            dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
            baseRepository.Insert(dbModel);
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;

            return result;
        }

        public bool DeleteTempItem(Guid itemId)
        {

            var model = baseRepository.GetList<EmailTempItem>().FirstOrDefault(p => p.Id == itemId);
            if (model != null)
            {
                model.IsDeleted = true;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);
                model.UpdateDate = DateTime.Now;
                baseRepository.Update(model);

                //ClearTempateItemsCache(itemId);
                return true;
            }
            else
            {
                throw new BLException("This Item is not found.");
            }

        }

        public SystemResult DeleteTemplate(Guid id)
        {
            SystemResult result = new SystemResult();

            var model = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == id);// _emailTempRepo.GetByKey(id);
            if (model != null)
            {
                model.IsDeleted = true;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);
                model.UpdateDate = DateTime.Now;
                baseRepository.Update(model);


                result.Succeeded = true;
            }
            else
            {
                throw new BLException("This template is not found.");
            }
            return result;
        }

        public SystemResult EnableTemplate(Guid id)
        {
            SystemResult result = new SystemResult();

            var model = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == id);// _emailTempRepo.GetByKey(id);
            if (model != null)
            {
                model.IsActive = true;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);
                model.UpdateDate = DateTime.Now;
                baseRepository.Update(model);
                result.Succeeded = true;
            }
            else
            {
                throw new BLException("This template is not found.");
            }

            return result;
        }

        public List<EmailTemplateDto> FindTemplate(EmailTempCondition cond)
        {
            if (cond == null)
            {
                throw new ArgumentNullException("cond");
            }
            try
            {
                cond.IsDeleted = false;
                return _emailTempRepo.Search(cond);
            }
            catch (Exception ex)
            {
                throw new BLException(ex.Message);
            }
        }

        public List<EmailTempItemDto> FindTempItem(EmailTempItemCondition cond)
        {
            if (cond == null)
            {
                throw new ArgumentNullException("cond");
            }
            try
            {
                cond.IsDeleted = false;
                return _emailTempItemRepo.Search(cond);
            }
            catch (Exception ex)
            {
                throw new BLException(ex.Message);
            }
        }

        public EmailTempItemDto GetTempItem(Guid itemId)
        {
            EmailTempItemDto EmailTemplateDto = null;


            var langs = GetSupportLanguage();

            if (itemId == Guid.Empty)
            {
                EmailTemplateDto = new EmailTempItemDto();
                EmailTemplateDto.Id = itemId;
                EmailTemplateDto.PlaceHolder = "";
                EmailTemplateDto.Propertity = "";
                EmailTemplateDto.ObjectType = "";
                EmailTemplateDto.DescId = new Guid();
                EmailTemplateDto.Description = "";
                EmailTemplateDto.Descriptions = LangUtil.GetMutiLangFromTranslation(new List<Translation>(), langs);

                EmailTemplateDto.Remark = "";
            }
            else
            {
                var emailTemplate = baseRepository.GetList<EmailTempItem>().FirstOrDefault(p => p.Id == itemId);


                if (emailTemplate != null)
                {
                    EmailTemplateDto = AutoMapperExt.MapTo<EmailTempItemDto>(emailTemplate);
                    var translates = _translationRepo.GetTranslation(EmailTemplateDto.DescId).ToList();
                    EmailTemplateDto.Descriptions = LangUtil.GetMutiLangFromTranslation(translates, langs);
                }
            }

            return EmailTemplateDto;
        }

        public EmailTypeItemsView GetEmailTypeItem(MailType emailType)
        {

            //string cacheKey = emailType.ToString() + "_items";
            //var emailTypeItems = CacheManager.Get<List<EmailTemplateDto>>(cacheKey);
            //if (emailTypeItems == null)
            //{
            var emailTypeItems = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);
            foreach (var item in emailTypeItems)
            {
                item.Descriptions = _translationRepo.GetMutiLanguage(item.DescId);
                item.Description = item.Descriptions.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            }
            // CacheManager.Insert(cacheKey, emailTypeItems);
            //}
            EmailTypeItemsView view = new EmailTypeItemsView();
            view.EmailType = emailType;
            view.EmailTypeId = emailType.ToString();
            view.EmailItems = emailTypeItems.Select(d => d.Id).ToList();

            var cm = _codeMasterRepo.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.EmailType.ToString(), emailType.ToString());
            view.Description = cm.Description;
            return view;

        }

        public List<EmailTempItemDto> GetEmailTempItem(MailType emailType)
        {
            var items = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);

            foreach (var item in items)
            {
                item.Descriptions = _translationRepo.GetMutiLanguage(item.DescId);
                item.Description = item.Descriptions.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            }

            return items;

        }
        public List<EmailTempItemDto> GetEmailTypeTempItems(MailType emailType)
        {
            var items = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);
            return items;
        }
        public List<EmailTempItemDto> GetEmailTempItems()
        {
            return _emailTempItemRepo.Search(new EmailTempItemCondition());
        }

        public EmailTemplateDto GetTemplate(Guid id)
        {

            var dto = new EmailTemplateDto();
            var model = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == id);
            if (model != null)
            {
                dto = AutoMapperExt.MapTo<EmailTemplateDto>(model);
                dto.LangText = Enum.GetName(typeof(Language), model.Lang);
                return dto;
            }
            else
            {
                return null;
            }

        }

        public EmailTemplateDto GetActiveTemplate(MailType mailType, Language lang, NoticeType noticeType)
        {
            var model = baseRepository.GetList<EmailTemplate>().Where(d => d.EmailType == mailType && d.Lang == lang && !d.IsDeleted && d.IsActive).FirstOrDefault();
            if (model != null)
            {
                var dto = new EmailTemplateDto();
                dto = AutoMapperExt.MapTo<EmailTemplateDto>(model);
                if (noticeType == NoticeType.Email)
                {
                    dto.EmailContent = model.EmailContent;
                }
                else if (noticeType == NoticeType.InteractMessage)
                {
                    dto.MessageContent = model.MessageContent;
                }
                return dto;
            }
            else
            {
                throw new BLException("This template is not found.");
            }
        }

        public SystemResult SaveEmailTypeItems(EmailTypeItemsView entity)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;
            var olds = baseRepository.GetList<EmailTypeTempItem>().Where(d => d.MailType == entity.EmailType);
            foreach (var item in olds)
            {
                item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;
                item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Update(item);
            }

            List<EmailTypeTempItem> data = new List<EmailTypeTempItem>();
            foreach (var item in entity.EmailItems)
            {
                EmailTypeTempItem typeItem = new EmailTypeTempItem();
                typeItem.Id = Guid.NewGuid();
                typeItem.MailType = entity.EmailType;
                typeItem.ItemId = item;
                data.Add(typeItem);
            }
            baseRepository.Insert(data);
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;

        }

        public SystemResult UpdateTempItem(EmailTempItemDto temp)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;

            var emailtempItem = baseRepository.GetList<EmailTempItem>().FirstOrDefault(p => p.Id == temp.Id);
            emailtempItem = AutoMapperExt.MapTo<EmailTempItem>(temp);
            emailtempItem.PlaceHolder = temp.PlaceHolder?.Trim() ?? string.Empty;
            emailtempItem.ObjectType = temp.ObjectType?.Trim() ?? string.Empty;
            emailtempItem.Propertity = temp.Propertity?.Trim() ?? string.Empty;
            //baseRepository.Update(temp);
            if (emailtempItem.DescId == Guid.Empty)
            {
                emailtempItem.DescId = Guid.NewGuid();
                //ba.Update(temp);
            }
            var trans = _translationRepo.GetTranslation(emailtempItem.DescId);
            if (temp.Descriptions != null)
            {
                foreach (var item in temp.Descriptions)
                {
                    var oldDesc = trans.FirstOrDefault(d => d.Lang == item.Language);
                    if (oldDesc != null)
                    {
                        oldDesc.Value = item.Desc ?? string.Empty;
                        baseRepository.Update(oldDesc);
                    }
                    else
                    {
                        var newDesc = new Translation();
                        newDesc.Id = Guid.NewGuid();
                        newDesc.TransId = temp.DescId;
                        newDesc.Value = item.Desc ?? string.Empty;
                        newDesc.Lang = item.Language;
                        baseRepository.Insert(newDesc);
                    }
                }
            }
            baseRepository.Update(emailtempItem);

            UnitOfWork.Submit();

            result.Succeeded = true;


            return result;
        }



        public SystemResult UpdateTemplate(EmailTemplateDto model)
        {
            SystemResult result = new SystemResult();
            var dbModel = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == model.Id);
            if (dbModel != null)
            {
                dbModel = AutoMapperExt.MapTo<EmailTemplate>(model);
                dbModel.UpdateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.UpdateDate = DateTime.Now;
                baseRepository.Update(dbModel);
            }
            result.Succeeded = true;
            return result;
        }

        public SystemResult DeleteTempItem(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;
            var models = baseRepository.GetList<EmailTempItem>().Where(d => ids.Contains(d.Id)).ToList();
            if (models != null && models.Count() > 0)
            {

                foreach (var item in models)
                {
                    item.IsDeleted = true;
                }
                baseRepository.Update(models);
                int rows = UnitOfWork.Submit();
                result.Succeeded = true;
            }
            else
            {
                throw new BLException("The template item list is not found.");
            }
            return result;
        }

        public EmailTemplateDto GetNoticeTemp(MailType mailType, Language mailLang, NoticeType noticeType)
        {
            throw new NotImplementedException();
        }
    }
}
