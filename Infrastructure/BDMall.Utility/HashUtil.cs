
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BDMall.Utility
{
    public  class HashUtil
    {
        public static string Base64md5(string content)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
            byte[] md5 = MD5CSP.ComputeHash(bytes);
            string c = BASE64Encode(md5);
            return c;

        }
        public static String MD5(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] result = md5.ComputeHash(bytes);
            StringBuilder strbul = new StringBuilder(400);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="source">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Md5Encrypt(string source, int length = 32)//默认参数
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            byte[] bytes = Encoding.UTF8.GetBytes(source);//这里需要区别编码的
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }


        public static String HashPwd(string passwd)
        {
            return Base64md5(passwd);
        }

        private static String BASE64Encode(byte[] content)
        {
            char[] b64char = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" + "0123456789+/").ToCharArray();

            StringBuilder sb = new StringBuilder();
            int i;
            byte v1, v2, v3, v4, v5, v6;

            for (i = 0; i < (content.Length - 2); i += 3)
            {
                v1 = (byte)(content[i] >> 2);
                v1 &= 0x3f;
                v2 = (byte)(content[i] << 4);
                v2 &= 0x30;
                v3 = (byte)(content[i + 1] >> 4);
                v3 &= 0x0f;
                v4 = (byte)(content[i + 1] << 2);
                v4 &= 0x3c;
                v5 = (byte)(content[i + 2] >> 6);
                v5 &= 0x03;
                v6 = content[i + 2];
                v6 &= 0x3f;

                sb.Append(b64char[v1]);
                switch (content.Length % 3)
                {
                    case 1:
                        v1 = (byte)(content[i] >> 2);
                        v1 &= 0x3f;
                        v2 = (byte)(content[i] << 4);
                        v2 &= 0x30;

                        sb.Append(b64char[v1]);
                        sb.Append(b64char[v2]);
                        sb.Append('=');
                        sb.Append('=');
                        break;

                    case 2:
                        v1 = (byte)(content[i] >> 2);
                        v1 &= 0x3f;
                        v2 = (byte)(content[i] << 4);
                        v2 &= 0x30;
                        v3 = (byte)(content[i + 1] >> 4);
                        v3 &= 0x0f;
                        v4 = (byte)(content[i + 1] << 2);
                        v4 &= 0x3c;

                        sb.Append(b64char[v1]);
                        sb.Append(b64char[v2 + v3]);
                        sb.Append(b64char[v4]);
                        sb.Append('=');
                        break;
                }
            }
            return (sb.ToString());
        }
    }
}
