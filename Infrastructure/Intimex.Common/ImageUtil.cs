using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Intimex.Common
{
    public class ImageUtil
    {
        public static void GenerateThumbnailImageStream(string fileName, string newFileName, int newWidth)
        {
            Stream outputStream = new FileStream(newFileName, FileMode.Create);
            Image b = Image.FromFile(fileName);
            if (b.Width > newWidth)
            {
                int newHeight = (int)Math.Round((double)(((double)(b.Height * newWidth)) / ((double)b.Width)));
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(() => { return false; });
                Image b2 = b.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);
                if (b.RawFormat.Equals(ImageFormat.Png) | b.RawFormat.Equals(ImageFormat.Bmp))
                {
                    MemoryStream MemStream = new MemoryStream();
                    b2.Save(MemStream, b.RawFormat);
                    MemStream.WriteTo(outputStream);
                    MemStream.Close();
                }
                else
                {
                    b2.Save(outputStream, b.RawFormat);
                }
                b2.Dispose();
            }
            else if (b.RawFormat.Equals(ImageFormat.Png) | b.RawFormat.Equals(ImageFormat.Bmp))
            {
                MemoryStream MemStream = new MemoryStream();
                b.Save(MemStream, b.RawFormat);
                MemStream.WriteTo(outputStream);
                MemStream.Close();
            }
            else
            {
                b.Save(outputStream, b.RawFormat);
            }
            outputStream.Flush();
            outputStream.Close();

            b.Dispose();
        }


        public static Bitmap CreateThumbnail(Bitmap source, int thumbWi, int thumbHi, bool maintainAspect)
        {
            // return the source image if it's smaller than the designated thumbnail 
            if (source.Width < thumbWi && source.Height < thumbHi) return source;

            System.Drawing.Bitmap ret = null;
            try
            {
                int wi, hi;

                wi = thumbWi;
                hi = thumbHi;
                if (maintainAspect)
                {
                    // maintain the aspect ratio despite the thumbnail size parameters 
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }

                // original code that creates lousy thumbnails 
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Z
                ret = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(ret))
                {
                    g.InterpolationMode =
    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }
            }
            catch
            {
                ret = null;
            }

            return ret;
        }

        public static void CreateJpeg(string originalImagePath, string filePath, int width, int height)
        {
            using (Bitmap myBitmap = new Bitmap(originalImagePath))
            {
                CreateJpeg(myBitmap, filePath, 75, width, height);
            }
        }

        /// <summary>
        /// 生成宽度为width，高度为height的缩略图
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="floder"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void CreateImg(string originalImagePath, string floder, string fileName, int width, int height)
        {
            using (Bitmap myBitmap = new Bitmap(originalImagePath))
            {
                //CreateImg(myBitmap, floder, fileName, 75, width, height);
                MakeThumbnail(originalImagePath, Path.Combine(floder, fileName), width, height);
            }
        }

        public static void CreateJpeg(Bitmap myBitmap, string folder, string fileName)
        {
            CreateJpeg(myBitmap, Path.Combine(folder, fileName), 75, 200, 100);
        }

        public static void CreateJpeg(Bitmap myBitmap, string folder, string fileName, int myQuality, int width, int height)
        {
            CreateJpeg(myBitmap, Path.Combine(folder, fileName), myQuality, width, height);
        }



        public static void CreateJpeg(Bitmap myBitmap, string filePath, int myQuality, int width, int height)
        {

            System.Drawing.Image myThumbnail = CreateThumbnail(myBitmap, width, height, false);

            //Configure JPEG Compression Engine
            using (System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters())
            {
                long[] quality = new long[1];
                quality[0] = myQuality;
                using (System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality))
                {
                    encoderParams.Param[0] = encoderParam;

                    System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICI = arrayICI[x];
                            break;
                        }
                    myThumbnail.Save(filePath, jpegICI, encoderParams);
                }
            }
            myThumbnail.Dispose();
        }

        public static void CreateImg(Bitmap myBitmap, string floder, string fileName, int myQuality, int width, int height)
        {

            System.Drawing.Image myThumbnail = CreateThumbnail(myBitmap, width, height, false);

            //Configure JPEG Compression Engine
            using (System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters())
            {
                long[] quality = new long[1];
                quality[0] = myQuality;
                using (System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality))
                {
                    encoderParams.Param[0] = encoderParam;

                    System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICI = arrayICI[x];
                            break;
                        }

                    if (Directory.Exists(floder) == false)//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(floder);
                    }
                    myThumbnail.Save(Path.Combine(floder, fileName), jpegICI, encoderParams);
                }
            }
            myThumbnail.Dispose();
        }

        /// <summary>
        /// 另外一種生成縮略圖的方法，壓縮大小偏大
        /// 如果圖片小於縮略圖的尺寸，突出部分留白
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int destWidth, int destHeight)
        {
            //获取原始图片  
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            //缩略图画布宽高  
            int towidth = destWidth;
            int toheight = destHeight;
            //原始图片写入画布坐标和宽高(用来设置裁减溢出部分)  
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放)  
            int bg_x = 0;
            int bg_y = 0;
            int bg_w = towidth;
            int bg_h = toheight;
            //倍数变量  
            double multiple = 0;
            //获取宽长的或是高长与缩略图的倍数  
            if (originalImage.Width >= originalImage.Height)
                multiple = (double)originalImage.Width / (double)destWidth;
            else
                multiple = (double)originalImage.Height / (double)destHeight;
            //上传的图片的宽和高小等于缩略图  
            if (ow <= destWidth && oh <= destHeight)
            {
                //缩略图按原始宽高  
                bg_w = originalImage.Width;
                bg_h = originalImage.Height;
                //空白部分用背景色填充  
                bg_x = Convert.ToInt32(((double)towidth - (double)ow) / 2);
                bg_y = Convert.ToInt32(((double)toheight - (double)oh) / 2);
            }
            //上传的图片的宽和高大于缩略图  
            else
            {
                //宽高按比例缩放  
                bg_w = Convert.ToInt32((double)originalImage.Width / multiple);
                bg_h = Convert.ToInt32((double)originalImage.Height / multiple);
                //空白部分用背景色填充  
                bg_y = Convert.ToInt32(((double)destHeight - (double)bg_h) / 2);
                bg_x = Convert.ToInt32(((double)destWidth - (double)bg_w) / 2);
            }
            //新建一个bmp图片,并设置缩略图大小.  
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并设置背景色  
            g.Clear(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            //在指定位置并且按指定大小绘制原图片的指定部分  
            //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            try
            {
                string floder = Path.GetDirectoryName(thumbnailPath);

                if (!Directory.Exists(floder))//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(floder);
                }
                //获取图片类型  
                string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
                //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
                switch (fileExtension)
                {
                    case ".gif": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                    case ".jpg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case ".jpeg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case ".bmp": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                    case ".png": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }


            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            //System.Drawing.Image initImage = System.Drawing.Image.FromFile(originalImagePath);


            ////原图宽高均小于模版，不作处理，直接保存
            //if (initImage.Width <= destWidth && initImage.Height <= destHeight)
            //{
            //    initImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //}
            //else
            //{
            //    //模版的宽高比例
            //    double templateRate = (double)destWidth / destHeight;
            //    //原图片的宽高比例
            //    double initRate = (double)initImage.Width / initImage.Height;

            //    //原图与模版比例相等，直接缩放
            //    if (templateRate == initRate)
            //    {
            //        //按模版大小生成最终图片
            //        System.Drawing.Image templateImage = new System.Drawing.Bitmap(destWidth, destHeight);
            //        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
            //        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //        templateG.Clear(Color.White);
            //        templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, destWidth, destHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
            //        templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        try
            //        {
            //            string floder = Path.GetDirectoryName(thumbnailPath);

            //            if (!Directory.Exists(floder))//如果不存在就创建file文件夹
            //            {
            //                Directory.CreateDirectory(floder);
            //            }
            //            //获取图片类型  
            //            string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
            //            //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
            //            switch (fileExtension)
            //            {
            //                case ".gif": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
            //                case ".jpg": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
            //                case ".jpeg": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
            //                case ".bmp": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
            //                case ".png": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
            //            }
            //        }
            //        catch (System.Exception e)
            //        {
            //            throw e;
            //        }
            //        finally
            //        {
            //            initImage.Dispose();
            //            templateImage.Dispose();
            //            templateG.Dispose();
            //        }

            //    }
            //    //原图与模版比例不等，裁剪后缩放
            //    else
            //    {
            //        //裁剪对象
            //        System.Drawing.Image pickedImage = null;
            //        System.Drawing.Graphics pickedG = null;

            //        //定位
            //        Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
            //        Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

            //        //宽为标准进行裁剪
            //        if (templateRate > initRate)
            //        {
            //            //裁剪对象实例化
            //            pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
            //            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

            //            //裁剪源定位
            //            fromR.X = 0;
            //            fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
            //            fromR.Width = initImage.Width;
            //            fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

            //            //裁剪目标定位
            //            toR.X = 0;
            //            toR.Y = 0;
            //            toR.Width = initImage.Width;
            //            toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
            //        }
            //        //高为标准进行裁剪
            //        else
            //        {
            //            pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
            //            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

            //            fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
            //            fromR.Y = 0;
            //            fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
            //            fromR.Height = initImage.Height;

            //            toR.X = 0;
            //            toR.Y = 0;
            //            toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
            //            toR.Height = initImage.Height;
            //        }

            //        //设置质量
            //        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //        //裁剪
            //        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

            //        //按模版大小生成最终图片
            //        System.Drawing.Image templateImage = new System.Drawing.Bitmap(destWidth, destHeight);
            //        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
            //        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //        templateG.Clear(Color.White);
            //        templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, destHeight, destWidth), new System.Drawing.Rectangle(0, 0, pickedImage.Height, pickedImage.Width), System.Drawing.GraphicsUnit.Pixel);
            //        try
            //        {
            //            string floder = Path.GetDirectoryName(thumbnailPath);

            //            if (!Directory.Exists(floder))//如果不存在就创建file文件夹
            //            {
            //                Directory.CreateDirectory(floder);
            //            }
            //            //获取图片类型  
            //            string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
            //            //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
            //            switch (fileExtension)
            //            {
            //                case ".gif": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
            //                case ".jpg": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
            //                case ".jpeg": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
            //                case ".bmp": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
            //                case ".png": templateImage.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
            //            }
            //        }
            //        catch (System.Exception e)
            //        {
            //            throw e;
            //        }
            //        finally
            //        {
            //            initImage.Dispose();
            //            templateImage.Dispose();
            //            templateG.Dispose();
            //        }



            ////关键质量控制
            ////获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
            //ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
            //ImageCodecInfo ici = null;
            //foreach (ImageCodecInfo i in icis)
            //{
            //    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
            //    {
            //        ici = i;
            //    }
            //}
            //EncoderParameters ep = new EncoderParameters(1);
            //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

            ////保存缩略图
            //templateImage.Save(thumbnailPath, ici, ep);
            ////templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

            ////释放资源
            //templateG.Dispose();
            //templateImage.Dispose();

            //pickedG.Dispose();
            //pickedImage.Dispose();
            //}
            //    }

            //    //释放资源
            //    initImage.Dispose();

        }


        /// <summary>
        /// 获取原图旋转角度(IOS和Android相机拍的照片)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int ReadPictureDegree(string path)
        {
            int rotate = 0;
            using (var image = Image.FromFile(path))
            {
                foreach (var prop in image.PropertyItems)
                {
                    if (prop.Id == 0x112)
                    {
                        if (prop.Value[0] == 6)
                            rotate = 90;
                        if (prop.Value[0] == 8)
                            rotate = -90;
                        if (prop.Value[0] == 3)
                            rotate = 180;
                        prop.Value[0] = 1;
                    }
                }
            }
            return rotate;
        }

        /// <summary>
        ///  Rotate & Flip 旋转与翻转
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rotateFlipType"></param>
        /// <returns></returns>
        public static bool KiRotate(string path, RotateFlipType rotateFlipType)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(path))
                {
                    bitmap.RotateFlip(rotateFlipType);
                    bitmap.Save(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static void ImgSaveAs(string oldPath, string newPath)
        {
            //DirectoryInfo directoryInfo = new DirectoryInfo(oldPath);
            try
            {
                if (oldPath.IndexOf("http") < 0)
                {
                    FileInfo file = new FileInfo(oldPath);
                    if (file != null)
                    {
                        FileInfo newFile = new FileInfo(newPath);
                        if (newFile != null)
                        {
                            newFile.Delete();
                        }
                        file.MoveTo(newPath);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
