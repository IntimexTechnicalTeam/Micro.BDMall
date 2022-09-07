namespace BDMall.Domain
{
    public class CountryDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Name_e { get; set; }
        
        public string Name_c { get; set; }
        
        public string Name_s { get; set; }
        
        public string Name_j { get; set; }

        /// <summary>
        /// 三位字母 ec-ship 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 兩位字母
        /// </summary>
        public string Code2 { get; set; }

        /// <summary>
        /// 三位字母
        /// </summary>
        public string Code3 { get; set; }

        public int Seq { get; set; }
        /// <summary>
        /// 郵政編碼是否必填
        /// </summary>
        public bool IsNeedPostalCode { get; set; }

        public List<MutiLanguage> Names { get; set; }

        public List<ProvinceDto> Procince { get; set; }


    }
}
