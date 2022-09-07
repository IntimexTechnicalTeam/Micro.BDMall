namespace BDMall.Repository
{
    public interface IExpressCompanyRepository : IDependency
    {
        /// <summary>
        /// 根据快递code获取快递公司
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExpressCompanyDto GetByCode(string code);

        List<ExpressCompanyDto> GetActiveExpress();
    }
}
