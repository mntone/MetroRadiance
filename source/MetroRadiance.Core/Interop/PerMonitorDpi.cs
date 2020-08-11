using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;
using MetroRadiance.Platform;

namespace MetroRadiance.Interop
{
	/// <summary>
	/// Windows 8.1 の Per-Monitor DPI 機能へアクセスできるようにします。
	/// </summary>
	public static class PerMonitorDpi
	{
		/// <summary>
		/// Per-Monitor DPI 機能をサポートしているかどうかを示す値を取得します。
		/// </summary>
		/// <returns>
		/// マニフェストでPer-monitor DPIが定義されており、
		/// 動作しているオペレーティング システムが Windows 8.1 (NT 6.3)、もしくは Windows 10 (10.0.x) の場合は true、それ以外の場合は false。
		/// </returns>
		[Obsolete("MetroRadiance.Platform.DpiHelper.IsPerMonitorDpiSupported を使用してください")]
		public static bool IsSupported => DpiHelper.IsPerMonitorDpiSupported;

		/// <summary>
		/// 現在の <see cref="HwndSource"/> が描画されているモニターの DPI 設定値を取得します。
		/// </summary>
		/// <param name="hwndSource">DPI を取得する対象の Win32 ウィンドウを特定する <see cref="HwndSource"/> オブジェクト。</param>
		/// <returns><paramref name="hwndSource"/> が描画されているモニターの DPI 設定値。サポートされていないシステムの場合はシステムの DPI 設定値。</returns>
		public static Dpi GetDpi(this HwndSource hwndSource)
		{
			return DpiHelper.GetDpiForWindow(hwndSource.Handle);
		}

		/// <summary>
		/// 指定したハンドルのウィンドウが描画されているモニターの DPI 設定値を取得します。
		/// </summary>
		/// <param name="hWnd">DPI を取得する対象の Win32 ウィンドウを示すウィンドウ ハンドル。</param>
		/// <returns><paramref name="hWnd"/> のウィンドウが描画されているモニターの DPI 設定値。サポートされていないシステムの場合は <see cref="Dpi.Default"/>。</returns>
		[Obsolete("MetroRadiance.Platform.DpiHelper.GetDpiForWindow(IntPtr) を使用してください")]
		public static Dpi GetDpi(IntPtr hWnd)
		{
			if (!IsSupported) return Dpi.Default;

			return DpiHelper.GetDpiForWindow(hWnd);
		}
	}
}
