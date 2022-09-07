namespace BDMall.Enums
{
    public enum CodeMasterFunction
    {
        //系统语言
        Language,
        //开通语言
        SupportLanguage,
        //邮件类型
        EmailType,
        //邮件模块
        Email,
        //客人咨询类型
        InquiryType,
        //圖片尺寸
        ProductImgSize,
        /// <summary>
        /// 附加圖片尺寸
        /// </summary>
        ProductAdditionalImgSize,
        //產品目錄的圖片尺寸
        CatalogImgSize,
        //訂單
        Order,
        /// <summary>
        /// 订单状态
        /// </summary>
        OrderStatus,
        /// <summary>
        /// 庫存
        /// </summary>
        Inventory,
        /// <summary>
        /// 庫存預留
        /// </summary>
        InvtReserved,
        /// <summary>
        /// 庫存預留狀態
        /// </summary>
        InvtReservedState,
        /// <summary>
        /// 庫存預留類型
        /// </summary>
        InvtReservedType,

        /// <summary>
        /// 文件上傳過濾器
        /// </summary>
        FileUploadFilter,
        /// <summary>
        /// 站內信類別
        /// </summary>
        MessageType,
        /// <summary>
        /// 站內信來源對象
        /// </summary>
        SendFromType,
        /// <summary>
        /// 網站設定
        /// </summary>
        Seo,

        /// <summary>
        /// ECShip设定
        /// </summary>
        ECShip,
        /// <summary>
        /// 優惠券設定
        /// </summary>
        CouponSetting,
        /// <summary>
        /// 團購活動狀態
        /// </summary>
        GroupPurchaseStatus,

        /// <summary>
        /// 自動生成產品編號設定
        /// </summary>
        ProductCodeSetting,
        /// <summary>
        /// 自動生成訂單編號設定
        /// </summary>
        OrderNoSetting,

        /// <summary>
        /// 郵件每次發送數量
        /// </summary>
        MailSendFrequency,

        /// <summary>
        /// 系統Logo
        /// </summary>
        SystemLogo,

        /// <summary>
        /// 商城设定
        /// </summary>
        MallSetting,

        /// <summary>
        ///  圖片限制大小
        /// </summary>
        ImageLimitSize,

        /// <summary>
        /// 庫存交易類型
        /// </summary>
        InvtTransType,

        /// <summary>
        /// 推廣
        /// </summary>
        Promotion,
        /// <summary>
        /// 貨幣
        /// </summary>
        Currency,

        /// <summary>
        /// 通用設定
        /// </summary>
        Common,

        /// <summary>
        /// 屬性佈局
        /// </summary>
        AttrLayout,

        /// <summary>
        /// 移動端顏色
        /// </summary>
        AppColor,

        /// <summary>
        /// 快遞服務類型(ECShip用)
        /// </summary>
        ExpressService,
        /// <summary>
        /// 智郵站站點(ECShip用)
        /// </summary>
        IPostStation,

        /// <summary>
        /// 柜台
        /// </summary>
        CollectionOffice,

        /// <summary>
        /// 平台基本信息
        /// </summary>
        MallInfo,

        PageMobile,
        PageDesktop,
        PageApp,

        /// <summary>
        /// 产品单位
        /// </summary>
        ProductUnit,
        /// <summary>
        /// 包装单位
        /// </summary>
        CTNUnit,

        /// <summary>
        /// 重量单位
        /// </summary>
        WeightUnit,

        /// <summary>
        /// ECS快递服务类型
        /// </summary>
        ECShipService,

        /// <summary>
        /// 付運方法
        /// </summary>
        ShippingMethod,

        /// <summary>
        /// 系統開啟的付運方法
        /// </summary>
        //ActiveShippingMethod,

        /// <summary>
        /// 退換單類型
        /// </summary>
        ReturnOrderType,
        /// <summary>
        /// 退換單狀態
        /// </summary>
        ReturnOrderStatus,
        /// <summary>
        /// 自動生成退貨單號設定
        /// </summary>
        ReturnOrderNoSetting,
        /// <summary>
        /// 数据长度限制
        /// </summary>
        DataLengthLimit,

        /// <summary>
        /// 记录当前发送Eamiler的APP
        /// </summary>
        EmailerSetting,
        /// <summary>
        /// 記錄當前發送郵件的APPNo
        /// </summary>
        EmailSetting,
        /// <summary>
        /// 到貨通知
        /// </summary>
        ArrivalNotify,

        /// <summary>
        /// MailTracking 信息
        /// </summary>
        MailTracking,

        /// <summary>
        /// GS1參數
        /// </summary>
        GS1,

        /// <summary>
        /// App端推廣組件佈局類型
        /// </summary>
        AppPrmtLayoutType,
        /// <summary>
        /// PC版Web端推廣組件佈局類型
        /// </summary>
        PCPrmtLayoutType,
        /// <summary>
        /// Mobile版Web端推廣組件佈局類型
        /// </summary>
        MobilePrmtLayoutType,

        /// <summary>
        /// 客戶端類型
        /// </summary>
        ClientSideType,
        /// <summary>
        /// 推廣封面佈局類型
        /// </summary>
        PrmtCoverLayout,
        /// <summary>
        /// 推廣標題佈局類型
        /// </summary>
        PrmtHeaderLayout,
        /// <summary>
        /// 推廣控件邊框佈局類型
        /// </summary>
        PrmtBorderLayout,
        /// <summary>
        /// 推廣產品列表佈局類型
        /// </summary>
        PrmtProductLstLayout,
        /// <summary>
        /// 推廣樣式類型
        /// </summary>
        PrmtStyleType,
        /// <summary>
        /// 移動應用推廣樣式類型
        /// </summary>
        AppPrmtStyleType,
        /// <summary>
        /// 產品審批結果
        /// </summary>
        ProdApproveResult,
        /// <summary>
        /// 計劃任務
        /// </summary>
        Schedule,
        /// <summary>
        /// 服務時間間隔單位
        /// </summary>
        ServiceIntervalUnit,
        /// <summary>
        /// LC付運方式的生成快遞單編號規則
        /// </summary>
        LCTrackingNoSetting,
        /// <summary>
        /// 报告下载设定
        /// </summary>
        DLReportSetting,
        //时间
        Time,
        /// <summary>
        /// 批量上傳產品設定
        /// </summary>
        MassUploadSetting,
        /// <summary>
        /// 寬限期
        /// </summary>
        GracePeriod,

        /// <summary>
        /// MPGS设定
        /// </summary>
        MPGSSetting,

        /// <summary>
        /// 代理信息
        /// </summary>
        Proxy,

        /// <summary>
        /// 订单自动发货设定
        /// </summary>
        OrderMassProcess,

        /// <summary>
        /// 订单检测是否已经给钱给银行
        /// </summary>
        OrderRecon,

        /// <summary>
        /// 同步收据到Transin
        /// </summary>
        UploadOrderTransin,
        /// <summary>
        /// 報表類型
        /// </summary>
        ReportType,
        /// <summary>
        /// 订单处理设定
        /// </summary>
        OrderProcessSetting,

        /// <summary>
        /// 網上郵
        /// </summary>
        StampOnNet,

        /// <summary>
        /// 自定義菜單位置
        /// </summary>
        CustomMenuPosition,

        /// <summary>
        /// 自定義菜單類型
        /// </summary>
        CustomMenuType,

        /// <summary>
        /// IP地址区域Code
        /// </summary>
        IPAreaCode,

        /// <summary>
        /// IP地址国际Code
        /// </summary>
        IPInterCode,

        /// <summary>
        /// 会员浏览记录
        /// </summary>
        MemberBrowseingHis,

        /// <summary>
        /// 自動生成商家編號規則
        /// </summary>
        MerchantNoSetting,

        /// <summary>
        /// 推廣類型
        /// </summary>
        PrmtLayoutType,

        /// <summary>
        /// 產品Icon邏輯設定
        /// </summary>
        ProductIconLogic,

        /// <summary>
        /// TranSinApiServer
        /// </summary>
        TranSinApiServer,

        /// <summary>
        /// TransinApi
        /// </summary>
        TranSinApi,
        /// <summary>
        ///  Email开关
        /// </summary>
        EmailSwitch,
        /// <summary>
        /// WhatsApp开关
        /// </summary>
        WhatsAppSwitch,
        /// <summary>
        /// WhatsApp設定                                               
        /// </summary>
        WhatsAppSetting,

        /// <summary>
        /// 抢飞活动设定
        /// </summary>
        EventSetting,
    }
}
