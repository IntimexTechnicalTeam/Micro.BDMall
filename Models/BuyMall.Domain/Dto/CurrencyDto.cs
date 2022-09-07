namespace BDMall.Domain
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        public int Seq { get; set; }



        /// <summary>
        /// 默認貨幣
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注多語言ID
        /// </summary>
        public string RemarkTransId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备注,多语言 
        /// </summary>
        public List<MutiLanguage> Remarks { get; set; }


    }
}
