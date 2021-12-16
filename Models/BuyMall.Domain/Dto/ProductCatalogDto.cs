using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductCatalogDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 多语言Id
        /// </summary>      
        public Guid NameTransId { get; set; }

        /// <summary>
        /// 父目录ID
        /// </summary>    
        public Guid ParentId { get; set; } =Guid.Empty;


        /// <summary>
        /// 目录图标
        /// </summary>  
        public string SmallIcon { get; set; }

        public string BigIcon { get; set; }

        public string OriginalIcon { get; set; }
        /// <summary>
        /// 目录手机图标
        /// </summary>  

        public string MSmallIcon { get; set; }

        public string MBigIcon { get; set; }

        public string MOriginalIcon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>       
        public int Seq { get; set; } = 0;

        /// <summary>
        /// 层级
        /// </summary>

        public int Level { get; set; } = 0;

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

       
        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }

        public string Desc { get; set; } = "";

        public List<MutiLanguage> Descs { get; set; } = new List<MutiLanguage>();


    }
}
