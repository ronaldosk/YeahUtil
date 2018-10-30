//***********************************************************************
//对XML文件操作的工具类集合
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Specialized;

namespace Yeah.Utils
{
    public class XMLHelper2
    {
        public List<string> classnamelist = new List<string>();
        public List<NodeName> nnlist = new List<NodeName>();
        #region 亲儿子
        /// <summary>
        /// XML文件名
        /// </summary>
        string xmlFileName { get; set; }
        XmlNode XmlRootNode = null;
        string XMLTemplateFile { get; set; }
        #endregion

        #region 和生成类代码有关的成员
        StringCollection code_StringArray = new StringCollection();
        string code_ClassName { get; set; }
        string code_NameSpace { get; set; }
        #endregion

        public XMLHelper2(string filename, string xmlpath, string csname, string nsname)
        {
            xmlFileName = filename;
            XMLTemplateFile = xmlpath;
            code_ClassName = csname;
            code_NameSpace = nsname;
            code_StringArray.Clear();

            XmlDocument xmld = new XmlDocument();
            xmld.Load(xmlpath);
            XmlRootNode = xmld.FirstChild;
            if (XmlRootNode.Name == "xml")    //如果是注释信息
            {
                XmlRootNode = XmlRootNode.NextSibling;
            }
        }

        #region 一些实现特定功能的方法
        string AddCodeStrToArray(string code)
        {
            string newcodestr = "";
            string laststr = code_StringArray[code_StringArray.Count - 1];
            int ret = JudgeIndentation(laststr);
            if (ret == 0)
                code_StringArray.Add(code);
            else if (ret == 1)
                code_StringArray.Add(string.Format("\t{0}", code));
            else if (ret == 2)
                code_StringArray.Add(string.Format("\t{0}", code));
            return newcodestr;
        }
        string[] RightIndentationStrArray = { "if", "{", "while", "else" };
        string[] LeftIndentationStrArray = { "}", "break", "continue", "return" };

