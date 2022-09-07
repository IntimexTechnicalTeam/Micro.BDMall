namespace BDMall.Domain
{
    public class RnpQuestionDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }

        public string Content { get; set; }

        public string DataType { get; set; }

        public int? Type { get; set; }

        public bool? IsRequired { get; set; }

        public int? Seq { get; set; }

    }
}
