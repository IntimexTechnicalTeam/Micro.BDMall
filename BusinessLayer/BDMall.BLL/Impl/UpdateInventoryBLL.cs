using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class UpdateInventoryBLL : BaseBLL, IUpdateInventoryBLL
    {
        public IInventoryRepository InvRepository;

        public UpdateInventoryBLL(IServiceProvider services) : base(services)
        {
            InvRepository = Services.Resolve<IInventoryRepository>();
        }

        /// <summary>
        /// 更新Inverntory表
        /// </summary>
        /// <param name="insertLst"></param>
        /// <param name="transIOTyp"></param>
        /// <param name="transType"></param>
        /// <returns></returns>
        public async Task<SystemResult> DealProductInventory(List<InvTransactionDtlDto> insertLst, InvTransIOType? transIOTyp, InvTransType transType)
        {
            var result = new SystemResult();
            foreach (var transDtl in insertLst)
            {
                transDtl.Id = Guid.NewGuid();     
                transDtl.IOType = transIOTyp.Value;
                transDtl.WHId = transDtl.FromId;
                transDtl.IsActive = true;
                transDtl.IsDeleted = false;
                transDtl.CreateBy = Guid.Parse(CurrentUser.UserId);

                if (transDtl.TransType == InvTransType.Purchase)
                {
                    transDtl.WHId = transDtl.ToId;
                }
                else if (transDtl.TransType == InvTransType.Relocation)
                {
                    var toTransDtl = new InvTransactionDtl()
                    {
                        Id = Guid.NewGuid(),
                        
                        IOType = InvTransIOType.I,
                        WHId = transDtl.ToId,
                        BizId = transDtl.BizId,
                        TransDate = transDtl.TransDate,
                        TransQty = transDtl.TransQty,
                        TransType = transDtl.TransType,
                        Sku = transDtl.Sku,
                        IsActive = true,
                        IsDeleted = false,
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                    };

                    baseRepository.Insert(toTransDtl);//轉倉操作需要增加一條相對的入倉記錄
                }
                var dbTransDtl = AutoMapperExt.MapTo<InvTransactionDtl>(transDtl);
                baseRepository.Insert(dbTransDtl);//新增庫存詳細記錄                            

                #region 根據業務邏輯處理不同的庫存交易

                switch (transType)
                {
                    case InvTransType.Purchase: await UpdateInvWhenPurchase(transDtl); break;
                    case InvTransType.Relocation:UpdateInvWhenRelocation(transDtl); break;                     
                    case InvTransType.PurchaseReturn:UpdateInvWhenPurchaseReturn(transDtl); break;                      
                    case InvTransType.SalesShipment: UpdateInvWhenSalesShipment(transDtl); break;
                    case InvTransType.SalesReturn:
                    case InvTransType.DeliveryReturn: UpdateInvWhenReturn(transDtl);break;                      
                    default: break;                   
                }

                #endregion

                //NoticeUpdateForAppInvt(transDtl.Sku);
            }

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 采购,更新Inventory
        /// </summary>
        /// <returns></returns>
        async Task<SystemResult> UpdateInvWhenPurchase(InvTransactionDtlDto transDtl)
        {
            var result = new SystemResult();

            #region  採購交易處理

            Guid whId = transDtl.ToId;
            var inventory = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = whId })?.FirstOrDefault();
            if (inventory != null)
            {
                #region 添加庫存數量

                //庫存記錄已存在，使用交易數量更新
                inventory.Quantity += transDtl.TransQty;
                baseRepository.Update(inventory);

                #endregion
            }
            else
            {
                //庫存記錄不存在，則創建
                //創建前檢查對應SKU和倉庫ID是否存在有效數據

                var prodSku = baseRepository.Any<ProductSku>(x => x.Id == transDtl.Sku && x.IsActive && !x.IsDeleted);
                var warehouse = baseRepository.GetModel<Warehouse>(x => x.Id == whId && x.IsActive && !x.IsDeleted);
                if (prodSku && warehouse != null)
                {
                    inventory = new Inventory()
                    {
                        Id = Guid.NewGuid(),
                        Sku = transDtl.Sku,
                        Quantity = transDtl.TransQty,
                        WHId = whId,
                        MerchantId = warehouse.MerchantId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    baseRepository.Insert(inventory);

                    var prodQty = baseRepository.GetModel<ProductQty>(x => x.IsActive && !x.IsDeleted && x.SkuId == transDtl.Sku);
                    if (prodQty == null)
                    {
                        prodQty = new ProductQty()
                        {
                            Id = Guid.NewGuid(),
                            SkuId = transDtl.Sku,
                            InvtActualQty = transDtl.TransQty,
                            InvtHoldQty = 0,
                            InvtReservedQty = 0,
                            SalesQty = transDtl.TransQty,
                        };
                        baseRepository.Insert(prodQty);
                    }
                }

                ////回写redis,相当于补录
                await UpdateProductQtyCache(transDtl.Sku);
            }

            #endregion

            #region 清空以往庫存量低以及售罄通知記錄的歷史狀態

            //try
            //{
            //    var changeNotifyRes = InventoryChangeNotifyBLL.FinishInventoryChangeNotify(new InventoryChangeNotify() { SkuId = transDtl.Sku });
            //}
            //catch (Exception ex)
            //{
            //    SaveError(GetType().FullName, ClassUtility.GetMethodName(), "Send  InventoryChangeNotify Fail", ex.Message);
            //}

            #endregion

            #region 到貨通知

            //try
            //{
            //    var notifyResult = ArrivalNotifyBLL.PushAllArrivalNotify(transDtl.Sku, false);
            //}
            //catch (Exception ex)
            //{
            //    SaveError(GetType().FullName, ClassUtility.GetMethodName(), "Send ArrivalNotify Fail", ex.Message);
            //}


            #endregion

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 调拨，更新Inventory
        /// </summary>
        /// <returns></returns>
        SystemResult UpdateInvWhenRelocation(InvTransactionDtlDto transDtl)
        {
            var result = new SystemResult();

            #region 出庫

            var whIdFrom = transDtl.FromId;
            var inventoryIn = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = whIdFrom })?.FirstOrDefault();

            if (inventoryIn == null) throw new BLException(InventoryErrorEnum.RecordNotExsit.ToString());
            if (inventoryIn.Quantity < transDtl.TransQty) throw new BLException(InventoryErrorEnum.InventoryQtyNotEnough.ToString());

            inventoryIn.Quantity -= transDtl.TransQty;
            inventoryIn.UpdateDate= DateTime.Now;
            baseRepository.Update(inventoryIn);

            #endregion

            #region 入庫

            var whIdIn = transDtl.ToId;
            var inventory = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = whIdIn }).FirstOrDefault();
            if (inventory != null)
            {               
                //庫存記錄已存在，使用交易數量更新
                inventory.Quantity += transDtl.TransQty;
                inventory.UpdateDate = DateTime.Now;
                baseRepository.Update(inventory);
            }
            else
            {
                var prodSku = baseRepository.Any<ProductSku>(x => x.Id == transDtl.Sku && x.IsActive && !x.IsDeleted);
                var warehouse = baseRepository.GetModel<Warehouse>(x => x.Id == whIdIn && x.IsActive && !x.IsDeleted);

                if (!prodSku || warehouse == null) throw new BLException(InventoryErrorEnum.AttachedDataIncomplete.ToString());

                inventory = new Inventory { Id = Guid.NewGuid(), Sku = transDtl.Sku, Quantity = transDtl.TransQty, WHId = whIdIn, MerchantId = warehouse.MerchantId, IsActive = true, IsDeleted = false };
                baseRepository.Insert(inventory);
            }
            #endregion

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 采购退回，更新Inventory
        /// </summary>
        /// <returns></returns>
        SystemResult UpdateInvWhenPurchaseReturn(InvTransactionDtlDto transDtl)
        {
            var result = new SystemResult();

            var inventory = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = transDtl.FromId })?.FirstOrDefault();
            if (inventory == null) throw new BLException(InventoryErrorEnum.RecordNotExsit.ToString());

            if (inventory.Quantity < transDtl.TransQty) throw new BLException(InventoryErrorEnum.InventoryQtyNotEnough.ToString());

            inventory.Quantity -= transDtl.TransQty;
            inventory.UpdateDate = DateTime.Now;
            baseRepository.Update(inventory);

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 銷售出貨，更新Inventory
        /// </summary>
        /// <returns></returns>
        SystemResult UpdateInvWhenSalesShipment(InvTransactionDtlDto transDtl)
        {
            var result = new SystemResult();

            var inventory = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = transDtl.FromId })?.FirstOrDefault();
            if (inventory == null) throw new BLException(InventoryErrorEnum.RecordNotExsit.ToString());

            if (inventory.Quantity < transDtl.TransQty) throw new BLException(InventoryErrorEnum.InventoryQtyNotEnough.ToString());   //库存不足

            inventory.Quantity -= transDtl.TransQty;
            inventory.UpdateDate = DateTime.Now;
            baseRepository.Update(inventory);

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        ///  銷售退回或發貨退回，更新Inventory
        /// </summary>
        /// <returns></returns>
        SystemResult UpdateInvWhenReturn(InvTransactionDtlDto transDtl)
        {
            var result = new SystemResult();

            var inventory = InvRepository.GetInventoryList(new InventoryDto() { Sku = transDtl.Sku, WHId = transDtl.ToId })?.FirstOrDefault();
            if (inventory != null)
            {
                //庫存記錄已存在，使用交易數量更新              
                inventory.Quantity += transDtl.TransQty;
                inventory.UpdateDate = DateTime.Now;
                baseRepository.Update(inventory);
            }
            else
            {
                //增加新庫存記錄
                //庫存記錄不存在，則創建
                //創建前檢查對應SKU和倉庫ID是否存在有效數據
                var prodSku = baseRepository.Any<ProductSku>(x => x.Id == transDtl.Sku && x.IsActive && !x.IsDeleted);
                var warehouse = baseRepository.GetModel<Warehouse>(x => x.Id == transDtl.ToId && x.IsActive && !x.IsDeleted);

                if (!prodSku || warehouse == null) throw new BLException(InventoryErrorEnum.AttachedDataIncomplete.ToString());

                inventory = new Inventory() { Id = Guid.NewGuid(), Sku = transDtl.Sku, Quantity = transDtl.TransQty, WHId = warehouse.Id, MerchantId = warehouse.MerchantId, };
                baseRepository.Insert(inventory);
            }
            result.Succeeded = true;
            return result;

        }

       

        /// <summary>
        /// 更新ProductQtyCache
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        async Task UpdateProductQtyCache(Guid SkuId, int Qty = 0)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            await RedisHelper.ZAddAsync(InvtActualQtyKey, (0, SkuId.ToString()));
            await RedisHelper.ZAddAsync(SalesQtyKey, (0, SkuId.ToString()));
            await RedisHelper.ZAddAsync(InvtReservedQtyKey, (0, SkuId.ToString()));
            await RedisHelper.ZAddAsync(InvtHoldQtyKey, (0, SkuId.ToString()));
        }


    }
}
