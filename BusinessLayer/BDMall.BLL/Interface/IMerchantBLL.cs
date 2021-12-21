using BDMall.Domain;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IMerchantBLL:IDependency
    {

        List<KeyValue> GetMerchantCboSrcByCond(bool containMall);

        PageData<MerchantView> GetMerchLstByCond(MerchantPageInfo condition);
    }
}
