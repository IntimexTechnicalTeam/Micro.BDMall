using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using Web.MQ;

namespace HandleItemExpireService
{
    /// <summary>
    /// 处理订单支付超时服务类
    /// </summary>
    public class DealOrderPayTimeOutService : ConsumerHostServiceBase, IBackDoor
    {

        public DealOrderPayTimeOutService(IServiceProvider services) : base(services)
        {

        }

        protected override string Queue => MQSetting.WeChatPayTimeOutQueue;
        protected override string Exchange => MQSetting.WeChatPayTimeOutExchange;

        protected override string categoryName => this.GetType().FullName;

        protected override async Task<SystemResult> Handle(string msg)
        {
            using var scope = base.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IShoppingCartBLL>();
            var baseRepository = scope.ServiceProvider.GetService<IBaseRepository>();
            var codeMasterBLL = scope.ServiceProvider.GetService<ICodeMasterBLL>();
            var orderBLL = scope.ServiceProvider.GetService<IOrderBLL>();
            var dealProductQtyCacheBLL = scope.ServiceProvider.GetService<IDealProductQtyCacheBLL>();
            var result = new SystemResult();

            var dbOrder = await baseRepository.GetModelAsync<Order>(x => x.Id ==Guid.Parse(msg) && !x.IsPaid && x.Status == OrderStatus.ReceivedOrder);
            if (dbOrder == null)
            {
                SaveLog($"找不到待付款的订单记录：{msg}", false);
                return result;
            }

            var timeOut = codeMasterBLL.GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Order.ToString(), "OrderPayTimeout")?.Value ?? "30";
            TimeSpan ts = dbOrder.ExpireDate - dbOrder.CreateDate;

            if (ts.Minutes >= timeOut.ToInt())
            {
                var order = AutoMapperExt.MapTo<OrderDto>(dbOrder);
                order.OrderDetails = baseRepository.GetList<OrderDetail>(x => x.OrderId == dbOrder.Id).ToList();

                var deliverys = baseRepository.GetList<OrderDelivery>(x => x.OrderId == order.Id && x.IsActive && !x.IsDeleted).ToList();
                order.OrderDeliverys = AutoMapperExt.MapToList<OrderDelivery, OrderDeliveryDto>(deliverys);

                order.Remark = "支付超时，取消订单";
                orderBLL.UpdateOrderStatusToECancel(order, null);

                await dealProductQtyCacheBLL.UpdateQtyWhenPayTimeOut(order.Id);
            }

            //通知用户订单已取消



            return result;
        }


    }
}
