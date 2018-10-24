using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yeah.Utils
{
    public class YMath
    {
        #region 私有成员
        static double _accuracyLinear = 0.1;
        #endregion

        #region 保护成员

        #endregion

        #region 公有成员
        public static double accuracyLinear { get { return _accuracyLinear; } set { _accuracyLinear = (value >= 0) ? value : 0.1; } }
        #endregion

        #region 构造函数
        public YMath()
        {
        }
        #endregion

        #region 私有有方法

        #endregion

        #region 保护方法

        #endregion

        #region 公有方法

        public static int isEqual(double a, double b)
        {
            if (Math.Abs(a - b) <= accuracyLinear)
                return 0;
            else
            {
                if (a > b) return 1;
                else return -1;
            }
        }
        #endregion
    }

}
