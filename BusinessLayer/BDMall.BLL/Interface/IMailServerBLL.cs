using BDMall.Domain;

using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IMailServerBLL : IDependency
    {
        MailServerInfo GetById(Guid id);
        PageData<MailServerView> GetMailServerSettings(MailServerCond cond);

        SystemResult SaveMailServer(MailServerInfo info);

        SystemResult DeleteMailServer(List<string> ids);

        SystemResult GetMailServerByMail(string mail);
    }
}
