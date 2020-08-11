using System;
using MetroRadiance.Interop;

namespace MetroRadiance.Platform
{
	internal sealed class UnawareDpiHelper : IDpiHelper
	{
		public bool IsDpiAwareSupported => false;

		public bool IsPerMonitorDpiSupported => false;

		public bool IsPerMonitorDpiVersion2Supported => false;

		public Dpi GetDpiForSystem()
			=> Dpi.Default;

		public Dpi GetDpiForMonitor(IntPtr hMonitor)
			=> Dpi.Default;

		public Dpi GetDpiForWindow(IntPtr hWnd)
			=> Dpi.Default;
	}
}
