using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Domain
{
    /// <summary>
    /// 庫存交易資料
    /// </summary>
    /// <remarks>前端交互用</remarks>
    public class InvTransView
    {
        /// <summary>
        /// 發貨方ID
        /// </summary>
        public Guid TransFrom { get; set; }
        /// <summary>
        /// 收貨方ID
        /// </summary>
        public Guid TransTo { get; set; }
        /// <summary>
        /// 交易類型（採購、調撥、採購退回）
        /// </summary>
        public InvTransType TransType { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransDate { get; set; }
        /// <summary>
        /// 經手人
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNum { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 庫存交易項詳細列表
        /// </summary>
        public List<InvTransItemView> TransactionItemList { get; set; } = new List<InvTransItemView>();
    }
}
