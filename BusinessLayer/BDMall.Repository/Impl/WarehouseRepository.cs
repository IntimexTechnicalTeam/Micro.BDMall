namespace BDMall.Repository
{
    public class WarehouseRepository : PublicBaseRepository, IWarehouseRepository
    {
        public WarehouseRepository(IServiceProvider service) : base(service)
        {
        }

        /// <summary>
        /// 獲取指定條件的倉庫資料列表
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        /// <returns></returns>
        public List<Warehouse> GetWarehouseList(WarehouseDto cond)
        {
            List<Warehouse> warehouseLst = new List<Warehouse>();

            if (cond != null)
            {
                var warehouseQuery =  baseRepository.GetList<Warehouse>().Where(x => x.IsActive && !x.IsDeleted);
              
                if (cond.MerchantId != Guid.Empty)
                {
                    warehouseQuery = warehouseQuery.Where(x => x.MerchantId == cond.MerchantId);
                }
                if (!string.IsNullOrEmpty(cond.PhoneNum))
                {
                    warehouseQuery = warehouseQuery.Where(x => x.PhoneNum.Contains(cond.PhoneNum.Trim()));
                }
                if (!string.IsNullOrEmpty(cond.PostalCode))
                {
                    warehouseQuery = warehouseQuery.Where(x => x.PostalCode.Contains(cond.PostalCode.Trim()));
                }
                if (!string.IsNullOrEmpty(cond.Remarks))
                {
                    warehouseQuery = warehouseQuery.Where(x => x.Remarks.Contains(cond.Remarks.Trim()));
                }

                warehouseLst = warehouseQuery.ToList();
            }

            return warehouseLst;
        }

    }
}
