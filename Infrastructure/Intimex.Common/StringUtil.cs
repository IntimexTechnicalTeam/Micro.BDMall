using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Intimex.Common
{
    public class StringUtil
    {

        public static string FilterHTMLFunction(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                var replaceON = Regex.Replace(val, @"on\w+=""[^""]+""", " ", RegexOptions.IgnoreCase);//替換on開頭=""的方法

                replaceON = Regex.Replace(val, @"on\w+='[^""]+'", " ", RegexOptions.IgnoreCase);//替換on開頭=''的方法

                replaceON = Regex.Replace(replaceON, @"<script>", HttpUtility.UrlEncode("<script>"), RegexOptions.IgnoreCase);

                replaceON = Regex.Replace(replaceON, @"</script>", HttpUtility.UrlEncode("</script>"), RegexOptions.IgnoreCase);

                return replaceON;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将计算机符号转换为HTML符号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToHTML(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("\n", "<br/>");
                str = str.Replace("\r", "<br/>");
                str = str.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                str = str.Replace("<<", "&lt;&lt;");
                str = str.Replace(">>", "&gt;&gt;");
                return str;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 随机生成不重复数字字符串 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNum(int length)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < length; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        /// <summary>
        /// 检测输入的字符串是否存在HTMl标签
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool CheckHasHTMLTag(string val)
        {
            bool result = false;
            if (val == null)
            {
                return result;
            }
            var pattern = "<\\s*(img|br|p|b|/p|a|div|iframe|button|script|i|html|form|input|frameset|body|table|br|label|link|li|style).*?>";
            var mateches = Regex.Matches(val, pattern);
            if (mateches.Count > 0)
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 随机生成字符串（数字和字母混和）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < length; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;

            //int rep = 0;
            //long num2 = DateTime.Now.Ticks;
            //Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            //string str = @"0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";
            //string result = string.Empty;
            //for (int i = 0; i < length; i++)
            //{
            //    result += str.Substring(0 + random.Next(36), 1).ToUpper();
            //}
            //return result;
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GetRandString(int len, int count)
        {
            List<string> list = new List<string>();
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue)
            {
                //ShowError(string.Format("Math.Pow(36, {0}) 超出 long最大值！", len));
                return null;
            }
            if (count == 1)
            {
                list.Add(GenerateRandomCode(len));
                return list;
            }
            long all_count = (long)max_value;

            int minCount = 2;
            int actualCount = count;
            if (actualCount == 1)//計算數量最少為2個
            {
                actualCount = minCount;
            }

            long stepLong = all_count / actualCount;
            if (stepLong > int.MaxValue)
            {
                //ShowError(string.Format("stepLong ({0}) 超出 int最大值！", stepLong));
                return null;
            }
            int step = (int)stepLong;
            if (step < 3)
            {
                //ShowError("step 不能小于 3!");
                return null;
            }
            long begin = 0;

            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (count == 1)
                {
                    break;
                }
                if (list.Count == actualCount)
                {
                    break;
                }
            }

            list = SortByRandom(list);

            return list;
        }

        //数字+字母
        private const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetChart(int len, long value)
        {
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }

            return str.ToString();
        }

        // <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private static List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }

            return charList;
        }
        /// <summary>
        /// 拼裝編號
        /// </summary>
        /// <param name="perfix"></param>
        /// <param name="numberFormat"></param>
        /// <param name="postfix"></param>
        /// <param name="postfixLength"></param>
        /// <returns></returns>
        public static string GenerateNumber(string perfix, string numberFormat, int postfix, int postfixLength)
        {
            string result = "";
            string midPart = "";
            string postfixNumber = "";

            midPart = DateTime.Now.ToString(numberFormat);
            postfixNumber = postfix.ToString().PadLeft(postfixLength, '0');

            result = perfix + midPart + postfixNumber;

            return result;
        }

        /// <summary>
        /// 判斷字符串是否為數字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StringIsNumberic(string value)
        {
            try
            {
                int var1 = Convert.ToInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 单词变成单数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToSingular(string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(word))
            {
                return plural1.Replace(word, "${keep}y");
            }
            else if (plural2.IsMatch(word))
            {
                return plural2.Replace(word, "${keep}");
            }
            else if (plural3.IsMatch(word))
            {
                return plural3.Replace(word, "${keep}");
            }
            else if (plural4.IsMatch(word))
            {
                return plural4.Replace(word, "${keep}");
            }

            return word;
        }
        /// <summary>
        /// 单词变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToPlural(string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(word))
            {
                return plural1.Replace(word, "${keep}ies");
            }
            else if (plural2.IsMatch(word))
            {
                return plural2.Replace(word, "${keep}s");
            }
            else if (plural3.IsMatch(word))
            {
                return plural3.Replace(word, "${keep}es");
            }
            else if (plural4.IsMatch(word))
            {
                return plural4.Replace(word, "${keep}s");
            }

            return word;
        }

        public static bool IsAllChinese(string word)
        {
            Regex reg = new Regex("^[\u4E00-\u9FA5]{0,}$");
            for (int i = 0; i < word.Length; i++)
            {
                if (!reg.IsMatch(word[i].ToString()))
                {
                    return false;
                }

            }
            return true;
        }

        public static bool IsContainChinese(string word)
        {
            Regex reg = new Regex("^[\u4E00-\u9FA5]{0,}$");
            for (int i = 0; i < word.Length; i++)
            {
                if (reg.IsMatch(word[i].ToString()))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 判断是否包含数字和字母
        /// </summary>
        /// <returns></returns>
        public static bool ExistCharNum(string content)
        {
            string regstr = @"(^[a-z]+\d+).*";
            return Regex.IsMatch(content, regstr, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(string source)
        {
            return Regex.IsMatch(source, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 生成年月日是分秒的字符串
        /// </summary>
        /// <param name="perfix"></param>
        /// <returns></returns>
        public static string GenerateDateString(string perfix)
        {
            return perfix + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

    }
}
