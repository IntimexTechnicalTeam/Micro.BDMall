using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class CMSContent : BaseEntity<Guid>
    {
        public Guid CategoryId { get; set; }
        public string Image { get; set; }
        public int Key { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_E { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_C { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_S { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_J { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_E { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_C { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_S { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_J { get; set; }
        //  [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_E { get; set; }
        // [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_C { get; set; }
        // [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_S { get; set; }
        // [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_J { get; set; }
       
    }
}