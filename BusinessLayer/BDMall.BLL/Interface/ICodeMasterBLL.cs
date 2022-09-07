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
        List<CodeMasterDto> GetCodeMasters(CodeMasterModule module, CodeMasterFunction function);

        /// <summary>
        /// 获取字码主档的分页数据
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition cond);
        List<CodeMasterDto> GetCodeMasters(string module, string function, string key);
        /// <summary>
        /// 獲取指定module，function,key的設定
        /// Date:2017/11/03
        /// Author:Justin  
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        CodeMasterDto GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key);

        /// <summary>
        /// 根據key、value獲取字碼主檔
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        CodeMasterDto GetCodeMaster(CodeMasterModule module, CodeMasterFunction function, string key, string value);


        /// <summary>
        /// 通過字碼主檔Id獲取字碼主檔信息
        /// </summary>
        /// <param name="id">字碼主檔Id</param>
        /// <returns></returns>
        CodeMasterDto GetCodeMasterById(int id);

        /// <summary>
        /// 通過key獲取字碼主檔信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        CodeMasterDto GetCodeMasterByKey(string module, string function, string key);

        /// <summary>
        ///插入字碼主檔
        /// </summary>
        /// <param name="codeMaster"></param>
        void InsertCodeMaster(CodeMasterDto codeMaster);
        /// <summary>
        /// 更新字碼主檔
        /// </summary>
        /// <param name="model"></param>
        void UpdateCodeMaster(CodeMasterDto model);


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
        List<CodeMasterDto> GetMallInfo();
        /// <summary>
        /// 更新平台基本信息
        /// </summary>
        /// <param name="info"></param>
        void UpdateMallInfo(List<CodeMasterDto> info);
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
        SystemResult SaveSystemLogo(SystemLogo logo);
    }
}
