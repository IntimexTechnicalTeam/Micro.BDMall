using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IEmailTempRepository : IDependency
    {
        List<EmailTempItemDto> GetEmailTypeTempItems(MailType type);

        List<EmailTemplateDto> Search(EmailTempCondition cond);

        List<EmailTemplateDto> GetTemplateByMailType(MailType type);
    }
}
