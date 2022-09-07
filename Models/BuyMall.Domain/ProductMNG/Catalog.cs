namespace BDMall.Domain
{
    public class Catalog
    {
        public Guid Id { get; set; }

        public string Name { get; set; }    

        public string ImgS { get;set; }  

        public Guid ParentId { get;set;}    

        public List<Catalog> Children { get; set; } = new List<Catalog>();
    }
}
