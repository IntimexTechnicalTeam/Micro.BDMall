namespace BDMall.BLL
{
    /// <summary>
    /// 处理缓存中的ProductQty
    /// </summary>
    public class DealProductQtyCacheBLL : BaseBLL, IDealProductQtyCacheBLL
    {
        public DealProductQtyCacheBLL(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// 当订单状态改变时，更新库存
        /// </summary>
        /// <param name="orderStatusInfo"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenOrderStateChange(UpdateStatusCondition orderStatusInfo)
        {
            var result = new SystemResult();

            var order = await baseRepository.GetModelByIdAsync<Order>(orderStatusInfo.OrderId);
            if (order != null)
            {               
                switch (order.Status)
                {
                    case OrderStatus.PaymentConfirmed: result = await UpdateQtyWhenPaymentConfirmed(order.Id); result.ReturnValue = order; break;
                    case OrderStatus.DeliveryArranged: result = await UpdateQtyWhenOrderDeliveryArranged(order.Id); result.ReturnValue = order; break;
                    case OrderStatus.SCancelled: result = await UpdateQtyWhenOrderCancel(order.Id); break;
                    default: result.Succeeded = true; result.ReturnValue = null; break;
                }
            }        
            return result;
        }

        /// <summary>
        /// 采购入库,采购退回,銷售退回，發貨退回时更新库存(ProductQty)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenPurchaseOrReturn(List<InvTransactionDtlDto> list)
        {
            var result = new SystemResult();

            foreach (var item in list)
            {
                switch (item.TransType)
                {
                    case InvTransType.Purchase: result = await UpdateQtyWhenPurchasing(item.Sku, item.TransQty); break;
                    case InvTransType.PurchaseReturn: result = await UpdateQtyWhenPurchaseReturn(item.Sku, item.TransQty); break;
                    case InvTransType.Relocation: result.Succeeded = true; break;          //调拨不处理ProductQty,直接break
                    case InvTransType.SalesReturn:
                    case InvTransType.DeliveryReturn:  result = await UpdateQtyWhenReturn(item.Sku, item.TransQty); break;
                    default:break;
                }
            }
            return result;
        }

        /// <summary>
        /// 采购入库成功，更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPurchasing(Guid SkuId, int Qty)
        {
            var result = new SystemResult();
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            if (InvtHoldQty < 0) InvtHoldQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //设置实际库存数
            var InvtActualQty = await  RedisHelper.ZIncrByAsync(InvtActualQtyKey, SkuId.ToString(), Qty);

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

            var productQty = new TmpProductQty { InvtActualQty = (int)InvtActualQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenPurchasing };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 采购退回，更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty">退回数量</param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPurchaseReturn(Guid SkuId, int Qty)
        {
            var result = new SystemResult();

            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            if (InvtHoldQty < 0) InvtHoldQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //设置实际库存数       
            var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
            InvtActualQty = InvtActualQty - Qty < 0 ? 0 : InvtActualQty - Qty;
            await  RedisHelper.ZAddAsync(InvtActualQtyKey, (InvtActualQty, SkuId));

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

            var productQty = new TmpProductQty { InvtActualQty = (int)InvtActualQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenReturn };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 銷售退回或發貨退回时更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty">退回数量</param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenReturn(Guid SkuId, int Qty)
        {
            var result = new SystemResult();

            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            if (InvtHoldQty < 0) InvtHoldQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //设置实际库存数
            var InvtActualQty = await  RedisHelper.ZIncrByAsync(InvtActualQtyKey, SkuId.ToString(), Qty);

            //设置可销售库存数           
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;

            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId.ToString()));

            var productQty = new TmpProductQty { InvtActualQty = (int)InvtActualQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenReturn };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 加入购物车，进行Hold货时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public async Task UpdateQtyWhenAddToCart(Guid SkuId, int Qty)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
            if (InvtActualQty < 0) InvtActualQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //设置Hold货数量
            var InvtHoldQty = await  RedisHelper.ZIncrByAsync(InvtHoldQtyKey, SkuId.ToString(), Qty);

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            if (SalesQty < 0) SalesQty = 0;
            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

            var productQty = new TmpProductQty { InvtHoldQty = (int)InvtHoldQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenAddToCart };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
        }

        /// <summary>
        /// 当移除购物车上的物品时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public async Task UpdateQtyWhenDeleteCart(Guid SkuId, int Qty)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
            if (InvtActualQty < 0) InvtActualQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //获取Hold货数
            var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            //计算Hold货数
            InvtHoldQty = InvtHoldQty - Qty < 0 ? 0 : InvtHoldQty - Qty;
            //设置Hold货数
            await  RedisHelper.ZAddAsync(InvtHoldQtyKey, (InvtHoldQty, SkuId.ToString()));

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            if (SalesQty < 0) SalesQty = 0;

            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId.ToString()));

            var productQty = new TmpProductQty { InvtHoldQty = (int)InvtHoldQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenDeleteCart };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
        }

        /// <summary>
        /// 当修改购物车上的物品数量时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="NewQty">变化后的</param>
        /// <param name="OldQty">变化前的</param>
        /// <returns></returns>
        public async Task UpdateQtyWhenModifyCart(Guid SkuId, int NewQty, int OldQty)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
            if (InvtActualQty < 0) InvtActualQty = 0;

            var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //计算Hold货数
            var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            InvtHoldQty = InvtHoldQty + (NewQty - OldQty);
            //设置Hold货数量
            await  RedisHelper.ZAddAsync(InvtHoldQtyKey, (InvtHoldQty, SkuId.ToString()));

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            if (SalesQty < 0) SalesQty = 0;
            await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId.ToString()));

            var productQty = new TmpProductQty { InvtHoldQty = (int)InvtHoldQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenModifyCart };
            var msg = GenMsg(productQty);

            await baseRepository.InsertAsync(msg);
            this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());

        }

        /// <summary>
        /// 当订单状态变更为PaymentConfirmed时
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPaymentConfirmed(Guid OrderId)
        {
            var result = new SystemResult();
            var reservedLst = await baseRepository.GetListAsync<InventoryReserved>(x => x.OrderId == OrderId && x.IsActive && !x.IsDeleted
                                               && x.ProcessState == InvReservedState.RESERVED);

            if (reservedLst != null && reservedLst.Any())
            {
                string SalesQtyKey = $"{CacheKey.SalesQty}";
                string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
                string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
                string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

                var rLst = reservedLst.GroupBy(g => g.Sku).Select(s => new { Sku = s.Key, Qty = s.Sum(x => x.ReservedQty) });
                foreach (var item in rLst)
                {
                    Guid SkuId = item.Sku; int Qty = item.Qty;

                    var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
                    if (InvtActualQty < 0) InvtActualQty = 0;

                    //设置预留数
                    var InvtReservedQty = await  RedisHelper.ZIncrByAsync(InvtReservedQtyKey, SkuId.ToString(), Qty);

                    //获取Hold数
                    var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
                    //计算Hold数
                    InvtHoldQty = InvtHoldQty - Qty < 0 ? 0 : InvtHoldQty - Qty;
                    //设置Hold数
                    await  RedisHelper.ZAddAsync(InvtHoldQtyKey, (InvtHoldQty, SkuId));

                    //设置可销售库存数
                    var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
                    if (SalesQty < 0) SalesQty = 0;
                    await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

                    var productQty = new TmpProductQty { InvtReservedQty = (int)InvtReservedQty, SalesQty = (int)SalesQty, SkuId = SkuId, InvtHoldQty = (int)InvtHoldQty, QtyType = QtyType.WhenPay };
                    var msg = GenMsg(productQty);

                    //发送MQ消息
                    await baseRepository.InsertAsync(msg);
                    this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
                }
                result.Succeeded = true;
            }
            return result;
        }

        /// <summary>
        /// 当订单状态变更为DeliveryArranged,已安排发货
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>  ---not ok
        async Task<SystemResult> UpdateQtyWhenOrderDeliveryArranged(Guid OrderId)
        {
            var result = new SystemResult();
            var reservedLst = await baseRepository.GetListAsync<InventoryReserved>(x => x.OrderId == OrderId && x.IsActive && !x.IsDeleted
                                               && x.ProcessState == InvReservedState.FINISH);

            if (reservedLst != null && reservedLst.Any())
            {
                string SalesQtyKey = $"{CacheKey.SalesQty}";
                string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
                string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
                string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

                var rLst = reservedLst.GroupBy(g => g.Sku).Select(s => new { Sku = s.Key, Qty = s.Sum(x => x.ReservedQty) });

                foreach (var item in rLst)
                {
                    Guid SkuId = item.Sku; int Qty = item.Qty;

                    var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
                    if (InvtHoldQty < 0) InvtHoldQty = 0;

                    //取出实际库存
                    var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
                    //计算实际库存
                    InvtActualQty = InvtActualQty - Qty < 0 ? 0 : InvtActualQty - Qty;
                    //设置实际库存
                    await  RedisHelper.ZAddAsync(InvtActualQtyKey, (InvtActualQty, SkuId));

                    //取出预留数
                    var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
                    //计算预留数
                    InvtReservedQty = InvtReservedQty - Qty < 0 ? 0 : InvtReservedQty - Qty;
                    //设置预留数
                    await  RedisHelper.ZAddAsync(InvtReservedQtyKey, (InvtReservedQty, SkuId));

                    //设置可销售库存数
                    var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
                    if (SalesQty < 0) SalesQty = 0;

                    await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId.ToString()));

                    var productQty = new TmpProductQty { InvtReservedQty = (int)InvtReservedQty, InvtActualQty = (int)InvtActualQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenDeliveryArranged };
                    var msg = GenMsg(productQty);

                    await baseRepository.InsertAsync(msg);
                    this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
                }
                result.Succeeded = true;
            }
            return result;
        }

        /// <summary>
        /// 当订单取消时，回滚预留数
        /// </summary>
        /// <param name="SkuId"></param>       
        /// <param name="Qty"></param>
        /// <returns></returns>  
        async Task<SystemResult> UpdateQtyWhenOrderCancel(Guid OrderId)
        {
            var result = new SystemResult();
            var reservedLst = await baseRepository.GetListAsync<InventoryReserved>(x => x.OrderId == OrderId && x.IsActive && !x.IsDeleted
                                               && x.ProcessState == InvReservedState.CANCEL);

            if (reservedLst != null && reservedLst.Any())               //当订单为已付款，处理中
            {
                string SalesQtyKey = $"{CacheKey.SalesQty}";
                string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
                string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
                string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

                var rLst = reservedLst.GroupBy(g => g.Sku).Select(s => new { Sku = s.Key, Qty = s.Sum(x => x.ReservedQty) });

                foreach (var item in rLst)
                {
                    Guid SkuId = item.Sku; int Qty = item.Qty;

                    var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
                    if (InvtActualQty < 0) InvtActualQty = 0;

                    var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
                    if (InvtHoldQty < 0) InvtHoldQty = 0;

                    //获取预留数
                    var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
                    //计算预留数
                    InvtReservedQty = InvtReservedQty - Qty < 0 ? 0 : InvtReservedQty - Qty;
                    //设置预留数并返回更新后的预留数
                    await  RedisHelper.ZAddAsync(InvtReservedQtyKey, (InvtReservedQty, SkuId));

                    //设置可销售库存数
                    var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
                    if (SalesQty < 0) SalesQty = 0;
                    await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

                    var productQty = new TmpProductQty { InvtReservedQty = (int)InvtReservedQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenOrderCancel };
                    var msg = GenMsg(productQty);

                    await baseRepository.InsertAsync(msg);
                    this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
                }
                result.Succeeded = true;
            }
            else
            {
                //订单已创建了，订单是待付款状态,取消订单
                result = await UpdateQtyWhenPayTimeOut(OrderId);
                result.Succeeded = true;
            }

            return result;
        }

        /// <summary>
        /// 支付超时，支付失败，或者后台在待付款状态时手动取消订单，恢复Hold货数
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenPayTimeOut(Guid OrderId)
        {
            var result = new SystemResult();

            var details = await baseRepository.GetListAsync<OrderDetail>(x => x.OrderId == OrderId);
            if (details != null && details.Any())
            {
                string SalesQtyKey = $"{CacheKey.SalesQty}";
                string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
                string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
                string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

                foreach (var item in details)
                {
                    Guid SkuId = item.SkuId; int Qty = item.Qty;

                    var InvtActualQty = await  RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
                    if (InvtActualQty < 0) InvtActualQty = 0;

                    var InvtReservedQty = await  RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
                    if (InvtReservedQty < 0) InvtReservedQty = 0;

                    //获取Hold数
                    var InvtHoldQty = await  RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
                    //计算Hold数
                    InvtHoldQty = InvtHoldQty - Qty < 0 ? 0 : InvtHoldQty - Qty;
                    //设置Hold数
                    await  RedisHelper.ZAddAsync(InvtHoldQtyKey, (InvtHoldQty, SkuId));

                    //设置可销售库存数
                    var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
                    if (SalesQty < 0) SalesQty = 0;
                    await  RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

                    var productQty = new TmpProductQty { InvtHoldQty = (int)InvtHoldQty, SalesQty = (int)SalesQty, SkuId = SkuId, QtyType = QtyType.WhenPayTimeOut };
                    var msg = GenMsg(productQty);

                    await baseRepository.InsertAsync(msg);
                    this.rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());

                }
                result.Succeeded = true;
            }

            return result;
        }

        private PushMessage GenMsg(TmpProductQty ProductQty)
        {
            var msg = new PushMessage
            {
                Id = Guid.NewGuid(),
                ItemId = ProductQty.SkuId,
                MsgType = MQType.UpdateInvt,
                QueueName = MQSetting.WeChatUpdateQtyQueue,
                ExchangeName = MQSetting.WeChatUpdateQtyExchange,
                MsgContent = JsonUtil.ToJson(ProductQty)
            };

            return msg;
        }

        
    }
}
