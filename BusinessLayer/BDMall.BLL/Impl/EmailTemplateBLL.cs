using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
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
        public EmailTemplateBLL(IServiceProvider services) : base(services)
        {
            settingBLL = Services.Resolve<ISettingBLL>();
            _translationRepo = Services.Resolve<ITranslationRepository>();
        }
        //public SystemResult AddTempItem(EmailTempItemDto model)
        //{
        //    SystemResult result = new SystemResult();

        //    UnitOfWork.IsUnitSubmit = true;
        //    EmailTempItem dbModel = new EmailTempItem();
        //    dbModel = AutoMapperExt.MapTo<EmailTempItem>(model);
        //    dbModel.Id = Guid.NewGuid();
        //    dbModel.DescId = _translationRepo.InsertMutiLanguage(model.Descriptions, TranslationType.EmailTempItem);
        //    dbModel.IsActive = true;
        //    dbModel.IsDeleted = false;
        //    dbModel.CreateDate = DateTime.Now;
        //    dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
        //    baseRepository.Insert(model);

        //    UnitOfWork.Submit();
        //    result.Succeeded = true;
        //    return result;
        //}

        ///// <summary>
        ///// 檢查郵件模板內容的數字是否有超過限制數值
        ///// </summary>
        ///// <param name="model">郵件模板內容</param>
        //private SystemResult ContentWordQtyCheck(EmailTemplateDto model)
        //{
        //    var sysRslt = new SystemResult();
        //    sysRslt.Succeeded = true;

        //    if (model != null)
        //    {
        //        string overFlowFmt = Resources.Message.DataLengthOverFlow;
        //        int wordQtyLimited = Runtime.Setting.UnlimitedContentWordQty;

        //        if (model.EmailContent?.Length > 0)
        //        {
        //            if (model.EmailContent.Length > wordQtyLimited)
        //            {
        //                sysRslt.Message = string.Format(overFlowFmt, BDMall.Resources.Label.EmailContent, wordQtyLimited.ToString());
        //                sysRslt.Succeeded = false;
        //                return sysRslt;
        //            }
        //        }
        //    }

        //    return sysRslt;
        //}

        //public SystemResult AddTemplate(EmailTemplateDto model)
        //{
        //    SystemResult result = new SystemResult();

        //    var wordQtyChk = ContentWordQtyCheck(model);
        //    if (!wordQtyChk.Succeeded)
        //    {
        //        throw new Exception(wordQtyChk.Message);
        //    }
        //    var dbModel = AutoMapperExt.MapTo<EmailTemplate>(model);
        //    dbModel.Id = Guid.NewGuid();
        //    dbModel.IsActive = true;
        //    dbModel.IsDeleted = false;
        //    dbModel.CreateDate = DateTime.Now;
        //    dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
        //    baseRepository.Insert(dbModel);
        //    result.Succeeded = true;
        //    result.Message = BDMall.Resources.Message.SaveSuccess;

        //    return result;
        //}

        //public bool DeleteTempItem(Guid itemId)
        //{

        //    var model = baseRepository.GetList<EmailTempItem>().FirstOrDefault(p => p.Id == itemId);
        //    if (model != null)
        //    {
        //        model.IsDeleted = true;
        //        model.UpdateBy = Guid.Parse(CurrentUser.UserId);
        //        model.UpdateDate = DateTime.Now;
        //        baseRepository.Update(model);

        //        //ClearTempateItemsCache(itemId);
        //        return true;
        //    }
        //    else
        //    {
        //        throw new BLException("This Item is not found.");
        //    }

        //}

        //public SystemResult DeleteTemplate(Guid id)
        //{
        //    SystemResult result = new SystemResult();

        //    var model = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == id);// _emailTempRepo.GetByKey(id);
        //    if (model != null)
        //    {
        //        model.IsDeleted = true;
        //        model.UpdateBy = Guid.Parse(CurrentUser.UserId);
        //        model.UpdateDate = DateTime.Now;
        //        baseRepository.Update(model);


        //        result.Succeeded = true;
        //    }
        //    else
        //    {
        //        throw new BLException("This template is not found.");
        //    }
        //    return result;
        //}

        //public SystemResult EnableTemplate(Guid id)
        //{
        //    SystemResult result = new SystemResult();

        //    var model = baseRepository.GetList<EmailTemplate>().FirstOrDefault(p => p.Id == id);// _emailTempRepo.GetByKey(id);
        //    if (model != null)
        //    {
        //        model.IsActive = true;
        //        model.UpdateBy = Guid.Parse(CurrentUser.UserId);
        //        model.UpdateDate = DateTime.Now;
        //        baseRepository.Update(model);
        //        result.Succeeded = true;
        //    }
        //    else
        //    {
        //        throw new BLException("This template is not found.");
        //    }

        //    return result;
        //}

        //public List<EmailTemplateDto> FindTemplate(EmailTempCondition cond)
        //{
        //    if (cond == null)
        //    {
        //        throw new ArgumentNullException("cond");
        //    }
        //    try
        //    {
        //        cond.IsDeleted = false;
        //        return _emailTempRepo.Search(cond);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BLException(ex.Message);
        //    }
        //}

        //public List<EmailTemplateDto> FindTempItem(EmailTempItemCondition cond)
        //{
        //    if (cond == null)
        //    {
        //        throw new ArgumentNullException("cond");
        //    }
        //    try
        //    {
        //        cond.IsDeleted = false;
        //        return _emailTempItemRepo.Search(cond);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BLException(ex.Message);
        //    }
        //}

        //public EmailTemplateDto GetTempItem(Guid itemId)
        //{
        //    EmailTemplateDto EmailTemplateDto = null;

        //    try
        //    {
        //        var langs = _codeMasterRepo.GetSupportLanguage();

        //        if (itemId == Guid.Empty)
        //        {
        //            EmailTemplateDto = new EmailTemplateDto();
        //            EmailTemplateDto.Id = itemId;
        //            EmailTemplateDto.PlaceHolder = "";
        //            EmailTemplateDto.Propertity = "";
        //            EmailTemplateDto.ObjectType = "";
        //            EmailTemplateDto.DescId = new Guid();
        //            EmailTemplateDto.Description = "";
        //            EmailTemplateDto.Descriptions = LangUtil.GetMutiLangFromTranslation(new List<Translation>(), langs);

        //            EmailTemplateDto.Remark = "";
        //        }
        //        else
        //        {
        //            EmailTemplateDto = _emailTempItemRepo.GetByKey(itemId);

        //            if (EmailTemplateDto != null)
        //            {
        //                var translates = _translationRepo.GetTranslation(EmailTemplateDto.DescId).ToList();
        //                EmailTemplateDto.Descriptions = LangUtil.GetMutiLangFromTranslation(translates, langs);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SaveError(this.GetType().FullName, ClassUtility.GetMethodName(), "", ex.Message);
        //        //throw new BLException("");
        //        throw ex;
        //    }
        //    return EmailTemplateDto;
        //}

        //public EmailTypeItemsView GetEmailTypeItem(MailType emailType)
        //{
        //    try
        //    {
        //        string cacheKey = emailType.ToString() + "_items";
        //        var emailTypeItems = CacheManager.Get<List<EmailTemplateDto>>(cacheKey);
        //        if (emailTypeItems == null)
        //        {
        //            emailTypeItems = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);
        //            foreach (var item in emailTypeItems)
        //            {
        //                item.Descriptions = _translationRepo.GetMutiLanguage(item.DescId);
        //                item.Description = item.Descriptions.FirstOrDefault(p => p.Language == ReturnDataLanguage)?.Desc ?? "";
        //            }
        //            CacheManager.Insert(cacheKey, emailTypeItems);
        //        }
        //        EmailTypeItemsView view = new EmailTypeItemsView();
        //        view.EmailType = emailType;
        //        view.EmailTypeId = emailType.ToString();
        //        view.EmailItems = emailTypeItems.Select(d => d.Id).ToList();

        //        var cm = _codeMasterRepo.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.EmailType.ToString(), emailType.ToString());
        //        view.Description = cm.Description;
        //        return view;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CreateUnknownException(ex);
        //    }
        //}

        //public List<EmailTemplateDto> GetEmailTempItem(MailType emailType)
        //{
        //    try
        //    {
        //        var cacheKey = emailType.ToString() + "_items";
        //        List<EmailTemplateDto> items = CacheManager.Get<List<EmailTemplateDto>>(cacheKey);

        //        if (items != null)
        //        {
        //            return items;
        //        }

        //        items = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);

        //        foreach (var item in items)
        //        {
        //            item.Descriptions = _translationRepo.GetMutiLanguage(item.DescId);
        //            item.Description = item.Descriptions.FirstOrDefault(p => p.Language == ReturnDataLanguage)?.Desc ?? "";
        //        }

        //        CacheManager.Insert(cacheKey, items);
        //        return items;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CreateUnknownException(ex);
        //    }
        //}
        //public List<EmailTemplateDto> GetEmailTypeTempItems(MailType emailType)
        //{
        //    try
        //    {
        //        var cacheKey = emailType.ToString() + "_items";
        //        List<EmailTemplateDto> items = CacheManager.Get<List<EmailTemplateDto>>(cacheKey);

        //        if (items != null)
        //        {
        //            return items;
        //        }
        //        items = _emailTypeTempItemRepo.FindTempItemByEmailType(emailType);
        //        CacheManager.Insert(cacheKey, items);
        //        return items;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CreateUnknownException(ex);
        //    }
        //}
        //public List<EmailTemplateDto> GetEmailTempItems()
        //{
        //    try
        //    {
        //        return _emailTempItemRepo.Search(new EmailTempItemCondition());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw CreateUnknownException(ex);
        //    }
        //}

        //public EmailTemplate GetTemplate(Guid id)
        //{

        //    try
        //    {
        //        var cacheKey = "EmailTemplate_" + id.ToString();
        //        EmailTemplate model = CacheManager.Get<EmailTemplate>(cacheKey);

        //        if (model != null)
        //        {
        //            return model;
        //        }

        //        model = _emailTempRepo.GetByKey(id);
        //        if (model != null)
        //        {

        //            model.LangText = Enum.GetName(typeof(Language), model.Lang);
        //            CacheManager.Insert(cacheKey, model);
        //            return model;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        throw new BLException(GenDbEntityValidationMessage(dbex));
        //    }
        //    catch (BLException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }
        //}

        //public EmailTemplate GetActiveTemplate(MailType mailType, Language lang, NoticeType noticeType)
        //{

        //    try
        //    {
        //        string cacheKey = mailType.ToString() + lang.ToString();
        //        var model = CacheManager.Get<EmailTemplate>(cacheKey);


        //        if (model != null)
        //        {
        //            _emailTempRepo.CacheLog.Debug("Exist " + cacheKey);
        //            return model;
        //        }
        //        else
        //        {
        //            _emailTempRepo.CacheLog.Debug("Empty " + cacheKey);
        //        }
        //        model = _emailTempRepo.Entities.Where(d => d.EmailType == mailType && d.Lang == lang && !d.IsDeleted && d.IsActive).FirstOrDefault();
        //        if (model != null)
        //        {
        //            if (noticeType == NoticeType.Email)
        //            {
        //                model.EmailContent = model.EmailContent;
        //            }
        //            else if (noticeType == NoticeType.InteractMessage)
        //            {
        //                model.MessageContent = model.MessageContent;
        //            }
        //            CacheManager.Insert(cacheKey, model);
        //            return model;
        //        }
        //        else
        //        {
        //            throw new BLException("This template is not found.");
        //        }
        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        throw new BLException(GenDbEntityValidationMessage(dbex));
        //    }
        //    catch (BLException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }

        //}

        //public SystemResult SaveEmailTypeItems(EmailTypeItemsView entity)
        //{
        //    SystemResult result = new SystemResult();
        //    try
        //    {

        //        UnitOfWork.IsUnitSubmit = true;
        //        var olds = _emailTypeTempItemRepo.Entities.Where(d => d.ClientId == CurrentUser.ClientId && d.MailType == entity.EmailType);
        //        foreach (var item in olds)
        //        {
        //            item.IsDeleted = true;
        //            _emailTypeTempItemRepo.Update(item);
        //        }
        //        //_emailTypeTempItemRepo.Delete(olds);

        //        List<EmailTypeTempItem> data = new List<EmailTypeTempItem>();
        //        foreach (var item in entity.EmailItems)
        //        {
        //            EmailTypeTempItem typeItem = new EmailTypeTempItem();
        //            typeItem.Id = Guid.NewGuid();
        //            typeItem.MailType = entity.EmailType;
        //            typeItem.ItemId = item;
        //            data.Add(typeItem);
        //        }
        //        _emailTypeTempItemRepo.Insert(data);
        //        UnitOfWork.Submit();
        //        result.Succeeded = true;
        //        //var emailTypeItems = _emailTypeTempItemRepo.FindTempItemByEmailType(entity.EmailType);
        //        string cacheKey = entity.EmailType.ToString() + "_items";
        //        //CacheManager.Insert(cacheKey, emailTypeItems);
        //        CacheManager.NoticUpdate(cacheKey);
        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        result.Message = GenDbEntityValidationMessage(dbex);

        //    }
        //    catch (DbUpdateException dbue)
        //    {
        //        // result.Message = BDMall.Resources.Message.UpdateFail;
        //        result.Message = dbue.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }
        //    return result;




        //}

        //public SystemResult UpdateTempItem(EmailTemplateDto temp)
        //{
        //    SystemResult result = new SystemResult();
        //    try
        //    {

        //        UnitOfWork.IsUnitSubmit = true;
        //        temp.PlaceHolder = temp.PlaceHolder?.Trim() ?? string.Empty;
        //        temp.ObjectType = temp.ObjectType?.Trim() ?? string.Empty;
        //        temp.Propertity = temp.Propertity?.Trim() ?? string.Empty;
        //        _emailTempItemRepo.Update(temp);
        //        if (temp.DescId == Guid.Empty)
        //        {
        //            temp.DescId = Guid.NewGuid();
        //            _emailTempItemRepo.Update(temp);
        //        }
        //        var trans = _translationRepo.GetTranslation(temp.DescId);
        //        if (temp.Descriptions != null)
        //        {
        //            foreach (var item in temp.Descriptions)
        //            {
        //                var oldDesc = trans.FirstOrDefault(d => d.Lang == item.Language);
        //                if (oldDesc != null)
        //                {
        //                    oldDesc.Value = item.Desc ?? string.Empty;
        //                    _translationRepo.Update(oldDesc);
        //                }
        //                else
        //                {
        //                    var newDesc = new Translation();
        //                    newDesc.Id = Guid.NewGuid();
        //                    newDesc.TransId = temp.DescId;
        //                    newDesc.Value = item.Desc ?? string.Empty;
        //                    newDesc.Lang = item.Language;
        //                    _translationRepo.Insert(newDesc);
        //                }
        //            }
        //        }

        //        UnitOfWork.Submit();

        //        ClearTempateItemsCache(temp.Id);

        //        result.Succeeded = true;

        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        result.Message = GenDbEntityValidationMessage(dbex);

        //    }
        //    catch (DbUpdateException dbue)
        //    {
        //        // result.Message = BDMall.Resources.Message.UpdateFail;
        //        result.Message = dbue.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 清除邮件選项对应邮件类型的缓存
        ///// </summary>
        ///// <param name="tempItemId"></param>
        //private void ClearTempateItemsCache(Guid tempItemId)
        //{
        //    //清缓存
        //    var mts = _emailTypeTempItemRepo.Entities.Where(d => d.ItemId == tempItemId).Select(d => d.MailType);
        //    foreach (var item in mts)
        //    {
        //        CacheManager.Remove(item + "_items");
        //    }
        //}

        //public SystemResult UpdateTemplate(EmailTemplate model)
        //{
        //    SystemResult result = new SystemResult();
        //    try
        //    {
        //        var cacheKey = "EmailTemplate_" + model.Id.ToString();

        //        _emailTempRepo.Update(model);

        //        CacheManager.NoticUpdate(cacheKey);//根據id cache
        //        CacheManager.Insert(cacheKey, model);

        //        var key2 = model.EmailType.ToString() + model.Lang.ToString();//根據郵件類型和語言cache
        //        CacheManager.NoticUpdate(key2);
        //        CacheManager.Insert(key2, model);

        //        result.Succeeded = true;
        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        result.Message = GenDbEntityValidationMessage(dbex);

        //    }
        //    catch (DbUpdateException dbue)
        //    {
        //        // result.Message = BDMall.Resources.Message.UpdateFail;
        //        result.Message = dbue.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }
        //    return result;
        //}

        //public SystemResult DeleteTempItem(List<Guid> ids)
        //{
        //    SystemResult result = new SystemResult();
        //    try
        //    {
        //        UnitOfWork.IsUnitSubmit = true;
        //        var models = _emailTempItemRepo.Entities.Where(d => ids.Contains(d.Id)).ToList();
        //        if (models != null && models.Count() > 0)
        //        {

        //            foreach (var item in models)
        //            {
        //                item.IsDeleted = true;
        //            }
        //            _emailTempItemRepo.Update(models);
        //            int rows = UnitOfWork.Submit();
        //            result.Succeeded = true;


        //        }
        //        else
        //        {
        //            throw new BLException("The template item list is not found.");
        //        }
        //        return result;
        //    }
        //    catch (DbEntityValidationException dbex)
        //    {
        //        throw new BLException(GenDbEntityValidationMessage(dbex));
        //    }
        //    catch (Exception ex)
        //    {

        //        throw CreateUnknownException(ex);
        //    }
        //}

        //public EmailTemplate GetNoticeTemp(MailType mailType, Language mailLang, NoticeType noticeType)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
