using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
//using dyc.utils;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.Win32;
using YUtils;

namespace RadanPlugIn.UC
{
    /// <summary>
    /// 零件属性类
    /// </summary>
    public class PartInfo
    {
        public string name { get; set; }
        public string value { get; set; }

    }

    /// <summary>
    /// 属性类
    /// </summary>
    public class AttributeInfo
    {
        public string name { get; set; }
        public string value { get; set; }

    }

    public struct SheetInfo
    {
        public string stockid;
        public string NumAvailable;
        public string Material;
        public string Thickness;
        public string ThickUnits;
        public string SheetX;
        public string SheetY;
        public string SheetUnits;
        public string Exclude;
        public string Priority;
    }

    /// <summary>
    /// Drg文件中的零件信息
    /// </summary>
    public struct DrgPartInfo
    {
        public string name;
        public string num;
        public string mate;
        public string mode;
        public string length;
        public string width;
        public string weight;
        public string area;//净面积
        public string areah;//包括孔的面积
        public string filepath;
        public string nestfilepath;
        public string maoweight;
        public string outerperim;
        public string perim;
        public string cost;
        public string Pierce;//引线/穿孔次数

    }

    /// <summary>
    /// 加工参数
    /// </summary>
    public class ProcessPram
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// 加工类型
    /// </summary>
    public enum _ProcType
    {
        /// <summary>
        /// 无加工
        /// </summary>
        procType_Null = 0,
        /// <summary>
        /// 切割
        /// </summary>
        procType_Cutting = 1,
        /// <summary>
        /// 激光切割
        /// </summary>
        procType_LaserCutting = 2
    }
    /// <summary>
    /// 加工类型处理类
    /// </summary>
    public class ProcSetting
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="procname">加工名称</param>
        public ProcSetting(_ProcType ptype)
        {
            this.ProcName = "";
            this.ProcType = ptype;
        }

