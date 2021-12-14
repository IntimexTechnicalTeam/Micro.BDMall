using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    /// <summary>
    /// 庫存交易詳細項資料
    /// </summary>
    public class InvTransactionDtl : BaseEntity<Guid>
    {
        /// <summary>
        /// 交易流水編號（YYYYMMDD000000000001）
        /// </summary>
        //[MaxLength(20)]
        //[DefaultValue("")]
        //[Column(TypeName = "varchar", Order = 3)]
        //public string SerialNo { get; set; }
        /// <summary>
        /// 交易類型（採購、調撥、採購退回 等）
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public InvTransType TransType { get; set; }
        /// <summary>
        /// 交易進出類型
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public InvTransIOType IOType { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public DateTime TransDate { get; set; }
        /// <summary>
        /// 業務記錄ID
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public Guid BizId { get; set; }
        /// <summary>
        /// 庫存單元ID
        /// </summary>
        [Required]
        [Column(Order = 8)]
        public Guid Sku { get; set; }
        //[ForeignKey("Sku")]
        //public virtual ProductSku ProductSkuInfo { get; set; }
        /// <summary>
        /// 交易數量
        /// </summary>
        [Required]
        [Column(Order = 9)]
        public int TransQty { get; set; }
        /// <summary>
        /// 倉庫ID
        /// </summary>
        [Required]
        [Column(Order = 10)]
        public Guid WHId { get; set; }
    }
}