        /// <summary>
        /// 根据上一行的字符串判断本行缩进方向的标记，参照是上一行的起始位
        /// </summary>
        /// <param name="laststr"></param>
        /// <returns>0--不缩进；1→缩进；2←缩进</returns>
        private int JudgeIndentation(string laststr)
        {
            foreach (string str in RightIndentationStrArray)
            {
                if (laststr.IndexOf(str) == 0)
                    return 1;
            }

            foreach (string str in LeftIndentationStrArray)
            {
                if (laststr.IndexOf(str) == 0)
                    return 2;
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// 生成能够对指定模版格式的XML文件进行读写的C#类
        /// </summary>
        /// <param name="xmlpath">xml模版文件的路径</param>
        /// <param name="csname">生成的类名</param>
        /// <param name="nsname">类的命名空间名称</param>
        /// <param name="filename">生成类文件的名称</param>
        public void GenerateClass()
        {
            //写开头
            WriteBegin(code_NameSpace);
            //新建实体类
            CreatClass(XmlRootNode.Name, XmlRootNode);
            //获取xml的方法
            CreatGetMethHeader(code_NameSpace, XmlRootNode);
            CreatGetMethod(XmlRootNode);
            CreatGetMethend(XmlRootNode);
            //生成xml的方法
            CreatSetMethHeader(XmlRootNode);
            CreatSetMethod(XmlRootNode);
            CreatSetMethend();
            //写结尾
            WriteEnd();

            StreamWriter switer = new StreamWriter(xmlFileName, false, Encoding.UTF8);
            foreach (string str in code_StringArray)
            {
                switer.WriteLine(str);
                switer.Flush();
                switer.Close();
            }

        }
        /// <summary>
        /// 类的开头
        /// </summary>
        /// <param name="nsname"></param>
        /// <returns>返回当前行数</returns>
        private void WriteBegin(string nsname)
        {
            code_StringArray.Add("//***********************************************************************");
            code_StringArray.Add("//***********************这是通过一段自定生成的代码v1.***************************");
            code_StringArray.Add("//*********用来根据指定格式的XML文件，自动生成能读写这个格式XML文件的C#类***************");
            code_StringArray.Add("//***********************************************************************");
            code_StringArray.Add("//***********************************************************************");
            code_StringArray.Add("//*******************2013-10-17********叶 全********************************");
            code_StringArray.Add("//**********************200844859@qq.com**********************************");
            code_StringArray.Add("");
            code_StringArray.Add("using System;");
            code_StringArray.Add("using System.Collections.Generic;");
            code_StringArray.Add("using System.Text;");
            code_StringArray.Add("using System.Xml;");
            code_StringArray.Add("");
            code_StringArray.Add(string.Format("namespace {0}", nsname));
            code_StringArray.Add("{");
        }

        /// <summary>
        /// 获取所有的实体类
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="rootnode"></param>
        /// <param name="sw"></param>
        private void CreatClass(string classname, XmlNode rootnode)
        {
            //如果该类已经存在，则不创建
            if (!classnamelist.Contains(classname))
            {
                classnamelist.Add(classname);
                NodeName nn = new NodeName();
                List<string> attlist = new List<string>();
                List<string> cnlist = new List<string>();
                nn.CurrentName = rootnode.Name;
                code_StringArray.Add(string.Format("  public class {0}", classname));
                code_StringArray.Add("  {");
                if (rootnode.Attributes != null)
                {
                    foreach (XmlAttribute att in rootnode.Attributes)
                    {
                        if (!attlist.Contains(att.Name))
                        {
                            attlist.Add(att.Name);
                            string wl = string.Format("    public string {0}", att.Name);
                            code_StringArray.Add(wl + "{get;set;}");
                        }
                    }
                }
                if (rootnode.ChildNodes != null)
                {
                    foreach (XmlNode child in rootnode.ChildNodes)
                    {
                        if (!cnlist.Contains(child.Name))
                        {
                            cnlist.Add(child.Name);
                            string wl = string.Format("    public List<{0}>  {1}List ", child.Name, child.Name);
                            code_StringArray.Add(wl + "{get;set;}");
                        }
                    }
                }
                nn.AttributeNames = attlist;
                nn.CnNames = cnlist;
                nnlist.Add(nn);
                code_StringArray.Add("  }");
            }
            foreach (XmlNode child in rootnode.ChildNodes)
            {
                CreatClass(child.Name, child);
            }
        }
        /// <summary>
        /// 获取xml方法的开头
        /// </summary>
        /// <param name="csname"></param>
        /// <param name="sw"></param>
        private void CreatGetMethHeader(string csname, XmlNode curnode)
        {
            code_StringArray.Add(string.Format("  public class {0}", csname));
            code_StringArray.Add(string.Format("  {"));
            code_StringArray.Add(string.Format("    public {0} GetXMLInfo(string xmlpath)", curnode.Name));
            code_StringArray.Add(string.Format("    {"));
            code_StringArray.Add(string.Format("      XmlDocument xmldoc = new XmlDocument();"));
            code_StringArray.Add(string.Format("      xmldoc.Load(xmlpath);"));
            code_StringArray.Add(string.Format("      XmlNode xml{0} = xmldoc.FirstChild;", curnode.Name.ToLower()));
            code_StringArray.Add(string.Format("      if (xml{0}.Name == \"xml\")", curnode.Name.ToLower()));
            code_StringArray.Add("      {");
            code_StringArray.Add(string.Format("         xml{0} = xml{0}.NextSibling;", curnode.Name.ToLower()));
            code_StringArray.Add("      }");
            code_StringArray.Add(string.Format("      {0}  _{1} = new {2}();", classnamelist[0], classnamelist[0].ToLower(), classnamelist[0]));
            //code_StringArray.Add("      foreach(XmlNode child in root.ChildNodes)  ");
            //code_StringArray.Add("      {");
            //code_StringArray.Add("        {0}  _{1} = new {2}();", classnamelist[1], classnamelist[1].ToLower(), classnamelist[1]);
            //code_StringArray.Add("      }");
            //code_StringArray.Add("       return _ddsoftdrawinfo;");
            //code_StringArray.Add("    }");
        }
        /// <summary>
        /// 获取xml方法的主要方法，调用递归
        /// </summary>
        /// <param name="csname"></param>
        /// <param name="sw"></param>
        private void CreatGetMethod(XmlNode curnode)
        {
            string partent = curnode.Name;
            string partentlow = partent.ToLower();
            string xmlpartent = "xml" + partentlow;
            foreach (XmlNode childnode in curnode.ChildNodes)
            {
                string child = childnode.Name;
                string childlow = child.ToLower();
                string xmlchild = "xml" + childlow;
                code_StringArray.Add(string.Format("foreach (XmlNode {0} in {1}.ChildNodes)", xmlchild, xmlpartent));
                code_StringArray.Add("{");
                code_StringArray.Add(string.Format("    if ({0}.Name == \"{1}\")", xmlchild, child));
                code_StringArray.Add("  {");
                code_StringArray.Add(string.Format("      List<{0}>  _{1}list = new List<{0}>();", child, childlow));
                code_StringArray.Add(string.Format("      {0}  _{1} = new {0}();", child, childlow));

                code_StringArray.Add(string.Format("foreach (XmlAttribute att{0} in {1}.Attributes)", childlow, xmlchild));
                code_StringArray.Add("    {");
                if (childnode.Attributes != null)
                {
                    foreach (XmlAttribute att in childnode.Attributes)
                    {
                        string attname = att.Name;
                        code_StringArray.Add(string.Format(" if (att{0}.Name == \"{1}\")", childlow, attname));
                        code_StringArray.Add("      {");
                        code_StringArray.Add(string.Format("        _{0}.{1} = att{0}.Value;", childlow, attname));
                        code_StringArray.Add("      }");
                    }
                }
                code_StringArray.Add("    }");
                CreatGetMethod(childnode);
                code_StringArray.Add(string.Format("       _{0}list.Add(_{0});", childlow));
                code_StringArray.Add(string.Format("      _{0}.{1}List = _{2}list;", partentlow, child, childlow));
                code_StringArray.Add("  }");
                code_StringArray.Add("}");
            }
        }
        /// <summary>
        /// 获取xml方法的结尾
        /// </summary>
        /// <param name="csname"></param>
        /// <param name="sw"></param>
        private void CreatGetMethend(XmlNode curnode)
        {
            code_StringArray.Add(string.Format("       return _{0};", curnode.Name.ToLower()));
            code_StringArray.Add("    }");
        }


        /// <summary>
        /// 生成xml方法的开头
        /// </summary>
        /// <param name="curnode"></param>
        /// <param name="sw"></param>
        private void CreatSetMethHeader(XmlNode curnode)
        {
            code_StringArray.Add(string.Format("    public void SetXMLInfo({0} _{1}, string xmlpath)", curnode.Name, curnode.Name.ToLower()));
            code_StringArray.Add("    {");
            code_StringArray.Add("      XmlDocument xmldoc = new XmlDocument();");
            code_StringArray.Add("      XmlDeclaration xmldecl;");
            code_StringArray.Add("      xmldecl = xmldoc.CreateXmlDeclaration(\"1.0\", \"gb2312\", null);");
            code_StringArray.Add("      xmldoc.AppendChild(xmldecl);");
            #region  写入根节点
            code_StringArray.Add(string.Format("      XmlElement xml{0} = xmldoc.CreateElement( \"{1}\");", curnode.Name.ToLower(), curnode.Name));
            int i = 0;
            foreach (XmlAttribute att in curnode.Attributes)
            {
                string attname = att.Name;
                code_StringArray.Add(string.Format("      xml{0}.SetAttribute(\"{1}\", _{2}.{3});", curnode.Name.ToLower(), attname, curnode.Name.ToLower(), attname));
                i++;
            }
            code_StringArray.Add(string.Format("      xmldoc.AppendChild(xml{0});", curnode.Name.ToLower()));
            #endregion
        }

        /// <summary>
        ///  生成xml方法的结尾
        /// </summary>
        /// <param name="sw"></param>
        private void CreatSetMethend()
        {
            code_StringArray.Add("      xmldoc.Save(xmlpath);");
            code_StringArray.Add("    }");
        }

        /// <summary>
        ///  生成xml方法的主要方法，调用递归
        /// </summary>
        /// <param name="curnode"></param>
        /// <param name="sw"></param>
        private void CreatSetMethod(XmlNode curnode)
        {
            string partent = curnode.Name;
            string partentlow = partent.ToLower();
            string xmlpartent = "xml" + partentlow;
            foreach (XmlNode childnode in curnode.ChildNodes)
            {
                string child = childnode.Name;
                string childlow = child.ToLower();
                string xmlchild = "xml" + childlow;
                code_StringArray.Add(string.Format("      if (_{0}.{1}List!=null)", partentlow, child));
                code_StringArray.Add("      {");
                code_StringArray.Add(string.Format("      XmlElement {0} = xmldoc.CreateElement( \"{1}\");", xmlchild, child));
                code_StringArray.Add(string.Format("      foreach ({0} _{1} in _{2}.{3}List)", child, childlow, partentlow, child));
                code_StringArray.Add("      {");
                int i = 0;
                if (childnode.Attributes != null)
                {
                    foreach (XmlAttribute att in childnode.Attributes)
                    {
                        string attname = att.Name;
                        code_StringArray.Add(string.Format("      {0}.SetAttribute(\"{1}\", _{2}.{3});", xmlchild, attname, childlow, attname));
                        i++;
                    }
                    code_StringArray.Add(string.Format("     {0}.AppendChild({1});", xmlpartent, xmlchild));
                }
                CreatSetMethod(childnode);
                code_StringArray.Add("      }");
                code_StringArray.Add("      }");
            }
        }

        /// <summary>
        /// 类结尾
        /// </summary>
        /// <param name="sw"></param>
        private void WriteEnd()
        {
            code_StringArray.Add("  }");
            code_StringArray.Add("}");
        }

        #region
        private void CreatSetMethHeader1(List<NodeName> nonalist, StreamWriter sw)
        {
            //StreamWriter sw = new StreamWriter(csfullname, true, Encoding.UTF8);
            NodeName nnroot = nonalist[0];
            string rootname = nnroot.CurrentName;
            code_StringArray.Add(string.Format("    public void SetXMLInfo({0} _{1}, string xmlpath)", rootname, rootname.ToLower()));
            code_StringArray.Add("    {");
            code_StringArray.Add("      XmlDocument xmldoc = new XmlDocument();");
            code_StringArray.Add("      XmlDeclaration xmldecl;");
            code_StringArray.Add("      xmldecl = xmldoc.CreateXmlDeclaration(\"1.0\", \"gb2312\", null);");
            code_StringArray.Add("      xmldoc.AppendChild(xmldecl);");
            //code_StringArray.Add("      XmlElement xmlroot = xmldoc.CreateElement( \"{0}\");", rootname);
            //int i = 0;
            //foreach (string att in nonalist[0].AttributeNames)
            //{
            //    string attname = nnroot.AttributeNames[i];
            //    code_StringArray.Add("xmlroot.SetAttribute(\"{0}\", {1}.{2});", attname, rootname, attname);
            //    i++;
            //}
            //code_StringArray.Add("     xmldoc.AppendChild(xmlroot);");
            //sw.Flush();
            //sw.Close();
        }
        private void CreatSetMethod1(NodeName nona, string xmlpartent, int k, StreamWriter sw)
        {
            k++;
            string rootname = nona.CurrentName;
            //写自身的属性信息
            code_StringArray.Add(string.Format("   XmlElement  xml{0} = xmldoc.CreateElement( \"{1}\");", rootname.ToLower(), rootname));
            int j = 0;
            //TitleInfo _titleinfo in _title.TitleInfoList
            code_StringArray.Add(string.Format("foreach({0} _{1} in _{2}.{3}List)", rootname, rootname.ToLower(), xmlpartent, rootname));
            code_StringArray.Add("{");
            foreach (string att in nona.AttributeNames)
            {
                string attname = nona.AttributeNames[j];
                code_StringArray.Add(string.Format("xml{0}.SetAttribute(\"{1}\", _{2}.{3});", rootname.ToLower(), attname, rootname.ToLower(), attname));
                j++;
            }
            code_StringArray.Add("}");
            //寻找与子节点名称相同的节点
            foreach (string nnchild in nona.CnNames)
            {
                foreach (NodeName nn in nnlist)
                {
                    if (nn.CurrentName == nnchild)
                    {
                        NodeName nodecutrrent = nn;
                        string xmlchild = "xml" + nnchild.ToLower();
                        code_StringArray.Add(string.Format("foreach({0} _{1} in _{2}.{3}List)", nnchild, nnchild.ToLower(), rootname.ToLower(), nnchild));
                        code_StringArray.Add("{");
                        code_StringArray.Add(string.Format("   XmlElement {0} = xmldoc.CreateElement( \"{1}\");", xmlchild, nnchild));
                        int i = 0;
                        foreach (string att in nn.AttributeNames)
                        {
                            string attname = nn.AttributeNames[i];
                            code_StringArray.Add(string.Format("xmlroot.SetAttribute(\"{0}\", {1}.{2});", attname, nnchild, attname));
                            i++;
                        }
                        foreach (string nnchild1 in nodecutrrent.CnNames)
                        {
                            foreach (NodeName nn1 in nnlist)
                            {
                                if (nn1.CurrentName == nnchild1)
                                {
                                    string xmlchild1 = "xml" + nnchild1.ToLower();
                                    CreatSetMethod1(nn1, xmlchild1, k, sw);
                                }
                            }
                        }
                        code_StringArray.Add(string.Format("     {0}.AppendChild({1});", xmlpartent, xmlchild));
                        code_StringArray.Add("}");
                    }
                }
            }
        }
        private void CreatSetMethend1(StreamWriter sw)
        {
            code_StringArray.Add("      xmldoc.Save(xmlpath);");
            code_StringArray.Add("    }");
        }
        #endregion


    }

    public class NodeName
    {
        public string CurrentName { get; set; }
        public List<string> AttributeNames { get; set; }
        public List<string> CnNames { get; set; }
    }

    public class XMLClassGenerate
    {
        public string XMlClassName { get; set; }
        public string NameSpace { get; set; }
        public List<string> StrUsing = new List<string>();
        public List<ClassHelper> ChildClass = new List<ClassHelper>();
    }

    public class ClassHelper
    {
        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 类描述，写在注释中
        /// </summary>
        public string ClassDisc { get; set; }
        /// <summary>
        /// 类成员
        /// </summary>
        public List<ClassMember> Member = new List<ClassMember>();
        /// <summary>
        /// 类方法
        /// </summary>
        public List<ClassMethod> Method = new List<ClassMethod>();

    }

    #region 类属性成员类
    public class ClassMember
    {
        #region 私有成员
        /// <summary>
        /// 成员的操作方法体字符串序列
        /// </summary>
        List<string> m_Body = new List<string>();
        /// <summary>
        /// 成员定义字符串序列
        /// </summary>
        List<string> m_Define;

        /// <summary>
        /// 额外的定义字符串 主要用来在成员名称后添加"{ get; set; }" 和默认的new 语句
        /// </summary>
        string DefineAddtionStr
        {
            get
            {
                if (returnType == DataType.DataType_IntList)
                    return " = new List<int>();";
                else if (returnType == DataType.DataType_DoubleList)
                    return " = new List<double>();";
                else if (returnType == DataType.DataType_StringList)
                    return " = new List<string>();";
                else
                    return " { get; set; }";
            }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">属性成员名</param>
        /// <param name="desc">描述，写在注释中</param>
        /// <param name="returnType"> 返回值数据类型</param>
        /// <param name="sectype">封装性</param>
        /// <param name="isstatic">修饰符</param>
        public ClassMember(string name,string desc,DataType returnType,SecurityType sectype, bool isstatic)
        {
            this.Name = name;
            this.Description = desc;
            this.returnType = returnType;
            this.securityType = sectype;
            this.isStatic = isstatic;
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 返回值数据类型
        /// </summary>
        public DataType returnType = DataType.DataType_Void;
        /// <summary>
        /// 封装类型
        /// </summary>
        public SecurityType securityType = SecurityType.SecurityType_Public;
        /// <summary>
        /// 是否静态成员
        /// </summary>
        public bool isStatic = false;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取成员定义字符串序列
        /// </summary>
        /// <returns></returns>
        public List<string> GetMemberDefineStr()
        {
            m_Define = new List<string>();
            m_Define.Clear();
            m_Define.Add("/" +"/ "+ Description);
            string def = SecurityTypeHelper.GetTypeStr(securityType) + " " + (isStatic ? "" : "static ") + DataTypeHelper.GetTypeStr(returnType) + " " + Name;
            if (m_Body.Count <= 0)
            {
                def += DefineAddtionStr;
                m_Define.Add(def);
                return m_Define;
            }
            else
            {
                m_Define.Add(def);
                m_Define.AddRange(m_Body);
                return m_Define;
            }

        }

        public void ClearMemberBody()
        {
            m_Body.Clear();
        }

        public void AddStrToMemberBody(string codeStr)
        {
            m_Body.Add(codeStr);
        }

        public void InsertStrToMemberBody(int index,string codeStr)
        {
            m_Body.Insert(index,codeStr);
        }
    }
    #endregion

    #region 类方法成员类
    public class ClassMethod : ClassMember
    {
        public ClassMethod(string name,string desc,DataType returnType,SecurityType sectype, bool isstatic)
            :base(name,desc,returnType,sectype,isstatic)
        {
        }
    }
    #endregion
    #region 数据类型
    /// <summary>
    /// 枚举-数据类型
    /// </summary>
    public enum DataType
    {
        DataType_Void = 0,
        DataType_Int = 1,
        DataType_String = 2,
        DataType_Double = 3,
        DataType_IntList = 4,
        DataType_StringList = 5,
        DataType_DoubleList = 6
    }

    public class DataTypeHelper
    {
        public static string GetTypeStr(DataType type)
        {
            switch (type)
            {
                case DataType.DataType_Void:
                    return "void";
                case DataType.DataType_Int:
                    return "int";
                case DataType.DataType_Double:
                    return "double";
                case DataType.DataType_String:
                    return "string";
                case DataType.DataType_IntList:
                    return "List<int>";
                case DataType.DataType_DoubleList:
                    return "List<double>";
                case DataType.DataType_StringList:
                    return "List<string>";
                default:
                    return "void";
            }
        }
    }
    #endregion

    #region 安全性
    /// <summary>
    /// 枚举-数据类型
    /// </summary>
    public enum SecurityType
    {
        SecurityType_Public = 0,
        SecurityType_Private = 1,
        SecurityType_Protect = 2
    }

    public class SecurityTypeHelper
    {
        public static string GetTypeStr(SecurityType type)
        {
            switch (type)
            {
                case SecurityType.SecurityType_Public:
                    return "public";
                case SecurityType.SecurityType_Private:
                    return "private";
                case SecurityType.SecurityType_Protect:
                    return "protect";
                default:
                    return "public";
            }
        }
    }
    #endregion

}
