﻿using BarcodeLib;
using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using Yeah.Core.API;

namespace YUtils
{
    public class MsgUtils
    {
        public static MessageBoxResult MessageBox(string message, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, params object[] parameters)
        {
            string msg = message;
            if (parameters.Length > 0)
            {
                msg = string.Format(message, parameters);
            }
            return System.Windows.MessageBox.Show(msg, "YUtils", button, icon);
        }

    }

    #region 自动关闭指定进程窗口类
    /// 调用示例如下
    /// try
    ///{
    ///    KillWindowThread.AddKillWindow("#32770", "MxDrawX");
    ///    KillWindowThread.StartKillWindowThread();
    ///    using (FrmPartPreTreat dlg = new FrmPartPreTreat())
    ///    {
    ///        dlg.ShowDialog();
    ///    }
    ///}
    ///finally
    ///{
    ///    KillWindowThread.StopKillWindowThread();
    ///}
    public class KillWindowThread
    {
        static private Dictionary<string, string> _KilledWindowCollection = new Dictionary<string, string>();
        static private bool _KillWindowThreadIsRun = false;
        static private bool _StopKillWindowThread = false;
        static private void KillMxDrawProc()
        {
            while (true)
            {
                Thread.Sleep(10);
                foreach (string key in _KilledWindowCollection.Keys)
                {
                    string winText = _KilledWindowCollection[key];
                    IntPtr p = APIsWndProc.FindWindow(key, winText);
                    if (p.ToInt32() > 0)
                        APIsUser32.SendMessage(p, 16, 0, 0);
                }
                if (_StopKillWindowThread)
                {
                    _KillWindowThreadIsRun = false;
                    break;
                }
            }
        }
        static public void AddKillWindow(string className, string windowText)
        {
            if (!_KilledWindowCollection.ContainsKey(className))
            {
                _KilledWindowCollection.Add(className, windowText);
            }
            else
                _KilledWindowCollection[className] = windowText;
        }
        static public void StartKillWindowThread()
        {
            if (_KillWindowThreadIsRun) return;
            _KillWindowThreadIsRun = false;
            _StopKillWindowThread = false;
            Thread thread = new Thread(new ThreadStart(KillMxDrawProc));
            thread.Start();
        }
        static public void StopKillWindowThread()
        {
            _StopKillWindowThread = true;
        }
    }
    #endregion

    #region INI文件操作
    public class INIHelper
    {
        #region Win API
        public string _Path = string.Empty;
        private bool flag;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public INIHelper(string path)
        {
            this._Path = path;
        }

        /// <summary>
        /// Write *.ini
        /// </summary>
        /// <param name="section">Node</param>
        /// <param name="key">字段</param>
        /// <param name="value">属性值</param>
        public void WriteValue(string section, string key, string value)
        {
            APIsWin32.WritePrivateProfileString(section, key, value, _Path);
        }

        /// <summary>
        /// Read *.ini
        /// </summary>
        /// <param name="section">Node</param>
        /// <param name="key">字段</param>
        /// <returns>属性值</returns>
        public string ReadValue(string section, string key)
        {
            StringBuilder builder = new StringBuilder(1024);
            APIsWin32.GetPrivateProfileString(section, key, "", builder, 1024, _Path);
            return builder.ToString();
        }
        /// <summary>
        /// 读取Count
        /// </summary>
        public int GetCount(string section, string key)
        {
            StringBuilder builder = new StringBuilder(255);
            APIsWin32.GetPrivateProfileString(section, key, "", builder, 255, _Path);
            return Convert.ToInt16(builder.ToString());
        }

