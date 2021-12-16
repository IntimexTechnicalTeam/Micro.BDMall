using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class MerchantBLL : BaseBLL, IMerchantBLL
    {
        public MerchantBLL(IServiceProvider services) : base(services)
        {
        }

        public List<KeyValue> GetMerchantCboSrcByCond(bool containMall)
        {
            var merchantList = new List<KeyValue>();

            var isMerchant = CurrentUser.LoginType <= LoginType.ThirdMerchantLink ? true : false;
            if (containMall && !isMerchant)
                merchantList.Add(new KeyValue { Id = Guid.Empty.ToString(), Text = BDMall.Resources.Value.Mall });

            var query = (from d in baseRepository.GetList<Merchant>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = d.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into TransTemp
                         from tt in TransTemp.DefaultIfEmpty()
                         where (!isMerchant || (isMerchant && d.Id == Guid.Parse(CurrentUser.UserId))) && d.IsDeleted == false
                         select new KeyValue
                         {
                             Id = d.Id.ToString(),
                             Text = tt != null ? tt.Value : string.Empty,
                         });

            var list = query.OrderBy(o => o.Text).ToList();
            merchantList.AddRange(list);
            return merchantList;
        }
    }
}

