using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;

namespace Intimex.Common
{
    public  class LangUtil
    {
        public static Language GetLang(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = "C";
                }
                return (Language)Enum.Parse(typeof(Language), lang.Trim().ToUpper());
            }
            catch (Exception)
            {

                return Language.C;
                //throw;
            }
        }

        public static List<SystemLang> GetAllLanguages(Language lang)
        {
            List<SystemLang> list = new List<SystemLang>();
            Type enumType;
            switch (lang)
            {
                case Language.E:
                    enumType = typeof(LanguageName_E);
                    break;
                case Language.C:
                    enumType = typeof(LanguageName_C);
                    break;
                case Language.S:
                    enumType = typeof(LanguageName_S);
                    break;
                case Language.J:
                    enumType = typeof(LanguageName_J);
                    break;
                case Language.P:
                    enumType = typeof(LanguageName_P);
                    break;
                default:
                    enumType = typeof(LanguageName_E);
                    break;
            }

            list.Add(new SystemLang { Code = Language.E.ToString(), Text = Enum.GetName(enumType, 0) });
            list.Add(new SystemLang { Code = Language.C.ToString(), Text = Enum.GetName(enumType, 1) });
            list.Add(new SystemLang { Code = Language.S.ToString(), Text = Enum.GetName(enumType, 2) });
            //list.Add(new SystemLang { Code = Language.J.ToString(), Text = Enum.GetName(enumType, 3) });
            //list.Add(new SystemLang { Code = Language.P.ToString(), Text = Enum.GetName(enumType, 4) });
            return list;
        }

        public static List<MutiLanguage> GetMutiLangFromTranslation(List<Translation> translates, List<SystemLang> systemLangs)
        {
            List<MutiLanguage> list = new List<MutiLanguage>();
            if (translates == null || translates.Count == 0 || translates[0] == null)
            {
                foreach (var lang in systemLangs)
                {
                    list.Add(new MutiLanguage { Desc = "", Lang = lang });
                }
            }
            else
            {
                bool exist = false;
                foreach (var supportLang in systemLangs)
                {
                    exist = false;
                    foreach (var tran in translates)
                    {
                        if (tran != null)
                        {
                            if (supportLang.Code.Trim() == tran.Lang.ToString().Trim())
                            {
                                exist = true;
                                list.Add(new MutiLanguage { Desc = tran.Value ?? "", Lang = supportLang });
                            }
                        }

                    }
                    if (!exist)
                    {
                        list.Add(new MutiLanguage { Desc = "", Lang = supportLang });
                    }
                }
            }

            return list;
        }
    }
}