        /// <summary>
        /// 确保资源释放
        /// </summary>
        ~INIHelper()
        {
            APIsWin32.WritePrivateProfileString(null, null, null, _Path);
        }
        /// <summary>
        /// 返回该配置文件中所有Section名称的集合
        /// </summary>
        /// <returns></returns>
        public ArrayList ReadSections()
        {
            byte[] buffer = new byte[65535];
            int rel = APIsWin32.GetPrivateProfileSectionNamesA(buffer, buffer.GetUpperBound(0), this._Path);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
        /// <summary>
        /// 获取指定节点的所有Key的名称
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public ArrayList ReadKeys(string sectionName)
        {
            byte[] buffer = new byte[5120];
            int rel = APIsWin32.GetPrivateProfileStringA(sectionName, null, "", buffer, buffer.GetUpperBound(0), this._Path);

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
        /// <summary>
        /// 读取指定节点下的所有key 和value
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public ArrayList GetIniSectionValue(string section)
        {
            ArrayList li = new ArrayList();
            byte[] buffer = new byte[32768];
            int bufLen = 0;
            bufLen = APIsWin32.GetPrivateProfileSection(section, buffer, buffer.GetUpperBound(0), this._Path);
            if (bufLen > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bufLen; i++)
                {
                    if (buffer[i] != 0)
                    {
                        sb.Append((char)buffer[i]);
                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            li.Add(sb.ToString());
                            sb = new StringBuilder();
                        }
                    }
                }
            }
            return li;
        }
        /// <summary>
        /// 从指定的节点中获取一个整数值( Long，
        /// 找到的key的值；如指定的key未找到，就返回默认值。
        /// 如找到的数字不是一个合法的整数，函数会返回其中合法的一部分。如，对于“xyz=55zz”这个条目，函数返回55。)
        /// </summary>
        public int GetIniKeyValueForInt(string section, string key)
        {
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0) return 0;
            return APIsWin32.GetPrivateProfileInt(section, key, 0, this._Path);
        }
        /// <summary>
        /// 修改指定section的key的值
        /// </summary>
        public bool EditIniKey(string section, string key, string value)
        {
            try
            {
                if (section.Trim().Length <= 0 || key.Trim().Length <= 0 || value.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (!APIsWin32.WritePrivateProfileString(section, key, value, this._Path))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 删除指定section的指定key
        /// </summary>
        public bool DeleteIniKey(string section, string key)
        {

            try
            {
                if (section.Trim().Length <= 0 || key.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (!APIsWin32.WritePrivateProfileString(section, key, null, this._Path))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 删除指定section
        /// </summary>
        public bool DeleteIniSection(string section)
        {
            try
            {
                if (section.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (!APIsWin32.WritePrivateProfileString(section, null, null, this._Path))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
    #endregion

    #region C++dll调用
    public class DllInvoke
    {
        private IntPtr hLib;
        public DllInvoke(string DLLPath)
        {
            hLib = APIsWin32.LoadLibrary(DLLPath);
        }

        public IntPtr GetIntPtr()
        {
            return hLib;
        }

        //将要执行的函数转换为委托
        public Delegate Invoke(string APIName, Type t)
        {
            IntPtr api = APIsWin32.GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
    }
    #endregion

    #region 对内存进行数据读写
    public class UserMemoryStream
    {
        public static string outputfileName;

        public UserMemoryStream(string filename)
        {
            outputfileName = filename;
        }
        /// <summary>
        /// 从文件中加载到MemoryStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public MemoryStream CreateFromFile(string fileName)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                try
                {
                    byte[] buff = new byte[fs.Length];
                    fs.Read(buff, 0, (int)fs.Length);
                    ms.Write(buff, 0, buff.Length);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
                return ms;
            }
            catch
            {
                return null;
            }
        }
        static public MemoryStream CreateFromBytes(byte[] buff)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(buff, 0, buff.Length);
                return ms;
            }
            catch
            {
                return null;
            }
        }

        public bool SaveToFile(string inputString)
        {
            try
            {
                byte[] buff = Encoding.Default.GetBytes(inputString);
                MemoryStream ms = CreateFromBytes(buff);
                UserMemoryStream.SaveToFile(ms, outputfileName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool SaveToFile(MemoryStream ms, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                try
                {
                    //ms.Position = 0;
                    ms.WriteTo(fs);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class UserStreamWrapper
    {
        public Stream _Stream = null;
        public UserStreamWrapper(Stream stream)
        {
            _Stream = stream;
        }

        public byte ReadByte()
        {
            byte[] buffer = new byte[sizeof(byte)];
            _Stream.Read(buffer, 0, buffer.Length);
            return buffer[0];
        }
        public Int16 ReadInt16()
        {
            byte[] buffer = new byte[sizeof(Int16)];
            _Stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt16(buffer, 0);
        }
        public Int32 ReadInt32()
        {
            byte[] buffer = new byte[sizeof(Int32)];
            _Stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }
        public Int64 ReadInt64()
        {
            byte[] buffer = new byte[sizeof(Int64)];
            _Stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt64(buffer, 0);
        }
        public double ReadDouble()
        {
            byte[] buffer = new byte[sizeof(double)];
            _Stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToDouble(buffer, 0);
        }
        public void Write(byte b)
        {
            byte[] buffer = new byte[1];
            buffer[0] = b;
            _Stream.Write(buffer, 0, buffer.Length);
        }
        public void Write(Int16 i)
        {
            byte[] buffer = BitConverter.GetBytes(i);
            _Stream.Write(buffer, 0, buffer.Length);
        }
        public void Write(Int32 i)
        {
            byte[] buffer = BitConverter.GetBytes(i);
            _Stream.Write(buffer, 0, buffer.Length);
        }
        public void Write(Int64 i)
        {
            byte[] buffer = BitConverter.GetBytes(i);
            _Stream.Write(buffer, 0, buffer.Length);
        }
        public void Write(double i)
        {
            byte[] buffer = BitConverter.GetBytes(i);
            _Stream.Write(buffer, 0, buffer.Length);
        }
    }
    #endregion

    #region 随机生成验证码
    public class CreateSecCode
    {
        public void CreateCode()
        {
            //创建一个包含随机内容的验证码文本    
            Random rand = new Random();
            int len = rand.Next(4, 6);//返回一个大于等于4，小于6的整型随机数 
            char[] chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();//定义一个char数组

            //用于追加字符串，效率比string更高效
            StringBuilder myStr = new StringBuilder();

            for (int iCount = 0; iCount < len; iCount++)
            {
                myStr.Append(chars[rand.Next(chars.Length)]);
            }
            string text = myStr.ToString();//生成的随机内容

            // 保存验证码到 session 中以便其他模块使用   
            //this.Session["checkcode"] = text;

            Size ImageSize = Size.Empty;//用于存储验证码图片大小
            System.Drawing.Font myFont = new System.Drawing.Font("MS Sans Serif", 20);

            // 计算验证码图片大小   
            using (Bitmap bmp = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    SizeF size = g.MeasureString(text, myFont, 10000);//获取特定字体样式的文本大小
                    ImageSize.Width = (int)size.Width + 8;
                    ImageSize.Height = (int)size.Height + 8;
                }
            }

            // 创建验证码图片   
            using (Bitmap bmp = new Bitmap(ImageSize.Width, ImageSize.Height))
            {
                // 绘制验证码文本   
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    using (StringFormat f = new StringFormat())
                    {
                        f.Alignment = StringAlignment.Near;
                        f.LineAlignment = StringAlignment.Center;
                        f.FormatFlags = StringFormatFlags.NoWrap;
                        g.DrawString(text, myFont, Brushes.Red, new RectangleF(0, 0, ImageSize.Width, ImageSize.Height), f);
                    }//using   
                }//using   

                // 制造噪声 杂点面积占图片面积的 8%   
                int num = ImageSize.Width * ImageSize.Height * 8 / 100;

                for (int iCount = 0; iCount < num; iCount++)
                {
                    // 在随机的位置使用随机的颜色设置图片的像素   
                    int x = rand.Next(ImageSize.Width);
                    int y = rand.Next(ImageSize.Height);
                    int r = rand.Next(255);
                    int g = rand.Next(255);
                    int b = rand.Next(255);
                    Color c = Color.FromArgb(r, g, b);
                    bmp.SetPixel(x, y, c);
                }//for                

                // 输出图片   
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //this.Response.ContentType = "image/png";
                //ms.WriteTo(this.Response.OutputStream);
                ms.Close();
            }//using
            myFont.Dispose();
        }
    }
    #endregion

    #region 组合
    public class Combination
    {
        public Combination()
        {
        }

        /// <summary>
        /// 按照组合方式处理后，数据源数组各项在输出结果中对应的下标总集
        /// </summary>
        /// <param name="MaxNum">数据源数组数据个数，组合公式中的下标值</param>
        /// <param name="SampleWinWidth">参与选择的元素个数，组合公式中的上标值</param>
        /// <param name="ResultList">结果总集，可以根据上标值后期作划分处理得到各子数组</param>
        public void GetCombinationIndexList(int MaxNum, int SampleWinWidth, ref List<int> ResultList)
        {
            List<int> TempList = new List<int>();
            List<int> OutputList = new List<int>();
            ResultList.Clear();
            GetIndexList(ref TempList, 0, MaxNum, SampleWinWidth);
            GetNewList(ref OutputList, TempList, SampleWinWidth);
            foreach (int value in OutputList)
            {
                ResultList.Add(value - 1);
            }
        }

        private void GetIndexList(ref List<int> OutputList, int startindex, int MaxNum, int winLength)
        {
            int index = 0;
            for (index = startindex + 1; index <= MaxNum - (winLength - 1); index++)
            {
                OutputList.Add(index);
                if (winLength > 1)
                    GetIndexList(ref OutputList, index, MaxNum, winLength - 1);
            }
        }

        private void GetNewList(ref List<int> OutputList, List<int> InputList, int winLength)
        {
            int curtempindex = 2;
            List<int> TempList = new List<int>();
            for (int i = 0; i < InputList.Count; i++)
            {
                int value = InputList[i];
                if (i < winLength)
                {
                    TempList.Add(value);
                    OutputList.Add(value);
                }
                else
                {
                    for (int j = 0; j < TempList.Count; j++)
                    {
                        int tempvalue = TempList[j];
                        if (curtempindex < winLength - 1)
                        {
                            OutputList.Add(value);
                            curtempindex++;
                            TempList[curtempindex] = value;
                            break;
                        }
                        if (value > tempvalue + 1)
                        {
                            OutputList.Add(tempvalue);
                            TempList[j] = tempvalue;
                        }
                        else if (value == tempvalue + 1)
                        {
                            OutputList.Add(value);
                            TempList[j] = value;
                            curtempindex = j;
                            break;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region  RSA加解密处理
    public class RSAProc
    {
        string privateKey = "";
        string publicKey = "";
        string filename = "";

        public RSAProc(string pubkey, string privkey, string filepath)
        {
            filename = filepath;
            if (string.IsNullOrEmpty(pubkey) && string.IsNullOrEmpty(privkey))
                RSAKey();
            else
            {
                privateKey = privkey;
                publicKey = pubkey;
            }
        }

        public void outprivatekey(string code)
        {
            if (code == "DDSOFT")
            {
                INIHelper ir = new INIHelper(filename);
                ir.WriteValue("RSA", "privatekey", privateKey);
            }
        }

        public void outpublickey(string code)
        {
            if (code == "DDSOFT")
            {
                INIHelper ir = new INIHelper(filename);
                ir.WriteValue("RSA", "publickey", publicKey);
            }
        }
        //产生私钥和公钥
        public void RSAKey()
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
                outprivatekey("DDSOFT");
                outpublickey("DDSOFT");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //公钥加密函数
        public string RSAEncryptPub(string orgstr)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider myrsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                myrsa.FromXmlString(publicKey);
                //把你要加密的内容转换成byte[]
                byte[] PlainTextBArray = (new UnicodeEncoding()).GetBytes(orgstr);
                //使用.NET中的Encrypt方法加密
                byte[] CypherTextBArray = myrsa.Encrypt(PlainTextBArray, false);
                //最后吧加密后的byte[]转换成Base64String，加密后的内容
                return Convert.ToBase64String(CypherTextBArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //私钥解密函数
        public string RSADecryptPrv(string privKey, string encstr)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider myrsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                //得到私钥
                myrsa.FromXmlString(privKey);
                //把原来加密后的String转换成byte[]
                byte[] PlainTextBArray = Convert.FromBase64String(encstr);
                //使用.NET中的Decrypt方法解密
                byte[] DypherTextBArray = myrsa.Decrypt(PlainTextBArray, false);
                //转换解密后的byte[]，得到加密前的内容
                string outstr = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return outstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
        //私钥加密函数
        public string RSAEncryptPrv(string orgstr)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider myrsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                myrsa.FromXmlString(privateKey);
                //把你要加密的内容转换成byte[]
                byte[] PlainTextBArray = (new UnicodeEncoding()).GetBytes(orgstr);
                //使用.NET中的Encrypt方法加密
                byte[] CypherTextBArray = myrsa.Encrypt(PlainTextBArray, false);
                //最后吧加密后的byte[]转换成Base64String，加密后的内容
                return Convert.ToBase64String(CypherTextBArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //公钥解密函数
        public string RSADecryptPub(string pubKey, string encstr)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider myrsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                //得到私钥
                myrsa.FromXmlString(pubKey);
                //把原来加密后的String转换成byte[]
                byte[] PlainTextBArray = Convert.FromBase64String(encstr);
                //使用.NET中的Decrypt方法解密
                byte[] DypherTextBArray = myrsa.Decrypt(PlainTextBArray, false);
                //转换解密后的byte[]，得到加密前的内容
                string outstr = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return outstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
    }
    #endregion

    #region 工具类-条码生成类
    /// <summary>
    /// 条码生成
    /// </summary>
    public class EncodingHandler
    {
        #region QRCodeProvider.dll
        public static Bitmap createQRCode(String strCode, int widthAndHeight)
        {
            MultiFormatWriter mfwr = new MultiFormatWriter();
            ByteMatrix matrix = mfwr.encode(strCode, BarcodeFormat.QR_CODE, widthAndHeight, widthAndHeight);
            if (matrix != null)
            {
                Bitmap bitmap = matrix.ToBitmap();
                return bitmap;
            }
            return null;
        }
        #endregion

        #region barcodelib.dll
        static System.Drawing.Image GetBarcode(int height, int width, BarcodeLib.TYPE type, string code, out System.Drawing.Image image)
        {
            image = null;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;//图片背景颜色
            b.ForeColor = System.Drawing.Color.Black;//条码颜色
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;//图片格式
            System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);//字体设置
            b.LabelFont = font;

            b.Height = height;//图片高度设置(px单位)
            b.Width = width;//图片宽度设置(px单位)

            image = b.Encode(type, code);//生成图片
            byte[] buffer = b.GetImageData(SaveTypes.GIF);//转换byte格式
            //byte转换图片格式
            MemoryStream oMemoryStream = new MemoryStream(buffer);
            //設定資料流位置  
            oMemoryStream.Position = 0;
            //return buffer;
            //return image;
            return System.Drawing.Image.FromStream(oMemoryStream);
        }

        public static string GetBarcodeString(int height, int width, BarcodeLib.TYPE type, string code, out System.Drawing.Image image)
        {
            image = null;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;//图片背景颜色
            b.ForeColor = System.Drawing.Color.Black;//条码颜色
            b.IncludeLabel = true;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;//图片格式
            System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);//字体设置
            b.LabelFont = font;

            b.Height = height;//图片高度设置(px单位)
            b.Width = width;//图片宽度设置(px单位)

            image = b.Encode(type, code);//生成图片
            byte[] buffer = b.GetImageData(SaveTypes.GIF);//转换byte格式
            //byte转换图片格式
            MemoryStream oMemoryStream = new MemoryStream(buffer);
            //設定資料流位置  
            oMemoryStream.Position = 0;
            //return buffer;
            //return image;
            return System.Convert.ToBase64String(buffer, 0, buffer.Length);
        }

        public static string BarImageURL(string barCodeBM, string imagfolder, int diffCount, bool boverwrite = false)
        {
            System.Drawing.Image image;
            int width = 500, height = 100;
            if (string.IsNullOrEmpty(barCodeBM))
                return "";
            string SRC = (imagfolder + barCodeBM + ".png").Replace(imagfolder.Substring(0, imagfolder.Length - diffCount), "").Replace("\\", "/");
            if (boverwrite)
            {
                if (File.Exists(SRC))
                    File.Delete(SRC);
                image = GetBarcode(height, width, BarcodeLib.TYPE.CODE128, barCodeBM, out image);
                if (!File.Exists(imagfolder + barCodeBM + ".png"))
                    image.Save(imagfolder + barCodeBM + ".png");
            }
            else
            {
                if (!File.Exists(SRC))
                {
                    image = GetBarcode(height, width, BarcodeLib.TYPE.CODE128, barCodeBM, out image);
                    if (!File.Exists(imagfolder + barCodeBM + ".png"))
                        image.Save(imagfolder + barCodeBM + ".png");
                }
            }
            return SRC + "?temp=" + DateTime.Now.Millisecond.ToString();
        }
        #endregion
    }
    #endregion
}