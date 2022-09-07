namespace BDMall.Domain
{
    public class SysCustomization
    {
        /// <summary>
        /// 優惠券是否啟用
        /// </summary>
        public bool CouponSwitch { get; set; }
        /// <summary>
        /// 是否啟用郵件服務器的SSL
        /// </summary>
        public bool MailServerSSLSwitch { get; set; }
        /// <summary>
        /// 郵箱設定
        /// </summary>
        public List<CustomizationValue> EmailSettings { get; set; }

        /// <summary>
        /// 圖片尺寸設定
        /// </summary>
        public List<CustomizationValue> ImgSizeSettings { get; set; }

        /// <summary>
        /// 默認語言
        /// </summary>
        public string DefaultLanguage { get; set; }
        /// <summary>
        /// 是否開啟自動生成OrderNo
        /// </summary>
        public bool OrderNoAutoGenSwitch { get; set; }
        /// <summary>
        /// 自動生成OrderNo的設定
        /// </summary>
        public List<CustomizationValue> OrderNoAutoGenSettings { get; set; }
        /// <summary>
        /// 是否開啟自動生成ProdCode
        /// </summary>
        public bool ProdCodeAutoGenSwitch { get; set; }
        /// <summary>
        /// 自動生成ProdCode的設定
        /// </summary>
        public List<CustomizationValue> ProdCodeAutoGenSettings { get; set; }
        /// <summary>
        /// 系統支持語言設定
        /// </summary>
        public List<CustomizationValue> SupportLanguageSettings { get; set; }

        /// <summary>
        /// 系统开放的付运方法
        /// </summary>
        public List<CustomizationValue> MerchantShipMethods { get; set; }

        /// <summary>
        /// Refund報表運行參數設定
        /// </summary>
        public CustomizationValue CR2ReportSetting { get; set; }
        /// <summary>
        /// Invoice報表運行參數設定
        /// </summary>
        public List<CustomizationValue> CR3ReportSettings { get; set; }
        /// <summary>
        /// DCA報表運行參數設定
        /// </summary>
        public CustomizationValue DCAReportSetting { get; set; }
        /// <summary>
        /// ISU退款報表運行參數設定
        /// </summary>
        public CustomizationValue ISUReportSetting { get; set; }
        /// <summary>
        /// 訪問令牌過時時間設定
        /// </summary>
        public List<CustomizationValue> SessionControlSettings { get; set; }

        /// <summary>
        /// 訂單冷靜期
        /// </summary>
        public CustomizationValue GracePeroidSetting { get; set; }

        /// <summary>
        /// 支付過期時間
        /// </summary>
        public CustomizationValue PayTimeOutSetting { get; set; }

        /// <summary>
        /// GS1執行時間
        /// </summary>
        public CustomizationValue GS1RunHourSetting { get; set; }

        /// <summary>
        /// 購物車貨物過期時間
        /// </summary>
        public CustomizationValue ShopCartTimeOutSetting { get; set; }

        /// <summary>
        /// 維護模式啟用標識
        /// </summary>
        public bool MaintainModeSwitch { get; set; }

        public SysCustomization()
        {
            EmailSettings = new List<CustomizationValue>();

            ImgSizeSettings = new List<CustomizationValue>();
            OrderNoAutoGenSettings = new List<CustomizationValue>();
            ProdCodeAutoGenSettings = new List<CustomizationValue>();
            SupportLanguageSettings = new List<CustomizationValue>();
            MerchantShipMethods = new List<CustomizationValue>();
            CR3ReportSettings = new List<CustomizationValue>();
            SessionControlSettings = new List<CustomizationValue>();
        }
    }
}
