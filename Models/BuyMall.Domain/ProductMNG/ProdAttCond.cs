namespace BDMall.Domain
{
    public class ProdAttCond
    {
        /// <summary>
        /// 0-獲取所有屬性，1//庫存屬性 2//非庫存屬性
        /// </summary>
        public int Type { get; set; }

        public List<Guid> CatalogIds { get; set; } 
    }
}
