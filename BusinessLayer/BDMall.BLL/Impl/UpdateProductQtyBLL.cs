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
using Web.MQ;

namespace BDMall.BLL
{
    /// <summary>
    /// 消费者服务类,用于更新ProductQty表
    /// </summary>
    public class UpdateProductQtyBLL : BaseBLL, IUpdateProductQtyBLL
    {
        public Dictionary<QtyType, Func<TmpProductQty, Task<int>>> dicQtyMethord = new Dictionary<QtyType, Func<TmpProductQty, Task<int>>>();

        IDealProductQtyRepository ProductQtyRepository;

        public UpdateProductQtyBLL(IServiceProvider services) : base(services)
        {
            dicQtyMethord.Add(QtyType.WhenPurchasing, UpdateQtyWhenPurchasing);
            dicQtyMethord.Add(QtyType.WhenReturn, UpdateQtyWhenReturn);
            dicQtyMethord.Add(QtyType.WhenAddToCart, UpdateQtyWhenAddToCart);
            dicQtyMethord.Add(QtyType.WhenDeleteCart, UpdateQtyWhenDeleteCart);
            dicQtyMethord.Add(QtyType.WhenModifyCart, UpdateQtyWhenModifyCart);
            dicQtyMethord.Add(QtyType.WhenPay, UpdateQtyWhenPay);
            dicQtyMethord.Add(QtyType.WhenDeliveryArranged, UpdateQtyWhenDeliveryArranged);
            dicQtyMethord.Add(QtyType.WhenOrderCancel, UpdateQtyWhenOrderCancel);
            dicQtyMethord.Add(QtyType.WhenPayTimeOut, UpdateQtyWhenPayTimeOut);

            ProductQtyRepository = this.Services.GetService(typeof(IDealProductQtyRepository)) as IDealProductQtyRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">PushMessage.Id</param>
        /// <returns></returns>
        public async Task<SystemResult> DealProductQty(Guid Id)
        {
            var result = new SystemResult() { Succeeded = false };
            var msg = await baseRepository.GetModelByIdAsync<PushMessage>(Id);

            if (msg == null)
            {
                result.Message = $"找不到记录{Id}";
                return result;
            }

            if (msg.State == MQState.Deal)
            {
                result.Message = $"记录{Id}已处理";
                return result;
            }

            if (msg.MsgType != MQType.UpdateInvt)
            {
                result.Message = $"记录{Id}的类型不是UpdateInvt";
                return result;
            }

            var tmpProductQty = JsonUtil.ToObject<TmpProductQty>(msg.MsgContent);
            tmpProductQty.Id = Id;
            int doFlag = await dicQtyMethord[tmpProductQty.QtyType].Invoke(tmpProductQty);

            result.Succeeded = doFlag > 0 ? true : false;

            return result;
        }

        private async Task<int> UpdateQtyWhenPurchasing(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenPurchasing(tmpProductQty.SkuId, tmpProductQty.InvtActualQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenReturn(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenReturn(tmpProductQty.SkuId, tmpProductQty.InvtActualQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenAddToCart(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenAddToCart(tmpProductQty.SkuId, tmpProductQty.InvtHoldQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenDeleteCart(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenDeleteCart(tmpProductQty.SkuId, tmpProductQty.InvtHoldQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenModifyCart(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenModifyCart(tmpProductQty.SkuId, tmpProductQty.InvtHoldQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenPay(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenPay(tmpProductQty.SkuId, tmpProductQty.InvtReservedQty, tmpProductQty.SalesQty, tmpProductQty.InvtHoldQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenDeliveryArranged(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenDeliveryArranged(tmpProductQty.SkuId, tmpProductQty.InvtReservedQty, tmpProductQty.InvtActualQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenOrderCancel(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenOrderCancel(tmpProductQty.SkuId, tmpProductQty.InvtReservedQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }

        private async Task<int> UpdateQtyWhenPayTimeOut(TmpProductQty tmpProductQty)
        {
            return await ProductQtyRepository.UpdateQtyWhenPayTimeOut(tmpProductQty.SkuId, tmpProductQty.InvtHoldQty, tmpProductQty.SalesQty, tmpProductQty.Id);
        }


        /// <summary>
        /// 补偿回写Qty
        /// </summary>
        /// <returns></returns>
        public async Task<SystemResult> HandleQtyAsync()
        {
            var result = new SystemResult();

            string queue = MQSetting.WeChatUpdateQtyQueue;
            string exchange = MQSetting.WeChatUpdateQtyExchange;

            var list = await baseRepository.GetListAsync<PushMessage>(x => x.State == MQState.UnDeal && x.QueueName == queue
                          && x.ExchangeName == exchange && x.MsgType == MQType.UpdateInvt);

            var query = list.OrderBy(o => o.CreateDate).Take(100).ToList();
            if (query != null && query.Any())
            {
                //this.Logger.LogInformation($"一共{query.Count()}条");
                foreach (var item in query)
                {
                    //this.Logger.LogInformation($"正在发送MQ消息,队列={queue},消息={item.Id}");
                    rabbitMQService.PublishMsg(queue, exchange, item.Id.ToString());
                }
                //this.Logger.LogInformation($"全部发送完了.....");
                result.Succeeded = true;
            }
            return result;
        }

    }
}
