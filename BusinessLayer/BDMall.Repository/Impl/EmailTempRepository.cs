using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class EmailTempRepository : PublicBaseRepository, IEmailTempRepository
    {

        public EmailTempRepository(IServiceProvider service) : base(service)
        {

        }

        public List<EmailTempItemDto> GetEmailTypeTempItems(MailType type)
        {
            var emailTypeItems = (from d in baseRepository.GetList<EmailTypeTempItem>()
                                  join e in baseRepository.GetList<EmailTempItem>() on d.ItemId equals e.Id
                                  select new EmailTempItem
                                  {
                                      Propertity = e.Propertity,
                                      Id = d.ItemId,
                                      PlaceHolder = e.PlaceHolder,
                                      ObjectType = e.ObjectType
                                  }).ToList();
            var dtos = AutoMapperExt.MapToList<EmailTempItem, EmailTempItemDto>(emailTypeItems);

            return dtos;
        }

        private string GetEmailTypeDesc(MailType mailType)
        {

            var query = from c in baseRepository.GetList<CodeMaster>()
                        join cn in baseRepository.GetList<Translation>() on c.DescTransId equals cn.TransId into cnTemp
                        from cnt in cnTemp
                        where c.Key == mailType.ToString()
                        && c.Module == CodeMasterModule.System.ToString() && c.Function == CodeMasterFunction.EmailType.ToString()
                        && c.IsActive && !c.IsDeleted
                        && ((cnt == null) || (cnt != null && cnt.Lang == CurrentUser.Lang && cnt.IsActive && !cnt.IsDeleted))
                        select cnt;
            var emailType = query.FirstOrDefault();
            if (emailType != null)
            {
                return emailType.Value;
            }
            return string.Empty;
        }

        public List<EmailTemplateDto> Search(EmailTempCondition cond)
        {

            var query = from d in baseRepository.GetList<EmailTemplate>()
                        where d.Id != Guid.Empty
                        select d;

            if (cond.IsActive.HasValue)
            {
                query = query.Where(d => d.IsActive == cond.IsActive.Value);
            }

            if (cond.IsDeleted.HasValue)
            {
                query = query.Where(d => d.IsDeleted == cond.IsDeleted.Value);
            }

            if (!string.IsNullOrEmpty(cond.Name))
            {
                query = query.Where(d => d.Name.Contains(cond.Name));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                query = query.Where(d => d.Code.Contains(cond.Code));
            }
            if (cond.Language.HasValue && (int)cond.Language != -1)
            {
                query = query.Where(d => d.Lang == cond.Language);
            }
            if (cond.Type.HasValue && ((int)cond.Type) != -1)
            {
                query = query.Where(x => x.EmailType == cond.Type.Value);
            }
            var data = query.ToList();
            var langs = GetSupportLanguage();
            var dtos = AutoMapperExt.MapToList<EmailTemplate, EmailTemplateDto>(data);

            foreach (var item in dtos)
            {
                item.LangText = langs.SingleOrDefault(d => d.Code == item.Lang.ToString())?.Text;
                item.EmailTypeDesc = GetEmailTypeDesc(item.EmailType);
            }
            return dtos;
        }

        public List<EmailTemplateDto> GetTemplateByMailType(MailType type)
        {
            var query = baseRepository.GetList<EmailTemplate>().Where(p => p.IsActive && !p.IsDeleted && p.EmailType == type).ToList();
            var dtos = AutoMapperExt.MapToList<EmailTemplate, EmailTemplateDto>(query);
            return dtos;
        }
    }
}
