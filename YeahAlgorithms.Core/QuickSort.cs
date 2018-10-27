using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahAlgorithms.Core
{
    public static class QuickSort
    {
        #region 整型数组快速排序
        static int[] _a = default(int[]);
        public static int[] QSortAsc(this int[] a)
        {
            _a = a;
            int n = _a.Length;
            SortAsc(0, n - 1);
            return _a;
        }

        /// <summary>
        /// 快速排序的执行
        /// </summary>
        /// <param name="left">哨兵left</param>
        /// <param name="right">哨兵right</param>
        /// <param name="n">集合中元素总数</param>
        /// <param name="a">集合数组</param>
        /// 使用示例：Sort(0,n-1);
        static void SortAsc(int left, int right)
        {
            int i, j, t;
            int temp;
            if (left > right)
                return;

            temp = _a[left];//temp中存的就是基准数
            i = left;
            j = right;
            while (i != j)
            {
                //先从右往左找
                while (_a[j] >= temp && i < j)
                    j--;
                while (_a[i] <= temp && i < j)
                    i++;

                //交换位置
                if (i < j)
                {
                    t = _a[i];
                    _a[i] = _a[j];
                    _a[j] = t;
                }
            }
            //最终将基准数归位
            _a[left] = _a[i];
            _a[i] = temp;

            SortAsc(left, i - 1);//继续处理左边的
            SortAsc(i + 1, right);//继续处理右边的
            return;
        }

        public static int[] QSortDesc(this int[] a)
        {
            _a = a;
            int n = _a.Length;
            SortDesc(0, n - 1);
            return _a;
        }

        /// <summary>
        /// 快速排序的执行
        /// </summary>
        /// <param name="left">哨兵left</param>
        /// <param name="right">哨兵right</param>
        /// <param name="n">集合中元素总数</param>
        /// <param name="a">集合数组</param>
        /// 使用示例：Sort(0,n-1);
        static void SortDesc(int left, int right)
        {
            int i, j, t;
            int temp;
            if (left > right)
                return;

            temp = _a[left];//temp中存的就是基准数
            i = left;
            j = right;
            while (i != j)
            {
                //先从右往左找
                while (_a[j] <= temp && i < j)
                    j--;
                while (_a[i] >= temp && i < j)
                    i++;

                //交换位置
                if (i < j)
                {
                    t = _a[i];
                    _a[i] = _a[j];
                    _a[j] = t;
                }
            }
            //最终将基准数归位
            _a[left] = _a[i];
            _a[i] = temp;

            SortDesc(left, i - 1);//继续处理左边的
            SortDesc(i + 1, right);//继续处理右边的
            return;
        }

        public static string ShowAll(this int[] a)
        {
            string outMsg = "******************* 数组数据如下 *******************\r\n{";
            for (int i = 0; i < _a.Length; i++)
                outMsg += string.Format(_a[i] + ",");
            outMsg.TrimEnd(',');
            outMsg += "}\r\n******************* 数组数据展示完毕 *******************\n";

            return outMsg;
        }
        #endregion

        #region Double数组快速排序
        static double[] _d = default(double[]);
        public static double[] QSortAsc(this double[] d)
        {
            _d = d;
            int n = _d.Length;
            dSortAsc(0, n - 1);
            return _d;
        }

        /// <summary>
        /// 快速排序的执行
        /// </summary>
        /// <param name="left">哨兵left</param>
        /// <param name="right">哨兵right</param>
        /// <param name="n">集合中元素总数</param>
        /// <param name="a">集合数组</param>
        /// 使用示例：Sort(0,n-1);
        static void dSortAsc(int left, int right)
        {
            int i, j;
            double t, temp;
            if (left > right)
                return;

            temp = _d[left];//temp中存的就是基准数
            i = left;
            j = right;
            while (i != j)
            {
                //先从右往左找
                while (_d[j] >= temp && i < j)
                    j--;
                while (_d[i] <= temp && i < j)
                    i++;

                //交换位置
                if (i < j)
                {
                    t = _d[i];
                    _d[i] = _d[j];
                    _d[j] = t;
                }
            }
            //最终将基准数归位
            _d[left] = _d[i];
            _d[i] = temp;

            dSortAsc(left, i - 1);//继续处理左边的
            dSortAsc(i + 1, right);//继续处理右边的
            return;
        }

        public static double[] dQSortDesc(this double[] d)
        {
            _d = d;
            int n = _d.Length;
            dSortDesc(0, n - 1);
            return _d;
        }

        /// <summary>
        /// 快速排序的执行
        /// </summary>
        /// <param name="left">哨兵left</param>
        /// <param name="right">哨兵right</param>
        /// <param name="n">集合中元素总数</param>
        /// <param name="a">集合数组</param>
        /// 使用示例：Sort(0,n-1);
        static void dSortDesc(int left, int right)
        {
            int i, j;
            double temp, t;
            if (left > right)
                return;

            temp = _d[left];//temp中存的就是基准数
            i = left;
            j = right;
            while (i != j)
            {
                //先从右往左找
                while (_d[j] <= temp && i < j)
                    j--;
                while (_d[i] >= temp && i < j)
                    i++;

                //交换位置
                if (i < j)
                {
                    t = _d[i];
                    _d[i] = _d[j];
                    _d[j] = t;
                }
            }
            //最终将基准数归位
            _d[left] = _d[i];
            _d[i] = temp;

            dSortDesc(left, i - 1);//继续处理左边的
            dSortDesc(i + 1, right);//继续处理右边的
            return;
        }

        public static string ShowAll(this double[] d)
        {
            string outMsg = "******************* 数组数据如下 *******************\r\n{";
            for (int i = 0; i < _d.Length; i++)
                outMsg += string.Format(_d[i] + ",");
            outMsg.TrimEnd(',');
            outMsg += "}\r\n******************* 数组数据展示完毕 *******************\n";

            return outMsg;
        }
        #endregion

    }


}
