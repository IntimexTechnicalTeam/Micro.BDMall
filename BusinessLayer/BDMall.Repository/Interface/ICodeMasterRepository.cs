namespace BDMall.Repository
{
    public interface ICodeMasterRepository:IDependency
    {
        List<CodeMasterDto> GetCodeMasters(CodeMasterModule module, CodeMasterFunction function);
        List<CodeMasterDto> GetCodeMasters(string module, string function);
        CodeMasterDto GetCodeMaster(string module, string function, string key);
        CodeMasterDto GetCodeMaster(string module, string function, string key, string value);
        List<CodeMasterDto> GetCodeMasters(Guid clientId, string module, string function, string key);


        /// <summary>
        /// 获取分页的字码主档
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition cond);
    }
}
