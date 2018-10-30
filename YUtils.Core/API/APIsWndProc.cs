using System;
using System.Runtime.InteropServices;

namespace Yeah.Core.API
{
	public class APIsWndProc
	{
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]   //ÕÒ×Ó´°Ìå 
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLong(
			IntPtr window,
			int index,
			WndProc value);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLong(
			IntPtr window,
			int index,
			GetWndProc value);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLong(
			IntPtr window,
			int index,
			IntPtr value);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(
			IntPtr window,
			int index);

		[DllImport("user32.dll")]
		public static extern int CallWindowProc(
			IntPtr value,
			IntPtr Handle,
			int Msg,
			IntPtr WParam,
			IntPtr LParam);

		[DllImport("user32.dll")]
		public static extern int DefWindowProc(
			IntPtr Handle,
			int Msg,
			IntPtr WParam,
			IntPtr LParam);

		public delegate int GetWndProc(IntPtr hWnd, int Msg, IntPtr WParam, IntPtr LParam);
		public delegate void WndProc(ref System.Windows.Forms.Message m);
	}
}
