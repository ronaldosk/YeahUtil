using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YUtils.DataStruct;

namespace YUtils
{
    public static class StringExtensions
    {
        public static string LowerFirstChar(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static string UperFirstChar(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public static string CamelCaseNameC(this string sName)
        {
            sName = sName.TrimEnd();
            DoubleLink<char> dlStr = new DoubleLink<char>();
            for (int i = 0; i < sName.Length; i++)
            {
                dlStr.Append(i, sName[i]);
            }
            string msg = "";
            dlStr.ShowAll(out msg);

            //此处实际是针对具体业务场景，针对性的重新实现了DoubleLink类型的DelNodeByCondition逻辑，参照DelNodeByCondition的代码说明
            int size = dlStr.GetSize();
            int newsize = 0, j = 0;
            if (size == 1)
                return dlStr.Get(0).ToString();
            while (size != newsize || j != newsize)
            {
                size = newsize;
                if (dlStr.Get(j) == 32)//这个是比较相等的方法
                {
                    dlStr.Del(j);
                    dlStr.Update(j, char.ToUpper(dlStr.Get(j)));//这就是特殊增加的后续数据处理逻辑
                    j--;
                }
                newsize = dlStr.GetSize();
                j++;
            }

            string outStr = "";
            for (int i = 0; i < dlStr.GetSize(); i++)
                outStr += dlStr.Get(i).ToString();


            return outStr;
        }

        public static int GetASCIILength(this string str)
        {
            if (str.Length == 0)
                return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        public static string CamelCaseName(this string str)
        {
            if (String.IsNullOrEmpty(str)) return str;
            string[] words = Regex.Split(str, "[_\\-\\. ]");
            return string.Join("", words.Select(FirstCharToUpper));
        }

        public static string LowerCamelCaseName(this string str)
        {
            if (String.IsNullOrEmpty(str)) return str;
            string[] words = Regex.Split(str, "[_\\-\\. ]");
            return string.Join("", words.Select(FirstCharToLower));
        }

        public static string FirstCharToLower(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length == 0)
                return str;
            if (str.Length == 1) return str.ToLower();
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static string FirstCharToUpper(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length == 0)
                return str;
            if (str.Length == 1) return str.ToUpper();
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public static string ToEmpty(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length == 0)
                return string.Empty;
            if (str.Trim().Equals("''")) return string.Empty;
            return str;
        }

        public static string SingleQuoteToDoubleQuote(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length == 0)
                return str;
            return Regex.Replace(str, "[']", "\"");
        }


        #region 公有方法
        public static Decimal ToDec(this string source)
        {
            try
            {
                return string.IsNullOrEmpty(source) ? Convert.ToDecimal(0) : Convert.ToDecimal(source);
            }
            catch (SystemException ex)
            {
                return Convert.ToDecimal(0);
            }
        }

        public static int ToInt(this string source)
        {
            try
            {
                return string.IsNullOrEmpty(source) ? 0 : int.Parse(source);
            }
            catch (SystemException ex)
            {
                return 0;
            }
        }

        public static long ToLong(this string source)
        {
            try
            {
                return string.IsNullOrEmpty(source) ? 0 : long.Parse(source);
            }
            catch (SystemException ex)
            {
                return 0;
            }
        }

        public static double ToDouble(this string source)
        {
            try
            {
                return string.IsNullOrEmpty(source) ? Convert.ToDouble(0) : Convert.ToDouble(source);
            }
            catch (SystemException ex)
            {
                return Convert.ToDouble(0);
            }
        }

        public static float ToFloat(this string source)
        {
            try
            {
                return string.IsNullOrEmpty(source) ? Convert.ToSingle(0) : Convert.ToSingle(source);
            }
            catch (SystemException ex)
            {
                return Convert.ToSingle(0);
            }
        }

        #endregion

        /// <summary>
        /// 按照指定的前缀过滤字符串
        /// </summary>
        /// <param name="baseString">待处理的字符串</param>
        /// <param name="PrefixStrings">定义前缀的字符串</param>
        /// <param name="splitchars">前缀的字符串中字符的分隔字符集</param>
        /// <returns></returns>
        public static string IgnorePrefix(this string baseString,string PrefixStrings, params char[] splitchars)
        {
            string[] PrefixStringArry = PrefixStrings.SortDesc(',', '，');
            foreach (var str in PrefixStringArry)
            {
                if (baseString.IndexOf(str) == 0)
                {
                    baseString = baseString.Remove(0, str.Length);
                    return baseString;
                }
            }

            return baseString;
        }

        public static string[] SortDesc(this string PrefixStrings, params char[] splitchars)
        {
            string[] PrefixStringArry = PrefixStrings.Split(splitchars);
            PrefixStringArry = PrefixStringArry.OrderByDescending(p => p).ToArray();
            return PrefixStringArry;
        }

        public static string[] SortAsc(this string PrefixStrings, params char[] splitchars)
        {
            string[] PrefixStringArry = PrefixStrings.Split(splitchars);
            PrefixStringArry = PrefixStringArry.OrderBy(p => p).ToArray();
            return PrefixStringArry;
        }

    }
}