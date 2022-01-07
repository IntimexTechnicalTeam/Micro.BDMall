using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public  class FileUtil
    {
        public static void MoveFile(string srcFileFullName, string targetPath, string fileName)
        {
            try
            {
                string filePath = Path.Combine(targetPath, fileName);

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                File.Copy(srcFileFullName, filePath, true);
                File.Delete(srcFileFullName);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 复制文件，如果文件存在则覆盖
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        public static void CopyFile(string src, string target)
        {
            try
            {
                FileInfo fi = new FileInfo(target);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                if (File.Exists(src))
                {
                    File.Copy(src, target, true);
                }
                else
                {
                    throw new Exception("文件複製異常：src:" + src + "，target:" + target);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 複製文件到指定的文件夾
        /// </summary>
        /// <param name="srcFileFullName">複製目標文件全路徑</param>
        /// <param name="targetPath">目標文件夾</param>
        /// <param name="fileName">目標文件名</param>
        public static void CopyFile(string srcFileFullName, string targetPath, string fileName)
        {
            try
            {
                string filePath = Path.Combine(targetPath, fileName);

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                if (File.Exists(srcFileFullName))
                {
                    File.Copy(srcFileFullName, filePath, true);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
            }

        }
    }
}
