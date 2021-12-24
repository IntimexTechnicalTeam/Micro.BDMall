using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using BDMall.Enums;
using System.Net;
using Intimex.Common;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface ISettingBLL : IDependency
    {
        ///// <summary>
        ///// 获取平台服务器
        ///// 列如： http://api.microlab.net.cn
        ///// </summary>
        ///// <returns></returns>
        //string GetPlatformServer();

        ///// <summary>
        ///// 獲取系統發送郵件賬號
        ///// </summary>
        ///// <returns></returns>
        //string GetEmailSender();
        ///// <summary>
        ///// 獲取銷售價格的稅率
        ///// </summary>
        ///// <returns></returns>
        //double GetSalePriceTaxRate();

        ///// <summary>
        ///// 獲取默認的語言
        ///// </summary>
        ///// <returns></returns>
        //Language GetDefaultLanguage();

        /// <summary>
        /// 獲取系统可用的語言
        /// </summary>
        /// <returns></returns>
        List<SystemLang> GetSupportLanguages();

        List<SystemLang> GetSupportLanguages(Language rtnLang);


        ///// <summary>
        ///// 获取SEO METADATA  Keywords
        ///// </summary>
        ///// <returns></returns>
        //Dictionary<string, string> GetSiteKeywords(string path);

        ///// <summary>
        ///// 获取SEO METADATA  Description
        ///// </summary>
        ///// <returns></returns>
        //Dictionary<string, string> GetSiteDescription(string path);

        ///// <summary>
        ///// 获取系统设置的图片的各个尺寸
        ///// </summary>
        ///// <returns></returns>
        //List<ImageSize> GetProductImageSize();
        //List<ImageSize> GetProductAdditionalImageSize();
        //ImageSize GetSmallProductImageSize();
        //ImageSize GetMiddleProductImageSize();
        //ImageSize GetBigProductImageSize();
        //ImageSize GetLargeProductImageSize();


        //ImageSize GetSmallCatalogImageSize();
        //ImageSize GetBigCatalogImageSize();

        /// <summary>
        /// 獲取上傳圖片的大小限制
        /// </summary>
        /// <returns></returns>
        double GetProductImageLimtSize();

        ///// <summary>
        ///// 獲取catalog上傳圖片的大小限制
        ///// </summary>
        ///// <returns></returns>
        //double GetCatalogImageLimtSize();

        ///// <summary>
        ///// 获取属性的图片的大小限制
        ///// </summary>
        ///// <returns></returns>
        //double GetAttributeImageLimitSize();
        ///// <summary>
        ///// 獲取是否開啟預訂功能的值
        ///// </summary>
        ///// <returns></returns>
        //bool GetPresellSwitch();

        ///// <summary>
        ///// 獲取指定庫存交易類型的進出庫類別
        ///// </summary>
        ///// <param name="typ">庫存交易類型</param>
        ///// <returns></returns>
        //InvTransIOType? GetInvTransIOType(InvTransType typ);

        ///// <summary>
        ///// 獲取庫存交易類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetInvTransTypeLst();


        ///// <summary>
        ///// 获取SEO METADATA  Keywords
        ///// </summary>
        ///// <returns></returns>
        //List<MutiLanguage> GetSiteKeywords();


        ///// <summary>
        ///// 获取SEO METADATA  Description
        ///// </summary>
        ///// <returns></returns>
        //List<MutiLanguage> GetSiteDescription();


        ///// <summary>
        ///// 自動生成編號
        ///// </summary>
        ///// <param name="module"></param>
        ///// <param name="function"></param>
        ///// <returns></returns>
        //string AutoGenerateNumber(CodeMasterModule module, CodeMasterFunction function);

        ///// <summary>
        ///// 自动生成订单编号
        ///// </summary>
        ///// <param name="module"></param>
        ///// <param name="function"></param>
        ///// <returns></returns>
        //string AutoGenerateOrderNumber(CodeMasterModule module, CodeMasterFunction function);

        ///// <summary>
        ///// 自動生成LC(Local CourierPost快遞)的快遞編號
        ///// </summary>
        ///// <param name="module"></param>
        ///// <param name="function"></param>
        ///// <returns></returns>
        //SystemResult AutoGenerateLCTrackingNo(CodeMasterModule module, CodeMasterFunction function);


        ///// <summary>
        ///// 当前语言的描述内容
        ///// </summary>
        ///// <returns></returns>
        //MutiLanguage GetCurrentSiteDescription();
        //MutiLanguage GetCurrentSiteKeywords();


        ///// <summary>
        ///// 判斷庫存模塊是否開啟
        ///// </summary>
        ///// <returns></returns>
        //bool IsInventoryEnable();

        ///// <summary>
        ///// 是否開啟預售功能
        ///// </summary>
        ///// <returns></returns>
        //bool IsPresellEnable();

        ///// <summary>
        ///// 檢查是否需要登入購買
        ///// </summary>
        ///// <returns></returns>
        //bool CheckBuyNeedLogin();

        ///// <summary>
        ///// 获取ECShip的账号信息
        ///// </summary>
        ///// <returns></returns>
        //ECShipAccountInfo GetECShipAccountInfo();

        ///// <summary>
        ///// 生成Ship label時用這套賬號密碼
        ///// </summary>
        ///// <param name="merchId"></param>
        ///// <returns></returns>
        //ECShipAccountInfo GetECSSoazAccountInfo();

        //MailTrackingAccountInfo GetMailTrackingAccountInfo();

        ///// <summary>
        ///// 获取cms内容长度限制
        ///// </summary>
        //int GetCMSContentLimit();

        ///// <summary>
        ///// 判斷是否需要落單通知
        ///// </summary>
        ///// <returns></returns>
        //bool CheckSentRecOrderNotice();

        ///// <summary>
        ///// 獲取訂單處理超時的設定天數
        ///// </summary>
        ///// <returns></returns>
        //int GetOrderProcTimeoutDay();

        ///// <summary>
        ///// 獲取App端的推廣佈局類型列表
        ///// </summary>
        //List<CodeMaster> GetAppPromotionLayouts();

        ///// <summary>
        ///// 獲取推廣樣式列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetAppPrmtStyleTypes();

        ////List<CodeMaster> GetShippingMethods();

        ///// <summary>
        ///// 獲取客戶端類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetClientSideTypes();

        ///// <summary>
        ///// 獲取推廣封面不拒絕類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtCoverLayouts();

        ///// <summary>
        ///// 獲取推廣類型
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtGategoryLayouts();

        ///// <summary>
        ///// 獲取推廣標題佈局類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtHeaderLayouts();
        ///// <summary>
        ///// 獲取推廣邊框佈局類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtBorderLayouts();
        ///// <summary>
        ///// 獲取推廣產品列表佈局類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtProductLstLayouts();
        ///// <summary>
        ///// 獲取推廣樣式列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetPrmtStyleTypes();
        ///// <summary>
        ///// 獲取產品審批結果
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetProductApproveResults();

        ///// <summary>
        ///// 獲取與外部系統文件交互目錄
        ///// </summary>
        //string GetExtSysFileExcFolder();

        ///// <summary>
        ///// 獲取計劃任務資料列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetScheduleJobs();
        ///// <summary>
        ///// 獲取計劃任務時間間隔單位列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetServiceIntervalUnits();

        ///// <summary>
        ///// 獲取審批狀態值列表
        ///// </summary>
        //List<KeyValue> GetApproveStatuses();

        ///// <summary>
        ///// 获取用户（后台）的token过期时间（单位：秒）
        ///// </summary>
        ///// <returns></returns>
        //int GetUserAccessTokenExpire();

        ///// <summary>
        ///// 获取用户（后台）的session过期时间（单位：秒）
        ///// </summary>
        ///// <returns></returns>
        //int GetUserSessionTimeout();

        ///// <summary>
        ///// 获取会员（前台）的token过期时间（单位：秒）
        ///// </summary>
        ///// <returns></returns>
        //int GetMemberAccessTokenExpire();

        ///// <summary>
        ///// 获取会员（前台）的session过期时间（单位：秒）
        ///// </summary>
        ///// <returns></returns>
        //int GetMemberSessionTimeout();

        ///// <summary>
        ///// 获取设定的前台最大的在线客户数
        ///// </summary>
        ///// <returns></returns>
        //int GetMaxClinetOLQty();

        ///// <summary>
        ///// 獲取訂單冷靜期值
        ///// </summary>
        ///// <returns></returns>
        //int GetOrderGracePeriodValue();

        ///// <summary>
        ///// 獲取客服工作時間段列表
        ///// </summary>
        ///// <param name="merchantId">商家ID（傳入空值時則返回所有數據）</param>
        //List<CSOfficeHour> GetCSOfficeHourList(Guid? recId);

        //SystemLogo GetSystemLogos();
        ///// <summary>
        ///// 獲取代理信息
        ///// </summary>
        ///// <returns></returns>
        //WebProxy GetProxy();

        ///// <summary>
        ///// 獲取報表類型列表
        ///// </summary>
        ///// <returns></returns>
        //List<CodeMasterDto> GetReportTypeList();

        ///// <summary>
        ///// 檢查是否災難模式
        ///// </summary>
        //bool CheckContingencyMode();

        ///// <summary>
        ///// 獲取維護模式開關值
        ///// </summary>
        //bool GetMaintainModeSwitch();

        ///// <summary>
        ///// 獲取推薦產品數量
        ///// </summary>
        ///// <param name="type">客戶端類別</param>
        //int GetRecommendProdQty(ClientSideType type);

        ///// <summary>
        ///// 獲取佣金計算方式清單
        ///// </summary>
        //List<KeyValue> GetCMCalculateTypes();

        ///// <summary>
        ///// 獲取Transin API的連結地址
        ///// </summary>
        //string GetTransinApiUrl();

        ///// <summary>
        ///// 獲取Transin API的驗證憑證
        ///// </summary>
        //string GetTransinApiAuthToken();

        ///// <summary>
        ///// 獲取觀看廣告後可賺取的Transin T-Marks值
        ///// </summary>
        //decimal GetTransinDefaultAdTMarksVal();

        ///// <summary>
        ///// 獲取觀看廣告後可賺取的Transin T-Points值
        ///// </summary>
        //decimal GetTransinDefaultAdTPointsVal();

        ///// <summary>
        ///// 獲取觀看Transin廣告的時長
        ///// </summary>
        //int GetTransinDefaultAdTime();

        ///// <summary>
        ///// 获取活动符合金额
        ///// </summary>
        //List<CodeMasterDto> GetEventFitAmount();

    }
}
