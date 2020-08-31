using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace MetroRadiance.Interop.Win32
{
#pragma warning disable IDE1006
	[StructLayout(LayoutKind.Sequential)]
	public struct APPBARDATA
	{
		public int cbSize;
		public IntPtr hWnd;
		public uint uCallbackMessage;
		public AppBarEdges uEdge;
		public RECT rc;
		public IntPtr lParam;
	}
#pragma warning restore IDE1006
}
