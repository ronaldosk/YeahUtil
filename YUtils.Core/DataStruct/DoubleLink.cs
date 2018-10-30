//双向链表
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUtils.DataStruct
{
    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BdNode<T>
    {
        public T Data { set; get; }
        public BdNode<T> Next { set; get; }
        public BdNode<T> Prev { set; get; }
        public BdNode(T val, BdNode<T> prev, BdNode<T> next)
        {
            this.Data = val;
            this.Prev = prev;
            this.Next = next;
        }
    }

    public class DoubleLink<T>
    {
        //表头
        private readonly BdNode<T> _linkHead;
        //节点个数
        private int _size;
        public DoubleLink()
        {
            _linkHead = new BdNode<T>(default(T), null, null);//双向链表 表头为空
            _linkHead.Prev = _linkHead;
            _linkHead.Next = _linkHead;
            _size = 0;
        }
        public int GetSize() => _size;
        public bool IsEmpty() => (_size == 0);
        //通过索引查找
        private BdNode<T> GetNode(int index)
        {
            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException("索引溢出或者链表为空");
            if (index < _size / 2)//正向查找
            {
                BdNode<T> node = _linkHead.Next;
                for (int i = 0; i < index; i++)
                    node = node.Next;
                return node;
            }
            //反向查找
            BdNode<T> rnode = _linkHead.Prev;
            int rindex = _size - index - 1;
            for (int i = 0; i < rindex; i++)
                rnode = rnode.Prev;
            return rnode;
        }
        public T Get(int index) => GetNode(index).Data;
        public T GetFirst() => GetNode(0).Data;
        public T GetLast() => GetNode(_size - 1).Data;
        // 将节点插入到第index位置之前
        public void Insert(int index, T t)
        {
            if (_size < 1 || index >= _size)
                throw new Exception("没有可插入的点或者索引溢出了");
            if (index == 0)
                Append(_size, t);
            else
            {
                BdNode<T> inode = GetNode(index);
                BdNode<T> tnode = new BdNode<T>(t, inode.Prev, inode);
                inode.Prev.Next = tnode;
                inode.Prev = tnode;
                _size++;
            }
        }
        //追加到index位置之后
        public void Append(int index, T t)
        {
            BdNode<T> inode;
            if (index == 0)
                inode = _linkHead;
            else
            {
                index = index - 1;
                if (index < 0)
                    throw new IndexOutOfRangeException("位置不存在");
                inode = GetNode(index);
            }
            BdNode<T> tnode = new BdNode<T>(t, inode, inode.Next);
            inode.Next.Prev = tnode;
            inode.Next = tnode;
            _size++;
        }
        public void Del(int index)
        {
            BdNode<T> inode = GetNode(index);
            inode.Prev.Next = inode.Next;
            inode.Next.Prev = inode.Prev;
            _size--;
        }
        public void DelFirst() => Del(0);
        public void DelLast() => Del(_size - 1);
        public void ShowAll(out string outMsg)
        {
            outMsg = "******************* 链表数据如下 *******************\r\n";
            for (int i = 0; i < _size; i++)
                outMsg += string.Format("(" + i + ")=" + Get(i));
            outMsg += "\r\n******************* 链表数据展示完毕 *******************\n";
        }
        public void Update(int index, T newData)
        {
            GetNode(index).Data = newData;
        }

        #region 删除和参照值一样的节点
        #region 重要说明 -dev 这个逻辑应该可以推广给List类等，【如果用递归当然也可以，本逻辑是不用递归，而是使用while来实现，这样一次循环就解决完问题，时间复杂度低】（划重点）
        //这段逻辑可用于处理类似“要对符合某条件（例如值相等）下，某个节点删除，然后后续节点做一些数据处理”的场景需求
        //逻辑的核心理念是：，具体逻辑的说明如下：
        //因为要满足上述需求场景，但是做完指定节点删除之后，集合里节点少了，
        //那么通过每次循环时，处理前集合大小和处理后集合大小是否不同，来判定是否本次做了删除处理，
        //但是考虑到删除的节点肯定不是连续出现的，为防止一旦下一个正确节点，大小不会变，因此将索引值和当前集合大小比较一下，当索引值等于最新的集合大小，说明已经处理到最后一条记录了，循环可以结束
        #endregion
        /// <summary>
        /// 删除和参照值一样的节点
        /// 只当 T 的类型是string，char，int，double等这一类的可以通过ToString()只去比较相等的类型时调用
        /// </summary>
        /// <param name="sameData"></param>
        public void DelNodeByCondition(T sameData)
        {
            int size = GetSize();
            int newsize = 0, j = 0;
            while (size != newsize || j != newsize)
            {
                size = newsize;

                if (equals(Get(j), sameData))//1、符合某条件
                {
                    Del(j);//2、某个节点删除
                    //To do......//3、然后后续节点做一些数据处理
                    j--;//因为删除了一个节点，实际已经前移的数据本次并没有被处理，为了保证下次循环要去处理前移的数据，所以索引要减一
                }
                newsize = GetSize();
                j++;
            }
        }

        private bool equals(T Data1, T Data2)
        {
            if (Data1.ToString() == Data2.ToString())
                return true;
            else
                return false;
        }
        #endregion
    }


}
