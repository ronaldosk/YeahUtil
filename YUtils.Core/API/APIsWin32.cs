using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Yeah.Core.API
{
    public class APIsWin32
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
        /// <summary>
        /// 获取ini文件所有的Section
        /// </summary>
        [DllImport("Kernel32.dll")]
        public extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);
        /// <summary>
        /// 获取指定Section下的所有Key的值
        /// </summary>
        [DllImport("Kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string strSecName, string strKeyName, string sDefault, byte[] buffer, int nSize, string strFileName);
        /// <summary>
        /// 获取指定Section的key和value
        /// </summary>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string lpFileName);


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
}
