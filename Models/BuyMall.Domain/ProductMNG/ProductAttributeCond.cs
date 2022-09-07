namespace BDMall.Domain
{
    public class ProductAttributeCond
    {
        public PageInfo PageInfo { get; set; } = new PageInfo();
        /// <summary>
        /// 属性描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 是否库存属性
        /// </summary>
        public bool IsInv { get; set; }

    }
}
