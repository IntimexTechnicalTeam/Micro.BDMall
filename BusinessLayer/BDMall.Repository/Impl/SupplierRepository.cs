using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class SupplierRepository : PublicBaseRepository, ISupplierRepository
    {
        public SupplierRepository(IServiceProvider service) : base(service)
        {
        }

        /// <summary>
        /// 獲取指定條件的供應商資料列表
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        /// <returns></returns>
        public List<Supplier> GetSupplierList(Supplier cond)
        {
            List<Supplier> suppLst = new List<Supplier>();

            if (cond != null)
            {
                var suppQuery = baseRepository.GetList<Supplier>(x => x.IsActive && !x.IsDeleted );
                if (CurrentUser.IsMerchant)
                {
                    if (cond.MerchantId != Guid.Empty)
                    {
                        suppQuery = suppQuery.Where(x => x.MerchantId == cond.MerchantId);
                    }
                    else
                    {
                        suppQuery = suppQuery.Where(x => x.MerchantId == CurrentUser.MerchantId);
                    }
                }
                if (!string.IsNullOrEmpty(cond.Contact))
                {
                    suppQuery = suppQuery.Where(x => x.Contact.Contains(cond.Contact.Trim()));
                }
                if (!string.IsNullOrEmpty(cond.FaxNum))
                {
                    suppQuery = suppQuery.Where(x => x.FaxNum.Contains(cond.FaxNum.Trim()));
                }
                if (!string.IsNullOrEmpty(cond.PhoneNum))
                {
                    suppQuery = suppQuery.Where(x => x.PhoneNum.Contains(cond.PhoneNum.Trim()));
                }
                if (!string.IsNullOrEmpty(cond.Remarks))
                {
                    suppQuery = suppQuery.Where(x => x.Remarks.Contains(cond.Remarks.Trim()));
                }

                suppLst = suppQuery.ToList();
            }

            return suppLst;
        }

    }
}
