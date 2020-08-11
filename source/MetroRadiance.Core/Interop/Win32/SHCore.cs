using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MetroRadiance.Interop.Win32
{
	// ReSharper disable once InconsistentNaming
	public static class SHCore
	{
		[DllImport("Shcore.dll", EntryPoint = "GetProcessDpiAwareness", ExactSpelling = true, PreserveSig = false)]
		private static extern void _GetProcessDpiAwareness(IntPtr hProcess, out ProcessDpiAwareness value); // Windows 8.1 and above

		public static ProcessDpiAwareness GetProcessDpiAwareness(IntPtr hProcess)
		{
			ProcessDpiAwareness awareness;
			SHCore._GetProcessDpiAwareness(hProcess, out awareness);
			return awareness;
		}

		public static ProcessDpiAwareness GetCurrentProcessDpiAwareness()
		{
			var process = System.Diagnostics.Process.GetCurrentProcess();

			ProcessDpiAwareness awareness;
			SHCore._GetProcessDpiAwareness(process.Handle, out awareness);
			return awareness;
		}

		[DllImport("SHCore.dll", ExactSpelling = true, PreserveSig = false)]
		public static extern void GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);
	}
}
