using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Common
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

         
    }
}
