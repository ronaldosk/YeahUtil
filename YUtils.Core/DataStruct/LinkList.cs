using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUtils.DataStruct
{
    #region 单链表规范接口
    /// <summary>
    /// 单链表规范
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    public interface IList<T>
    {
        /// <summary>
        /// 求链表长度
        /// </summary>
        /// <returns></returns>
        int GetLength();

        /// <summary>
        /// 清空链表
        /// </summary>
        void Clear();

        /// <summary>
        /// 判空
        /// </summary>
        /// <returns></returns>
        bool IsEmpty();

        /// <summary>
        /// 在表索引位置前面插入节点
        /// </summary>
        /// <param name="user">节点数据域</param>
        /// <param name="i">索引</param>
        void Insert(T user, int i);

        /// <summary>
        /// 根据索引查询节点
        /// </summary>
        /// <param name="i">索引</param>
        /// <returns></returns>
        T GetUserByIndex(int i);

        /// <summary>
        /// 根据节点查询索引
        /// </summary>
        /// <param name="value">节点</param>
        /// <returns></returns>
        int GetIndexByUser(T user);


        ///// <summary>
        ///// 添加节点
        ///// </summary>
        ///// <param name="item">节点数据域</param>
        //void Append(T name);

        /// <summary>
        /// 删除索引位置的节点
        /// </summary>
        /// <param name="i">索引</param>
        /// <returns></returns>
        T Delete(int i);
    }
    #endregion

    #region 单链表节点类
    /// <summary>
    /// 节点类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        /// <summary>
        /// 数据域
        /// </summary>
        private T data;

        /// <summary>
        /// 引用域
        /// </summary>
        private Node<T> next;

        /// <summary>
        /// 有数据构造器
        /// </summary>
        /// <param name="val"></param>
        public Node(T val)
        {
            data = val;
        }

        /// <summary>
        /// 无数据构造器
        /// </summary>
        public Node()
        {
            data = default(T);
            next = null;
        }

        /// <summary>
        /// 数据域属性
        /// </summary>
        public T Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// 引用域属性
        /// </summary>
        public Node<T> Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }
    }
    #endregion

    #region 单链表规范实现
    /// <summary>
    /// 单链表规范实现
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    public class LinkList<T> : IList<T>
    {
        private static LinkList<T> _instance = null;
        public static LinkList<T> GetInstance()
        {
            if (null == _instance)
            {
                _instance = new LinkList<T>();
            }
            return _instance;
        }
        private Node<T> head;
        /// <summary>
        /// 单链表头节点属性
        /// </summary>
        public Node<T> Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        private LinkList()
        {
            head = null;
        }

        /// <summary>
        /// 求单链表长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            Node<T> p = head;
            int len = 0;
            while (p != null)
            {
                p = p.Next;
                len++;
            }
            return len;
        }

        /// <summary>
        /// 清空单链表
        /// </summary>
        public void Clear()
        {
            head = null;
        }

        /// <summary>
        /// 判空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return head == null;
        }

        /// <summary>
        /// 在表索引位置前面插入节点
        /// </summary>
        /// <param name="user">节点数据域</param>
        /// <param name="i">索引</param>
        public void Insert(T user, int i)
        {
            Console.WriteLine("插入位置" + i);
            //if (IsEmpty())
            //{
            //    Console.WriteLine("链表为空");
            //    Node<T> q = new Node<T>(user);
            //    head = q;
            //    return;
            //}
            if (i < 1)
            {
                Console.WriteLine("位置错误");
            }
            if (i == 1)
            {
                Node<T> q = new Node<T>(user);
                if (head == null)
                {
                    head = q;
                }
                else
                {
                    q.Next = head;
                    head = q;
                }
                return;
            }
            Node<T> p = head;
            Node<T> r = new Node<T>();
            int j = 1;
            while (j < i)
            {
                r = p;
                p = p.Next;
                j++;
            }
            if (j == i)
            {
                Node<T> q = new Node<T>(user);
                Node<T> m = r.Next;
                r.Next = q;
                q.Next = m;
            }
        }

        /// <summary>
        /// 根据索引查询节点
        /// </summary>
        /// <param name="i">索引</param>
        /// <returns></returns>
        public T GetUserByIndex(int i)
        {
            if (IsEmpty())
            {
                Console.WriteLine("空链表");
                return default(T);
            }

            Node<T> p = new Node<T>();
            p = head;
            int j = 1;
            while (p.Next != null && j < i)
            {
                p = p.Next;
                j++;
            }
            if (j == i)
            {
                return p.Data;
            }
            else
            {
                Console.WriteLine("空位");
            }
            return default(T);

        }

        /// <summary>
        /// 根据节点查询索引
        /// </summary>
        /// <param name="value">索引</param>
        /// <returns></returns>
        public int GetIndexByUser(T user)
        {
            if (IsEmpty())
            {
                Console.WriteLine("链表是空链表！");
                return -1;
            }
            Node<T> p = new Node<T>();
            p = head;
            int i = 1;
            while (((p != null) && (!p.Data.Equals(user))))
            {
                p = p.Next;
                i++;
            }
            if (p == null)
            {
                Console.WriteLine("不存在这样的节点。");
                return -1;
            }
            else
            {
                Console.WriteLine("找到了对应节点");
                return i;
            }
        }

        /// <summary>
        /// 删除索引位置的节点
        /// </summary>
        /// <param name="i">索引</param>
        /// <returns></returns>
        public T Delete(int i)
        {
            if (IsEmpty() || i < 1)
            {
                Console.WriteLine("链表为空或者位置错误");
                return default(T);
            }
            Node<T> q = new Node<T>();
            if (i == 1)
            {
                q = head;
                head = head.Next;
                return q.Data;
            }
            Node<T> p = head;
            int j = 1;
            while (p.Next != null && j < i)
            {
                q = p;
                p = p.Next;
                j++;
            }
            if (j == i)
            {
                q.Next = p.Next;
                return p.Data;
            }
            else
            {
                Console.WriteLine("位置不正确");
                return default(T);
            }
        }

        ///// <summary>
        ///// 向表尾添加元素
        ///// </summary>
        ///// <param name="data">节点数据域</param>
        //public void Append(T data)
        //{
        //    Node<T> q = new Node<T>(data);
        //    Node<T> p = new Node<T>();
        //    if (head == null)
        //    {
        //        head = q;
        //        return;
        //    }
        //    p = head;
        //    while (p.Next != null)
        //    {
        //        p = p.Next;
        //    }
        //    p.Next = q;
        //}

        ///// <summary>
        ///// 在单链表第i个位置后面插入一个值为item的节点
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="i"></param>
        //public void InsertPost(T data, int i)
        //{
        //    if (IsEmpty() || i < 1)
        //    {
        //        Console.WriteLine("链表为空或者位置错误");
        //        return;
        //    }
        //    if (i == 1)
        //    {
        //        Node<T> q = new Node<T>(data);
        //        q.Next = head.Next;
        //        head.Next = q;
        //        return;
        //    }
        //    Node<T> p = head;
        //    Node<T> r = new Node<T>();
        //    int j = 1;
        //    while (p.Next != null && j <= i)
        //    {
        //        r = p;
        //        p = p.Next;
        //        j++;
        //    }
        //    if (j == i + 1)
        //    {
        //        Node<T> q = new Node<T>(data);
        //        Node<T> m = r.Next;
        //        r.Next = q;
        //        q.Next = m;
        //    }
        //    else
        //    {
        //        Console.WriteLine("插入位置过大，error");
        //    }
        //}

    } 
    #endregion
}
