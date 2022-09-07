namespace HandleItemExpireService
{
    /// <summary>
    /// 处理购物车过期服务类
    /// </summary>
    public class DealShoppingCartService : ConsumerHostServiceBase,IBackDoor
    {

        public DealShoppingCartService(IServiceProvider services) : base(services)
        {
            
        }

        protected override string Queue => MQSetting.WeChatShoppingCartTimeOutQueue;
        protected override string Exchange => MQSetting.WeChatShoppingCartTimeOutExchange;

        protected override string categoryName => this.GetType().FullName;

        protected override async Task<SystemResult> Handle(string msg)
        {
            using var scope = base.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IShoppingCartBLL>();
            var baseRepository = scope.ServiceProvider.GetService<IBaseRepository>();
            var codeMasterBLL = scope.ServiceProvider.GetService<ICodeMasterBLL>();
            var result = new SystemResult();

            var item = await baseRepository.GetModelAsync<ShoppingCartItem>(x => x.Id == Guid.Parse(msg) && !x.IsDeleted);
            if (item == null)
            {
                SaveLog($"找不到有效的ShoppingCartItem记录：{msg}", false);
                return result;
            }

            var timeOut = codeMasterBLL.GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Order.ToString(), "ShopcartTimeout")?.Value ?? "30";
            TimeSpan ts = item.ExpireDate - item.CreateDate;
            if (ts.Minutes >= timeOut.ToInt())
            {
                item.Remark = "购物车过期";
                result = await service.RemoveFromCart(item);

                //直接删除
                string key = $"{CacheKey.ShoppingCart}_{item.MemberId}";
                await RedisHelper.DelAsync(key);

                result.Message = result.Message ?? msg;
                SaveLog(result.Message, result.Succeeded);
            }

            return result;
        }
    }
}
