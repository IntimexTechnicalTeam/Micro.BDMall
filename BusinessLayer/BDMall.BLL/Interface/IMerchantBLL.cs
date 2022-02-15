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

        Task<PageData<MicroMerchant>> GetMerchantListAsync(MerchantCond cond);

        MerchantView GetMerchById(Guid Id);

        Task<SystemResult> Save(MerchantView merchVw);

        Task<SystemResult> ActiveMerchantAsync(Guid merchID);

        Task<SystemResult> DeactiveMerchantAsync(Guid merchID);

        Task<SystemResult> LogicalDelMerchRec(string recIDsList);
        
        /// <summary>
        /// 获取系统所有ShipMethod
        /// </summary>
        /// <returns></returns>
        MerchantShipMethodMappingView GetAdminShipMethod();

        MerchantShipMethodMappingView GetMerchantShipMethods(Guid merchantId);

        void SaveShipMethodMapping(MerchantShipMethodMappingView mappingShipMethod);

        MerchantPromotionView GetMerchPromotionInfo(Guid merchID);

        bool SaveMerchantPromotion(MerchantPromotionView promotion);

        MerchantPromotionView GetEditingMerchPromotionInfo(Guid merchID);

        bool InsertMerchantPromotion(MerchantPromotionView promotion);

        bool UpdateMerchantPromotion(MerchantPromotionView promotion);

        SystemResult ApplyApprove(Guid id);

        Task<SystemResult> ApproveMerchantAsync(List<string> ids);

        Task<SystemResult> RejectMerchant(Guid merchId, string reason);

        PageData<MerchantSelectSummary> SearchMerchantList(MerchantCond cond);

        Task<MerchantInfoView> GetMerchantInfoAsync(Guid merchID);

        Task<PageData<MicroProduct>> GetMchProductListAsync(ProductCond cond);
    }
}
