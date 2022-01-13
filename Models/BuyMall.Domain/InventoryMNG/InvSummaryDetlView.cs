using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 庫存匯總詳細信息
    /// </summary>
    /// <remarks>前台交互用</remarks>
    public class InvSummaryDetlView : InvSummaryDetl
    {

        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        public Guid SKU => base.Sku;
      
        public Guid AttrVal1 { get; set; }
        /// <summary>
        /// 庫存屬性一描述
        /// </summary>
        public string AttrVal1Desc => base.AttrValue1Desc;
       
        /// <summary>
        /// 庫存屬性二描述
        /// </summary>
        public string AttrVal2Desc =>base.AttrValue2Desc;
        
        /// <summary>
        /// 庫存屬性三描述
        /// </summary>
        public string AttrVal3Desc =>base.AttrValue3Desc;
        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string LocName => base.WarehouseName;
        /// <summary>
        /// 庫存數量
        /// </summary>
        public int InventorySumQty => base.InventoryQty;


    }
}
