
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BDMall.Utility
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

        /// <summary>
        /// 獲取多種語言
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="namePrefix"></param>
        /// <param name="systenLangs">系统支持的语言</param>
        /// <returns></returns>
        public static List<MutiLanguage> GetMutiLang<T>(T entity, string namePrefix, List<SystemLang> systenLangs)
        {
            List<MutiLanguage> list = new List<MutiLanguage>();

            try
            {
                if (entity != null)
                {
                    Type t = entity.GetType();
                    foreach (SystemLang item in systenLangs)
                    {
                        MutiLanguage mutiLanguage = new MutiLanguage();
                        var columnName = namePrefix + "_" + item.Code.ToString().ToLower();
                        PropertyInfo info = t.GetProperty(columnName);
                        if (info != null)
                        {
                            object value = info.GetValue(entity, null);
                            if (value == null)
                            {
                                mutiLanguage.Desc = "";
                            }
                            else
                            {
                                mutiLanguage.Desc = value.ToString();
                            }
                        }
                        else
                        {
                            mutiLanguage.Desc = "";
                        }

                        mutiLanguage.Lang = item;

                        list.Add(mutiLanguage);

                    }
                }
                else
                {
                    foreach (SystemLang item in systenLangs)
                    {
                        MutiLanguage mutiLanguage = new MutiLanguage();

                        mutiLanguage.Lang = item;
                        mutiLanguage.Desc = "";

                        list.Add(mutiLanguage);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
    }
}
