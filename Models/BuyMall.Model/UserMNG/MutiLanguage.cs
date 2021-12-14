using BDMall.Enums;
using System;


namespace BDMall.Model
{
    public class MutiLanguage
    {
        public MutiLanguage()
        {
            this.Desc = "";
        }
        public SystemLang Lang { get; set; }

        private Language? _language;
        public Language Language
        {
            get
            {
                if (!_language.HasValue)
                {
                    if (Lang == null || string.IsNullOrEmpty(Lang.Code))
                    {
                        _language = Language.E;
                    }
                    else
                    {
                        _language = (Language)Enum.Parse(typeof(Language), Lang.Code);
                    }
                }
                return _language.Value;
            }
            set
            {

                _language = value;
            }
        }


        public string Desc { get; set; }
    }
}