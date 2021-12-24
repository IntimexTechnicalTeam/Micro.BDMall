using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Common
{
    public class PathUtil
    {
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
                    folder = $"{baseFolder}/" + FileFolderEnum.StoreLogo.ToString()+"/";
                    break;
                default:
                    break;
            }

            folder = $"/ClientResources{folder}";
            folder = folder.Replace('/', '\\');
            folder = $"{RootPath}{folder}";
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
    }
}
