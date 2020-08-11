using System;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;

namespace MetroRadiance.Platform
{
	public static class DpiHelper
	{
		private static IDpiHelper _dpiHelper;

		static DpiHelper()
		{
			var version = Environment.OSVersion.Version;
			if (version.Major == 10)
			{
				if (version.Build >= 15002)
				{	
					if (GetIsPerMonitorDpiSupportWindows81OrAbove())
					{
						_dpiHelper = new PerMonitorAwareVersion2DpiHelper();
						return;
					}
				}
				else if (version.Build >= 14342)
				{
					if (GetIsPerMonitorDpiSupportWindows81OrAbove())
					{
						_dpiHelper = new PerMonitorAwareExtendedDpiHelper();
						return;
					}
				}
				else
				{
					if (GetIsPerMonitorDpiSupportWindows81OrAbove())
					{
						_dpiHelper = new PerMonitorAwareDpiHelper();
						return;
					}
				}
			}
			else if (version.Major == 6 && version.Minor == 3)
			{
				if (GetIsPerMonitorDpiSupportWindows81OrAbove())
				{
					_dpiHelper = new PerMonitorAwareDpiHelper();
					return;
				}
			}

			if (User32.IsProcessDPIAware())
			{
				_dpiHelper = new SystemAwareDpiHelper();
			}
			else
			{
				_dpiHelper = new UnawareDpiHelper();
			}
		}

		private static bool GetIsPerMonitorDpiSupportWindows81OrAbove()
		{
			var awareness = SHCore.GetCurrentProcessDpiAwareness();
			return awareness == ProcessDpiAwareness.PROCESS_PER_MONITOR_DPI_AWARE;
		}

		/// <summary>
		/// DPI 機能をサポートしているかどうかを示す値を取得します。
		/// </summary>
		/// <remarks>
		/// Windows Vista (NT 6.0) 以降でこの機能の使用はできます。
		/// </remarks>
		/// <returns>
		/// プロセスが DPI Aware で動作している場合は true、それ以外の場合は false。
		/// </returns>
		public static bool IsDpiAwareSupported => _dpiHelper.IsDpiAwareSupported;

		/// <summary>
		/// Per-Monitor DPI 機能をサポートしているかどうかを示す値を取得します。
		/// </summary>
		/// <remarks>
		/// Windows 8.1 (NT 6.3) 以降でこの機能の使用はできます。
		/// </remarks>
		/// <returns>
		/// プロセスが Per-monitor DPI Aware で動作している場合は true、それ以外の場合は false。
		/// </returns>
		public static bool IsPerMonitorDpiSupported => _dpiHelper.IsPerMonitorDpiSupported;

		/// <summary>
		/// Per-Monitor DPI (Version 2) 機能をサポートしているかどうかを示す値を取得します。
		/// </summary>
		/// <remarks>
		/// Windows 10 Creators Update (1703, NT 10.0.15002) 以降でこの機能の使用はできます。
		/// </remarks>
		/// <returns>
		/// プロセスが Per-monitor DPI Aware で動作しており、
		/// 動作しているオペレーティング システムが Windows 10 Creators Update の場合は true、それ以外の場合は false。
		/// </returns>
		public static bool IsPerMonitorDpiVersion2Supported => _dpiHelper.IsPerMonitorDpiVersion2Supported;
		
		/// <summary>
		/// 現在のシステムの DPI 設定値を取得します。
		/// </summary>
		/// <returns>システムの DPI 設定値。</returns>
		public static Dpi GetDpiForSystem()
			=> _dpiHelper.GetDpiForSystem();
			
		/// <summary>
		/// 現在のモニターの DPI 設定値を取得します。
		/// </summary>
		/// <returns>モニターの DPI 設定値。</returns>
		public static Dpi GetDpiForMonitor(IntPtr hMonitor)
			=> _dpiHelper.GetDpiForMonitor(hMonitor);
			
		/// <summary>
		/// 現在のウィンドウの DPI 設定値を取得します。
		/// </summary>
		/// <returns>ウィンドウの DPI 設定値。</returns>
		public static Dpi GetDpiForWindow(IntPtr hWnd)
			=> _dpiHelper.GetDpiForWindow(hWnd);
	}
}
