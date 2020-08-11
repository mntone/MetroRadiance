using System;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;

namespace MetroRadiance.Platform
{
	internal class SystemAwareDpiHelper : IDpiHelper
	{
		private Lazy<Dpi> _systemDpi = new Lazy<Dpi>(() => GetDpi(IntPtr.Zero));

		public virtual bool IsDpiAwareSupported => true;

		public virtual bool IsPerMonitorDpiSupported => false;

		public virtual bool IsPerMonitorDpiVersion2Supported => false;

		public virtual Dpi GetDpiForSystem()
			=> this._systemDpi.Value;

		public virtual Dpi GetDpiForMonitor(IntPtr hMonitor)
			=> this._systemDpi.Value;

		public virtual Dpi GetDpiForWindow(IntPtr hWnd)
			=> GetDpi(hWnd);

		private static Dpi GetDpi(IntPtr hWnd)
		{
			IntPtr hDC = IntPtr.Zero;
			try
			{
				hDC = User32.GetWindowDC(hWnd);
				if (hDC != IntPtr.Zero)
				{
					var dpiX = Gdi32.GetDeviceCaps(hDC, DeviceCaps.LOGPIXELSX);
					var dpiY = Gdi32.GetDeviceCaps(hDC, DeviceCaps.LOGPIXELSY);
					return new Dpi((uint)dpiX, (uint)dpiY);
				}
			}
			finally
			{
				if (hDC != IntPtr.Zero)
				{
					User32.ReleaseDC(hWnd, hDC);
				}
			}

			return Dpi.Default;
		}
	}
}
