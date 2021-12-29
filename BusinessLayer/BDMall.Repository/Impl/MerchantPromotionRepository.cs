using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class MerchantPromotionRepository : PublicBaseRepository, IMerchantPromotionRepository
    {
        public MerchantPromotionRepository(IServiceProvider service) : base(service)
        {
        }

        public MerchantPromotion GetApprovePromotion(Guid merchID)
        {
            var query =  baseRepository.GetList<MerchantPromotion>(p=> p.IsActive && !p.IsDeleted && p.MerchantId == merchID && p.ApproveStatus == ApproveType.Pass)
                                    .OrderByDescending(o => o.CreateDate).FirstOrDefault();

            return query;
        }

        public MerchantPromotion GetNotApprovePromotion(Guid merchID)
        {
            var query = baseRepository.GetList<MerchantPromotion>(p => p.IsActive && !p.IsDeleted && p.MerchantId == merchID && p.ApproveStatus != ApproveType.Pass)
                                     .OrderByDescending(o => o.CreateDate).FirstOrDefault();
            return query;
        }
    }
}
