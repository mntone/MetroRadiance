using System;
using System.Runtime.InteropServices;

namespace MetroRadiance.Interop.Win32
{
	public static class Gdi32
	{
		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern int GetDeviceCaps(IntPtr hdc, DeviceCaps nIndex);
	}
}
