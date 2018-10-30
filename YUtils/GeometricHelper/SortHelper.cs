using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUtils.Core.GeometricHelper
{
    /// <summary>
    /// 几何对象集合数据的排序处理类
    /// </summary>
    public class SortHelper
    {
        /// <summary>
        /// 判断顺时针排序
        /// </summary>
        /// <param name="vPoints"></param>
        public static void ClockwiseSortPoints(List<Point> vPoints, out Point center)
        {
            center = new Point();
            
            //计算重心
            //double X = 0, Y = 0;
            //for (int i = 0; i < vPoints.Count; i++)
            //{
            //    X += vPoints[i].X;
            //    Y += vPoints[i].Y;
            //}
            //center.X = (int)X / vPoints.Count;
            //center.Y = (int)Y / vPoints.Count;

            center = FindBasePoint(vPoints);

            //冒泡排序
            for (int i = 0; i < vPoints.Count - 1; i++)
            {
                for (int j = 0; j < vPoints.Count - i - 1; j++)
                {
                    if (IsAntiClockwise(vPoints[j], vPoints[j + 1], center))
                    {
                        Point tmp = vPoints[j];
                        vPoints[j] = vPoints[j + 1];
                        vPoints[j + 1] = tmp;
                    }
                }
            }
        }

        //若点b大于点a,即点b在点a逆时针方向,返回true,否则返回false
        static bool IsAntiClockwise(Point a, Point b, Point center)
        {
            //begin del by ye 向量算法不必多此一举
            //if (a.X >= 0 && b.X< 0)
            //    return true;
            //if (a.X == 0 && b.X == 0)
            //    return a.Y > b.Y;
            //end del by ye s

            //向量OA和向量OB的叉积
            //叉积>0,则说明pa-pb-po的线路是逆时针
            //叉积<0,则说明pa-pb-po的线路是顺时针
            int det = Convert.ToInt32((a.X - center.X) * (b.Y - center.Y) - (b.X - center.X) * (a.Y - center.Y));
            if (det > 0)
                return true; //po - pa - pb   路径的走向为逆时针
            if (det < 0)
                return false;//po - pa - pb   路径的走向为顺时针
            //向量OA和向量OB共线，以距离判断大小
            double d1 = (a.X - center.X) * (a.X - center.X) + (a.Y - center.Y) * (a.Y - center.Y);
            double d2 = (b.X - center.X) * (b.X - center.Y) + (b.Y - center.Y) * (b.Y - center.Y);
            return d1 > d2;//po - pa - pb   路径的走向可理解为逆时针
        }

        public static Point FindBasePoint(List<Point> points)    //找基点，按y从小到大，如果y相同，按x从左到右
        {
            int i, j = 0;
            Point t = points[0];
            for (i = 1; i < points.Count; i++)
            {
                if (t.Y > points[i].Y || (t.Y == points[i].Y && t.X > points[i].X))
                {
                    j = i;
                    t = points[i];
                }
            }
            //t = points[0];
            //points[0] = points[j];
            //points[j] = t;

            t = points[j];
            return t;
        }
    }
}
