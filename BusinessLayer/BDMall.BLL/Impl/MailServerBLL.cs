using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Runtime;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class MailServerBLL : BaseBLL, IMailServerBLL
    {


        public MailServerBLL(IServiceProvider services) : base(services)
        {

        }
        public MailServerInfo GetById(Guid id)
        {

            MailServerInfo info = new MailServerInfo();
            if (id == Guid.Empty)
            {
                info.Id = Guid.Empty;
                info.MailServer = "";
                info.Port = "";
                info.IsSSL = false;
                info.Code = "";
            }
            else
            {
                var setting = UnitOfWork.DataContext.MailServers.FirstOrDefault(p => p.Id == id);
                if (setting != null)
                {
                    info.Id = setting.Id;
                    info.MailServer = setting.Server;
                    info.Port = setting.Port;
                    info.IsSSL = setting.IsSSL;
                    info.Code = setting.Code;
                }
            }
            return info;

        }

        public PageData<MailServerView> GetMailServerSettings(MailServerCond cond)
        {
            PageData<MailServerView> data = new PageData<MailServerView>();


            var query = (from m in UnitOfWork.DataContext.MailServers
                         where m.IsDeleted == false && m.IsActive == true
                         orderby m.CreateDate descending
                         select new MailServerView

                         {
                             Id = m.Id,
                             Code = m.Code,
                             IsSSL = m.IsSSL,
                             MailServer = m.Server,
                             Port = m.Port
                         });

            data.TotalRecord = query.Count();
            data.Data = query.Skip(cond.Offset).Take(cond.PageSize).ToList();
            return data;
        }


        public SystemResult SaveMailServer(MailServerInfo info)
        {
            SystemResult result = new SystemResult();

            if (info.Id == Guid.Empty)
            {
                InsertMailServer(info);
            }
            else
            {
                UpdateMailServer(info);
            }
            result.Succeeded = true;


            return result;
        }

        public SystemResult DeleteMailServer(List<string> ids)
        {
            SystemResult result = new SystemResult();

            List<MailServer> settings = new List<MailServer>();
            foreach (var item in ids)
            {
                var id = Guid.Parse(item);
                var setting = baseRepository.GetList<MailServer>().FirstOrDefault(p => p.Id == id);
                if (setting != null)
                {
                    setting.IsDeleted = true;
                    settings.Add(setting);
                }
            }
            if (settings.Count() > 0)
            {
                baseRepository.Update(settings);
            }
            result.Succeeded = true;

            return result;
        }

        public SystemResult GetMailServerByMail(string mail)
        {
            SystemResult result = new SystemResult();

            if (!string.IsNullOrEmpty(mail))
            {
                if (mail.Contains("@"))
                {
                    var codePart = mail.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries).ToList()[1];
                    var code = codePart.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];

                    var setting = UnitOfWork.DataContext.MailServers.FirstOrDefault(p => p.IsActive == true && p.IsDeleted == false && p.Code == code);

                    if (setting != null)
                    {
                        MailServerInfo info = new MailServerInfo();
                        info.Id = setting.Id;
                        info.MailServer = setting.Server;
                        info.Port = setting.Port;
                        info.IsSSL = setting.IsSSL;

                        result.ReturnValue = info;
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Message = BDMall.Resources.Message.MailServerNotExist;
                    }

                }
                else
                {
                    result.Succeeded = false;
                    result.Message = BDMall.Resources.Message.PleaseEnterCorrectEmail;
                }
            }
            else
            {
                result.Succeeded = false;
                result.Message = BDMall.Resources.Message.PleaseEnterCorrectEmail;
            }

            return result;
        }

        private void InsertMailServer(MailServerInfo info)
        {
            var setting = UnitOfWork.DataContext.MailServers.FirstOrDefault(p => p.Code == info.Code.Trim() && p.IsActive == true && p.IsDeleted == false);
            if (setting != null)
            {
                throw new BLException(BDMall.Resources.Message.RecordExist);
            }

            MailServer mail = new MailServer();
            mail.Id = Guid.NewGuid();
            mail.IsSSL = info.IsSSL;
            mail.Code = info.Code;
            mail.Server = info.MailServer;
            mail.Port = info.Port;
            baseRepository.Insert(mail);
        }

        private void UpdateMailServer(MailServerInfo info)
        {
            var setting = UnitOfWork.DataContext.MailServers.FirstOrDefault(p => p.Id == info.Id);

            if (setting != null)
            {
                setting.Server = info.MailServer;
                setting.Port = info.Port;
                setting.IsSSL = info.IsSSL;
                baseRepository.Update(setting);
            }
        }
    }
}
