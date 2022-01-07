using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum MailType
    {
        /// <summary>
        /// 会员注册通知
        /// </summary>
        MEM_REG_BUYER_NOT = 41,
        /// <summary>
        /// 系统发送重置链接邮件
        /// </summary>
        MEM_CHL_BUYER_NOT = 42,
        /// <summary>
        /// 会员修改密码
        /// </summary>
        MEM_CHP_BUYER_NOT = 43,

        /// <summary>
        /// 賣家（會員）咨詢，通知CS郵件
        /// </summary>
        SYS_INQ_USER_NOT = 44,

        /// <summary>
        /// 系统发送會員重置密码邮件
        /// </summary>
        MEM_TMP_BUYER_PWD = 45,

        /// <summary>
        /// 系统发送用戶重置密码邮件
        /// </summary>
        SYS_TMP_USER_PWD = 11,
        /// <summary>
        /// 发送系統用戶賬戶密码邮件
        /// </summary>
        SYS_USER_ACT_NOT = 100,
        /// <summary>
        /// 发送商戶賬戶密码邮件
        /// </summary>
        SYS_MCH_ACT_NOT = 101,

        /// <summary>
        /// 买家下单，通知买家
        /// </summary>
        ORD_ORDED_BUYER_NOT = 31,
        /// <summary>
        /// 买家下单，通知卖家
        /// </summary>
        ORD_ORDED_SAL_NOT = 32,
        /// <summary>
        /// 買家訂單付款，通知買家
        /// </summary>
        ORD_PAY_BUYER_NOT = 40,
        /// <summary>
        /// 買家訂單付款，通知賣家
        /// </summary>
        ORD_PAY_SAL_NOT = 33,
        /// <summary>
        /// 买家取消订单，通知买家
        /// </summary>
        ORD_BUYCL_BUYER_NOT = 34,
        /// <summary>
        /// 买家取消订单，通知卖家
        /// </summary>
        ORD_BUYCL_SAL_NOT = 35,
        /// <summary>
        /// 卖家确认订单，通知买家
        /// </summary>
        ORD_SALCFM_BUYER_NOT = 36,
        /// <summary>
        /// 卖家取消订单，通知买家
        /// </summary>
        ORD_SALCL_BUYER_NOT = 37,
        /// <summary>
        /// 订单发货，通知买家
        /// </summary>
        ORD_SEND_BUYER_NOT = 38,
        /// <summary>
        /// 订单收货，通知卖家
        /// </summary>
        ORD_RECE_SAL_NOT = 39,
        /// <summary>
        /// 退貨申請，通知 BuyDong Admin
        /// </summary>
        RTO_APPLY_ADMIN_NOT = 3010,
        /// <summary>
        /// 退貨審批，通知買家
        /// </summary>
        RTO_APPR_BUYER_NOT = 3011,
        /// <summary>
        /// 退貨審批，通知賣家安排退貨
        /// </summary>
        RTO_SEND_SELLER_NOT = 3012,
        /// <summary>
        /// 商家退貨發出，通知買家
        /// </summary>
        RTO_SENDED_BUYER_NOT = 3013,

        /// <summary>
        /// 推廣郵件模塊
        /// </summary>
        EMAILER = 91,

        /// <summary>
        /// 成功領取優惠券通知
        /// </summary>
        MEM_COUP_BUYER_NOT = 4061,
        /// <summary>
        /// 管理員贈送優惠券通知
        /// </summary>
        MEM_COUP_ADMIN_NOT = 4068,
        /// <summary>
        /// 註冊贈送優惠券通知
        /// </summary>
        MEM_COUP_REG_NOT = 4064,
        /// <summary>
        /// 登錄贈送優惠券通知
        /// </summary>
        MEM_COUP_LOG_NOT = 4065,
        /// <summary>
        /// 推薦贈送優惠券通知
        /// </summary>
        MEM_COUP_RECOM_NOT = 4066,
        /// <summary>
        /// 買滿贈送優惠券通知
        /// </summary>
        MEM_COUP_SHOPPING_NOT = 4069,
        /// <summary>
        /// 評論贈送優惠券通知
        /// </summary>
        MEM_COUP_COMMENT_NOT = 4067,
        /// <summary>
        /// 產品審核結果通知
        /// </summary>
        PRO_REJECT_MERCH_NOT = 2410,
        /// <summary>
        /// 商品返貨通知
        /// </summary>
        SCK_REFILL_BUYER_NOT = 2411,
        /// <summary>
        /// 缺貨通知
        /// </summary>
        SCK_OUT_MERCH_NOT = 2412,
        /// <summary>
        /// 庫存量低通知
        /// </summary>
        SCK_LOW_MERCH_NOT = 2413,
        /// <summary>
        /// 補貨提示
        /// </summary>
        SCK_REEP_MERCH_NOT = 2414,
        /// <summary>
        /// 未處理訂單提示 
        /// </summary>
        ORD_PEND_MERCH_NOT = 2415,
        /// <summary>
        /// 評論提醒管理員審核
        /// </summary>
        COMMENT_ADMIN_NOT = 2586,
        /// <summary>
        /// 評論審核完提醒商家
        /// </summary>
        COMMENT_MERCH_NOT = 2587,
        /// <summary>
        /// 商家回復提醒買家
        /// </summary>
        COMMENT_BUYER_NOT = 2588,
        /// <summary>
        ///WebStore Content Approval 
        /// </summary>
        STOCONT_APPR_NOT = 2598,
        /// <summary>
        /// 海外或內地郵品訂購用戶綁定驗證碼通知買家
        /// </summary>
        OMOS_VCD_BUYER_NOT = 102,
        /// <summary>
        /// 郵品服務訂購（只包含郵品服務）支付確認通知買家
        /// </summary>
        SOS_ORDS_BUYER_NOT = 3014,
        /// <summary>
        /// 郵品服務訂購（BuyDong 產品混合郵品服務）支付確認通知買家
        /// </summary>
        SOS_ORDM_BUYER_NOT = 3015,
        /// <summary>
        /// 郵品服務訂購增值支付確認通知買家
        /// </summary>
        SOS_TOPUP_BUYER_NOT = 3016,
    }
}
