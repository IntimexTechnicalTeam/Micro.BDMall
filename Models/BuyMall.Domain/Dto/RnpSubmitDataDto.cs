namespace BDMall.Domain
{
    public class RnpSubmitDataDto
    {
        public Guid Id { get; set; }
        public Guid SubmitId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        public string Display { get; set; }

        public string Result { get; set; }

        public int? Qty { get; set; }

        public decimal? Price { get; set; }
    }
}
