using System;
using System.Text.RegularExpressions;

namespace YeahAI
{
    public class AIMath
    {
        public AIMath()
        {
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <returns><c>true</c>, if number was ised, <c>false</c> otherwise.</returns>
        /// <param name="strNumber">String number.</param>
        public bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }

        public bool IsInteger(String strNumber)
        {
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidIntegerPattern + ")");

            return objNumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// 判断是否是布尔类型
        /// </summary>
        /// <returns><c>true</c>, if boolean was ised, <c>false</c> otherwise.</returns>
        /// <param name="strBool">String bool.</param>
        public bool IsBoolean(string strBool)
        {
            if (strBool.Trim().ToLower() == "false")
                return true;
            else if (strBool.Trim().ToLower() == "true")
                return true;
            else
                return false;
            
        }
    }


}

