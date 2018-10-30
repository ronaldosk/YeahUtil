using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace SwPLDM
{
    /// <summary>
    /// 函数操作基础类
    /// </summary>
    public class TdaBase
    {
        #region 文件操作
        static public bool IsChildPath(string strParentPath, string strChildPath, bool bSamePath)
        {
            strChildPath = strChildPath.Trim().ToLower();
            strParentPath = strParentPath.Trim().ToLower();
            strChildPath = GetFullPath(strChildPath);
            strParentPath = GetFullPath(strParentPath);
            int iChildLen = strChildPath.Length;
            int iParentLen = strParentPath.Length;
            if (iChildLen == 0 || iParentLen == 0) return false;
            if (bSamePath && strChildPath == strParentPath) return true;
            if (iChildLen < iParentLen) return false;
            if (strChildPath.Substring(0, iParentLen) != strParentPath) return false;
            return true;
        }
        static public string GetRelaPath(string strParentPath, string strChildPath)
        {
            if (IsChildPath(strParentPath, strChildPath, true))
            {
                strChildPath = GetFullPath(strChildPath);
                strParentPath = GetFullPath(strParentPath);
                return strChildPath.Substring(strParentPath.Length);
            }
            else
                return strChildPath;
        }

        static public bool ListSaveToFile(StringCollection strList, string strFilePathName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFilePathName, false, Encoding.Unicode);
                foreach (string line in strList)
                    sw.WriteLine(line);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 取得系统目录
        /// </summary>
        /// <returns></returns>
        static public string GetSystemDirectory()
        {
            return Environment.SystemDirectory;
        }
        static public string GetFullPath(string FilePath)
        {
            FilePath = FilePath.Trim();
            if (!FilePath.EndsWith("\\"))
                FilePath = FilePath + "\\";
            return FilePath;
        }
        static public string ExtractFilePath(string fileName)
        {
            int pos = fileName.LastIndexOf(@"\");
            if (pos >= 0) return fileName.Substring(0, pos + 1);
            else
                return "";
        }
        static public long GetFileSize(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileInfo fInfo = new FileInfo(fileName);
                return fInfo.Length;
            }
            else
                return -1;
        }
        static public string ExtractFileExt(string FileName)
        {
            if (FileName.Trim() != "")
            {
                FileInfo fInfo = new FileInfo(FileName);
                return fInfo.Extension;
            }
            else
                return "";
        }
        /// <summary>
        /// 遍历文件路径，直到遇到文件夹titleName为止；
        /// </summary>
        /// <param name="fileName">系统文件路径</param>
        /// <param name="titleName">要查找的截至的字符串</param>
        /// <returns></returns>
        static public string GetSubFilePathName(string fileName, string titleFirstName, string titleLastName)
        {
            int i = fileName.IndexOf(titleFirstName);
            if (i > 0)
            {
                fileName = fileName.Substring(0, i + titleFirstName.Length).ToString() + @"\" + titleLastName.ToString();
                if (fileName.Substring(fileName.Length - 1, 1) != "\\")
                    fileName += "\\";
            }
            return fileName;
        }

        /// <summary>
        /// 获得上级文件目录路径
        /// </summary>
        /// <param name="fileName">当前文件目录</param>
        /// <returns></returns>
        static public string GetParentFilePathName(string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(fileName);
            DirectoryInfo dirInfo2 = dirInfo.Parent;
            return dirInfo2.FullName + "\\";
        }
        /// <summary>
        /// 转换文件类型
        /// </summary>
        /// <param name="fileName">文件路径名</param>
        /// <param name="fileExt">文件类型</param>
        static public string ChangeFileExt(string fileName, string fileExt)
        {
            string path = ExtractFilePath(fileName);
            string fName = ExtractOnlyFName(fileName);
            return path + fName + fileExt;
        }
        static public string ExtractFileName(string FileName)
        {
            if (FileName.Trim() != "")
            {
                FileInfo fInfo = new FileInfo(FileName);
                return fInfo.Name;
            }
            else
                return "";
        }
        static public string ExtractOnlyFName(string fileName)
        {
            fileName = ExtractFileName(fileName);
            int pos = fileName.LastIndexOf('.');
            if (pos >= 0)
                return fileName.Substring(0, pos);
            return fileName;
        }
        static public string GetSubStringSec(string str, int secNo, string strSp)
        {
            str = str.ToLower();
            strSp = strSp.ToLower();
            int p = str.IndexOf(strSp, 0);
            if (secNo == 1)
            {
                if (p == -1) return str;
                else
                    return str.Substring(0, p);
            }
            else
                if (secNo > 1)
                {
                    if (p == -1) return "";
                    else
                    {
                        int len = strSp.Length;
                        str = str.Substring(p + len, str.Length - p - len);
                        return GetSubStringSec(str, secNo - 1, strSp);
                    }

                }
            return "";
        }
        static public StringCollection ListLoadFromFile(string strFilePathName)
        {
            try
            {
                StringCollection strList = new StringCollection();
                Stream stream = File.OpenRead(strFilePathName);
                StreamReader streamReader = new StreamReader(stream);
                string strLine;
                while ((strLine = streamReader.ReadLine()) != null)
                {
                    strList.Add(strLine);
                }
                streamReader.Close();
                stream.Close();
                return strList;
            }
            catch
            { return null; }

        }
        static public bool SearchFile(string path, ArrayList fileList, string fileFilters)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fis = di.GetFiles(fileFilters);
                foreach (FileInfo fi in fis)
                {
                    fileList.Add(fi.FullName);
                }
                DirectoryInfo[] dis = di.GetDirectories();
                foreach (DirectoryInfo dirInfo in dis)
                {
                    SearchFile(dirInfo.FullName, fileList, fileFilters);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        static public int GetFileNameLength(string strFileName)
        {
            return strFileName.Length;
        }
        //add at 2010-8-25
        /// <summary>
        /// 採用遞迴的方式遍歷，資料夾和子檔中的所有檔。
        /// </summary>
        static private List<string> FindFile(string dirPath, string fileExt, List<string> templistFiles) //參數dirPath為指定的目錄
        {
            //在指定目錄及子目錄下查找檔
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            try
            {
                foreach (DirectoryInfo d in dirInfo.GetDirectories())//查找子目錄
                {
                    FindFile(dirInfo + d.ToString() + @"\", fileExt, templistFiles);
                }
                foreach (FileInfo f in dirInfo.GetFiles(fileExt)) //查找文件
                    templistFiles.Add(f.FullName);
                return templistFiles;
            }
            catch (Exception e)
            {
                return templistFiles;
            }
        }
        /// <summary>
        /// 採用遞迴的方式遍歷，資料夾和子檔中的所有檔。
        /// </summary>
        /// <param name="dirPath">要遍历的目录</param>
        /// <param name="fileExt">文件类型</param>
        /// <returns></returns>
        static public List<string> FindFloder(string dirPath, string fileExt)
        {
            List<string> tmpFiles = new List<string>();
            return FindFile(dirPath, fileExt, tmpFiles);
        }
        /// <summary>
        /// 刪除指定資料夾下的所有檔;
        /// </summary>
        /// <param name="dir">文件目录</param>
        /// <param name="fileExt">要删除的文件类型</param>
        static public void DeleteFloder(string targetDir, string fileExt)
        {
            try
            {
                if (Directory.Exists(targetDir))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(targetDir);
                    DirectoryInfo[] dirEntries = dirInfo.GetDirectories();
                    foreach (DirectoryInfo subDir in dirEntries)
                        subDir.Delete(true);
                    foreach (FileInfo f in dirInfo.GetFiles(fileExt))
                        f.Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 複製指定目錄的所有檔
        /// </summary>
        /// <param name="sourceDir">原始目錄</param>
        /// <param name="targetDir">目標目錄</param>
        /// <param name="overWrite">如果為true,覆蓋同名檔,否則不覆蓋</param>
        /// <param name="copySubDir">如果為true,包含目錄,否則不包含</param>
        static public void CopyFloder(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //複製目前的目錄檔
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf("\\") + 1));
                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }
            //複製子目錄
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf("\\") + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    CopyFloder(sourceSubDir, targetSubDir, overWrite, true);
                }
            }
        }
        #endregion

        #region 字符串操作相关
        static public ArrayList CharCount(string str, char cTag)
        {//返回字符串中含有指定字符的位置的集合
            ArrayList intArr = new ArrayList();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == cTag)
                {
                    intArr.Add(i);
                }
            }
            return intArr;
        }

        static public ArrayList DecodeString(string input, string sFlag)
        {//将input安sFlag分隔成子字符串列表
            ArrayList list = new ArrayList();
            string strSub;
            int pos = input.IndexOf(sFlag, 0, input.Length);
            while (pos >= 0)
            {
                strSub = input.Substring(0, pos);
                // if (strSub.Trim()!="")
                //    list.Add(strSub.Trim());
                list.Add(strSub);
                input = input.Substring(pos + sFlag.Length, input.Length - pos - sFlag.Length);
                pos = input.IndexOf(sFlag, 0, input.Length);
            }
            //删除最后一个空字符串
            if (input.Trim() != "")
                list.Add(input);
            return list;
        }
        /// <summary>
        /// 比较两个字符串的大小
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        static public int CompareText(string strA, string strB)
        {
            return strA.ToUpper().CompareTo(strB.ToUpper());
        }
        /// <summary>
        /// 判断两个字符串是否相等
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        static public bool EqualText(string strA, string strB)
        {
            return CompareText(strA, strB) == 0;
        }
        /// <summary>
        /// 查找子串的位置,
        /// </summary>
        /// <param name="strSub">子串</param>
        /// <param name="strAll">要查找的字符串</param>
        /// <param name="igoreCase">是否要忽略大小写,如果是则为True</param>
        /// <returns>返回子串的位置</returns>
        static public int PosX(string strSub, string strAll, bool igoreCase)
        {

            if (igoreCase)
            {
                return strAll.IndexOf(strSub, StringComparison.OrdinalIgnoreCase);
                //return strAll.ToUpper().IndexOf(strSub.ToUpper());
            }
            else
                return strAll.IndexOf(strSub);
        }
        static public string[] ArrayListToArray(ArrayList list)
        {
            string[] strArr = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                strArr[i] = list[i].ToString();
            return strArr;
        }
        static public string[] DecodeStringToArray(string input, string sFlag)
        {
            ArrayList list = DecodeString(input, sFlag);
            string[] strArr = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strArr[i] = list[i].ToString();
            }
            return strArr;
        }
        static public string ArraylistToString(ArrayList list, string sFlag)
        {//将Arraylist转换成字符串
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0) result = list[i].ToString();
                else
                    result = result + sFlag + list[i].ToString();
            }
            return result;
        }
        static public string GetNumberString(string str, int number)
        {
            string result = "";
            for (int i = 0; i < number; i++)
            {
                result += str;
            }
            return result;
        }
        static public int GetSubStringCount(string input, string sFlag)
        {//取得input中sFlag的个数
            int iCount = 0;
            int pos = input.IndexOf(sFlag, 0, input.Length);
            while (pos >= 0)
            {
                iCount++;
                input = input.Substring(pos + sFlag.Length, input.Length - pos - sFlag.Length);
                pos = input.IndexOf(sFlag, 0, input.Length);
            }

            return iCount;
        }
        public int IndexOfChar(string strSour, int StartIndex, char[] cEndTag)
        {//返回数组中字符在strSour的开始位置
            for (int i = StartIndex; i < strSour.Length; i++)
            {
                char c = strSour[i];
                if (IsInArr(c, cEndTag))
                {
                    return i;
                }
            }

            return -1;
        }

        public int IndexOfArrary(string[] objs, string obj)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                string Aobj = objs[i];
                if (Aobj.Trim().ToUpper() == obj.Trim().ToUpper())
                    return i;
            }
            return -1;
        }
        static public bool IsInArr(object objTag, params object[] objArr)
        {//判断对象是否在数组中
            foreach (object obj in objArr)
            {
                if (obj == objTag) return true;
            }
            return false;
        }

        static public string ReplaceTags(string strSour, string strTag, string[] strArr)
        {//用string替换cTag定义的标记
            string strDest;
            int p = strSour.IndexOf(strTag);
            int iCount = 0;
            while (p > 0)
            {
                string str = strSour.Substring(0, p);
                str += strArr[iCount];
                str += strSour.Substring(p + strTag.Length, strSour.Length - p - strTag.Length);
                strSour = str;
                //				strSour=strSour.Substring(0,p)+strArr[iCount]
                //					+strSour.Substring(p+strTag.Length,strSour.Length-p-strTag.Length);
                p = strSour.IndexOf(strTag);
                iCount += 1;
            }
            strDest = strSour;
            return strDest;
        }
        static public string sortDecimalString(string str, string flag, bool bAscOrDesc)
        {//string str="12.0,14.8,5,3,16";排序成"3,5,12.0,14.8,16"等形式
            string[] strs = DecodeStringToArray(str, flag);
            ArrayList list = new ArrayList();
            foreach (string s in strs)
            {
                int index = 0;
                Decimal d = Decimal.Parse(s);
                foreach (object obj in list)
                {
                    Decimal d2 = Decimal.Parse(obj.ToString());
                    if (bAscOrDesc)
                    {
                        if (d2 >= d) break;
                    }
                    else
                    {
                        if (d2 <= d) break;
                    }
                    ++index;
                }
                list.Insert(index, s);
            }
            if (list.Count > 0)
                return ArraylistToString(list, flag) + flag;
            else
                return "";
        }
        static public void ChangeChar(ref string str, int index, char c)
        {
            char[] chars = str.ToCharArray(0, str.Length);
            chars[index] = c;
            //string strTmp=new string(chars);
            str = new string(chars);
            //str.Remove(index,1);
            //str.Insert(0,c);
        }
        static public string GetHtmlText(string htmlText)
        {//将Html页面中的空格&nbsp;去除掉
            string result = "";
            result = htmlText.Replace("&nbsp;", "");
            return result;
        }

        #endregion

        #region 数字操作相关
        static public void Dec(ref int i)
        {
            Dec(ref i, 1);
        }
        static public void Dec(ref int i, int step)
        {
            i -= step;
        }
        static public void Inc(ref int i)
        {
            Inc(ref i, 1);
        }
        static public void Inc(ref int i, int step)
        {
            i += step;
        }
        static public bool IsNumber(string str)
        {//判断字符串是否是整数形式
            if (str == null || str.Length == 0) return false;
            if (str[0] == '0') return false;
            foreach (char c in str)
            {
                if (!char.IsNumber(c)) return false;
            }
            return true;
        }
        static public bool IsDateTime(string str)
        {//判断字符串是否是整数形式
            try
            {
                DateTime t = DateTime.Parse(str);
                return (t.CompareTo(DateTime.MinValue) > 0);
            }
            catch
            {
                return false;
            }
        }
        static public bool IsInt(string str)
        {//判断字符串是否是整数形式
            try
            {
                int i = Convert.ToInt32(str);
                return (i > int.MinValue);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 判断是否是大于或等于0的整数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bIncludeZero"></param>
        /// <returns></returns>
        static public bool IsInt_puls(string str, bool bIncludeZero)
        {
            try
            {
                int i = Convert.ToInt32(str);
                return (i > 0) || ((i == 0) && bIncludeZero);
            }
            catch
            {
                return false;
            }
        }
        static public bool IsDouble(string str)
        {//判断字符串是否是浮点数数形式

            try
            {
                double d = Convert.ToDouble(str);
                return (d > Double.MinValue);
            }
            catch
            {
                return false;
            }
        }
        static public int LetterToInt(string str)
        {
            int result = 0;
            for (int i = 1; i <= str.Length; i++)
            {
                char c = char.ToUpper(str[i]);
                int d = (c - 'A' + 1);
                result += result * 26 + d;
            }
            return result;
        }
        /// <summary>
        /// 将钱转换成大写的钱数值,如贰仟伍佰萬
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static public string MoneyToChinese(double number)
        {//将数字转换成人民币
            string numList = "零壹贰叁肆伍陆柒捌玖";
            string rmbList = "分角元拾佰仟萬拾佰仟亿拾佰仟萬";
            string tempOutString = null;//


            if (number > 9999999999999.99) return null;

            //将小数转化为整数字符串
            string tempNumberString = Convert.ToInt64(number * 100).ToString();
            int tempNmberLength = tempNumberString.Length;
            int i = 0;
            while (i < tempNmberLength)
            {
                int oneNumber = Int32.Parse(tempNumberString.Substring(i, 1));
                string oneNumberChar = numList.Substring(oneNumber, 1);
                string oneNumberUnit = rmbList.Substring(tempNmberLength - i - 1, 1);
                if (oneNumberChar != "零")
                    tempOutString += oneNumberChar + oneNumberUnit;
                else
                {
                    if (oneNumberUnit == "亿" || oneNumberUnit == "萬" || oneNumberUnit == "元" || oneNumberUnit == "零")
                    {
                        while (tempOutString.EndsWith("零"))
                        {
                            tempOutString = tempOutString.Substring(0, tempOutString.Length - 1);
                        }

                    }
                    if (oneNumberUnit == "亿" || (oneNumberUnit == "萬" && !tempOutString.EndsWith("亿")) || oneNumberUnit == "元")
                    {
                        tempOutString += oneNumberUnit;
                    }
                    else
                    {
                        bool tempEnd = tempOutString.EndsWith("亿");
                        bool zeroEnd = tempOutString.EndsWith("零");
                        if (tempOutString.Length > 1)
                        {
                            bool zeroStart = tempOutString.Substring(tempOutString.Length - 2, 2).StartsWith("零");
                            if (!zeroEnd && (zeroStart || !tempEnd))
                                tempOutString += oneNumberChar;
                        }
                        else
                        {
                            if (!zeroEnd && !tempEnd)
                                tempOutString += oneNumberChar;
                        }
                    }
                }
                i += 1;
            }

            while (tempOutString.EndsWith("零"))
            {
                tempOutString = tempOutString.Substring(0, tempOutString.Length - 1);
            }

            while (tempOutString.EndsWith("元"))
            {
                tempOutString = tempOutString + "整";
            }

            return tempOutString;



        }
        /// <summary>
        /// 百分比转换成浮点数,如90.4%->0.904
        /// </summary>
        /// <param name="strBFB"></param>
        /// <returns></returns>
        static public double PercentToDouble(string strBFB)
        {//将百分比转换成double
            int p = strBFB.IndexOf("%", 0, strBFB.Length);
            if (p >= 0)
            {
                strBFB = strBFB.Substring(0, p);
                Double result = double.Parse(strBFB) / 100;
                return result;
            }
            else
                return -1;
        }
        /// <summary>
        /// 四舍五入函数
        /// </summary>
        /// <param name="value">要舍入的数</param>
        /// <param name="count">保留的小数位</param>
        /// <returns></returns>
        static public double NewRound(string value, int count)
        {
            double dValue;
            if (value == "" || value == null)
                return 0;
            try
            {
                dValue = Convert.ToDouble(value);
            }
            catch
            {
                dValue = 0;
            }
            if (dValue < 0)
                dValue = Math.Round(dValue + 5 / Math.Pow(10, count + 1), count, MidpointRounding.AwayFromZero);
            else if (dValue > 0)
                dValue = Math.Round(dValue, count, MidpointRounding.AwayFromZero);
            else
                dValue = 0;
            return dValue;
        }
        /// <summary>
        /// double 转换字符串,并保留几位小数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="dotCount"></param>
        /// <returns></returns>
        static public string DataToString(decimal d, int dotCount)
        {//保留小数点几位,不够用0填充 
            if (dotCount >= 0)
            {
                d = decimal.Round(d, dotCount);
                string ret = d.ToString();
                if (dotCount <= 0) return ret;
                int p = ret.IndexOf(".");
                if (p >= 0)
                {
                    int len = ret.Length;
                    for (int i = 1; i <= dotCount - (len - p - 1); i++)
                        ret = ret + "0";
                    return ret;
                }
                else
                {
                    ret = ret + ".";
                    for (int i = 1; i <= dotCount; i++)
                        ret = ret + "0";
                    return ret;
                }
            }
            else
                return d.ToString();

        }
        static public decimal DataToDot(decimal d, int dotCount)
        {//保留小数点几位
            string data = DataToString(d, dotCount);
            return decimal.Parse(data);
        }
        /// <summary>
        /// 保留小数点几位
        /// </summary>
        /// <param name="d">数</param>
        /// <param name="dotCount">小数点位数</param>
        /// <returns></returns>
        static public double DataToDot(double d, int dotCount)
        {
            string data = DataToString(d, dotCount);
            return double.Parse(data);
        }
        static public string DataToString(double d, int dotCount)
        {//保留小数点几位
            return DataToString(new decimal(d), dotCount);
        }
        /// <summary>
        /// 保留小数点几位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public double strToDouble(object data)
        {
            return strToDouble(data, -1);
        }
        /// <summary>
        /// 保留小数点几位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dotCount"></param>
        /// <returns></returns>
        static public double strToDouble(object data, int dotCount)
        {//保留小数点几位
            double d = 0;
            try
            {
                if (IsDBNull(data) == false)
                {
                    if (data.ToString() == "") return 0;
                    else
                    {
                        d = double.Parse(data.ToString().Trim());
                        if (dotCount >= 0)
                            d = DataToDot(d, dotCount);
                    }
                }
                return d;
            }
            catch
            {
                return d;
            }
        }
        /// <summary>
        /// 保留小数点几位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errDefValue"></param>
        /// <returns></returns>
        static public double strToDouble(object data, double errDefValue)
        {
            return strToDouble(data, -1, errDefValue);
        }
        /// <summary>
        /// 保留小数点几位
        /// </summary>
        /// <param name="data">数</param>
        /// <param name="dotCount">小数点位数</param>
        /// <param name="errDefValue">异常</param>
        /// <returns></returns>
        static public double strToDouble(object data, int dotCount, double errDefValue)
        {
            try
            {
                double d = 0;
                if (IsDBNull(data) == false)
                {
                    if (data.ToString() == "") return 0;
                    else
                    {
                        d = double.Parse(data.ToString().Trim());
                        if (dotCount >= 0)
                            d = DataToDot(d, dotCount);
                    }
                }
                return d;
            }
            catch
            {
                return errDefValue;
            }
        }
        static public int strToInt(string data)
        {
            return strToInt(data, 0);
        }
        static public int strToInt(string data, int defaultValue)
        {
            try
            {
                if (IsDBNull(data)) return defaultValue;
                else
                {
                    if (data.ToString() == "") return defaultValue;
                    else
                    {
                        return int.Parse(data.ToString());
                    }
                }
            }
            catch
            {
                return defaultValue;
            }
        }
        static public bool strToBool(string data)
        {
            return strToBool(data, false);
        }
        static public bool strToBool(string data, bool defaultValue)
        {
            try
            {
                if (IsDBNull(data)) return defaultValue;
                else
                {
                    if (data.ToString() == "") return defaultValue;
                    else
                    {
                        return bool.Parse(data.ToString());
                    }
                }
            }
            catch
            {
                return defaultValue;
            }
        }
        static public bool StrToBool_Delphi(string strBool, bool defaultValue)
        {
            if (strBool == "-1") return true;
            else if (strBool == "0") return false;
            else
                return defaultValue;
        }
        static public string BoolToStr_Delphi(bool b)
        {
            if (b) return "-1";
            else
                return "0";
        }
        static public double ObjectToDouble(object str, double Errdef)
        {
            if ((str == null) || (str == DBNull.Value)) return Errdef;
            try
            {
                return double.Parse(str.ToString().Trim());
            }
            catch
            {
                return Errdef;
            }
        }
        static public double ObjectToDouble(object str)
        {
            return ObjectToDouble(str, -1);
        }
        static public int ObjectToInt(object str, int nullValue)
        {
            if ((str == null) || (str == DBNull.Value)) return nullValue;
            try
            {
                return int.Parse(str.ToString().Trim());
            }
            catch
            {
                return nullValue;
            }
        }
        static public int ObjectToInt(object str)
        {
            return ObjectToInt(str, -1);
        }
        static public string ObjectToStr(object strObj)
        {
            if (IsDBNull(strObj)) return string.Empty;
            else
                return strObj.ToString();
        }
        static public string ObjectToStr(object strObj, string nullValue)
        {
            if (IsDBNull(strObj)) return nullValue;
            else
                return strObj.ToString();
        }
        static public DateTime ObjectToDateTime(object obj)
        {
            if (IsDBNull(obj)) return DateTime.MinValue;
            else
                return DateTime.Parse(obj.ToString());
        }
        static public DateTime ObjectToDateTime(object obj, DateTime errValue)
        {
            if (IsDBNull(obj)) return errValue;
            else
                return DateTime.Parse(obj.ToString());
        }
        static public bool IsDBNull(object obj)
        {
            return (obj == null) || (obj == DBNull.Value);
        }
        #endregion

        
    }
    #region win api

    public partial class TdaWin32Api
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int frequency, int duration);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]//移动文件	
        public static extern bool MoveFile(String src, String dst);
        [DllImport("kernel32")]//写INI文件
        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//读ini文件（字符
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//读ini文件（数字
        public static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
        [DllImport("kernel32")]
        public static extern void Sleep(int milliseconds);

        /// <summary> 
        /// 原型是 :HMODULE LoadLibrary(LPCTSTR lpFileName); 
        /// </summary> 
        /// <param name="lpFileName">DLL 文件名 </param> 
        /// <returns> 函数库模块的句柄 </returns> 
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);
        /// <summary> 
        /// 原型是 : FARPROC GetProcAddress(HMODULE hModule, LPCWSTR lpProcName); 
        /// </summary> 
        /// <param name="hModule"> 包含需调用函数的函数库模块的句柄 </param> 
        /// <param name="lpProcName"> 调用函数的名称 </param> 
        /// <returns> 函数指针 </returns> 
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        /// <summary> 
        /// 原型是 : BOOL FreeLibrary(HMODULE hModule); 
        /// </summary> 
        /// <param name="hModule"> 需释放的函数库模块的句柄 </param> 
        /// <returns> 是否已释放指定的 Dll</returns> 
        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
    #endregion

    #region public class TdaIniFile//Ini操作类
    public class TdaIniFile
    {

        #region static
        static public List<KeyValuePair<string, string>> ReadSection(string sec, string iniFilePathName)
        {

            if (!File.Exists(iniFilePathName))
                throw new FileNotFoundException(iniFilePathName);
            string[] array = File.ReadAllLines(iniFilePathName, Encoding.Default);
            bool inSect = false;
            List<KeyValuePair<string, string>> sect = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < array.Length; i++)
            {
                string line = array[i].Trim();
                if (!inSect)
                {
                    if (line.IndexOf("[" + sec + "]", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        inSect = true;
                    }
                    continue;
                }
                else
                {
                    Match match = Regex.Match(line, @"\[(\w)+\]", RegexOptions.IgnoreCase);
                    if (match.Success)
                        break;
                    else
                    {
                        match = Regex.Match(line, @"(?<key>(\w+))\s*=\s*(?<val>(\w*))", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            Group gkey = match.Groups["key"];
                            Group gVal = match.Groups["val"];
                            if (gkey != null)
                            {
                                KeyValuePair<string, string> kv = new KeyValuePair<string, string>(gkey.Value, gVal.Value);
                                sect.Add(kv);
                            }
                        }
                    }
                }
            }

            return sect;
        }
        static public string ReadStringDef(string sec, string key, string def, string iniFilePathName)
        {
            StringBuilder result = new StringBuilder(2048);
            int iret = TdaWin32Api.GetPrivateProfileString(sec, key, def, result, result.Capacity, iniFilePathName);
            return result.ToString();
        }
        static public string ReadString(string sec, string key, string iniFilePathName)
        {
            return ReadStringDef(sec, key, "", iniFilePathName);
        }
        static public int ReadIntDef(string sec, string key, int def, string iniFilePathName)
        {
            return TdaWin32Api.GetPrivateProfileInt(sec, key, def, iniFilePathName);
        }
        static public int ReadInt(string sec, string key, string iniFilePathName)
        {
            return ReadIntDef(sec, key, 0, iniFilePathName);
        }
        static public bool WriteString(string sec, string key, string val, string iniFilePathName)
        {
            return TdaWin32Api.WritePrivateProfileString(sec, key, val, iniFilePathName);
        }
        static public void DeleteFile(string iniFilePathName)
        {
            if (File.Exists(iniFilePathName))
                File.Delete(iniFilePathName);
        }
        #endregion
    }
    #endregion

    public class TdaItem
    {
        public object _name;
        public object _value;
        public TdaItem() : this(null, null) { }
        public TdaItem(object Name, object Value)
        {
            _name = Name;
            _value = Value;
        }
        public object Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
    public class TdaItemT<T, U>
    {
        public T _name;
        public U _value;
        public TdaItemT() //: this(null, null) 
        { }
        public TdaItemT(T Name, U Value)
        {
            _name = Name;
            _value = Value;
        }
        public T Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public U Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
    public class TdaItem2
    {
        public object _name;
        public object _value1, _value2;
        public TdaItem2(object Name, object Value1, object Value2)
        {
            _name = Name;
            _value1 = Value1;
            _value2 = Value2;
        }
        public string ToString(string format)
        {
            return string.Format(format, _name, _value1, _value2);
        }
        public object Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public object Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }
        public object Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

    }
    public class TdaItem3
    {
        public object _name;
        public object _value1, _value2, _value3;
        public TdaItem3(object Name, object Value1)
        {
            _name = Name;
            _value1 = Value1;
            _value2 = null;
            _value3 = null;
        }
        public TdaItem3(object Name, object Value1, object Value2)
        {
            _name = Name;
            _value1 = Value1;
            _value2 = Value2;
            _value3 = null;
        }
        public TdaItem3(object Name, object Value1, object Value2, object Value3)
        {
            _name = Name;
            _value1 = Value1;
            _value2 = Value2;
            _value3 = Value3;
        }
        public object Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }
        public object Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }
        public object Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }


    }
    public class TdaItemList : IEnumerable
    {
        public object _name;
        public ArrayList _List;
        public TdaItemList(object Name, params object[] objValues)
        {
            _name = Name;
            _List = new ArrayList();
            foreach (object obj in objValues)
                _List.Add(obj);
        }
        #region IEnumerable 成员
        private class Enumerator : IEnumerator
        {
            private ArrayList m_list = null;
            private int m_pos = -1;
            public Enumerator(ArrayList list)
            {
                m_list = list;
            }
            #region IEnumerator 成员

            public void Reset()
            {
                m_pos = -1;
            }

            public object Current
            {
                get
                {
                    return m_list[m_pos];
                }
            }

            public bool MoveNext()
            {
                // TODO:  添加 Enumerator.MoveNext 实现
                m_pos++;
                return m_pos < m_list.Count;
            }

            #endregion

        }

        public IEnumerator GetEnumerator()
        {
            // TODO:  添加 TdaItemList.GetEnumerator 实现
            return new Enumerator(_List);
        }

        #endregion
    }
}
