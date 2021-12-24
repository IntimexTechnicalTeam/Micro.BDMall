using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Enums
{

    //     系统模块首位1
    //     产品模块首位2
    //     订单模块首位3
    //     会员模块首位4
    //     商家模块首位5
    //     配送模块首位6
    //     市场模块首位7
    //     仓储模块首位8
    //     客戶服務模塊首位9

    /// <summary>
    ///  系统模块首位1
    ///  </summary>
    public enum SystemErrorEnum
    {
        Error = 1,
        LogOut = 1000,
        NoPermission = 1001,
        /// <summary>
        /// 數據庫配置異常
        /// </summary>
        DBConfigError = 1002,
        /// <summary>
        /// 傳入的數據為空
        /// </summary>
        IncomingDataEmpty = 1003,
        /// <summary>
        /// 記錄不存在
        /// </summary>
        RecordNotExsit = 1004,
        /// <summary>
        /// 數據轉換失敗
        /// </summary>
        InternalConversionErr = 1005,
        #region 文件目錄相關
        /// <summary>
        /// 目錄不存在
        /// </summary>
        FolderNotExsit = 1101,
        /// <summary>
        /// 目錄已存在
        /// </summary>
        FolderExsit = 1102,
        /// <summary>
        /// 文件不存在
        /// </summary>
        FileNotExsit = 1103,
        /// <summary>
        /// 目錄層級超限
        /// </summary>
        FolderLayerOverrun = 1104,
        /// <summary>
        /// 文件類型不符合
        /// </summary>
        FileTypeNotMatch = 1105,
        #endregion
        #region 郵件相關

        /// <summary>
        /// 信息參數錯誤
        /// </summary>
        MessageParamErr = 1201,
        /// <summary>
        /// 信息發送的收發對象為空
        /// </summary>
        MessageObjectEmpty = 1202,
        /// <summary>
        /// 信息不存在
        /// </summary>
        MessageNotExsit = 1203,
        /// <summary>
        /// 公告不存在
        /// </summary>
        AnnouncementNotExsit = 1204,
        /// <summary>
        /// 公告已發佈
        /// </summary>
        AnnouncementIsPublished = 1205,

        #endregion
    }
    /// <summary>
    ///  产品模块首位2
    /// </summary>
    public enum ProductErrorEnum
    {
        LoginOut = 2000,
        NoPermission = 2001,
        /// <summary>
        /// 傳入的數據為空
        /// </summary>
        IncomingDataEmpty = 2003,
        /// <summary>
        /// 記錄不存在
        /// </summary>
        RecordNotExsit = 2004,
        /// <summary>
        /// 內部轉換異常
        /// </summary>
        InternalConversionError = 2005,
        /// <summary>
        /// 產品已刪除
        /// </summary>
        ProductDeleted = 2006,
    }
    /// <summary>
    /// 商家模块異常問題枚舉
    /// </summary>
    /// <remarks>首位5</remarks>
    public enum MerchantErrorEnm
    {
        /// <summary>
        /// 記錄不存在
        /// </summary>
        RecordNotExsit = 5001,
        /// <summary>
        /// 傳入的數據為空
        /// </summary>
        IncomingDataEmpty = 5002,
        /// <summary>
        /// 重複數據
        /// </summary>
        RepeatingData = 5003,
        /// <summary>
        /// 內部轉換異常
        /// </summary>
        InternalConversionError = 5004
    }

    /// <summary>
    /// 市場推廣模塊異常問題枚舉
    /// </summary>
    public enum MarketingErrorEnum
    {
        /// <summary>
        /// 記錄不存在
        /// </summary>
        RecordNotExsit = 7001,
        /// <summary>
        /// 傳入的數據為空
        /// </summary>
        IncomingDataEmpty = 7002,
        /// <summary>
        /// 重複數據
        /// </summary>
        RepeatingData = 7003,
        /// <summary>
        /// 內部轉換異常
        /// </summary>
        InternalConversionErr = 7004,
        /// <summary>
        /// 數據校驗失敗
        /// </summary>
        DataVerifyFail = 7005,
        /// <summary>
        /// 圖片處理異常
        /// </summary>
        ImgHandleErr = 7006
    }

    /// <summary>
    /// 倉存模块異常問題枚舉
    /// </summary>
    public enum InventoryErrorEnum
    {
        /// <summary>
        /// 記錄不存在
        /// </summary>
        RecordNotExsit = 8001,
        /// <summary>
        /// 傳入的數據為空
        /// </summary>
        IncomingDataEmpty = 8002,
        /// <summary>
        /// 重複數據
        /// </summary>
        RepeatingData = 8003,
        /// <summary>
        /// 內部轉換異常
        /// </summary>
        InternalConversionErr = 8004,
        /// <summary>
        /// 庫存為零
        /// </summary>
        InventoryQtyZero = 8005,
        /// <summary>
        /// 總庫存數量不足扣除
        /// </summary>
        InventoryTotalQtyNotEnough = 8006,
        /// <summary>
        /// 倉庫庫存不足扣除
        /// </summary>
        InventoryQtyNotEnough = 8007,
        /// <summary>
        /// 附屬數據不完整
        /// </summary>
        AttachedDataIncomplete = 8008,
        /// <summary>
        /// 庫存交易類型不存在
        /// </summary>
        InvTransTypeNotExsit = 8009,
        /// <summary>
        /// 庫存交易出入庫類型不存在
        /// </summary>
        InvTransIOTypeNotExsit = 8010,
        /// <summary>
        /// 庫存調撥的交易數據不完整
        /// </summary>
        InvTransRelocationIncomplete = 8011,
        /// <summary>
        /// 庫存調撥的交易數據不正確
        /// </summary>
        InvTransRelocationIncorrect = 8012,
        /// <summary>
        /// 庫存保留數據不正確
        /// </summary>
        InvtHoldNotCorrect = 8013,
        /// <summary>
        /// 未知庫存交易類型
        /// </summary>
        UnknownInvTransType = 8100,
    }

    /// <summary>
    /// 客戶服務模塊錯誤
    /// </summary>
    public enum CustomerServiceErrorEnum
    {

    }

    public enum OrderErrorEnum
    {
        NoExchangeRateList = 3000,
        NoExchangeRate = 3001,
        /// <summary>
        /// 需要登入
        /// </summary>
        NeedLogin = 3002,
        /// <summary>
        /// 會員組別不符合
        /// </summary>
        MemberGroupNotMap = 3003,

        /// <summary>
        /// 下架
        /// </summary>
        OffSale = 3004,
        /// <summary>
        /// 低於至低購買量
        /// </summary>
        LessThenMin = 3005,
        /// <summary>
        /// 高於最高購買量
        /// </summary>
        MoreThenMax = 3006,

        /// <summary>
        /// 售罄
        /// </summary>
        Sellout = 3007,

        /// <summary>
        /// 產品失效
        /// </summary>
        ProductInvalid = 3008,
        /// <summary>
        /// 庫存數量不足
        /// </summary>
        OutOfStock = 3009,

        /// <summary>
        /// 不在发售时间
        /// </summary>
        NotSelling = 3010,
    }
}
