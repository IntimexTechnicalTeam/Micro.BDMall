using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class NameUtil
    {
        public static string GetOrderStatusName(string status)
        {
            try
            {
                return Enum.GetName(typeof(OrderStatus), int.Parse(status));
            }
            catch (Exception)
            {
                return "ERROR";
                //throw;
            }
        }

        public static string GetCountryName(string language, Country country)
        {
            if (country == null)
            {
                return "";
            }

            switch (language.ToUpper())
            {
                case "E":
                    return country.Name_e;

                case "C":
                    return country.Name_c;
                case "S":
                    return country.Name_s;
                case "J":
                    return country.Name_j;
                case "P":
                    return country.Name_j;
                default:
                    return string.Empty;
            }
        }

        public static string GetProviceName(string language, Province province)
        {
            if (province == null)
            {
                return "";
            }

            switch (language.ToUpper())
            {
                case "E":
                    return province.Name_e;

                case "C":
                    return province.Name_c;
                case "S":
                    return province.Name_s;
                case "J":
                    return province.Name_j;
                case "P":
                    return province.Name_j;
                default:
                    return string.Empty;
            }
        }

        public static string GetExpressCompanyName(string language, ExpressCompany express)
        {
            if (express == null)
            {
                return "";
            }
            switch (language.ToUpper())
            {
                //case "E":
                //    return express.expressage_mode_e;

                //case "C":
                //    return express.expressage_mode_c;
                //case "S":
                //    return express.expressage_mode_s;
                //case "J":
                //    return express.expressage_mode_j;
                //case "P":
                //    return express.expressage_mode_j;
                default:
                    return string.Empty;
            }
        }

        //public static string GetCatalogName(string language, WS.DBModel.ptxwebstore_b2b_catalog catalog)
        //{

        //    if (catalog == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return catalog.log_name_e;

        //        case "C":
        //            return catalog.log_name_c;
        //        case "S":
        //            return catalog.log_name_s;
        //        case "J":
        //            return catalog.log_name_j;
        //        case "P":
        //            return catalog.log_name_j;
        //        default:
        //            return string.Empty;
        //    }
        //}
        //public static string GetZoneRemark(string language, WS.DBModel.ptxwebstore_b2b_zone zone)
        //{
        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return zone.zone_remark_e;

        //        case "C":
        //            return zone.zone_remark_c;
        //        default:
        //            return string.Empty;
        //    }
        //}

        //public static string GetPayMethodName(string language, WS.DBModel.ptxwebstore_b2b_PaymentMethod catalog)
        //{
        //    if (catalog == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return catalog.pay_Name_e;

        //        case "C":
        //            return catalog.pay_Name_c;
        //        case "S":
        //            return catalog.pay_Name_s;
        //        case "J":
        //            return catalog.pay_Name_j;
        //        case "P":
        //            return catalog.pay_Name_j;
        //        default:
        //            return string.Empty;
        //    }
        //}

        ///// <summary>
        ///// 獲取屬性的名稱
        ///// </summary>
        ///// <param name="language"></param>
        ///// <param name="brand"></param>
        ///// <returns></returns>
        //public static string GetAttrTypeName(string language, WS.DBModel.ptxwebstore_b2b_brand brand)
        //{

        //    if (brand == null)
        //    {
        //        return "";
        //    }
        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return brand.desc_e;

        //        case "C":
        //            return brand.desc_c;
        //        case "S":
        //            return brand.desc_s;
        //        case "J":
        //            return brand.desc_j;
        //        //case "P":
        //        //    return brand.desc_p;
        //        default:
        //            return string.Empty;
        //    }
        //}
        ///// <summary>
        ///// 獲取屬性值的名稱
        ///// </summary>
        ///// <param name="language"></param>
        ///// <param name="catalog"></param>
        ///// <returns></returns>
        //public static string GetAttrValueName(string language, WS.DBModel.ptxwebstore_b2b_Attribute catalog)
        //{
        //    if (catalog == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return catalog.att_name_e;

        //        case "C":
        //            return catalog.att_name_c;
        //        case "S":
        //            return catalog.att_name_s;
        //        case "J":
        //            return catalog.att_name_j;
        //        case "P":
        //            return catalog.att_name_j;
        //        default:
        //            return string.Empty;
        //    }



        //}


        ///// <summary>
        ///// 獲取圖片屬性
        ///// </summary>
        ///// <param name="language"></param>
        ///// <param name="imageType"></param>
        ///// <returns></returns>
        //public static string GetImageTypeDesc(string language, WS.DBModel.ptxwebstore_b2b_image_type imageType)
        //{
        //    if (imageType == null)
        //    {
        //        return "";
        //    }
        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return imageType.type_desc_e;

        //        case "C":
        //            return imageType.type_desc_c;
        //        case "S":
        //            return imageType.type_desc_s;
        //        case "J":
        //            return imageType.type_desc_j;
        //        case "P":
        //            return imageType.type_desc_e;
        //        default:
        //            return string.Empty;
        //    }
        //}
        ///// <summary>
        ///// 獲取產品類型名稱
        ///// </summary>
        ///// <param name="language"></param>
        ///// <param name="productType"></param>
        ///// <returns></returns>
        //public static string GetProductTypeName(string language, ptxwebstore_b2b_prodtype productType)
        //{
        //    if (productType == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return productType.desc_e;
        //        case "C":
        //            return productType.desc_c;
        //        case "S":
        //            return productType.desc_s;
        //        case "J":
        //            return productType.desc_j;
        //        case "P":
        //            return productType.desc_e;
        //        default:
        //            return string.Empty;
        //    }
        //}

        //public static string GetSpecificationName(string language, ptxwebstore_b2b_product product)
        //{
        //    if (product == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return product.specification_e;
        //        case "C":
        //            return product.specification_c;
        //        case "S":
        //            return product.specification_s;
        //        case "J":
        //            return product.specification_j;
        //        case "P":
        //            return product.specification_e;
        //        default:
        //            return string.Empty;
        //    }
        //}
        //public static string GetLocationName(string language, ptxwebstore_b2b_location location)
        //{
        //    if (location == null)
        //    {
        //        return "";
        //    }

        //    switch (language.ToUpper())
        //    {
        //        case "E":
        //            return location.loc_name_e;
        //        case "C":
        //            return location.loc_name_c;
        //        case "S":
        //            return location.loc_name_s;
        //        case "J":
        //            return location.loc_name_j;
        //        case "P":
        //            return location.loc_name_e;
        //        default:
        //            return string.Empty;
        //    }
        //}


        /// <summary>
        /// 根據當前語言獲取對象中對應的語言
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="language"></param>
        /// <param name="name"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetMultiResourceName<T>(string language, string name, T t)
        {
            string result = "";
            string resourceName = name + "_" + language.ToLower();

            if (t != null)
            {
                PropertyInfo[] properites = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo item in properites)
                {
                    if (item.Name.Trim() == resourceName.Trim())
                    {
                        object objResult = item.GetValue(t, null);
                        result = (objResult == null ? string.Empty : objResult).ToString().Trim();
                        break;
                    }
                }
            }
            return result;

        }
        /// <summary>
        /// 获取CMS内容的主体
        /// </summary>
        /// <param name="language"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetCMSContent(Language language, CMSContent content)
        {
            if (content == null)
            {
                return "";
            }

            switch (language)
            {
                case Language.E:
                    return content.Content_E;
                case Language.C:
                    return content.Content_C;
                case Language.S:
                    return content.Content_S;
                case Language.J:
                    return content.Content_J;
                case Language.P:
                    return content.Content_E;
                default:
                    return string.Empty;
            }
        }

        public static string GetCMSDesc(Language language, CMSContent content)
        {
            if (content == null)
            {
                return "";
            }

            switch (language)
            {
                case Language.E:
                    return content.Desc_E;
                case Language.C:
                    return content.Desc_C;
                case Language.S:
                    return content.Desc_S;
                case Language.J:
                    return content.Desc_J;
                case Language.P:
                    return content.Desc_E;
                default:
                    return string.Empty;
            }
        }
        public static string GetCMSName(Language language, CMSContent content)
        {
            if (content == null)
            {
                return "";
            }

            switch (language)
            {
                case Language.E:
                    return content.Name_E;
                case Language.C:
                    return content.Name_C;
                case Language.S:
                    return content.Name_S;
                case Language.J:
                    return content.Name_J;
                case Language.P:
                    return content.Name_E;
                default:
                    return string.Empty;
            }
        }
        public static string GetCategoryName(Language language, CMSCategory content)
        {
            if (content == null)
            {
                return "";
            }

            switch (language)
            {
                case Language.E:
                    return content.Name_E;
                case Language.C:
                    return content.Name_C;
                case Language.S:
                    return content.Name_S;
                case Language.J:
                    return content.Name_J;
                case Language.P:
                    return content.Name_E;
                default:
                    return string.Empty;
            }
        }

        public static string GetCMSTitle(Language language, CMSContent content)
        {
            if (content == null)
            {
                return "";
            }

            switch (language)
            {
                case Language.E:
                    return content.Name_E;
                case Language.C:
                    return content.Name_C;
                case Language.S:
                    return content.Name_S;
                case Language.J:
                    return content.Name_J;
                case Language.P:
                    return content.Name_E;
                default:
                    return string.Empty;
            }
        }


        /// <summary>
        /// 字符串內是否存在特殊字符（只允許包含字母、汉字、空格和逗号的組合）
        /// </summary>
        /// <param name="content">待查詢內容</param>
        /// <returns>是否存在特殊字符</returns>
        public static bool IsExistSpecialCharacter(string content)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(content))
            {
                string regStr = @"^[a-zA-Z\s\u4e00-\u9fa5,，]*$";
                Regex myRegex = new Regex(regStr);
                if (!myRegex.IsMatch(content))
                {
                    res = true;
                }
            }
            return res;
        }
    }
}
