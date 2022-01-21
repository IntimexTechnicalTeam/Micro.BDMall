using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class InventoryChangeNotifyBLL : BaseBLL, IInventoryChangeNotifyBLL
    {
        public InventoryChangeNotifyBLL(IServiceProvider services) : base(services)
        {
        }

        public SystemResult AddInventoryChangeNotify(InventoryChangeNotify notify)
        {
            var sysRslt = new SystemResult();

            if (notify != null)
            {
                //獲取未被處理的庫存變化通知
                var notifyRec = baseRepository.GetModel<InventoryChangeNotify>(x => x.SkuId == notify.SkuId && x.Type == notify.Type && !x.IsProcessed && x.IsActive && !x.IsDeleted);

                if (notifyRec == null)
                {
                    notifyRec = new InventoryChangeNotify()
                    {
                        Id = Guid.NewGuid(),
                        SkuId = notify.SkuId,
                        IsProcessed = false,
                        Type = notify.Type,
                        IsMerchNotified = true,
                        MerchNotifyDate = DateTime.Now,
                    };

                    //UnitOfWork.IsUnitSubmit = true;

                    baseRepository.Insert(notifyRec);

                    //UnitOfWork.Submit();

                    sysRslt.Succeeded = true;

                    //數據成功處理好，發出郵件
                    //SendNoticeToMerchant(notify);    //未完成
                }
                else
                {
                    if (!notifyRec.IsMerchNotified)
                    {
                        //UnitOfWork.IsUnitSubmit = true;

                        notifyRec.IsMerchNotified = true;
                        notifyRec.MerchNotifyDate = DateTime.Now;
                        baseRepository.Update(notifyRec);

                        //UnitOfWork.Submit();

                        sysRslt.Succeeded = true;

                        //數據成功處理好，發出郵件
                        //SendNoticeToMerchant(notify);    //未完成
                    }
                }
            }
            return sysRslt;
        }
    }
}
