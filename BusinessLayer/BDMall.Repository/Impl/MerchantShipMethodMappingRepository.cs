namespace BDMall.Repository
{
    public class MerchantShipMethodMappingRepository : PublicBaseRepository, IMerchantShipMethodMappingRepository
    {
        public MerchantShipMethodMappingRepository(IServiceProvider service) : base(service)
        {
        }

        public List<MerchantActiveShipMethodDto> GetShipMethidByMerchantId(Guid merchantId)
        {
            var query = (from m in baseRepository.GetList<MerchantActiveShipMethod>()
                         join c in baseRepository.GetList<CodeMaster>() on new { a1 = m.ShipCode, a2 = CodeMasterModule.System.ToString(), a3 = CodeMasterFunction.ShippingMethod.ToString() } equals new { a1 = c.Key, a2 = c.Module, a3 = c.Function } into ct
                         from cc in ct.DefaultIfEmpty()
                         join d in baseRepository.GetList<ExpressCompany>() on m.ShipCode equals d.Code into dc
                         from dd in dc.DefaultIfEmpty()
                         join t in baseRepository.GetList <Translation>() on new { a1 = cc.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         join dt in baseRepository.GetList<Translation>() on new { a1 = dd.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = dt.TransId, a2 = dt.Lang } into dtc
                         from dtt in dtc.DefaultIfEmpty()
                         where m.IsActive && !m.IsDeleted && m.MerchantId == merchantId
                         //&& m.IsEffect == true
                         select new 
                         {
                             shipmethod = m,
                             ctran = tt,
                             dtran = dtt
                         });

            var result = new List<MerchantActiveShipMethodDto>();
           
            foreach (var item in query)
            {
                var shipMethod = AutoMapperExt.MapTo<MerchantActiveShipMethodDto>(item.shipmethod);
                if (item.ctran != null) shipMethod.ShipMethodName = item.ctran.Value;               
                if (item.dtran != null) shipMethod.ShipMethodName = item.dtran.Value;             
                result.Add(shipMethod);
            }
            return result;
        }

    }
}
