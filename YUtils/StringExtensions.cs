using System;

namespace YUtils
{
    public static class StringExtensions
    {
        public static string LowerFirstChar(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }

    #region 工具类--对string对象的操作
    public static class NumberString
    {
        
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

        public static double ToDouble(this string  source)
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


    }

    #endregion
}