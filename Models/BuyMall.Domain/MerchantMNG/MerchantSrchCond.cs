namespace BDMall.Domain
{
    public class MerchantSrchCond
    {

        public MerchantSrchCond()
        {
            this.ApproveStatus = -1;
        }

        /// <summary>
        /// 商家主檔記錄ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// M Merch No.
        /// </summary>
        public string MerchNo { get; set; }
        /// <summary>
        /// 商家公司名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsActiveCond { get; set; }
        /// <summary>
        /// 是否已創建用戶
        /// </summary>
        public bool? IsAccountCreated { get; set; }

        public string AccountCreateDateB { get; set; }
        public string AccountCreateDateE { get; set; }

        public string ContactEmail { get; set; }
        public string OrderEmail { get; set; }

        public int ApproveStatus { get; set; }
    }
}
