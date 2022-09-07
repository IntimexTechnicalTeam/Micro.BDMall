namespace BDMall.Domain
{
    /// <summary>
    /// 庫存屬性資料列表
    /// </summary>
    public class InvAttributeLst
    {
        /// <summary>
        /// 庫存屬性一的值列表
        /// </summary>
        public List<KeyValue> AttrIList { get; set; } = new List<KeyValue>();
        /// <summary>
        /// 庫存屬性二的值列表
        /// </summary>
        public List<KeyValue> AttrIIList { get; set; } = new List<KeyValue>();
        /// <summary>
        /// 庫存屬性三的值列表
        /// </summary>
        public List<KeyValue> AttrIIIList { get; set; } = new List<KeyValue>();
    }
}
