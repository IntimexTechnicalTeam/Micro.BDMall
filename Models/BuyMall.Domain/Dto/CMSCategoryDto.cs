namespace BDMall.Domain
{
    public class CMSCategoryDto : BaseEntity<Guid>
    {

        public Guid ParentId { get; set; }
        public int Key { get; set; }
        public int Seq { get; set; }
        public int Style { get; set; }
        
        
        public string Desc_E { get; set; }
        
        
        public string Desc_C { get; set; }
        
        
        public string Desc_S { get; set; }

        
        
        public string Desc_J { get; set; }

        
        
        public string Name_E { get; set; }
        
        
        public string Name_C { get; set; }
        
        
        public string Name_S { get; set; }
        
        
        public string Name_J { get; set; }
        
        
        public string Content_E { get; set; }
        
        
        public string Content_C { get; set; }
        
        
        public string Content_S { get; set; }
        
        
        public string Content_J { get; set; }
        
        
        
        public string Image { get; set; }
        
        public string Desc { get; set; }
        
        public string Name { get; set; }
        
        public string Content { get; set; }
        
        public List<MutiLanguage> Descriptions { get; set; }
        
        public List<MutiLanguage> Names { get; set; }
        
        public List<MutiLanguage> Contents { get; set; }
        
        public string ImgPath { get; set; }


    }
}
