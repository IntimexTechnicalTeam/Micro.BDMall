namespace BDMall.Domain
{
    public class CMSContentDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Image { get; set; }
        public int Key { get; set; }


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
        // [MaxLength(10000)]

        public string Content_J { get; set; }

        public string Desc { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public List<MutiLanguage> Descriptions { get; set; }

        public List<MutiLanguage> Names { get; set; }

        public List<MutiLanguage> Contents { get; set; }

        public string CategoryName { get; set; }

        public string ImgPath { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string IsActiveDesc
        {
            get
            {
                if (IsActive)
                {
                    return BDMall.Resources.Label.Yes;
                }
                else
                {
                    return BDMall.Resources.Label.No;
                }
            }
        }

        public string CreateDateDesc
        {
            get
            {
                return CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public string UpdateDateDesc
        {
            get
            {
                if (UpdateDate != null)
                {
                    return UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return string.Empty;
            }
        }
        public Guid ParentCategoryId { get; set; }

        /// <summary>
        /// 內容字數限制
        /// </summary>
        public int ContentLimit { get; set; }
    }
}
