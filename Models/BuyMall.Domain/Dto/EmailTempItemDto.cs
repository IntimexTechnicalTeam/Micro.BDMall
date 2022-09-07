namespace BDMall.Domain
{
    public class EmailTempItemDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid DescId { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 占位符号
        /// 用双大括号开始结尾，例如{{Name}}
        /// </summary>
        public string PlaceHolder { get; set; }

        /// <summary>
        /// 目标对象的类型
        /// </summary>
        public string ObjectType { get; set; }



        /// <summary>
        /// 目标对象的属性
        /// </summary>
        public string Propertity { get; set; }

        public string Remark { get; set; }

        public List<MutiLanguage> Descriptions { get; set; }

    }
}
