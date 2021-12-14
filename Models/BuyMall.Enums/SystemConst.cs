namespace BDMall.Enums
{
    /// <summary>
    /// 模块常量
    /// </summary>
    public class ModuleConst
    {
        /// <summary>
        /// 账户模块
        /// </summary>
        public const string AccountModule = "ACCOUNTMNG";
        /// <summary>
        /// 产品模块
        /// </summary>
        public const string ProductModule = "PRODUCTMNG";
        /// <summary>
        /// 会员模块
        /// </summary>
        public const string MemberModule = "MEMBERMNG";
        /// <summary>
        /// 库存模块
        /// </summary>
        public const string IventoryModule = "INVENTORYMNG";
        /// <summary>
        /// 配送模块
        /// </summary>
        public const string DeliveryModule = "DELIVERYMNG";
        /// <summary>
        /// 市场模块
        /// </summary>
        public const string MarketingModule = "MARKETINGMNG";
        /// <summary>
        /// CMS模块
        /// </summary>
        public const string CMSModule = "CMSMNG";

        /// <summary>
        /// 商家模块
        /// </summary>
        public const string MerchantModule = "MERCHANTMNG";

        /// <summary>
        /// 系统管理模块
        /// </summary>
        public const string SystemModule = "SYSTEMMNG";

        /// <summary>
        /// 訂單管理模塊
        /// </summary>
        public const string OrderModule = "ORDERMNG";

        /// <summary>
        /// 報表模塊
        /// </summary>
        public const string ReportModule = "REPORTMNG";

        /// <summary>
        /// 技術部模塊
        /// </summary>
        public const string TechnicalModule = "TECHNICALMNG";

        /// <summary>
        /// 個人設定
        /// </summary>
        public const string PersonalSetting = "PERSONALSETTING";

        /// <summary>
        /// 商戶使用模塊
        /// </summary>
        //public const string MerchantUsing = "MERCHANTUSING";




    }


    /// <summary>
    /// 功能产品
    /// </summary>
    public class FunctionConst
    {

        #region account
        public const string User_Edit = "EditUser";
        public const string User_Del = "DeleteUser";
        public const string User_Search = "SearchUser";
        #endregion

        #region order
        public const string Order_MassProcess = "MassProcessFlow";

        #endregion


        #region product
        public const string Prod_View = "ViewProd";
        public const string Prod_Edit = "EditProd";
        public const string Prod_Del = "DeleteProd";
        public const string Prod_Approve = "ApproveProd";
        public const string Prod_Search = "SearchProd";

        public const string Prod_Catalog_Edit = "EditProdCatalog";
        public const string Prod_Catalog_Del = "DelProdCatalog";

        public const string Prod_Attr_Edit = "EditProdAttr";
        public const string Prod_Attr_Del = "DelProdAttr";

        public const string Prod_AttrVal_Edit = "EditProdAttrVal";
        public const string Prod_AttrVal_Del = "DelProdAttrVal";


        #endregion


        #region merchant
        public const string Merch_Edit = "EditMerchant";
        public const string Merch_Search = "SearchMerchant";
        /// <summary>
        /// 审批商家，激活商家，创建登入账户等控制
        /// </summary>
        public const string Merch_Approve = "ApproveMerchant";
        /// <summary>
        /// 商家对外发布的信息，推广控制
        /// </summary>
        public const string Merch_Promt = "UpdateMerchPromt";
        /// <summary>
        /// 审批商家对外发布信息、推广
        /// </summary>
        public const string Merch_Approve_Promt = "ApproveMerchPromt";

        /// <summary>
        /// 修改商家付運方式
        /// </summary>
        public const string Merch_Delivery_Method = "UpdateMerchDeliveryMethod";

        /// <summary>
        /// 商家柜台配对
        /// </summary>
        public const string Merch_CC_Setting = "CCSetting";
        #endregion

        #region MARKETINGMNG

        /// <summary>
        /// promotion  banner ，product list
        /// </summary>
        public const string MKT_Promt = "MKTPromt";

        public const string MKT_Promt_Rule = "MKTPromtRule";

        public const string MKT_Promt_Code = "MKTPromtCode";

        public const string MKT_Promt_Coupon = "MKTPromtCoupon";

        public const string MKT_Promt_Emailer = "MKTPromtEmailer";

        public const string MKT_Promt_Message = "MKTPromtMessage";

        public const string MKT_Promt_Announcement = "MKTPromtAnnouncement";
        /// <summary>
        /// 特選產品，指定會員組別購買產品
        /// </summary>
        public const string MKT_Promt_MbrGroup_Prod = "MKTPromtMbrGroupProd";

        public const string MKT_Promt_Key = "MKTPromtKey";

        public const string MKT_Promt_Prod_Comment = "MKTPromtProdComment";

        public const string MKT_Promt_Discount = "MKTPromtDiscount";


        public const string MKT_MetaData = "MKTMetaData";
        public const string MKT_Promt_WhatsApp_Message = "MKTPromtWhatsAppMessage";
        #endregion

        #region Report
        public const string Rpt_FunHistory = "FunHistory";
        public const string Rpt_WishedItemReport = "WishedItemReport";
        public const string Rpt_ThisMonthRegMember = "ThisMonthRegMember";
        public const string Rpt_UnusedCoupMallFun = "UnusedCoupMallFun";
        public const string Rpt_OrderDetailsByMerchant = "OrderDetailsByMerchant";
        public const string Rpt_OrderNetAmounts = "OrderNetAmounts";
        public const string Rpt_OrderDetails = "OrderDetails";
        public const string Rpt_TransactionDetailedsMainOrder = "TransactionDetailedsMainOrder";
        public const string Rpt_SummaryGroupByWarehouse = "SummaryGroupByWarehouse";
        public const string Rpt_FSDOutstandingOrder = "FSDOutstandingOrder";
        public const string Rpt_FSDTranscationSummary = "FSDTranscationSummary";
        public const string Rpt_FSDSettlement = "FSDSettlement";
        public const string Rpt_FSDTransactionDetaileds = "FSDTransactionDetaileds";
        public const string Rpt_FSDRefuntRequestReport = "FSDRefuntRequestReport";
        public const string Rpt_SummaryGroupByDCAItem = "SummaryGroupByDCAItem";
        public const string Rpt_MemberSpendingRank = "MemberSpendingRank";
        public const string Rpt_LastMonthRegMember = "LastMonthRegMember";
        public const string Rpt_FSDRevenueReport = "FSDRevenueReport";
        public const string Rpt_ProductExportData = "ProductExportData";
        public const string Rpt_MonthlyStatementOutwardDetail = "MonthlyStatementOutwardDetail";
        #endregion




    }

    /// <summary>
    /// 系统常量
    /// </summary>
    public class StoreConst
    {
        public const string ClientId = "ClientId";
        public const string AppId = "AppId";
        public const string AppSecret = "AppSecret";

        /// <summary>
        /// token 保存在cookie中的key
        /// </summary>
        public const string AccessToken = "access_token";

        /// <summary>
        /// 用于sso token 检查的时间标记key
        /// </summary>
        public const string CHST = "CHKPSSTT";

        public const string UID = "uid";

        public const string UserType = "UserType";

        /// <summary>
        /// component pro sessionindex
        /// </summary>
        public const string COMPSSIndex = "COMPSSIndex";
        

        /// <summary>
        /// 未登入用户的角色编号
        /// </summary>
        public const string TempRole = "TempCustomer";

        /// <summary>
        /// 客户访问来源
        /// </summary>
        public const string ComeFrom = "ComeFrom";


        public const string LastLogin = "LastLogin";

        /// <summary>
        /// 運費加密鹽
        /// </summary>
        public const string DeliveryPriceSalt = "Intimex-Justin";

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        public const string AccessTokenExpire = "AccessTokenExpire";

        /// <summary>
        /// 记住我
        /// </summary>
        public const string RememberMe = "RememberMe";

        /// <summary>
        /// 登入流水号码
        /// </summary>
        public const string LoginSerialNO = "LoginSerialNO";

        /// <summary>
        /// seo 網站描述
        /// </summary>
        public const string SiteDescription = "SiteDescription";
        /// <summary>
        /// seo 網站關鍵字
        /// </summary>
        public const string SiteKeywords = "SiteKeywords";

        /// <summary>
        /// 商家管理員角色ID
        /// </summary>
        public const string ExternalMerchantAdminRoleId = "10A7C322-58B2-48F1-B28A-58A7CE6C2608";

        /// <summary>
        /// GS1商家管理員角色ID
        /// </summary>
        public const string ExternalGS1MerchantAdminRoleId = "C1791340-2469-4766-99F0-3CEFE42FA558";


        /// <summary>
        /// 商家管理員角色ID
        /// </summary> 
        public const string InternalMerchantAdminRoleId = "150B2ADA-F67F-41CD-8228-C0F6F2668669";


        /// <summary>
        /// GS1商家管理員角色ID
        /// </summary>
        public const string InternalGS1MerchantAdminRoleId = "31C2B4BF-0399-48EA-9808-E0FBE1279292";


        /// <summary>
        /// 冷靜期長度（單位：日）
        /// </summary>
        //public const int CalmeDateRange = 14;

        ///// <summary>
        ///// APP缓存过期时间
        ///// </summary>
        //public const int CacheTimeExpireTime = 10;

        /// <summary>
        ///  会员信息缓存key前缀
        /// </summary>
        public const string MemberCachePrefix = "MIF_";

        public const string ReturnUrl = "returnurl";

        public const string ReturnUrlContent = "sharp";

        #region 郵品訂購服務相關

        /// <summary>
        /// 是否初始進入郵品模塊
        /// </summary>
        public const string SOSInit = "sos_init";
        /// <summary>
        /// 郵品服務方式
        /// </summary>
        public const string SOSDeliveryType = "sos_deliverytype";
        /// <summary>
        /// 郵品領取地區
        /// </summary>
        public const string SOSDeliveryArea = "sos_deliveryarea";
        /// <summary>
        /// 郵品發行年份
        /// </summary>
        public const string SOSIssueYear = "sos_issueyear";
        /// <summary>
        /// 郵品發行計劃ID
        /// </summary>
        public const string SOSFormId = "sos_formid";
        /// <summary>
        /// 郵品訂購服務訂購計劃類型
        /// </summary>
        public const string SOSPlanType = "sos_plantype";
        /// <summary>
        /// 郵品項目ID
        /// </summary>
        public const string SOSIssueId = "sos_issueid";
        /// <summary>
        /// 郵品項目性質
        /// </summary>
        public const string SOSProgramType = "sos_programtype";
        /// <summary>
        /// 協議確認狀態
        /// </summary>
        public const string SOSAgreement = "sos_agreement";

        #endregion

    }

    public class CacheKeyConst
    {
        public const string SellOutSkuList= "SellOutSkuList";
    }
}
