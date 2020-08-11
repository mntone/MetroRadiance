using System;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;

namespace MetroRadiance.Platform
{
	internal class PerMonitorAwareDpiHelper : SystemAwareDpiHelper
	{
		public override bool IsPerMonitorDpiSupported => true;

		public override Dpi GetDpiForMonitor(IntPtr hMonitor)
		{
			uint dpiX, dpiY;
			SHCore.GetDpiForMonitor(hMonitor, MonitorDpiType.Default, out dpiX, out dpiY);

			return new Dpi(dpiX, dpiY);
		}

		public override Dpi GetDpiForWindow(IntPtr hWnd)
		{
			var hMonitor = User32.MonitorFromWindow(
				hWnd,
				MonitorDefaultTo.MONITOR_DEFAULTTOPRIMARY);

			return this.GetDpiForMonitor(hMonitor);
		}
	}
}
