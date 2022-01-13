using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Model
{
    /// <summary>
    /// 庫存調撥單
    /// </summary>
    public class RelocationOrder : BaseEntity<Guid>
    {
        /// <summary>
        /// 單號
        /// </summary>
        [Required]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 3)]
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 調出倉庫ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid ExportWHId { get; set; }
        //[ForeignKey("ExportWHId")]
        //public virtual Warehouse ExportWarehouse { get; set; }
        /// <summary>
        /// 調入倉庫ID
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public Guid ImportWHId { get; set; }
        //[ForeignKey("ImportWHId")]
        //public virtual Warehouse ImportWarehouse { get; set; }
        /// <summary>
        /// 調撥時間
        /// </summary>
        [Required]
        [Column(Order = 6)]
        public DateTime RelocateDate { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 7)]
        public string Remarks { get; set; } = "";
    }
}
