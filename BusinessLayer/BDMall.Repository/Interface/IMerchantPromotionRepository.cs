using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IMerchantPromotionRepository:IDependency
    {
        MerchantPromotion GetApprovePromotion(Guid merchID);

        MerchantPromotion GetNotApprovePromotion(Guid merchID);
    }
}
