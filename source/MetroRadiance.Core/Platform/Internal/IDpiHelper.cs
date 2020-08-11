using System;
using MetroRadiance.Interop;

namespace MetroRadiance.Platform
{
	internal interface IDpiHelper
	{
		bool IsDpiAwareSupported { get; }

		bool IsPerMonitorDpiSupported { get; }
		
		bool IsPerMonitorDpiVersion2Supported { get; }

		Dpi GetDpiForSystem();

		Dpi GetDpiForMonitor(IntPtr hMonitor);

		Dpi GetDpiForWindow(IntPtr hWnd);
	}
}
