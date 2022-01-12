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

    public interface IEmailTempItemBLL : IDependency
    {
        /// <summary>
        /// 新增郵件模板關鍵字
        /// </summary>
        /// <param name="model">關鍵字對象</param>
        /// <returns></returns>
        SystemResult AddTempItem(EmailTempItemDto model);

        /// <summary>
        /// 更新郵件模板關鍵字
        /// </summary>
        /// <param name="model">模板内容选项</param>
        /// <returns></returns>
        SystemResult UpdateTempItem(EmailTempItemDto model);

        /// <summary>
        /// 獲取郵件模板内容选项
        /// </summary>
        /// <param name="itemId">内容选项Id</param>
        /// <returns></returns>
        EmailTempItemDto GetTempItem(Guid itemId);
        /// <summary>
        /// 刪除郵件模板内容选项
        /// </summary>
        /// <param name="id">内容选项Id</param>
        /// <returns></returns>
        bool DeleteTempItem(Guid id);

        SystemResult DeleteTempItem(List<Guid> ids);

        /// <summary>
        /// 查找郵件模板關鍵字
        /// 注意：不包含已逻辑删除的模板關鍵字
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        List<EmailTempItemDto> FindTempItem(EmailTempItemCondition cond);
    }
}
