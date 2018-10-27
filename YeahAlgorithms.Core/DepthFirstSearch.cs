using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahAlgorithms.Core
{
    public class DepthFirstSearch
    {
        public static int boxNum;
        static int[] a = default(int[]);

        /// <summary>
        /// boxNum个扑克牌放到boxNum个盒子里，总共多少种放置方式 DFS(0);
        /// </summary>
        /// <param name="step">当前走过的步数，其他参数按照当下要做什么去增加</param>
        public static void DFS(int step)
        {
            int[] book =new int[a.Length];
            if(step == boxNum+1)//如果站在第boxNum个盒子前，代表前n个盒子已经处理好【结果-判断边界条件】
            {
                //return a.ShowAll();//例如，以字符串的形式输出数组的内容。
                return;
            }
            //此时站在第step个盒子前，该做什么呢？
            for(int i=0;i<boxNum;i++)
            {
                if(book[i] == 0)//表示i号扑克牌在当前待处理
                {
                    //开始尝试处理i号扑克牌
                    a[step] = i;//将i号扑克牌放到第step个盒子中
                    book[i] = 1;//将book[i]设为1，表示i号扑克牌已经不在手上
                    //第step个盒子已经有数据，接下来需要走到下一个盒子面前
                    DFS(step + 1);
                    book[i] = 0;//将刚才尝试的扑克牌收回，开始下一次尝试
                }
            }

            return;
        }
    }
}
