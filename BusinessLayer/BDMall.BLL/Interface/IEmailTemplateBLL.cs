using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using BDMall.Enums;
using System.Net;
using Intimex.Common;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IEmailTemplateBLL : IDependency, IEmailTypeItemBLL, IEmailTempItemBLL
    {
        /// <summary>
        /// 新增郵件模板
        /// </summary>
        /// <param name="model">郵件模板對象</param>
        /// <returns></returns>
        SystemResult AddTemplate(EmailTemplateDto model);

        /// <summary>
        /// 更新郵件模板
        /// </summary>
        /// <param name="model">郵件模板對象</param>
        /// <returns></returns>
        SystemResult UpdateTemplate(EmailTemplateDto model);

        /// <summary>
        /// 獲取郵件模板
        /// </summary>
        /// <param name="id">模板Id</param>
        /// <returns></returns>
        EmailTemplateDto GetTemplate(Guid id);

        /// <summary>
        /// 根据邮件类型和语言，返回当前有效的模板
        /// </summary>
        /// <param name="mailType">邮件类别</param>
        /// <param name="lang">邮件语言</param>
        /// <returns></returns>
        EmailTemplateDto GetActiveTemplate(MailType mailType, Language lang, NoticeType noticeType);

        /// <summary>
        /// 查找郵件模板
        /// 注意：不包含已逻辑删除的模板
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        List<EmailTemplateDto> FindTemplate(EmailTempCondition cond);



        /// <summary>
        /// 刪除郵件模板
        /// </summary>
        /// <param name="id">模板Id</param>
        /// <returns></returns>
        SystemResult DeleteTemplate(Guid id);


        /// <summary>
        /// 设置邮件模板有效，
        /// 与该邮件模板相同EmailType和Language的其它模板转为无效
        /// </summary>
        /// <param name="id"></param>
        SystemResult EnableTemplate(Guid id);


        /// <summary>
        /// 获取邮件模板
        /// </summary>
        /// <param name="mailType">邮件类别</param>
        /// <param name="mailLang">邮件语言</param>
        /// <param name="noticeType">通知類別</param>
        /// <returns></returns>
        EmailTemplateDto GetNoticeTemp(MailType mailType, Language mailLang, NoticeType noticeType);
    }
}
