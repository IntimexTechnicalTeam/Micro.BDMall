using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using BDMall.Enums;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface ICodeMasterBLL : IDependency
    {/// <summary>  
     /// 獲取指定module，function的設定
     /// Date:2017/10/31
     /// Author:Justin  
     /// </summary>
     /// <param name="module"></param>
     /// <param name="function"></param>
     /// <param name="lang"></param>
     /// <returns></returns>
     /// <remarks></remarks>
        List<CodeMaster> GetCodeMasters(CodeMasterModule module, CodeMasterFunction function);

        /// <summary>
        /// 获取字码主档的分页数据
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        PageData<CodeMaster> GetCodeMastersByPage(CodeMasterCondition cond);
        List<CodeMaster> GetCodeMasters(string module, string function, string key);
        /// <summary>
        /// 獲取指定module，function,key的設定
        /// Date:2017/11/03
        /// Author:Justin  
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        CodeMaster GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key);

        /// <summary>
        /// 根據key、value獲取字碼主檔
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        CodeMaster GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key, string value);


        /// <summary>
        /// 通過字碼主檔Id獲取字碼主檔信息
        /// </summary>
        /// <param name="id">字碼主檔Id</param>
        /// <returns></returns>
        CodeMaster GetCodeMasterById(int id);

        /// <summary>
        /// 通過key獲取字碼主檔信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        CodeMaster GetCodeMasterByKey(Guid clientId, string module, string function, string key);

        /// <summary>
        ///插入字碼主檔
        /// </summary>
        /// <param name="codeMaster"></param>
        void InsertCodeMaster(CodeMaster codeMaster);
        /// <summary>
        /// 更新字碼主檔
        /// </summary>
        /// <param name="model"></param>
        void UpdateCodeMaster(CodeMaster model);


        /// <summary>
        /// 刪除字碼主檔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteCodeMaster(int id);

        /// <summary>
        /// 实际物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ActualDelete(int id);

        /// <summary>
        /// 獲取發送郵箱設定
        /// </summary>
        /// <returns></returns>
        MailBox GetMailBox();
        /// <summary>
        /// 获取平台基本信息
        /// </summary>
        /// <returns></returns>
        List<CodeMaster> GetMallInfo();
        /// <summary>
        /// 更新平台基本信息
        /// </summary>
        /// <param name="info"></param>
        void UpdateMallInfo(List<CodeMaster> info);
        /// <summary>
        /// 获取平台基本信息
        /// </summary>
        /// <returns></returns>
        StoreInfo GetStoreInfo(Language lang);

        /// <summary>
        /// 獲取系統定制化功能清單
        /// </summary>
        SysCustomization GetSysCustomization();

        /// <summary>
        /// 更新系統定制化功能
        /// </summary>
        /// <param name="funcCustom">系統定制功能清單</param>
        SystemResult UpdateSysCustomization(SysCustomization funcCustom);

        /// <summary>
        /// 獲取系統的Logo
        /// </summary>
        /// <returns></returns>
        SystemLogo GetSystemLogos();

        /// <summary>
        /// 保存系统logo
        /// </summary>
        /// <param name="logo"></param>
        void SaveSystemLogo(SystemLogo logo);

        //PageData<IPoststationView> GetIPoststationByPage(CodeMasterCondition con);
        //IPoststationView GetIPoststation(int id);
        //SystemResult SaveIPoststation(IPoststationView view);

        //PageData<CounterCollectionView> GetCounterCollectionByPage(CodeMasterCondition con);

        //CounterCollectionView GetCounterCollection(int id);

        //SystemResult SaveCounterCollection(CounterCollectionView view);

    }
}
