using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Yeah.Core.API
{
    public class APIsWin32
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int frequency, int duration);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]//�ƶ��ļ�	
        public static extern bool MoveFile(String src, String dst);
        [DllImport("kernel32")]//дINI�ļ�
        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//��ini�ļ����ַ�
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//��ini�ļ�������
        public static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
        /// <summary>
        /// ��ȡini�ļ����е�Section
        /// </summary>
        [DllImport("Kernel32.dll")]
        public extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);
        /// <summary>
        /// ��ȡָ��Section�µ�����Key��ֵ
        /// </summary>
        [DllImport("Kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string strSecName, string strKeyName, string sDefault, byte[] buffer, int nSize, string strFileName);
        /// <summary>
        /// ��ȡָ��Section��key��value
        /// </summary>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string lpFileName);


        [DllImport("kernel32")]
        public static extern void Sleep(int milliseconds);

        /// <summary> 
        /// ԭ���� :HMODULE LoadLibrary(LPCTSTR lpFileName); 
        /// </summary> 
        /// <param name="lpFileName">DLL �ļ��� </param> 
        /// <returns> ������ģ��ľ�� </returns> 
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);
        /// <summary> 
        /// ԭ���� : FARPROC GetProcAddress(HMODULE hModule, LPCWSTR lpProcName); 
        /// </summary> 
        /// <param name="hModule"> ��������ú����ĺ�����ģ��ľ�� </param> 
        /// <param name="lpProcName"> ���ú��������� </param> 
        /// <returns> ����ָ�� </returns> 
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        /// <summary> 
        /// ԭ���� : BOOL FreeLibrary(HMODULE hModule); 
        /// </summary> 
        /// <param name="hModule"> ���ͷŵĺ�����ģ��ľ�� </param> 
        /// <returns> �Ƿ����ͷ�ָ���� Dll</returns> 
        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
