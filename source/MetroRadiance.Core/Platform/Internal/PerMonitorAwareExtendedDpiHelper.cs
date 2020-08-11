using System;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;

namespace MetroRadiance.Platform
{
	internal class PerMonitorAwareExtendedDpiHelper : PerMonitorAwareDpiHelper
	{
		public override Dpi GetDpiForSystem()
		{
			var dpi = User32.GetDpiForSystem();
			return new Dpi(dpi, dpi);
		}

		public override Dpi GetDpiForWindow(IntPtr hWnd)
		{
			var dpi = User32.GetDpiForWindow(hWnd);
			return new Dpi(dpi, dpi);
		}
	}
}
