using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml;
using YeahException;

namespace YeahTools
{
    /// <summary>
    /// XMLHelper.对XMl文件的各种应用工具类
    /// </summary>
    public class XMLHelper
    {
        public string xmlfilepath { get; set; }


        public XMLHelper()
        {
        }

        public XMLHelper(string xmlpath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlpath))
                    throw new Exception(YeahExceptionDesc.ExceptionDesc(YeahExceptionType.InvalidFilePath));
            }
            finally
            {

                this.xmlfilepath = xmlpath;
            }
        }

        #region 根据XMl节点，批量创建类
        List<XmlNodeClass> distinctnodes = new List<XmlNodeClass>();
        /// <summary>
        /// 根据XMl节点，批量创建类.
        /// </summary>
        /// <returns><c>true</c>, if entity by xml was created, <c>false</c> otherwise.</returns>
        public bool CreateClassByXML(string outputFolder)
        {
            try
            {
                if (File.Exists(xmlfilepath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlfilepath);
                    //取根结点
                    var root = xmlDoc.DocumentElement;//取到根结点

                    TravelNode(root);


                    string outStr = Statement(0, "using System;\r\nusing System.Xml;\r\nusing YeahException;\r\nusing System.Collections.Generic;");
                    outStr += Statement(0, "\r\nnamespace YeahTools." + Path.GetFileNameWithoutExtension(xmlfilepath));
                    outStr += BeginSignal(0);

                    foreach (XmlNodeClass nodeclass in distinctnodes)
                    {
                        outStr += NodeToClass(nodeclass.ClassName, nodeclass.ParentClassName, nodeclass.PtyList, nodeclass.ChildMemberList);
                    }

                    outStr += EndSignal(0);

                    StreamWriter sw = new StreamWriter(Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(xmlfilepath) + "Helper.cs"));
                    sw.Write(outStr);
                    sw.Flush();
                    sw.Close();
                    return true;
                }
                else
                    throw new Exception(YeahExceptionDesc.ExceptionDesc(YeahExceptionType.FileNotExists));
            }
            catch (SystemException ex)
            {
                return false;
            }

        }


        private void TravelNode(XmlNode node)
        {
            for (int i = 0; i < distinctnodes.Count; i++)
            {
                XmlNodeClass nodeClass = distinctnodes[i];
                if (nodeClass.ClassName == node.Name)
                {
                    foreach (var pty in node.Attributes)
                    {
                        XmlAttribute att = pty as XmlAttribute;
                        nodeClass.AddItem(att.Name);
                    }
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        TravelNode(child);
                        nodeClass.AddMember(child.Name);
                    }
                    return;
                }
            }

            XmlNodeClass nodeClassnew = new XmlNodeClass();
            nodeClassnew.ClassName = node.Name;
            foreach (var pty in node.Attributes)
            {
                XmlAttribute att = pty as XmlAttribute;
                nodeClassnew.AddItem(att.Name);
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                TravelNode(child);
            }
            nodeClassnew.ParentClassName = node.ParentNode.Name;
            distinctnodes.Add(nodeClassnew);
            return;
        }

        /// <summary>
        /// Node 节点的属性 转化成Class中的成员变量
        /// </summary>
        /// <returns>The pty to member.</returns>
        /// <param name="PtyName">Pty name.</param>
        string NodePtyToMember(string PtyName)
        {
            string outStr = Statement(2, string.Format("public string {0} {{get;set;}}", PtyName));

            return outStr;
        }

        string NodePtyToListMember(string memName)
        {
            string outStr = Statement(2, string.Format("public List<{0}> {1}List = new List<{0}>();", memName, memName.ToLower()));

            return outStr;
        }

        /// <summary>
        /// Node节点转化成Class.
        /// </summary>
        /// <returns>The to class.</returns>
        /// <param name="MemberName">Member name.</param>
        /// <param name="PtyList">Pty list.</param>
        string NodeToClass(string MemberName, string ParentName, StringCollection PtyList, StringCollection ChildMemberList)
        {

            #region 类定义
            string outStr = Statement(1, string.Format("public class {0}", MemberName));
            outStr += BeginSignal(1);

            #region 成员变量定义
            outStr += Statement(2, string.Format("public string ClassName {{get{{ return \"{0}\";}}}}", MemberName));

            foreach (string pty in PtyList)
            {
                outStr += NodePtyToMember(pty.Replace(":", "_"));
            }

            foreach (string memName in ChildMemberList)
            {
                outStr += NodePtyToListMember(memName);
            }
            #endregion

            #region 构造函数
            outStr += Statement(2, string.Format("public {0}()", MemberName));
            outStr += BeginSignal(2);
            foreach (string pty in PtyList)
            {
                outStr += Statement(3, string.Format("{0} = \"\";", pty.Replace(":", "_")));
            }
            outStr += EndSignal(2);
            #endregion

            #region 加载节点数据的方法
            outStr += "\r\n";
            outStr += Statement(2, string.Format("public void LoadFromXmlNode(XmlNode xmln)"));
            outStr += BeginSignal(2);
            outStr += Statement(3, string.Format("if (xmln.Name == ClassName)"));
            outStr += BeginSignal(3);
            foreach (string pty in PtyList)
            {
                outStr += Statement(4, string.Format("this.{0} = xmln.Attributes[\"{0}\"] == null ? \"\" : xmln.Attributes[\"{0}\"].Value;", pty));
            }

            outStr += EndSignal(3);
            outStr += EndSignal(2);
            #endregion

            outStr += EndSignal(1);
            #endregion

            return outStr;
        }

        /// <summary>
        /// Begin the signal.
        /// </summary>
        /// <returns>The signal.</returns>
        /// <param name="level">0-namespace,1-class,2-function,3-member-first</param>
        string BeginSignal(int level = 0, string str = "{")
        {
            string outStr = "";
            for (int i = 1; i <= level; i++)
            {
                outStr += "\t";
            }
            outStr += (str + "\r\n");

            return outStr;
        }

        /// <summary>
        /// End the signal.
        /// </summary>
        /// <returns>The signal.</returns>
        /// <param name="level">0-namespace,1-class,2-function,3-member-first</param>
        string EndSignal(int level = 0, string str = "}")
        {
            string outStr = "";
            for (int i = 1; i <= level; i++)
            {
                outStr += "\t";
            }
            outStr += (str + "\r\n");

            return outStr;
        }

        /// <summary>
        /// Statement
        /// </summary>
        /// <returns>The Statement.</returns>
        /// <param name="level">0-namespace,1-class,2-function,3-member-first</param>
        string Statement(int level = 0, string str = "")
        {
            string outStr = "";
            for (int i = 1; i <= level; i++)
            {
                outStr += "\t";
            }
            outStr += (str + "\r\n");

            return outStr;
        }
        #endregion

    }

    public class XmlNodeClass
    {
        public string ClassName;
        public string ParentClassName;
        public StringCollection PtyList = new StringCollection();
        public StringCollection ChildMemberList = new StringCollection();

        public void AddItem(string inputPty)
        {
            if (PtyList.IndexOf(inputPty) >= 0)
                return;
            else
                PtyList.Add(inputPty);
        }

        public void AddMember(string MemberName)
        {
            if (ChildMemberList.IndexOf(MemberName) >= 0)
                return;
            else
                ChildMemberList.Add(MemberName);
        }
    }



}
