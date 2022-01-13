using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IEmailTypeTempItemRepository : IDependency
    {
        List<EmailTempItemDto> FindTempItemByEmailType(MailType type);
    }
}
