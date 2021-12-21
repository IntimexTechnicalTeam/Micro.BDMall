using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IMerchantRepository:IDependency
    {
        PageData<MerchantView> SearchMerchByCond(MerchantPageInfo condition);
    }
}
