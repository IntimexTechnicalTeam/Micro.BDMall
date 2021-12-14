using BDMall.BLL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.MQ;

namespace HandleUpdateQtyService
{
    /// <summary>
    ///  消费者队列服务类
    /// </summary>
    public class DealProductQtyService : ConsumerHostServiceBase,IBackDoor
    {        
        public DealProductQtyService(IServiceProvider services) : base(services)
        { 
        
        
        }

        protected override string Queue => MQSetting.UpdateQtyQueue;
        protected override string Exchange => MQSetting.UpdateQtyExchange;

        protected override string categoryName => this.GetType().FullName;

        protected override async Task Handle(string msg)
        {          
            using var scope = base.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUpdateProductQtyBLL>();
            var result = await service.DealProductQty(Guid.Parse(msg));
            result.Message = result.Message ?? msg;
            SaveLog(result.Message, result.Succeeded);
        }

      
    }
}
