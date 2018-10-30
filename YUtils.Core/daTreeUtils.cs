using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SwPLDM
{
    public delegate void EventHandler_BindTreeNode(TdaTreeNode daNode, TreeNode node);

    public class TdaTreeView
    {
        #region 事件
        public event EventHandler_BindTreeNode OnBindTreeNode;
        #endregion
        private List<TdaTreeNode> _Nodes = new List<TdaTreeNode>();
        public List<TdaTreeNode> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; }
        }
        public TdaTreeNode CreateTreeNode()
        {
            return new TdaTreeNode(this);
        }
        public TdaTreeNode CreateTreeNode(string text)
        {
            TdaTreeNode node = CreateTreeNode();
            node.Text = text;
            return node;
        }
        public void AddNode(TdaTreeNode node)
        {
            _Nodes.Add(node);
            node.TreeView = this;
            node.Level = 0;
            BindEvent(node);
        }
        public void BindTreeView(TreeView tree)
        {
            try
            {
                tree.BeginUpdate();
                foreach (TdaTreeNode daNode in Nodes)
                {
                    TreeNode node = new TreeNode(daNode.Text);
                    tree.Nodes.Add(node);
                    daNode.BindTreeNode(node);
                }
            }
            finally
            {
                tree.EndUpdate();
            }
        }
        public void BindEvent()
        {
            foreach (TdaTreeNode node in _Nodes)
            {
                BindEvent(node);
                node.BindEvent();
            }
        }
        public void BindEvent(TdaTreeNode node)
        {
            node.OnBindTreeNode += this.OnBindTreeNode;
        }
        public void Sort(Comparer<TdaTreeNode> compare)
        {
            _Nodes.Sort(compare);
        }
    }

    public class TdaTreeNode
    {
        #region 成员
        private List<TdaTreeNode> _Nodes = new List<TdaTreeNode>();
        public List<TdaTreeNode> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; }
        }
        private int _level = 0;

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        private string _text = "";
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private object _tag = null;
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private TdaTreeNode _parent = null;
        public TdaTreeNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private TdaTreeView _treeView = null;
        public TdaTreeView TreeView
        {
            get { return _treeView; }
            set { _treeView = value; }
        }
        #endregion
        #region 事件
        public event EventHandler_BindTreeNode OnBindTreeNode;
        #endregion
        public TdaTreeNode()
        {
        }
        public TdaTreeNode(TdaTreeView treeView)
        {
            _treeView = treeView;
        }
        public void Insert(int index, TdaTreeNode node)
        {
            _Nodes.Insert(index, node);
            node.Parent = this;
            node.TreeView = this.TreeView;
            node.Level = this.Level + 1;
            BindEvent(node);
        }
        public void AddNode(TdaTreeNode node)
        {
            _Nodes.Add(node);
            node.Parent = this;
            node.TreeView = this.TreeView;
            node.Level = this.Level + 1;
            BindEvent(node);
        }
        public void BindTreeNode(TreeNode node)
        {
            try
            {
                node.Text = Text;
                node.Tag = this;
                if (OnBindTreeNode != null)
                    OnBindTreeNode(this, node);
                node.Nodes.Clear();
                foreach (TdaTreeNode daNode in Nodes)
                {
                    TreeNode nodeChild = new TreeNode();
                    node.Nodes.Add(nodeChild);
                    daNode.BindTreeNode(nodeChild);
                }
            }
            finally
            {
            }
        }
        public void BindEvent()
        {
            foreach (TdaTreeNode node in _Nodes)
            {
                BindEvent(node);
                node.BindEvent();
            }
        }
        public void BindEvent(TdaTreeNode node)
        {
            node.OnBindTreeNode = this.OnBindTreeNode;
        }
    }
}
