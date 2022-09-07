namespace BDMall.Domain
{
    public class CustomMenuDetailDto: BaseDto
    {
        public Guid Id { get; set; }    
        public Guid MenuId { get; set; }

        public string Value { get; set; }

        public int Seq { get; set; }

        public bool IsAnchor { get; set; }

    }
}