        public virtual string ProcName { get; set; }
        public _ProcType ProcType { get; set; }
        public virtual string Material { get; set; }
        public virtual string Thickness { get; set; }
    }

    /// <summary>
    /// 切割处理类
    /// </summary>
    public class CutProc : ProcSetting
    {
        public List<ProcessPram> PramList = null;

        public int PramCount
        {
            get
            {
                return PramList.Count;
            }
        }

        public CutProc(_ProcType ptype, string inifilepath)
            : base(ptype)
        {
            if (ptype != _ProcType.procType_Cutting)
            {
                //MessageBox.Show("加工类型错误，不是切割类型！");
                return;
            }
            else
                this.ProcName = "切割";
            PramList = new List<ProcessPram>();
            PramList.Clear();
            INIHelper inifile = new INIHelper(inifilepath);
            string Names = inifile.ReadValue("CutProcPramList", "Names");
            string Discs = inifile.ReadValue("CutProcPramList", "Describes");
            bool nodis = false;
            if (!string.IsNullOrEmpty(Names))
            {
                string[] namelist = Names.Split(',');
                string[] dislist = Discs.Split(',');
                if (dislist.Length != namelist.Length)
                    nodis = true;
                for (int i = 0; i < namelist.Length; i++)
                {
                    ProcessPram Pram = new ProcessPram();
                    Pram.Name = namelist[i];
                    if (nodis)
                        Pram.Describe = namelist[i];
                    else
                        Pram.Describe = dislist[i];
                    Pram.Value = "0";
                    PramList.Add(Pram);
                }
            }
        }
        public string Profile { get; set; }

        /// <summary>
        /// 切割条件
        /// </summary>
        public virtual string Condition { get; set; }
        /// <summary>
        /// 切割速度
        /// </summary>
        public virtual string Speed { get; set; }
        /// <summary>
        /// 切割功率
        /// </summary>
        public virtual string cPower { get; set; }
        /// <summary>
        /// 占空比
        /// </summary>
        public virtual string Scale { get; set; }
        /// <summary>
        /// 气体类型
        /// </summary>
        public virtual string QitiType { get; set; }
        /// <summary>
        /// 气体压力
        /// </summary>
        public virtual string cPascal { get; set; }
        /// <summary>
        /// 切割脉冲类型
        /// </summary>
        public virtual string cMaiType { get; set; }
        /// <summary>
        /// 切割喷嘴间隙
        /// </summary>
        public virtual string cOffset { get; set; }
        /// <summary>
        /// 穿孔功率
        /// </summary>
        public virtual string hPower { get; set; }
        /// <summary>
        /// 穿孔压力
        /// </summary>
        /// <summary>
        public virtual string hPascal { get; set; }
        /// 穿孔时间
        /// </summary>
        /// <summary>
        public virtual string hTimeSpan { get; set; }
        /// 穿孔脉冲类型
        /// </summary>
        /// <summary>
        public virtual string hMaiType { get; set; }
        /// 穿孔喷嘴间隙
        /// </summary>
        public virtual string hOffset { get; set; }

    }

    /// <summary>
    /// 激光切割参数类
    /// </summary>
    public class LaserCutProc : ProcSetting
    {
        public List<ProcessPram> PramList = null;

        public int PramCount
        {
            get
            {
                return PramList.Count;
            }
        }

        public LaserCutProc(_ProcType ptype, string inifilepath)
            : base(ptype)
        {
            if (ptype != _ProcType.procType_LaserCutting)
            {
                //MessageBox.Show("加工类型错误，不是激光切割类型！");
                return;
            }
            else
                this.ProcName = "激光切割";
            PramList = new List<ProcessPram>();
            PramList.Clear();
            INIHelper inifile = new INIHelper(inifilepath);
            string Names = inifile.ReadValue("LaserProcPramList", "Names");
            string Discs = inifile.ReadValue("LaserProcPramList", "Describes");
            bool nodis = false;
            if (!string.IsNullOrEmpty(Names))
            {
                string[] namelist = Names.Split(',');
                string[] dislist = Discs.Split(',');
                if (dislist.Length != namelist.Length)
                    nodis = true;
                for (int i = 0; i < namelist.Length; i++)
                {
                    ProcessPram Pram = new ProcessPram();
                    Pram.Name = namelist[i];
                    if (nodis)
                        Pram.Describe = namelist[i];
                    else
                        Pram.Describe = dislist[i];
                    Pram.Value = "0";
                    PramList.Add(Pram);
                }
            }
        }
        public string Profile { get; set; }
    }

    public class MachineSetting
    {
        public struct SettingParam
        {
            public string name;
            public string value;
        }

        public MachineSetting(int machId)
        {
            Initialize(machId);
        }

        public int machid { get; set; }
        public virtual void Initialize(int machineId)
        {
            this.machid = machineId;
        }
    }
    public class LaserMachine : MachineSetting
    {
        public LaserMachine(int machId, string inifilepath)
            : base(machId)
        {
            PramList = new List<ProcessPram>();
            PramList.Clear();
            INIHelper inifile = new INIHelper(inifilepath);
            string Names = inifile.ReadValue("LaserMachinePram", "Names");
            string Discs = inifile.ReadValue("LaserMachinePram", "Describes");
            bool nodis = false;
            if (!string.IsNullOrEmpty(Names))
            {
                string[] namelist = Names.Split(',');
                string[] dislist = Discs.Split(',');
                if (dislist.Length != namelist.Length)
                    nodis = true;
                for (int i = 0; i < namelist.Length; i++)
                {
                    ProcessPram Pram = new ProcessPram();
                    Pram.Name = namelist[i];
                    if (nodis)
                        Pram.Describe = namelist[i];
                    else
                        Pram.Describe = dislist[i];
                    Pram.Value = "0";
                    PramList.Add(Pram);
                }
            }
        }

        public List<ProcessPram> PramList = null;

        public int PramCount
        {
            get
            {
                return PramList.Count;
            }
        }
    }

    public class MachineSetOne : MachineSetting
    {
        public MachineSetOne(int machId)
            : base(machId)
        {
        }
        public override void Initialize(int machineId)
        {
            base.Initialize(machineId);
        }

        public string isCenterHole { get; set; }
        public string cdie { get; set; }
        public string isSmallHole { get; set; }
        public string die { get; set; }
        public string condition { get; set; }
        public string bsStartslowly { get; set; }
        public string yinrulen { get; set; }
        public string janlen { get; set; }
        public string bguaijiaoslowly { get; set; }
        public string staslowcondition { get; set; }
        public string jiaodu { get; set; }
        public string length { get; set; }
        public string condjiansu { get; set; }
        public string yanshi { get; set; }
        public string btop { get; set; }
        public string frog { get; set; }
    }
    #region old-multimodel class
    public class RadanMtfTool
    {
        public string name { get; set; }
        public double offset_X { get; set; }
        public double offset_Y { get; set; }
        public string sizeflag { get; set; }
        public string rotation { get; set; }
        public string ThumbFile { get; set; }
        public string description { get; set; }
        public Image ImageThumb { get; set; }

    }
    
    public class RadanMtfModel : RadanMtfTool
    {
        public string cplexnum { get; set; }
        public List<RadanMtfTool> mtflist = new List<RadanMtfTool>();

        public RadanMtfModel()
        {
            cplexnum = "0";
            mtflist.Clear();
        }
    }

    public class RadanMtfToolList
    {
        public List<RadanMtfTool> mtftoollist = new List<RadanMtfTool>();

        public RadanMtfToolList(string radandatpath, string mtffile)
        {
            RadMtfFile mtf = new RadMtfFile();
            mtf.Load(mtffile);
            mtftoollist = mtf.GetToolList(radandatpath);
        }
    }
    #endregion
    /// <summary>
    /// 工位类型 
    /// </summary>
    public enum _StationType
    {
        /// <summary>
        /// 只能放单把刀的工位类型
        /// </summary>
        isStand = 0,
        /// <summary>
        /// 子模的子工位类型
        /// </summary>
        isMulti = 1
    }
    /// <summary>
    /// 刀具类
    /// </summary>
    public class RadanMtf
    {
        /// <summary>
        /// 刀具名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 刀具尺寸X
        /// </summary>
        public double sizeX { get; set; }
        /// <summary>
        /// 刀具尺寸Y
        /// </summary>
        public double sizeY { get; set; }
        /// <summary>
        /// 刀具缩略图存放路径
        /// </summary>
        public string ThumbFile { get; set; }
        /// <summary>
        /// 刀具描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 下模间隙
        /// </summary>
        public double modeloffset { get; set; }
        /// <summary>
        /// 刀具缩略图Image对象
        /// </summary>
        public Image ImageThumb { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public string deg { get; set; }

    }
    /// <summary>
    /// 刀具集合
    /// </summary>
    public class RadanMtfList
    {
        public List<RadanMtf> mtftoollist = new List<RadanMtf>();

        /// <summary>
        /// 从radan刀具文件获取刀具集合
        /// </summary>
        /// <param name="radandatpath"></param>
        /// <param name="mtffile"></param>
        public RadanMtfList(string radandatpath, string mtffile)
        {
            RadMtfFile mtf = new RadMtfFile();
            mtf.Load(mtffile);
            mtftoollist = mtf.GetMtfList(radandatpath);
        }
    }
    /// <summary>
    /// 工位对象类
    /// </summary>
    public class RadanStation
    {
        /// <summary>
        /// 转塔上的主模工位号
        /// </summary>
        public int mainID { get; set; }
        /// <summary>
        /// 本工位显示在转塔上的工位号
        /// </summary>
        public int showID { get { return (stationType == _StationType.isMulti)?(mainID * 100 + id):mainID; } }
        /// <summary>
        /// 工位类型
        /// </summary>
        public _StationType stationType { get; set; }
        /// <summary>
        /// 工位ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 工位大小
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 工位旋转标识
        /// </summary>
        public string rotation { get; set; }
        /// <summary>
        /// X偏置
        /// </summary>
        public double offset_X { get; set; }
        /// <summary>
        /// Y偏置
        /// </summary>
        public double offset_Y { get; set; }
        /// <summary>
        /// 刀具
        /// </summary>
        public RadanMtf maftool{get;set;}
    }
    /// <summary>
    /// 子模对象类
    /// </summary>
    public class RadanModel
    {
        private int _cplexnum;
        private int _stationId;
        private string _description;
        /// <summary>
        /// 子模名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 子模描述
        /// </summary>
        public string description
        { 
            get { return (cplexnum > 0) ? (cplexnum.ToString() + "-子模") : "空子模"; } 
            set { _description = value; }
        }
        /// <summary>
        /// 子模的工位号
        /// </summary>
        public int stationId 
        {
            get { return _stationId; }
            set
            {
                _stationId = value;
                for (int i = 1; i < _cplexnum + 1; i++)
                {
                    RadanStation modelstation = stationlist[i - 1];
                    modelstation.mainID = stationId;
                }
            }
        }
        /// <summary>
        /// 子工位个数 子工位个数更改会导致内部子工位内容重置。
        /// </summary>
        public int cplexnum 
        {
            get { return _cplexnum; }
            set 
            {
                _cplexnum = value;
                stationlist.Clear();
                for (int i = 1; i < _cplexnum + 1; i++)
                {
                    RadanStation modelstation = new RadanStation();
                    modelstation.id = i;
                    modelstation.mainID = stationId;
                    modelstation.stationType = _StationType.isMulti;
                    modelstation.offset_X = 0;
                    modelstation.offset_Y = 0;
                    modelstation.rotation = rotation;
                    modelstation.size = size;

                    stationlist.Add(modelstation);
                }
            } 
        }
        /// <summary>
        /// 旋转标识
        /// </summary>
        public string rotation { get; set; }
        /// <summary>
        /// 子模所在工位大小
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 子工位列表
        /// </summary>
        public List<RadanStation> stationlist = new List<RadanStation>();

        public RadanModel()
        {
            name = "";
            stationId = 1;
            rotation = "YES";
            size = "A";
            cplexnum = 1;
        }
    }
    /// <summary>
    /// 转塔工位类
    /// </summary>
    public class RadTower
    {
        string stastring { get; set; }
        StringCollection strlist = new StringCollection();
        List<RadanStation> stationlist = new List<RadanStation>();
        public void Add(string strline)
        {
            strlist.Add(strline);
        }

        public void Add(string Id,string Sizerange,string Tooltype, string Tracknumber,string Autoindex,string Xdistance,string Ydistance,string clampzoneshape,string combination)
        {
            string strline = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", Id, Sizerange, Tooltype, Tracknumber, Autoindex, Xdistance, Ydistance, clampzoneshape, combination); 
            strlist.Add(strline);
        }

        public void Modify(int index,string Sizerange, string Tooltype, string Tracknumber, string Autoindex, string Xdistance, string Ydistance, string clampzoneshape, string combination)
        {
            string[] strline = strlist[index].Split('\t');
            if (!string.IsNullOrEmpty(Sizerange))
                strline[1] = Sizerange;
            if (!string.IsNullOrEmpty(Tooltype))
                strline[2] = Tooltype;
            if (!string.IsNullOrEmpty(Tracknumber))
                strline[3] = Tracknumber;
            if (!string.IsNullOrEmpty(Autoindex))
                strline[4] = Autoindex;
            if (!string.IsNullOrEmpty(Xdistance))
                strline[5] = Xdistance;
            if (!string.IsNullOrEmpty(Ydistance))
                strline[6] = Ydistance;
            if (!string.IsNullOrEmpty(clampzoneshape))
                strline[7] = clampzoneshape;
            if (!string.IsNullOrEmpty(combination))
                strline[8] = combination;
            string outstr = "";
            for (int i = 0; i < 9; i++)
            {
                outstr += (strline[i] + "\t");
            }
            strlist.RemoveAt(index);
            strlist.Insert(index, outstr);
        }
    }

    /// <summary>
    /// Radan系统参数类
    /// </summary>
    public class RadHelper
    {
        static public string CreateAbsoluteXPath(params string[] paths)
        {
            if (paths.Length <= 0) return "";
            string xpath = "";
            foreach (string path in paths)
            {
                xpath += "/drg:" + path.Trim();
            }
            return xpath;
        }
        static public string CreateRelationXPath(params string[] paths)
        {
            if (paths.Length <= 0) return "";
            string xpath = "";
            foreach (string path in paths)
            {
                xpath += "/drg:" + path.Trim();
            }
            return xpath.Substring(1, xpath.Length - 1);
        }

        static public string RegGetValue_LocalMachine(string key, string name)
        {
            string registData;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey path = hkml.OpenSubKey(key, true);
            registData = path.GetValue(name).ToString();
            return registData;
        }

        /// <summary>
        /// Current Version
        /// </summary>
        static public string RadanCurrVersion
        {
            get
            {

                object obj = RegGetValue_LocalMachine(@"SOFTWARE\Radan\Radraft", "Current Version");
                if (obj == null) return "";
                else
                    return obj.ToString();
            }
        }
        /// <summary>
        /// SOFTWARE\Radan\Radraft\29.1.02
        /// </summary>
        static public string RadanCurrVersionRegName
        {
            get
            {
                string currVersion = RadanCurrVersion;
                if (string.IsNullOrEmpty(currVersion)) return "";
                return @"SOFTWARE\Radan\Radraft\" + currVersion;
            }
        }
        /// <summary>
        /// radan_path
        /// </summary>
        static public string RadanPath
        {
            get
            {
                object obj = RegGetValue_LocalMachine(RadanCurrVersionRegName, "radan_path");
                if (obj == null) return "";
                else
                    return obj.ToString();
            }
        }
        /// <summary>
        /// dat_path
        /// </summary>
        static public string Radan_DataPath
        {
            get
            {
                object obj = RegGetValue_LocalMachine(RadanCurrVersionRegName, "dat_path");
                if (obj == null) return "";
                else
                    return obj.ToString();
            }
        }
        /// <summary>
        /// RADRAFT.exe
        /// </summary>
        static public string RadanDraftFilePath
        {
            get
            {
                string radanPath = RadanPath;
                if (string.IsNullOrEmpty(RadanPath)) return "";
                return radanPath + @"\bin\RADRAFT.exe";
            }
        }
        /// <summary>
        /// sheet_stock.xml
        /// </summary>
        static public string SteelStockFilePath
        {
            get
            {
                string datPath = Radan_DataPath;
                if (string.IsNullOrEmpty(datPath)) return "";
                return datPath + @"\sheet_stock.xml";
            }
        }
        /// <summary>
        /// E:\Radan\configuration_data\dat\nest_results.Administrator.xml
        /// </summary>
        static public string NestResultXmlFilePath
        {
            get
            {
                string datPath = Radan_DataPath;
                if (string.IsNullOrEmpty(datPath)) return "";
                return datPath + @"\nest_results." + Environment.UserName + ".xml";
            }
        }

        static public string NestSchedRnsFilePath
        {
            get
            {
                string datPath = Radan_DataPath;
                if (string.IsNullOrEmpty(datPath)) return "";
                return datPath + @"\nest_sched." + Environment.UserName + ".rns";
            }
        }

        static public bool Nest(int timeout)
        {
            string radFileName = RadanDraftFilePath;
            if (!File.Exists(radFileName)) return false;
            string radResultXmlFileName = NestResultXmlFilePath;
            if (File.Exists(radResultXmlFileName))
                File.Delete(radResultXmlFileName);
            string args = " -embedding -mac gwnest "; // ' 
            ShellExcute(radFileName, args, 60000);
            return File.Exists(radResultXmlFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <param name="timeout">超时时间,单位milliseconds</param>
        static public void ShellExcute(string fileName, string args = "", int timeout = 0)
        {
            Process p = new Process();
            p.StartInfo.FileName = fileName;
            if (!string.IsNullOrEmpty(args))
            {
                p.StartInfo.Arguments = args;
            }
            //GO
            p.Start();
            if (timeout > 0)
                p.WaitForExit(timeout);
        }

        static public string CurMachineId
        {
            get
            {
                string curpsysuserfile = CurPsysUser;

                StreamReader sr = new StreamReader(curpsysuserfile, System.Text.Encoding.Default);
                string str = "";
                bool bstart = false;
                while (sr.Peek() > 0)
                {
                    str = sr.ReadLine().ToString();
                    if (str.Contains("# Current machine tool being used"))
                    {
                        bstart = true;
                        continue;
                    }
                    if (bstart)
                    {
                        if (str.Contains("u10"))
                        {
                            break;
                        }
                    }
                }
                sr.Close();

                return str.Replace("u10", "").Trim();
            }
        }

        static public string CurPsysUser
        {
            get
            {
                string datPath = Radan_DataPath;
                if (string.IsNullOrEmpty(datPath)) return "";
                return datPath + @"\psys_user." + Environment.UserName;
            }
        }
    }

    #region RadanFiles类s
    /// <summary>
    /// 刀具文件类
    /// </summary>
    public class RadMtfFile : XmlDocument
    {
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/mtfile");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            this.FileName = fileName;
            base.Load(this.FileName);
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }

        public int ToolListCount
        {
            get
            {
                string xpath = "/drg:RadanToolFile/drg:ToolList";
                XmlNode xmlNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == "count")
                    {
                        return Convert.ToInt16(xmlAttr.Item(i).Value);
                    }
                }

                return 0;
            }
        }

        public List<RadanMtfTool> GetToolList(string radandatpath)
        {
            string radansymbols = radandatpath.Replace("configuration_data\\dat", "system-symbols\\");


            List<RadanMtfTool> mtftoollist = new List<RadanMtfTool>();
            string xpath = "/drg:RadanToolFile/drg:ToolList";
            XmlNode toollistNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            try
            {
                if (toollistNode != null)
                {
                    for (int i = 0; i < toollistNode.ChildNodes.Count; i++)
                    {
                        RadanMtfTool mtftool = new RadanMtfTool();
                        XmlNode toolNode = toollistNode.ChildNodes[i];
                        for (int j = 0; j < toolNode.ChildNodes.Count; j++)
                        {
                            XmlNode childNode = toolNode.ChildNodes[j];
                            if (childNode.Name == "ToolName")
                            {
                                mtftool.name = childNode.InnerText;
                                string filename = mtftool.name;

                                string symfilename = radansymbols + filename + ".sym";
                                if (!File.Exists(symfilename))
                                    continue;
                                RadSymFile sym = new RadSymFile();
                                sym.Load(symfilename);
                                Image img = sym.Thumbnail;
                                mtftool.ImageThumb = sym.Thumbnail;

                            }
                            if (childNode.Name == "Description")
                                mtftool.description = childNode.InnerText;
                            if (childNode.Name == "SizeX")
                                mtftool.offset_X = childNode.InnerText.ToDouble();
                            if (childNode.Name == "SizeY")
                                mtftool.offset_Y = childNode.InnerText.ToDouble();

                        }
                        mtftoollist.Add(mtftool);
                    }
                    if (mtftoollist == null) return null;
                    else
                    {
                        return mtftoollist;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public List<RadanMtf> GetMtfList(string radandatpath)
        {
            string radansymbols = radandatpath.Replace("configuration_data\\dat", "system-symbols\\");


            List<RadanMtf> mtftoollist = new List<RadanMtf>();
            string xpath = "/drg:RadanToolFile/drg:ToolList";
            XmlNode toollistNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            try
            {
                if (toollistNode != null)
                {
                    for (int i = 0; i < toollistNode.ChildNodes.Count; i++)
                    {
                        RadanMtf mtftool = new RadanMtf();
                        XmlNode toolNode = toollistNode.ChildNodes[i];
                        for (int j = 0; j < toolNode.ChildNodes.Count; j++)
                        {
                            XmlNode childNode = toolNode.ChildNodes[j];
                            if (childNode.Name == "ToolName")
                            {
                                mtftool.name = childNode.InnerText;
                                string filename = mtftool.name;

                                string symfilename = radansymbols + filename + ".sym";
                                if (!File.Exists(symfilename))
                                    continue;
                                RadSymFile sym = new RadSymFile();
                                sym.Load(symfilename);
                                Image img = sym.Thumbnail;
                                mtftool.ImageThumb = sym.Thumbnail;

                            }
                            if (childNode.Name == "Description")
                                mtftool.description = childNode.InnerText;
                            if (childNode.Name == "SizeX")
                                mtftool.sizeX = childNode.InnerText.ToDouble();
                            if (childNode.Name == "SizeY")
                                mtftool.sizeY = childNode.InnerText.ToDouble();

                        }
                        mtftoollist.Add(mtftool);
                    }
                    if (mtftoollist == null) return null;
                    else
                    {
                        return mtftoollist;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

    }
    /// <summary>
    /// 项目文件XML超类
    /// </summary>
    public class RadProjectFile : XmlDocument
    {
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/project");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            try
            {
                this.FileName = fileName;
                base.Load(this.FileName);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }
    }
    /// <summary>
    /// 项目文件类
    /// </summary>
    public class RadRPDFile : RadProjectFile
    {
        
        #region 零件
        /// <summary>
        /// 删除所有零件信息
        /// </summary>
        public void RemoveALlParts()
        {
            string xpath = "/drg:RadanProject/drg:Parts";
            XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            if (partsNode != null)
            {
                partsNode.RemoveAll();
            }
        }
        /// <summary>
        /// 获取所有任务零件
        /// </summary>
        /// <param name="Partpathlist"></param>
        public void GetParts(ref StringCollection Partpathlist)
        {
            string xpath = "/drg:RadanProject/drg:Parts";
            XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            if (partsNode != null)
            {
                for (int i = 1; i < partsNode.ChildNodes.Count; i++)
                {
                    string symbol = partsNode.ChildNodes[i].ChildNodes[1].InnerText;
                    Partpathlist.Add(symbol);
                }
            }
        }
        /// <summary>
        /// 添加任务零件
        /// </summary>
        /// <param name="listparts"></param>
        public void SetParts(List<List<PartInfo>> listparts)
        {
            string xpath = "/drg:RadanProject/drg:Parts";
            XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            if (partsNode != null)
            {
                for (int i = 0; i < listparts.Count; i++)
                {
                    List<PartInfo> partinfos = listparts[i];
                    XmlElement ele = this.CreateElement("Part");
                    XmlElement ele1 = this.CreateElement("Symbol");
                    ele1.InnerText = partinfos[0].value;
                    ele.AppendChild(ele1);
                    XmlElement ele2 = this.CreateElement("Kit");
                    ele2.InnerText = "-";
                    ele.AppendChild(ele2);
                    XmlElement ele3 = this.CreateElement("Number");
                    ele3.InnerText = partinfos[6].value;
                    ele.AppendChild(ele3);
                    XmlElement ele4 = this.CreateElement("NumExtra");
                    ele4.InnerText = "0";
                    ele.AppendChild(ele4);
                    XmlElement ele5 = this.CreateElement("Priority");
                    ele5.InnerText = "5";
                    ele.AppendChild(ele5);
                    XmlElement ele6 = this.CreateElement("Bin");
                    ele6.InnerText = "0";
                    ele.AppendChild(ele6);
                    XmlElement ele7 = this.CreateElement("Orient");
                    ele7.InnerText = "8";
                    ele.AppendChild(ele7);
                    XmlElement ele8 = this.CreateElement("Mirror");
                    ele8.InnerText = "n";
                    ele.AppendChild(ele8);
                    XmlElement ele9 = this.CreateElement("CCut");
                    ele9.InnerText = "none";
                    ele.AppendChild(ele9);
                    XmlElement ele10 = this.CreateElement("PickingCluster");
                    ele10.InnerText = "n";
                    ele.AppendChild(ele10);
                    XmlElement ele11 = this.CreateElement("Material");
                    ele11.InnerText = partinfos[5].value;
                    ele.AppendChild(ele11);
                    XmlElement ele12 = this.CreateElement("Thickness");
                    ele12.InnerText = partinfos[7].value;
                    ele.AppendChild(ele12);
                    XmlElement ele13 = this.CreateElement("ThickUnits");
                    ele13.InnerText = "mm";
                    ele.AppendChild(ele13);
                    XmlElement ele14 = this.CreateElement("Strategy");
                    ele.AppendChild(ele14);
                    XmlElement ele15 = this.CreateElement("Exclude");
                    ele15.InnerText = "n";
                    ele.AppendChild(ele15);
                    XmlElement ele16 = this.CreateElement("Made");
                    ele16.InnerText = "0";
                    ele.AppendChild(ele16);

                    ((XmlElement)partsNode).AppendChild(ele);
                }

            }
        }
        #endregion

        #region 板材

        /// <summary>
        /// 是否使用项目板材y-n
        /// </summary>
        public string UseProjectSheets
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:UseProjectSheets";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:UseProjectSheets";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 使用的板材信息
        /// </summary>
        public void AddSheetSource(List<SheetInfo> sheetlist)
        {
            try
            {
                string sheetcount = "0";
                string xpath = "/drg:RadanProject/drg:Sheets";
                XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                for(int j = 0;j<partsNode.Attributes.Count;j++)
                {
                    if (partsNode.Attributes[j].Name == "count")
                        sheetcount = partsNode.Attributes[j].Value;
                }
                if (partsNode != null)
                {
                    for (int i = 0; i < sheetlist.Count; i++)
                    {
                        SheetInfo sheetinfo = sheetlist[i];
                        XmlElement ele = this.CreateElement("Sheet");

                        XmlElement ele1 = this.CreateElement("NumAvailable");
                        ele1.InnerText = sheetinfo.NumAvailable;
                        ele.AppendChild(ele1);

                        XmlElement ele2 = this.CreateElement("Material");
                        ele2.InnerText = sheetinfo.Material;
                        ele.AppendChild(ele2);

                        XmlElement ele3 = this.CreateElement("Thickness");
                        ele3.InnerText = sheetinfo.Thickness;
                        ele.AppendChild(ele3);

                        XmlElement ele4 = this.CreateElement("ThickUnits");
                        ele4.InnerText = sheetinfo.ThickUnits;
                        ele.AppendChild(ele4);

                        XmlElement ele5 = this.CreateElement("SheetX");
                        ele5.InnerText = sheetinfo.SheetX;
                        ele.AppendChild(ele5);

                        XmlElement ele6 = this.CreateElement("SheetY");
                        ele6.InnerText = sheetinfo.SheetY;
                        ele.AppendChild(ele6);

                        XmlElement ele7 = this.CreateElement("SheetUnits");
                        ele7.InnerText = sheetinfo.SheetUnits;
                        ele.AppendChild(ele7);

                        XmlElement ele8 = this.CreateElement("StockID");
                        ele8.InnerText = sheetinfo.stockid;
                        ele.AppendChild(ele8);

                        XmlElement ele9 = this.CreateElement("Exclude");
                        ele9.InnerText = sheetinfo.Exclude;
                        ele.AppendChild(ele9);

                        XmlElement ele10 = this.CreateElement("Priority");
                        ele10.InnerText = sheetinfo.Priority;
                        ele.AppendChild(ele10);

                        ((XmlElement)partsNode).AppendChild(ele);
                    }

                    sheetcount += sheetlist.Count;
                }
            }
            catch(SystemException ex)
            {
            }
        }
        #endregion

        #region 输出选项
        /// <summary>
        /// 套料名称
        /// </summary>
        public string JobName
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:JobName";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:JobName";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 是否显示注释y-n
        /// </summary>
        public string Annotate
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Annotate";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Annotate";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 是否显示输出到绘图仪y-n
        /// </summary>
        public string Plot
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Plot";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Plot";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 是否显示运行y-n
        /// </summary>
        public string Graphics
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Graphics";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:Graphics";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 排样结果保存路径
        /// </summary>
        public string nests
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:NestFolder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:NestFolder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        #endregion

        #region 套料选项
        /// <summary>
        /// 是否真实形状套料y-n
        /// </summary>
        public string DoShaped
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:DoShaped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:DoShaped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 是否匹配材料套料y-n
        /// </summary>
        public string MatchMat
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MatchMat";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:MatchMat";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }

                xpath = "/drg:RadanProject/drg:MatchMat/drg:JobDetails/drg:MatchMat";
                nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }

            }
        }

        /// <summary>
        /// 每次套料计算时间（秒）
        /// </summary>
        public string MaxTime
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MaxTime";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MaxTime";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 是否最少程序y-n
        /// </summary>
        public string ProgRed
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ProgRed";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ProgRed";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 可接受的利用率
        /// </summary>
        public string ProgRedUtil
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ProgRedUtil";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ProgRedUtil";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 是否允许孔内套料y-n
        /// </summary>
        public string NestInHoles
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:NestInHoles";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:NestInHoles";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 首要安放位置：0代表左   1代表右   2代表下   3代表上
        /// </summary>
        public string PrimaryDir
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PrimaryDir";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PrimaryDir";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 次要安放位置：0代表左   1代表右   2代表下   3代表上
        /// </summary>
        public string SecondaryDir
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SecondaryDir";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SecondaryDir";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 每X张套料更新
        /// </summary>
        public string AUNumNests
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AUNumNests";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AUNumNests";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 每X张板材更新
        /// </summary>
        public string AUNumSheets
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AUNumSheets";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AUNumSheets";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 是否自动任务单更新
        /// </summary>
        public string AutoUpdate
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AutoUpdate";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AutoUpdate";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        #endregion

        #region 间隙
        /// <summary>
        /// 零件X方向间距
        /// </summary>
        public string PartGapX
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapX";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapX";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 零件Y方向间距
        /// </summary>
        public string PartGapY
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapY";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapY";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 零件XY间距
        /// </summary>
        public string PartGapXY
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapXY";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:PartGapXY";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 基准侧边距最小值
        /// </summary>
        public string MinDatumVert
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinDatumVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinDatumVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 非基准侧边距最小值
        /// </summary>
        public string MinNondatumVert
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinNondatumVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinNondatumVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 最优非基准侧边距最小值
        /// </summary>
        public string OptVert
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:OptVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:OptVert";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 夹钳边是否有间隙y-n
        /// </summary>
        public string ClampStripFlag
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampStripFlag";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampStripFlag";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 夹钳边使用夹钳带（n）还是夹钳区域（y）
        /// </summary>
        public string ClampZoneFlag
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampZoneFlag";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampZoneFlag";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 夹钳带宽度
        /// </summary>
        public string ClampStripWidth
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampStripWidth";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampStripWidth";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 夹钳区域宽度
        /// </summary>
        public string ClampSizeParallel
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampSizeParallel";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampSizeParallel";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 夹钳区域高度
        /// </summary>
        public string ClampSizePerpendicular
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampSizePerpendicular";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:ClampSizePerpendicular";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 非夹钳侧边距
        /// </summary>
        public string MinUnclamped
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinUnclamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinUnclamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 最优非夹钳侧边距
        /// </summary>
        public string OptUnclamped
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:OptUnclamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:OptUnclamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 非矩形板材边距
        /// </summary>
        public string SheetDrgBorder
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SheetDrgBorder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SheetDrgBorder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 最小夹钳
        /// </summary>
        public string MinClamped
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinClamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:MinClamped";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        #endregion

        #region 自动化
        #endregion

        #region 余料

        /// <summary>
        /// 是否寻找最佳余料y-n
        /// </summary>
        public string UseRemnants
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:UseRemnants";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:UseRemnants";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 余料文件夹
        /// </summary>
        public string RemnantUseFolder
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantUseFolder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantUseFolder";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }
        /// <summary>
        /// 余料最佳利用率 0到100之间
        /// </summary>
        public string RemnantPremium
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantPremium";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantPremium";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 余料优先级1~10 1最优先
        /// </summary>
        public string RemnantPriority
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantPriority";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantPriority";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 是否保存余料到文件夹y-n
        /// </summary>
        public string SaveRemnants
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SaveRemnants";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SaveRemnants";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 余料需要保存的最小面积
        /// </summary>
        public string RemnantAreaWorthSaving
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantAreaWorthSaving";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantAreaWorthSaving";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 仅保存矩形余料y-n
        /// </summary>
        public string SaveRectangularRemnantsOnly
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SaveRectangularRemnantsOnly";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:SaveRectangularRemnantsOnly";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 矩形长边沿着夹钳带
        /// </summary>
        public string AlignRectanglesWithClampStrip
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AlignRectanglesWithClampStrip";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:AlignRectanglesWithClampStrip";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 生成余料保存路径
        /// </summary>
        public string remnants
        {
            get
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantSaveFolder";
                XmlNode remnant = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (remnant != null)
                {
                    return ((XmlElement)remnant).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanProject/drg:RadanSchedule/drg:JobDetails/drg:RemnantSaveFolder";
                XmlNode remnant = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (remnant != null)
                {
                    ((XmlElement)remnant).InnerText = value;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Rns文件类
    /// </summary>
    public class RadRnsFile : XmlDocument
    {
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/rns");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            this.FileName = fileName;
            base.Load(this.FileName);
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }

        public string RemnantSaveFolder
        {
            get
            {
                string xpath = "/drg:RadanSchedule/drg:JobDetails/drg:RemnantSaveFolder";
                XmlNode xmlNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                string folder = xmlNode.InnerText;
                return folder;
            }
        }

        public string NestFolder
        {
            get
            {
                string xpath = "/drg:RadanSchedule/drg:JobDetails/drg:NestFolder";
                XmlNode xmlNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                string folder = xmlNode.InnerText;
                return folder;
            }
        }
        public void SetSheet(string width)
        {
            string xpath = "/drg:RadanSchedule/drg:JobDetails/drg:SheetSource";
            XmlNode xmlNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            for (int i = 0; i < xmlNode.Attributes.Count; i++)
            {
                XmlAttribute att = xmlNode.Attributes[i];
                if (att.Name == "source")
                {
                    att.Value = "multiple";
                    break;
                }
            }
            XmlElement sheetsnode = this.CreateElement("Sheets");
            sheetsnode.SetAttribute("count", "1");
            xmlNode.AppendChild(sheetsnode);

            XmlElement sheetnode = this.CreateElement("Sheet");
            sheetnode.SetAttribute("count", "1");
            sheetsnode.AppendChild(sheetnode);

            XmlElement ch1 = this.CreateElement("NumAvailable");
            ch1.InnerText = "9999";
            sheetnode.AppendChild(ch1);

            XmlElement ch2 = this.CreateElement("Material");
            ch2.InnerText = "-";
            sheetnode.AppendChild(ch2);

            XmlElement ch3 = this.CreateElement("Thickness");
            ch3.InnerText = "1";
            sheetnode.AppendChild(ch3);

            XmlElement ch4 = this.CreateElement("ThickUnits");
            ch4.InnerText = "mm";
            sheetnode.AppendChild(ch4);

            XmlElement ch5 = this.CreateElement("SheetX");
            ch5.InnerText = "25000";
            sheetnode.AppendChild(ch5);

            XmlElement ch6 = this.CreateElement("SheetY");
            ch6.InnerText = width;
            sheetnode.AppendChild(ch6);

            XmlElement ch7 = this.CreateElement("SheetUnits");
            ch7.InnerText = "mm";
            sheetnode.AppendChild(ch7);

            XmlElement ch8 = this.CreateElement("StockID");
            ch8.InnerText = "Stock43680";
            sheetnode.AppendChild(ch8);
        }

        public void SetParts(StringCollection partfiles)
        {
            string xpath = "/drg:RadanSchedule/drg:Parts";
            XmlNode xmlNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            for (int i = 0; i < xmlNode.Attributes.Count; i++)
            {
                XmlAttribute att = xmlNode.Attributes[i];
                if (att.Name == "count")
                {
                    att.Value = partfiles.Count.ToString();
                    break;
                }
            }
            for (int j = 0; j < partfiles.Count; j++)
            {
                XmlElement resnode = this.CreateElement("Part");
                xmlNode.AppendChild(resnode);
                XmlElement ch1 = this.CreateElement("Symbol");
                ch1.InnerText = partfiles[j];
                resnode.AppendChild(ch1);

                XmlElement ch2 = this.CreateElement("Kit");
                ch2.InnerText = "-";
                resnode.AppendChild(ch2);

                XmlElement ch3 = this.CreateElement("Number");
                ch3.InnerText = "1";
                resnode.AppendChild(ch3);

                XmlElement ch4 = this.CreateElement("NumExtra");
                ch4.InnerText = "0";
                resnode.AppendChild(ch4);

                XmlElement ch5 = this.CreateElement("Priority");
                ch5.InnerText = "5";
                resnode.AppendChild(ch5);

                XmlElement ch6 = this.CreateElement("Bin");
                ch6.InnerText = "0";
                resnode.AppendChild(ch6);

                XmlElement ch7 = this.CreateElement("Orient");
                ch7.InnerText = "8";
                resnode.AppendChild(ch7);

                XmlElement ch8 = this.CreateElement("Mirror");
                ch8.InnerText = "n";
                resnode.AppendChild(ch8);

                XmlElement ch9 = this.CreateElement("CCut");
                ch9.InnerText = "n";
                resnode.AppendChild(ch9);

                XmlElement ch10 = this.CreateElement("PickingCluster");
                ch10.InnerText = "n";
                resnode.AppendChild(ch10);

                XmlElement ch11 = this.CreateElement("Material");
                ch11.InnerText = "-";
                resnode.AppendChild(ch11);

                XmlElement ch12 = this.CreateElement("Thickness");
                ch12.InnerText = "1";
                resnode.AppendChild(ch12);

                XmlElement ch13 = this.CreateElement("ThickUnits");
                ch13.InnerText = "mm";
                resnode.AppendChild(ch13);

                XmlElement ch14 = this.CreateElement("Strategy");
                ch14.InnerText = partfiles[j];
                resnode.AppendChild(ch14);

            }
        }
    }
    /// <summary>
    /// Rcd文件节点类
    /// </summary>
    public class RadRcdNode
    {
        public XmlElement Element { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(Element.OwnerDocument.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/rcd");
                return xnml;
            }
        }
        public RadRcdNode(XmlElement xe)
        {
            this.Element = xe;
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }
        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateRelationXPath(paths);
            return Element.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateRelationXPath(paths);
            return Element.SelectNodes(xpath, xnml);
        }
    }
    /// <summary>
    /// Rcd文件XML超类
    /// </summary>
    public class RadRcdFile : XmlDocument
    {
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/rcd");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            this.FileName = fileName;
            base.Load(this.FileName);
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }

    }
    /// <summary>
    /// Drg文件类
    /// </summary>
    public class RadDrgFile : RadRcdFile
    {
        /// <summary>  

        /// 將 Byte 陣列轉換為 Image。  

        /// </summary>  

        /// <param name="Buffer">Byte 陣列。</param>          

        public Image BufferToImage(byte[] Buffer)
        {

            if (Buffer == null || Buffer.Length == 0) { return null; }

            byte[] data = null;

            Image oImage = null;

            Bitmap oBitmap = null;

            //建立副本  

            data = (byte[])Buffer.Clone();

            try
            {

                MemoryStream oMemoryStream = new MemoryStream(Buffer);

                //設定資料流位置  

                oMemoryStream.Position = 0;

                oImage = System.Drawing.Image.FromStream(oMemoryStream);

                //建立副本  

                oBitmap = new Bitmap(oImage);

            }

            catch
            {

                throw;

            }

            //return oImage;  

            return oBitmap;

        }

        public byte[] ImageToBuffer(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本  
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式  
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置  
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度  
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer  
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流  
                    oMemoryStream.Flush();
                }
            }
            return data;
        }

        public string PngString
        {
            get
            {
                string outstr = "";
                string xpath = "/drg:RadanCompoundDocument/drg:SetupSheet/drg:DrawingInformation/drg:Image[@image-type='png']";
                XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (bmpNode != null)
                    {
                        for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                        {
                            XmlNode childNode = bmpNode.ChildNodes[i];
                            if (childNode.NodeType == XmlNodeType.CDATA)
                            {
                                string dataStr = childNode.Value;
                                if (!string.IsNullOrEmpty(dataStr))
                                {
                                    outstr = dataStr;
                                    break;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(outstr)) return "";
                    else
                        return outstr;
                }
                catch
                {
                    return "";
                }
            }
        }

        public Image Png
        {
            get
            {
                MemoryStream ms = null;
                string xpath = "/drg:RadanCompoundDocument/drg:SetupSheet/drg:DrawingInformation/drg:Image[@image-type='png']";
                XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (bmpNode != null)
                    {
                        for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                        {
                            XmlNode childNode = bmpNode.ChildNodes[i];
                            if (childNode.NodeType == XmlNodeType.CDATA)
                            {
                                string dataStr = childNode.Value;
                                if (dataStr != "")
                                {
                                    ms = new MemoryStream(Convert.FromBase64String(dataStr));
                                    break;
                                }
                            }
                        }
                    }
                    if (ms == null) return null;
                    else
                    {
                        return Image.FromStream(ms, true, true);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public Image Thumbnail
        {
            get
            {
                MemoryStream ms = null;
                string xpath = "/drg:RadanCompoundDocument/drg:Thumbnail[@image-type='bmp']";
                XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (bmpNode != null)
                    {
                        for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                        {
                            XmlNode childNode = bmpNode.ChildNodes[i];
                            if (childNode.NodeType == XmlNodeType.CDATA)
                            {
                                string dataStr = childNode.Value;
                                if (dataStr != "")
                                {
                                    ms = new MemoryStream(Convert.FromBase64String(dataStr));
                                    break;
                                }
                            }
                        }
                    }
                    if (ms == null) return null;
                    else
                    {
                        return Image.FromStream(ms, true, true);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public string LYL
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='138']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        /// <summary>
        /// 带孔面积计算的利用率
        /// </summary>
        public string LYLH
        {
            get
            {
                double allareah = 0;
                string xpath = "/drg:RadanCompoundDocument/drg:SetupSheet/drg:Parts";
                XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (partsNode != null)
                {
                    for (int i = 0; i < partsNode.ChildNodes.Count; i++)
                    {
                        string cpathheader = string.Format("/drg:RadanCompoundDocument/drg:SetupSheet/drg:Parts/drg:Part[@num='{0:D}']", i + 1);
                        string cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='163']";
                        XmlNode areahNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                        string areah = ((XmlElement)areahNode).GetAttribute("value");
                        double dareah = 0;
                        if (!string.IsNullOrEmpty(areah))
                            dareah = Convert.ToDouble(areah);
                        allareah += dareah;
                    }
                }
                double length = Convert.ToDouble(Length);
                double width = Convert.ToDouble(Width);
                double lyl = Math.Round((allareah / (length * width)) * 100, 2);
                return Convert.ToString(lyl);
            }
        }

        public string Material
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='119']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
        }

        public string Mode
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='120']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        public string Length
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='124']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        public string Width
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='125']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        public string remarea
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='126']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
        }

        public string Size
        {
            get
            {
                return (Length + "x" + Width);
            }
        }

        public string Weight
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='162']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        public string Area
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='164']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return Round(((XmlElement)mateNode).GetAttribute("value"));
                }
                else
                    return "0";
            }
        }

        public int PartAllCount
        {
            get
            {
                int allcount = 0;
                foreach (DrgPartInfo part in PartsList)
                {
                    allcount += Convert.ToInt16(part.num);
                }
                return allcount;
            }
        }

        public string Round(string sor, int ct = 2)
        {
            double dData = 0;
            try
            {
                dData = Convert.ToDouble(sor);
                dData = Math.Round(dData, ct);
                return dData.ToString();
            }
            catch (SystemException ex)
            {
                return sor;
            }
        }
        public List<DrgPartInfo> PartsList
        {
            get
            {

                try
                {
                    double lyl = Convert.ToDouble(this.LYL) / 100;
                    double xishu = (1 - lyl) / lyl + 1;
                    List<DrgPartInfo> PartsListTemp = new List<DrgPartInfo>();
                    string xpath = "/drg:RadanCompoundDocument/drg:SetupSheet/drg:Parts";
                    XmlNode partsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                    if (partsNode != null)
                    {
                        for (int i = 0; i < partsNode.ChildNodes.Count; i++)
                        {
                            string cpathheader = string.Format("/drg:RadanCompoundDocument/drg:SetupSheet/drg:Parts/drg:Part[@num='{0:D}']", i + 1);
                            string cpath = cpathheader + "/drg:FilePath";
                            XmlNode nameNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string partname = ((XmlElement)nameNode).InnerText;

                            cpath = cpathheader + "/drg:NumOff";
                            XmlNode numNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string num = ((XmlElement)numNode).InnerText;

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='119']";
                            XmlNode mateNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string mate = ((XmlElement)mateNode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='120']";
                            XmlNode modeNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string mode = ((XmlElement)modeNode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='165']";
                            XmlNode lenNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string length = ((XmlElement)lenNode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='166']";
                            XmlNode widNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string width = ((XmlElement)widNode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='167']";
                            XmlNode Operimeternode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string OuterPerim = ((XmlElement)Operimeternode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='168']";
                            XmlNode perimeternode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string Perim = ((XmlElement)perimeternode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='164']";
                            XmlNode weiNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string weight = ((XmlElement)weiNode).GetAttribute("value");

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='162']";
                            XmlNode areaNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string area = ((XmlElement)areaNode).GetAttribute("value");//净面积

                            if (weight == "" && area != "")
                                weight = Convert.ToString(Convert.ToDouble(area) * Convert.ToDouble(mode) * 7.85);

                            cpath = cpathheader + "/drg:RadanAttributes/drg:Group/drg:Attr[@num='163']";
                            XmlNode areahNode = this.DocumentElement.SelectSingleNode(cpath, xnml);
                            string areah = ((XmlElement)areahNode).GetAttribute("value");

                            DrgPartInfo partinfo = new DrgPartInfo();
                            partinfo.filepath = partname;
                            partinfo.name = System.IO.Path.GetFileName(partname).Replace(".sym", "");
                            partinfo.num = num;
                            if (mate == "")
                                mate = Material;
                            partinfo.mate = mate;
                            if (mode == "")
                                mode = Mode;
                            partinfo.mode = mode;
                            partinfo.length = Round(length);
                            partinfo.width = Round(width);
                            partinfo.weight = Round(string.IsNullOrEmpty(weight) ? "0" : weight);
                            partinfo.area = Round(area);
                            partinfo.areah = Round(areah);
                            partinfo.perim = Round(string.IsNullOrEmpty(Perim) ? "0" : Perim);
                            partinfo.outerperim = Round(string.IsNullOrEmpty(OuterPerim) ? "0" : OuterPerim);
                            partinfo.Pierce = GetBlkNCCodeCountS(Path.GetFileName(partname).Replace(".sym", ""), "Pierce");
                            if (weight != "")
                                partinfo.maoweight = Round(Convert.ToString(xishu * Convert.ToDouble(weight)));
                            else
                                partinfo.maoweight = "0";
                            partinfo.nestfilepath = Path.GetFileName(FileName);
                            partinfo.cost = Round(Perim);
                            if (partinfo.cost == "")
                                partinfo.cost = "0";
                            PartsListTemp.Add(partinfo);
                        }
                        return PartsListTemp;
                    }
                    else
                        return PartsListTemp;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
        }

        public string[] GetBlkLines()
        {
            MemoryStream ms = null;
            XmlNode childNode = null;
            string xpath = "/drg:RadanCompoundDocument/drg:RadanFile[@extension='blk']";
            XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            try
            {
                if (bmpNode != null)
                {
                    for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                    {
                        childNode = bmpNode.ChildNodes[i];
                        if (childNode.NodeType == XmlNodeType.CDATA)
                        {
                            string dataStr = childNode.Value;
                            string[] lines = dataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            return lines;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string strErrror = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 获取代码中指定NC指令的个数
        /// </summary>
        /// <param name="ncCode"></param>
        /// <returns></returns>
        public int GetBlkNCCodeCount(string ncCode)
        {
            int ncCount = 0;
            string[] blkLines = GetBlkLines();
            if (blkLines != null)
            {
                foreach (string line in blkLines)
                    if (line.IndexOf(ncCode) >= 0)
                        ncCount++;
            }
            return ncCount;
        }
        /// <summary>
        /// 获取空行程idle stroke 
        /// </summary>
        /// <param name="ncCode"></param>
        /// <returns></returns>
        public double GetIdleStroke(string ncCode)
        {
            double IdleStroke = 0.0;
            string[] blkLines = GetBlkLines();
            if (blkLines != null)
            {
                foreach (string line in blkLines)
                {
                    if (line.IndexOf(ncCode) >= 0)
                    {
                        IdleStroke += GetSingleIdleStrokeByStr(ncCode, line);
                    }
                }
            }
            return IdleStroke;
        }
        public double IdleStroke_X { get; set; }
        public double IdleStroke_Y { get; set; }

        public double GetSingleIdleStrokeByStr(string ncCode, string line)
        {
            string str = line.Substring(line.IndexOf(ncCode));
            int xpos = str.IndexOf('X');
            int ypos = str.IndexOf('Y');
            if (xpos > 0 && ypos > 0)
            {
                double x = Convert.ToDouble(str.Substring(xpos + 1, ypos - xpos - 1));
                double y = Convert.ToDouble(str.Substring(ypos + 1));
                double tempx = x;
                double tempy = y;
                x = x - IdleStroke_X;
                y = y - IdleStroke_Y;
                IdleStroke_X = tempx;
                IdleStroke_Y = tempy;
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) / 1000;
            }
            else
                return 0;
        }
        /// <summary>
        /// 排样零件清单内信息分组预处理
        /// </summary>
        /// <param name="strsource"></param>
        /// <returns></returns>
        public StringCollection GetArryFromStr(string strsource)
        {
            strsource = strsource.TrimStart();
            int index = 0;
            int curindex = 0;
            bool bfinone = false;
            StringCollection outstrarry = new StringCollection();
            foreach (char ch in strsource)
            {
                if (ch == 32)
                {
                    if (bfinone)
                    {
                        index++;
                        continue;
                    }
                    string ss = strsource.Substring(curindex, index - curindex);
                    outstrarry.Add(ss);
                    bfinone = true;
                }
                else if (bfinone)
                {
                    curindex = index;
                    bfinone = false;
                }
                index++;
            }
            return outstrarry;
        }

        public string GetDrgInfo(string findstr)
        {
            StreamReader sr = new StreamReader(FileName);
            string str = "";
            while (sr.Peek() > 0)
            {
                str = sr.ReadLine();
                if (str.Contains(findstr))
                {
                    string[] splitstr = str.Split('$');
                    if (splitstr.Length == 2)
                    {
                        sr.Close();
                        string sss = splitstr[1].Substring(findstr.Length).TrimStart();
                        return sss.Substring(1).TrimStart();
                    }
                }
            }
            sr.Close();
            return "0";
        }

        public string GetBlkNCCodeCountS(string partname, string findstr)
        {
            if (string.IsNullOrEmpty(partname))
                return "";
            StreamReader sr = new StreamReader(FileName);
            string str = "";
            bool bpre = false;
            int pos = -1;
            while (sr.Peek() > 0)
            {
                str = sr.ReadLine();
                if (str.Contains(findstr))
                {
                    string[] splitstr = str.Split('$');
                    if (splitstr.Length == 2)
                    {
                        str = splitstr[1];
                        StringCollection strset = GetArryFromStr(str);
                        int position = 0;
                        if (findstr == "Part Name")
                        {
                            if ((strset[1] + " " + strset[2]) == findstr)
                            {
                                pos = 1;
                            }
                        }
                        else
                        {
                            foreach (string ss in strset)
                            {
                                if (ss.IndexOf(findstr) >= 0)
                                {
                                    pos = position;
                                    break;
                                }
                                position++;
                            }
                        }
                    }
                    bpre = true;
                }
                else if (bpre && pos != -1)
                {
                    string[] splitstr = str.Split('$');
                    if (splitstr.Length == 2)
                    {
                        str = splitstr[1];
                        StringCollection strset = GetArryFromStr(str);
                        if (strset[1].IndexOf(partname) >= 0)
                        {
                            if (findstr == "Part Name")
                                return strset[pos - 1];
                            else
                                return strset[pos];
                        }
                    }
                }
            }
            return "0";
        }

    }
    /// <summary>
    /// Symbol文件类
    /// </summary>
    public class RadSymFile : RadRcdFile
    {

        public string Material
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='119']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='119']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    ((XmlElement)mateNode).SetAttribute("value", value);
                }
            }
        }
        public string Thickness
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='120']";
                XmlNode xn = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (xn != null)
                    {
                        return ((XmlElement)xn).GetAttribute("value");
                    }
                    else
                        return "";
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='120']";
                XmlNode xn = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (xn != null)
                {
                    ((XmlElement)xn).SetAttribute("value", value);
                }
            }
        }
        /// <summary>
        /// 面积
        /// </summary>
        public string Area
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='162']";
                XmlNode xn = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (xn != null)
                    {
                        return ((XmlElement)xn).GetAttribute("value");
                    }
                    else
                        return "";
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 周长
        /// </summary>
        public string Perimeter
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='168']";
                XmlNode xn = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (xn != null)
                    {
                        return ((XmlElement)xn).GetAttribute("value");
                    }
                    else
                        return "";
                }
                catch
                {
                    return null;
                }
            }
        }

        public Image Thumbnail
        {
            get
            {
                MemoryStream ms = null;
                string xpath = "/drg:RadanCompoundDocument/drg:Thumbnail[@image-type='bmp']";
                XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (bmpNode != null)
                    {
                        for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                        {
                            XmlNode childNode = bmpNode.ChildNodes[i];
                            if (childNode.NodeType == XmlNodeType.CDATA)
                            {
                                string dataStr = childNode.Value;
                                if (dataStr != "")
                                {
                                    ms = new MemoryStream(Convert.FromBase64String(dataStr));
                                    break;
                                }
                            }
                        }
                    }
                    if (ms == null) return null;
                    else
                    {
                        return Image.FromStream(ms, true, true);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public string ThumbnailString
        {
            get
            {
                string outstr = "";
                string xpath = "/drg:RadanCompoundDocument/drg:Thumbnail[@image-type='bmp']";
                XmlNode bmpNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                try
                {
                    if (bmpNode != null)
                    {
                        for (int i = 0; i < bmpNode.ChildNodes.Count; i++)
                        {
                            XmlNode childNode = bmpNode.ChildNodes[i];
                            if (childNode.NodeType == XmlNodeType.CDATA)
                            {
                                string dataStr = childNode.Value;
                                if (!string.IsNullOrEmpty(dataStr))
                                {
                                    outstr = dataStr;
                                    break;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(outstr)) return "";
                    else
                        return outstr;
                }
                catch
                {
                    return "";
                }
            }
        }

        public byte[] ImageToBuffer(string dataStr, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = null;
            if (string.IsNullOrEmpty(dataStr))
                dataStr = "";
            ms = new MemoryStream(Convert.FromBase64String(dataStr));
            if (ms == null)
                return null;
            Image image = System.Drawing.Image.FromStream(ms, true, true);

            if (image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本  
                using (Bitmap oBitmap = new Bitmap(image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式  
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置  
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度  
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer  
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流  
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
        public string Length
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='165']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
        }

        public string Width
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='166']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
        }

        public string Weight
        {
            get
            {
                string xpath = "/drg:RadanCompoundDocument/drg:RadanAttributes/drg:Group/drg:Attr[@num='164']";
                XmlNode mateNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (mateNode != null)
                {
                    return ((XmlElement)mateNode).GetAttribute("value");
                }
                else
                    return "";
            }
        }
    }
    /// <summary>
    /// NestResult节点类
    /// </summary>
    public class RadNestResultXMLNode : RadRcdNode
    {
        public RadNestResultXMLNode(XmlElement xe)
            : base(xe)
        {
        }
        public override XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(Element.OwnerDocument.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/nestresults");
                return xnml;
            }
        }
    }
    /// <summary>
    /// NestResult文件程序节点
    /// </summary>
    public class RadNestResult_Program : RadNestResultXMLNode
    {
        public RadNestResult_Program(XmlElement xe)
            : base(xe)
        {
        }
        public string Drawing
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "Drawing");
                return attrValue;
            }
        }
        public string DrawingFile
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "DrawingFile");
                return attrValue;
            }
        }
        public string Material
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "Material");
                return attrValue;
            }
        }
        public string Thickness
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "Thickness");
                return attrValue;
            }
        }
        public string ThickUnits
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "ThickUnits");
                return attrValue;
            }
        }
        public string SheetX
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "SheetX");
                return attrValue;
            }
        }
        public string SheetY
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "SheetY");
                return attrValue;
            }
        }
        public string StockID
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "StockID");
                return attrValue;
            }
        }
        public string NumSheets
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgResults", "NumSheets");
                return attrValue;
            }
        }
        /// <summary>
        /// 利用率 71.3
        /// </summary>
        public string Utilisation
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgResults", "Utilisation");
                return attrValue;
            }
        }
        public string StdToolId
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgResults", "StdToolId");
                return attrValue;
            }
        }

        public string SheetUnits
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "SheetUnits");
                return attrValue;
            }
        }

        public string Machine
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "Machine");
                return attrValue;
            }
        }
        public string NumNozzles
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "NumNozzles");
                return attrValue;
            }
        }
        public string NumNozzlesPerSheet
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("ProgDetails", "NumNozzlesPerSheet");
                return attrValue;
            }
        }
        public List<RadNestResult_UsedPart> UseParts
        {
            get
            {
                List<RadNestResult_UsedPart> result = new List<RadNestResult_UsedPart>();
                XmlNodeList xnlist = SelectNodes("UsedParts", "UsedPart");
                foreach (XmlNode xn in xnlist)
                {
                    if (xn is XmlElement)
                    {
                        RadNestResult_UsedPart usedPart = new RadNestResult_UsedPart((XmlElement)xn);
                        result.Add(usedPart);
                    }
                }
                return result;

            }
        }
        public List<RadNestResult_RemnantMade> RemnantsMades
        {
            get
            {
                List<RadNestResult_RemnantMade> result = new List<RadNestResult_RemnantMade>();
                XmlNodeList xnlist = SelectNodes("RemnantsMade", "RemnantMade");
                foreach (XmlNode xn in xnlist)
                {
                    if (xn is XmlElement)
                    {
                        RadNestResult_RemnantMade remant = new RadNestResult_RemnantMade((XmlElement)xn);
                        result.Add(remant);
                    }
                }
                return result;

            }
        }
    }
    /// <summary>
    /// NestResult文件使用的零件节点
    /// </summary>
    public class RadNestResult_UsedPart : RadNestResultXMLNode
    {
        public RadNestResult_UsedPart(XmlElement xe)
            : base(xe)
        {
        }
        public string AnnotationItem
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("AnnotationItem");
                return attrValue;
            }
        }
        public string Symbol
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("Symbol");
                return attrValue;
            }
        }
        public string PerSheet
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("PerSheet");
                return attrValue;
            }
        }
        public string Total
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("Total");
                return attrValue;
            }
        }
        public string Bin
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("Bin");
                return attrValue;
            }
        }
    }
    /// <summary>
    /// NestResult文件余料节点
    /// </summary>
    public class RadNestResult_RemnantMade : RadNestResultXMLNode
    {
        public RadNestResult_RemnantMade(XmlElement xe)
            : base(xe)
        {
        }
        public string RemnantFile
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("RemnantFile");
                return attrValue;
            }
        }
        public string Rectangular
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("Rectangular");
                return attrValue;
            }
        }
        public string SheetX
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("SheetX");
                return attrValue;
            }
        }
        public string SheetY
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("SheetY");
                return attrValue;
            }
        }
        public string Area
        {
            get
            {
                string attrValue = GetXMLNodeInnerText("Area");
                return attrValue;
            }
        }
    }
    /// <summary>
    /// NestResult文件类
    /// </summary>
    public class RadNestResultFile : RadRcdFile
    {
        public override XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("drg", "http://www.radan.com/ns/nestresults");
                return xnml;
            }
        }
        public string OverallUtil
        {
            get
            {
                return GetXMLNodeInnerText("RadanNestResults", "NestRun", "NestRunTotals", "OverallUtil");
            }
        }
        public string UtilLastProg
        {
            get
            {
                return GetXMLNodeInnerText("RadanNestResults", "NestRun", "NestRunTotals", "UtilLastProg");
            }
        }
        public string NumPrograms
        {
            get
            {
                return GetXMLNodeInnerText("RadanNestResults", "NestRun", "NestRunTotals", "NumPrograms");
            }
        }
        public string NumSheets
        {
            get
            {
                return GetXMLNodeInnerText("RadanNestResults", "NestRun", "NestRunTotals", "NumSheets");
            }
        }
        public string NumRemnantsMade
        {
            get
            {
                return GetXMLNodeInnerText("RadanNestResults", "NestRun", "NestRunTotals", "NumRemnantsMade");
            }
        }
        public List<RadNestResult_Program> programs
        {
            get
            {
                List<RadNestResult_Program> result = new List<RadNestResult_Program>();
                XmlNodeList xnlist = SelectNodes("RadanNestResults", "NestRun", "Program");
                foreach (XmlNode xn in xnlist)
                {
                    if (xn is XmlElement)
                    {
                        RadNestResult_Program program = new RadNestResult_Program((XmlElement)xn);
                        result.Add(program);
                    }
                }
                return result;
            }
        }
    }
    #endregion

    public class FabNRPart
    {
        public string ID { get; set; }
        public int Anno { get; set; }
        public int UseCount { get; set; }
        public int UnUsecount { get; set; }
    }

    public class FabNRRem
    {
        public string ID { get; set; }
        public int Count { get; set; }
        public double Area { get; set; }
        public double Weight { get; set; }
        public double SheetX { get; set; }
        public double SheetY { get; set; }
    }

    public class FabNRSheet
    {
        public string ID { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
        public double Area { get; set; }
        public double Weight { get; set; }
        public double SheetX { get; set; }
        public double SheetY { get; set; }
        public string Mode { get; set; }
        public string Material { get; set; }
        public double RadanLYL { get; set; }
        public string Image { get; set; }

        public List<FabNRPart> partslist = new List<FabNRPart>();
        public List<FabNRRem> remslist = new List<FabNRRem>();




        public byte[] ImageToBuffer(string dataStr, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = null;
            if (string.IsNullOrEmpty(dataStr))
                dataStr = "";
            ms = new MemoryStream(Convert.FromBase64String(dataStr));
            if (ms == null)
                return null;
            Image image = System.Drawing.Image.FromStream(ms, true, true);

            if (image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本  
                using (Bitmap oBitmap = new Bitmap(image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式  
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置  
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度  
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer  
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流  
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
    }
    /// <summary>
    /// fabcost排样结果XML类
    /// </summary>
    public class FabNestResult : XmlDocument
    {
        #region 基本成员
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("nr", "Fabcostnms");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            try
            {
                this.FileName = fileName;
                base.Load(this.FileName);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }
        #endregion

        #region 自定义成员

        List<FabNRSheet> _sheetlist = new List<FabNRSheet>();
        /// <summary>
        /// 方案名称
        /// </summary>
        public string NestName
        {
            get
            {
                string xpath = "/nr:Project/nr:Name";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/nr:Project/nr:Name";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        /// <summary>
        /// 方案类型
        /// </summary>
        public int NestType
        {
            get
            {
                string xpath = "/nr:Project/nr:Type";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText.ToInt();
                }
                else
                    return -1;
            }
            set
            {
                string xpath = "/nr:Project/nr:Type";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value.ToString();
                }
            }
        }

        public string UserId
        {
            get
            {
                string xpath = "/nr:Project/nr:UserId";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/nr:Project/nr:UserId";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        public string UserName
        {
            get
            {
                string xpath = "/nr:Project/nr:UserName";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    return ((XmlElement)nest).InnerText;
                }
                else
                    return "";
            }
            set
            {
                string xpath = "/nr:Project/nr:UserName";
                XmlNode nest = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (nest != null)
                {
                    ((XmlElement)nest).InnerText = value;
                }
            }
        }

        public List<FabNRPart> GetSheetPartsInfo(XmlNode node)
        {
            List<FabNRPart> partslist = new List<FabNRPart>();
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode partnode = node.ChildNodes[i];
                FabNRPart part = new FabNRPart();
                part.ID = partnode.Attributes["id"].Value;
                part.UseCount = partnode.Attributes["UseCount"].Value.ToInt();
                part.Anno = partnode.Attributes["Anno"].Value.ToInt();
                part.UnUsecount = partnode.Attributes["UnUseCount"].Value.ToInt();

                partslist.Add(part);
            }
            return partslist;
        }

        public List<FabNRSheet> sheetsinfo
        {
            get
            {
                GetSheetsInfo(ref _sheetlist);
                return _sheetlist;
            }
        }
        /// <summary>
        /// 板材信息
        /// </summary>
        public void GetSheetsInfo(ref List<FabNRSheet> sheets)
        {
            try
            {
                sheets.Clear();
                string xpath = "/nr:Project/nr:SheetsInfo/Sheets";
                XmlNode sheetsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
                if (sheetsNode != null)
                {
                    for (int i = 0; i < sheetsNode.ChildNodes.Count; i++)
                    {
                        XmlNode sheetnode = sheetsNode.ChildNodes[i];
                        FabNRSheet sheet = new FabNRSheet();
                        sheet.ID = sheetnode.Attributes[0].Value;
                        sheet.Type = sheetnode.Attributes["type"].Value.ToInt();
                        sheet.Count = sheetnode.Attributes["count"].Value.ToInt();
                        sheet.Area = sheetnode.ChildNodes[0].InnerText.ToInt();
                        sheet.Weight = sheetnode.ChildNodes[1].InnerText.ToInt();
                        sheet.SheetY = sheetnode.ChildNodes[2].InnerText.ToInt();
                        sheet.SheetX = sheetnode.ChildNodes[3].InnerText.ToInt();
                        sheet.Material = sheetnode.ChildNodes[4].InnerText;
                        sheet.Mode = sheetnode.ChildNodes[5].InnerText;
                        sheet.RadanLYL = sheetnode.ChildNodes[6].InnerText.ToInt();
                        sheet.Image = sheetnode.ChildNodes[7].InnerText;

                        sheet.partslist = GetSheetPartsInfo(sheetnode.ChildNodes[8]);
                        sheet.remslist = GetSheetRemsInfo(sheetnode.ChildNodes[9]);

                        sheets.Add(sheet);
                    }
                }
            }
            catch
            {
            }
        }

        public void SetSheetsInfo(List<FabNRSheet> sheets)
        {
            string xpath = "/nr:Project/nr:SheetsInfo";
            XmlNode sheetsNode = this.DocumentElement.SelectSingleNode(xpath, xnml);
            if (sheetsNode != null)
            {
                XmlElement xele = this.CreateElement("Sheets");
                xele.SetAttribute("count", Convert.ToString(sheets.Count));
                foreach (FabNRSheet sh in sheets)
                {
                    XmlElement xel = this.CreateElement("sheet");
                    xel.SetAttribute("id", Convert.ToString(sh.ID));
                    xel.SetAttribute("type", Convert.ToString(sh.Type));
                    xel.SetAttribute("count", Convert.ToString(sh.Count));
                    XmlElement Area = this.CreateElement("Area");
                    Area.InnerText = Convert.ToString(sh.Area);
                    XmlElement Weight = this.CreateElement("Weight");
                    Weight.InnerText = Convert.ToString(sh.Weight);
                    XmlElement SheetX = this.CreateElement("SheetX");
                    SheetX.InnerText = Convert.ToString(sh.SheetX);
                    XmlElement SheetY = this.CreateElement("SheetY");
                    SheetY.InnerText = Convert.ToString(sh.SheetY);
                    XmlElement Material = this.CreateElement("Material");
                    SheetX.InnerText = Convert.ToString(sh.Material);
                    XmlElement Mode = this.CreateElement("Mode");
                    SheetY.InnerText = Convert.ToString(sh.Mode);
                    XmlElement LYL = this.CreateElement("LYL");
                    SheetY.InnerText = Convert.ToString(sh.RadanLYL);
                    xel.AppendChild(Area);
                    xel.AppendChild(Weight);
                    xel.AppendChild(SheetX);
                    xel.AppendChild(SheetY);
                    xel.AppendChild(Material);
                    xel.AppendChild(Mode);
                    xel.AppendChild(LYL);
                    xel = SetSheetPartsInfo(sh.partslist, xel, this);  //增加子结点parts
                    xel = SetSheetRemsInfo(sh.remslist, xel, this);    //增加子结点remnants
                    xele.AppendChild(xel);

                    ((XmlElement)sheetsNode).AppendChild(xele);
                }
            }
        }

        public XmlElement SetSheetPartsInfo(List<FabNRPart> parts, XmlElement xe, XmlDocument xdoc)
        {
            XmlElement xele = xdoc.CreateElement("Parts");
            xele.SetAttribute("count", Convert.ToString(parts.Count));
            foreach (FabNRPart pa in parts)
            {
                XmlElement xel = xdoc.CreateElement("part");
                xel.SetAttribute("id", Convert.ToString(pa.ID));
                xel.SetAttribute("UseCount", Convert.ToString(pa.UseCount));
                xel.SetAttribute("UnUseCount", Convert.ToString(pa.UnUsecount));
                xel.SetAttribute("Anno", Convert.ToString(pa.Anno));
                xele.AppendChild(xel);
            }
            xe.AppendChild(xele);
            return xe;
        }
        public XmlElement SetSheetRemsInfo(List<FabNRRem> rems, XmlElement xe, XmlDocument xdoc)
        {
            XmlElement xele = xdoc.CreateElement("Remnants");
            xele.SetAttribute("count", Convert.ToString(rems.Count));
            foreach (FabNRRem rem in rems)
            {
                XmlElement xel = xdoc.CreateElement("part");
                xel.SetAttribute("id", Convert.ToString(rem.ID));
                xel.SetAttribute("usecount", Convert.ToString(rem.Count));
                XmlElement Area = xdoc.CreateElement("Area");
                Area.InnerText = Convert.ToString(rem.Area);
                XmlElement Weight = xdoc.CreateElement("Weight");
                Weight.InnerText = Convert.ToString(rem.Weight);
                XmlElement SheetX = xdoc.CreateElement("SheetX");
                SheetX.InnerText = Convert.ToString(rem.SheetX);
                XmlElement SheetY = xdoc.CreateElement("SheetY");
                SheetY.InnerText = Convert.ToString(rem.SheetY);
                xel.AppendChild(Area);
                xel.AppendChild(Weight);
                xel.AppendChild(SheetX);
                xel.AppendChild(SheetY);
                xele.AppendChild(xel);
            }
            xe.AppendChild(xele);
            return xe;
        }
        private List<FabNRRem> GetSheetRemsInfo(XmlNode xmlNode)
        {
            List<FabNRRem> remslist = new List<FabNRRem>();
            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                XmlNode remnode = xmlNode.ChildNodes[i];
                FabNRRem rem = new FabNRRem();
                rem.ID = remnode.Attributes["id"].Value;
                rem.Count = remnode.Attributes["usecount"].Value.ToInt();

                rem.Area = remnode.ChildNodes[0].Value.ToDouble();
                rem.Weight = remnode.ChildNodes[1].Value.ToDouble();
                rem.SheetY = remnode.ChildNodes[2].Value.ToDouble();
                rem.SheetX = remnode.ChildNodes[3].Value.ToDouble();
                remslist.Add(rem);
            }
            return remslist;
        }
        #endregion
    }

    /// <summary>
    /// fabcost材料类型导入数据类
    /// </summary>
    public class FabMateTypeFile : XmlDocument
    {
        #region 基本成员
        public string FileName { get; set; }
        public virtual XmlNamespaceManager xnml
        {
            get
            {
                XmlNamespaceManager xnml = new XmlNamespaceManager(this.NameTable);
                xnml.AddNamespace("nr", "Fabcostnms");
                return xnml;
            }
        }
        public new void Load(string fileName)
        {
            try
            {
                this.FileName = fileName;
                base.Load(this.FileName);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public XmlNode SelectSingleNode(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.SelectSingleNode(xpath, xnml);
        }
        public XmlNodeList SelectNodes(params string[] paths)
        {
            string xpath = RadHelper.CreateAbsoluteXPath(paths);
            return this.DocumentElement.SelectNodes(xpath, xnml);
        }
        public string GetXMLNodeInnerText(params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return xn.InnerText;
        }
        public string GetXMLNodeAttriute(string attrName, params string[] paths)
        {
            XmlNode xn = SelectSingleNode(paths);
            if (xn == null) return null;
            else
                return ((XmlElement)xn).GetAttribute(attrName);
        }
        #endregion
    }

    public class RadanObj
    {
    }
}
