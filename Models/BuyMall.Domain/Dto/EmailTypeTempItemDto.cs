namespace BDMall.Domain
{
    public class EmailTypeTempItemDto : BaseDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 邮件类型
        /// </summary>
        public MailType MailType { get; set; }
        public Guid ItemId { get; set; }


    }
}
