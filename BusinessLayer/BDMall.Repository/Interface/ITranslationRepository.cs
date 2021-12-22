using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface ITranslationRepository:IDependency
    {
        /// <summary>
        /// 保存多语言列表
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Guid InsertMutiLanguage(List<MutiLanguage> items, TranslationType type);

        /// <summary>
        /// 更新多语言列表
        /// </summary>
        /// <param name="transId">多语言ID</param>
      //  /// <param name="module">所属模块</param>
        ///   <param name="items">多语言项目</param>
        /// <returns></returns>
        Guid UpdateMutiLanguage(Guid transId, List<MutiLanguage> items, TranslationType type);

        /// <summary>
        /// 查询多语言列表
        /// </summary>
        /// <param name="transId">多语言ID</param>
        /// <returns></returns>
        List<MutiLanguage> GetMutiLanguage(Guid transId);


        /// <summary>
        /// 查询多语言列表
        /// </summary>
        /// <param name="transId"></param>
        /// <returns></returns>
        List<Translation> GetTranslation(Guid transId);

        Translation GetTranslation(Guid transId, Language lang);

        /// <summary>
        /// 根据多语言id删除
        /// 物理删除
        /// </summary>
        /// <param name="transId"></param>
        void DeleteByTransId(Guid transId);

        string GetDescForLang(Guid transId, Language lang);

        Task<Translation> GetTranslationAsync(Guid transId, Language lang);

        List<Translation> GenTranslations(List<MutiLanguage> items, TranslationType type, Guid transId);
    }
}
