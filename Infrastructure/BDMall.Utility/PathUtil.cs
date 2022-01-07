using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class PathUtil
    {
        public static string GetBoostrapTableLang(string lang)
        {
            //string lang = WSCookie.GetUserLanguage();
            string url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-TW.min.js";
            if (lang == "E")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-en-US.min.js";
            }
            else if (lang == "C")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-TW.min.js";

            }
            else if (lang == "S")
            {
                url = "~/Scripts/bootstraptable/locale/bootstrap-table-zh-CN.min.js";
            }
            return url;
        }

        /// <summary>
        /// 生成物理路径
        /// </summary>
        /// <param name="RootPath"></param>
        /// <param name="merchantId"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetPhysicalPath(string RootPath, string merchantId, FileFolderEnum module)
        {
            string folder = string.Empty;

            string baseFolder = $"/{merchantId}";
            switch (module)
            {
                case FileFolderEnum.PaymentMehod:
                    folder = $"{baseFolder}/paymethod/images";
                    break;
                case FileFolderEnum.TempPath:
                    folder = $"/temp";
                    break;
                case FileFolderEnum.CSImg:
                    folder = $"{baseFolder}/csimage";
                    break;
                case FileFolderEnum.DefaultImage:
                    folder = $"/defaultimage";
                    break;
                case FileFolderEnum.MassUploadFilePath:
                    folder = $"/massuploadfile";
                    break;
                case FileFolderEnum.Attribute:
                    folder = $"{baseFolder}/attribute";
                    break;
                case FileFolderEnum.Promotion:
                    folder = $"{baseFolder}/promotion";
                    break;
                case FileFolderEnum.PromotionBanner:
                    folder = $"{baseFolder}/prmtbanner";
                    break;
                case FileFolderEnum.Product:
                    folder = $"{baseFolder}/product";
                    break;
                case FileFolderEnum.Catalog:
                    folder = $"{baseFolder}/catalog";
                    break;
                case FileFolderEnum.MenuIcon:
                    folder = $"/system/menu/";
                    break;
                case FileFolderEnum.MerchantPromotion:
                    folder = $"{baseFolder}/merchantpromotion/";
                    break;
                case FileFolderEnum.Cms:
                    folder = $"{baseFolder}/cms/";
                    break;
                case FileFolderEnum.ProductComment:
                    folder = $"{baseFolder}/productcomment";
                    break;
                case FileFolderEnum.OrderECShipLabel:
                    folder = baseFolder + "/orderecshiplabel";
                    break;
                case FileFolderEnum.ProductDefaultImage:
                    folder = "/images/system/";
                    break;
                case FileFolderEnum.CouponImage:
                    folder = $"{baseFolder}/CouponImage/";
                    break;
                case FileFolderEnum.ROrderImage:
                    folder = $"{baseFolder}/RtnOrderImage/";
                    break;
                case FileFolderEnum.CustomMenu:
                    folder = $"{baseFolder}/Menu/";
                    break;
                case FileFolderEnum.Video:
                    folder = "/video/";
                    break;
                case FileFolderEnum.StoreLogo:
                    folder = $"{baseFolder}/" + FileFolderEnum.StoreLogo.ToString() + "/";
                    break;
                default:
                    break;
            }

            folder = $"/ClientResources{folder}";
            folder = folder.Replace('/', '\\');
            folder = $"{RootPath}{folder}";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// 生成相对路径
        /// </summary>
        /// <param name="RootPath"></param>
        /// <param name="merchantId"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static string GetRelativePath(string merchantId, FileFolderEnum module)
        {
            string folder = string.Empty;
            string baseFolder = $"/ClientResources/{ merchantId}";
            switch (module)
            {
                case FileFolderEnum.PaymentMehod:
                    folder = $"{baseFolder}/paymethod/images";
                    break;
                case FileFolderEnum.TempPath:
                    folder = $"/ClientResources/temp";
                    break;
                case FileFolderEnum.CSImg:
                    folder = $"/ClientResources/csimage";
                    break;
                case FileFolderEnum.DefaultImage:
                    folder = $"/ClientResources/defaultimage";
                    break;
                case FileFolderEnum.MassUploadFilePath:
                    folder = $"/ClientResources/massuploadfile";
                    break;
                case FileFolderEnum.Attribute:
                    folder = $"{baseFolder}/attribute";
                    break;
                case FileFolderEnum.Promotion:
                    folder = $"{baseFolder}/promotion";
                    break;
                case FileFolderEnum.PromotionBanner:
                    folder = $"{baseFolder}/prmtbanner";
                    break;
                case FileFolderEnum.Product:
                    folder = $"{baseFolder}/product";
                    break;
                case FileFolderEnum.Catalog:
                    folder = $"{ baseFolder}/catalog";
                    break;
                case FileFolderEnum.MerchantPromotion:
                    folder = $"{baseFolder}/merchantpromotion";
                    break;
                case FileFolderEnum.MenuIcon:
                    folder = $"/ClientResources/system/menu";
                    break;
                case FileFolderEnum.Cms:
                    folder = $"{baseFolder}/cms";
                    break;
                case FileFolderEnum.ProductComment:
                    folder = $"{baseFolder}/productcomment";
                    break;
                case FileFolderEnum.OrderECShipLabel:
                    folder = $"{baseFolder}/orderecshiplabel";
                    break;
                case FileFolderEnum.CouponImage:
                    folder = $"{baseFolder}/CouponImage";
                    break;
                case FileFolderEnum.ROrderImage:
                    folder = $"{baseFolder }/RtnOrderImage";
                    break;
                case FileFolderEnum.CustomMenu:
                    folder = $"{baseFolder }/Menu";
                    break;
                case FileFolderEnum.StoreLogo:
                    folder = $"{baseFolder}/" + FileFolderEnum.StoreLogo.ToString() + "/";
                    break;
                default:
                    break;
            }

            return folder;
        }

        public static string GetDefaultImagePath()
        {
            var folder = "images\\system\\Default.png";

            return folder;
        }

        //public static string GetPMServer()
        //{
        //    var server = System.Configuration.ConfigurationManager.AppSettings["PMServer"];
        //    if (string.IsNullOrEmpty(server))
        //    {
        //        return GetSiteRoot();
        //    }
        //    else
        //    {

        //        return server;
        //    }
        //}
        ///// <summary>
        /////  獲取網站訪問根目錄 http://localhost:555
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSiteRoot()
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        var request = HttpContext.Current.Request;
        //        string path = request.Url.Scheme + "://" + request.Url.Host;
        //        if (request.Url.Port == 80)
        //        {
        //            return path;
        //        }
        //        else
        //        {
        //            return path + ":" + request.Url.Port;
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

    }
}
