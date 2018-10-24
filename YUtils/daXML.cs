using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace SwPLDM
{
    public class TdaXMLIniFile
    {//XML格式的INI配置文件的处理类
        private string _strIniFilePathName = "";
        private XmlDocument _xmlDoc = new XmlDocument();

        public TdaXMLIniFile(string strIniFilePathName)
        {
            if (File.Exists(strIniFilePathName))
            {
                _strIniFilePathName = strIniFilePathName;
                _xmlDoc.Load(_strIniFilePathName);
            }
            else
            {
                throw new Exception("File Not Find :" + strIniFilePathName);
            }
        }
        public string GetConfigValue(string strNode, string strAttribute)
        {
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = _xmlDoc.SelectSingleNode(strNode);

                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name.ToUpper() == strAttribute.ToUpper())
                    {
                        return xmlAttr.Item(i).Value;
                    }
                }
                return "";
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        public string GetConfigValue(string strNode)
        {
            string strReturn = "";
            try
            {
                //根据路径获取节点
                XmlNode xmlNode = _xmlDoc.SelectSingleNode(strNode);
                strReturn = xmlNode.InnerText.ToString();
            }
            catch (XmlException xmle)
            {
                System.Console.WriteLine(xmle.Message);
            }
            return strReturn;
        }

        public void SetConfigValue(string strNode, string newValue)
        {
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = _xmlDoc.SelectSingleNode(strNode);

                //设置节点值
                xmlNode.InnerText = newValue;
                SaveConfig();//保存
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        public void SetConfigValue(string strNode, string strAttribute, string newValue)
        {
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = _xmlDoc.SelectSingleNode(strNode);

                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;
                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name.ToUpper() == strAttribute.ToUpper())
                        xmlAttr.Item(i).Value = newValue;
                    break;
                }
                //				//如果指定的属性不存在,新建
                //				System.Xml.XmlAttribute attr = new XmlAttribute();
                //				attr.InnerText = strAttribute +"="+newValue;
                //				xmlNode.Attributes.Append(attr);
                SaveConfig();//保存
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        private void SaveConfig()
        {
            try
            {
                //保存设置的结果
                _xmlDoc.Save(_strIniFilePathName);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
    }
    public class TdaXML
    {//xml的常规操作

        static public string MakeXML(string strTag, string strValue)
        {
            string strRes;
            strRes = "<" + strTag + ">" + strValue + "</" + strTag + ">";
            return strRes;
        }
        static public string GetXML(string originStr, string tagStr)
        {
            string rv = "";

            try
            {
                int firstPos = originStr.IndexOf("<" + tagStr + ">");
                int secondPos = originStr.IndexOf("</" + tagStr + ">");
                int tagLen = tagStr.Length;

                if (firstPos != -1 && secondPos != -1)
                {
                    rv = originStr.Substring(firstPos + tagLen + 2, secondPos - firstPos - tagLen - 2);
                }
            }

            catch
            {
                rv = "Error : Aicaca.Com.XML.GetXML";
            }

            return rv;
        }
        static public void XmlToTreeview(XmlNode document, TreeNodeCollection nodes)
        {//将XML中的节点绑定到Treeview中
            foreach (XmlNode node in document.ChildNodes)
            {
                string text = (node.Value != null ? node.Value :
                    (node.Attributes != null &&
                    node.Attributes.Count > 0) ?
                    node.Attributes[0].Value : node.Name);
                TreeNode new_child = new TreeNode(text);
                nodes.Add(new_child);
                XmlToTreeview(node, new_child.Nodes);
            }
        }
        static public void XmlToListBox(XmlNode xnod, Int32 intLevel, ListBox lbNodes)
        {
            //将节点及它的子节点一同添加到listbox中
            //intLevel 控制缩进的深度
            XmlNode xnodWorking;
            String strIndent = new string(' ', 2 * intLevel);
            //如果节点有值，读取它的值
            string strValue = (string)xnod.Value;
            if (strValue != null)
            {
                strValue = " : " + strValue;
            }
            //将节点的详细信息添加到ListBox中
            lbNodes.Items.Add(strIndent + xnod.Name + strValue);
            //如果是元素节点，获取它的属性
            if (xnod.NodeType == XmlNodeType.Element)
            {
                XmlNamedNodeMap mapAttributes = xnod.Attributes;
                //将节点属性添加到ListBox中
                foreach (XmlNode xnodAttribute in mapAttributes)
                {
                    lbNodes.Items.Add(strIndent + " " + xnodAttribute.Name +
                        " : " + xnodAttribute.Value);
                }

                //如果还有子节点，就递归地调用这个程序
                if (xnod.HasChildNodes)
                {
                    xnodWorking = xnod.FirstChild;
                    while (xnodWorking != null)
                    {
                        XmlToListBox(xnodWorking, intLevel + 1, lbNodes);
                        xnodWorking = xnodWorking.NextSibling;
                    }
                }
            }

        }

    }

    public class TdaXMLNode
    {
        static public XmlNode SelectSingleNode_RelaPath(XmlNode nodeParent, string nodePath)
        {
            nodePath = nodePath.Trim();
            if (nodePath == "") return nodeParent;
            int index = nodePath.IndexOf('/');
            string tagName = "";
            if (index < 0)
            {
                tagName = nodePath;
                return GetSingleChildNode(nodeParent, tagName);
            }
            else if (index == 0)
            {
                if (nodePath.Length == 1) return nodeParent;
                else
                {
                    nodePath = nodePath.Substring(1);
                    return SelectSingleNode_RelaPath(nodeParent, nodePath);
                }
            }
            else//index > 0
            {
                tagName = nodePath.Substring(0, index);
                XmlNode tmpNode = GetSingleChildNode(nodeParent, tagName);
                if (tmpNode != null)
                {
                    nodePath = nodePath.Substring(index);
                    return SelectSingleNode_RelaPath(tmpNode, nodePath);
                }
            }
            return null;
        }
        static public List<XmlNode> SelectNodes_RelaPath(XmlNode nodeParent, string nodePath)
        {
            List<XmlNode> nodeList = new List<XmlNode>();
            nodePath = nodePath.Trim();
            if ((nodePath == "") || (nodePath == "/"))
            {
                nodeList.Add(nodeParent);
                return nodeList;
            }

            string tagName = "";
            int index = nodePath.IndexOf('/');
            if (index < 0)
            {
                tagName = nodePath;
                return GetChildNodes(nodeParent, tagName);
            }
            else if (index == 0)
            {
                nodePath = nodePath.Substring(1);
                return SelectNodes_RelaPath(nodeParent, nodePath);
            }
            else
            {
                tagName = nodePath.Substring(0, index);
                nodePath = nodePath.Substring(index);
                List<XmlNode> tmpNodeList = GetChildNodes(nodeParent, tagName);
                if (tmpNodeList != null)
                {
                    foreach (XmlNode tmpParent in tmpNodeList)
                    {
                        List<XmlNode> tmpChildNodeList = SelectNodes_RelaPath(tmpParent, nodePath);
                        if (tmpChildNodeList != null)
                        {
                            nodeList.AddRange(tmpChildNodeList);
                        }
                    }
                }
                if (nodeList.Count > 0)
                    return nodeList;
                else
                    return null;
            }

        }
        static public XmlNode GetSingleChildNode(XmlNode nodeParent, string childTagName)
        {
            return GetSingleChildNode(nodeParent, childTagName, true);
        }
        static public XmlNode GetSingleChildNode(XmlNode nodeParent, string childTagName, bool ignoreCase)
        {
            foreach (XmlNode nodeChild in nodeParent.ChildNodes)
            {
                if (string.Compare(nodeChild.Name, childTagName, ignoreCase) == 0)
                    return nodeChild;
            }
            return null;
        }
        static public List<XmlNode> GetChildNodes(XmlNode nodeParent, string childTagName)
        {
            return GetChildNodes(nodeParent, childTagName, true);
        }
        static public List<XmlNode> GetChildNodes(XmlNode nodeParent, string childTagName, bool ignoreCase)
        {
            List<XmlNode> nodelist = new List<XmlNode>();
            foreach (XmlNode nodeChild in nodeParent.ChildNodes)
            {
                if (string.Compare(nodeChild.Name, childTagName, ignoreCase) == 0)
                {
                    nodelist.Add(nodeChild);
                }
            }
            if (nodelist.Count > 0)
                return nodelist;
            else
                return null;
        }
    }
}
