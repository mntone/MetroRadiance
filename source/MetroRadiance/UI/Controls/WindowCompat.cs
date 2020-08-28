using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;
using MetroRadiance.Platform;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace MetroRadiance.UI.Controls
{
	public class WindowCompat : Window
	{
#if NET45 || NET451 || NET452 || NET46 || NET461
		public static bool HasBuiltInScaling => false;
#else
		public static bool HasBuiltInScaling => WindowsVersion.Is10RS1OrGreater;
#endif

		private HwndSource _source;

		/// <summary>
		/// システムの現在の DPI (プライマリ モニターの DPI)。
		/// </summary>
		public Dpi SystemDpi { get; private set; }

		/// <summary>
		/// このウィンドウが表示されているモニターの現在の DPI。
		/// </summary>
		public Dpi WindowDpi { get; private set; }

		#region DpiScaleTransform 依存関係プロパティ

		/// <summary>
		/// DPI スケーリングを実現する <see cref="Transform" /> を取得または設定します。
		/// </summary>
		public Transform DpiScaleTransform
		{
			get { return (Transform)this.GetValue(DpiScaleTransformProperty); }
			set { this.SetValue(DpiScaleTransformProperty, value); }
		}

		public static readonly DependencyProperty DpiScaleTransformProperty =
			DependencyProperty.Register(nameof(DpiScaleTransform), typeof(Transform), typeof(WindowCompat), new UIPropertyMetadata(Transform.Identity));

		#endregion

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			if (HasBuiltInScaling)
			{
				this.SystemDpi = Dpi.FromVisual(this);
			}
			else
			{
				this._source = (HwndSource)PresentationSource.FromVisual(this);
				if (this._source == null) return;
				this._source.AddHook(this.WndProc);

				this.SystemDpi = Dpi.FromPresentationSource(this._source);
			}
			this.WindowDpi = this.SystemDpi;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			var source = this._source;
			if (source != null)
			{
				source.RemoveHook(this.WndProc);
				this._source = null;
			}
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WindowsMessages.WM_DPICHANGED)
			{
				var dpiX = wParam.ToLoWord();
				var dpiY = wParam.ToHiWord();
				var rect = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				this.ChangeDpi(hwnd, new Dpi(dpiX, dpiY), rect);
				handled = true;
			}

			return IntPtr.Zero;
		}

		internal protected virtual void ChangeDpi(IntPtr hWnd, Dpi dpi, RECT rect)
		{
			this.DpiScaleTransform = dpi == this.SystemDpi
				? Transform.Identity
				: new ScaleTransform((double)dpi.X / this.SystemDpi.X, (double)dpi.Y / this.SystemDpi.Y);

			User32.SetWindowPos(
				hWnd,
				IntPtr.Zero,
				rect.Left,
				rect.Top,
				rect.Right - rect.Left,
				rect.Bottom - rect.Top,
				SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOOWNERZORDER);

			var oldDpi = this.WindowDpi;
			this.WindowDpi = dpi;
			this.OnDpiChanged(oldDpi, dpi);
		}

#if !(NET45 || NET451 || NET452 || NET46 || NET461)
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);

			var newDpi2 = new Dpi(newDpi);
			this.WindowDpi = newDpi2;
			this.OnDpiChanged(new Dpi(oldDpi), newDpi2);
		}
#endif

		protected virtual void OnDpiChanged(Dpi oldDpi, Dpi newDpi)
		{ }

		public static WindowCompat GetWindowCompat(DependencyObject dependencyObject)
			=> GetWindow(dependencyObject) as WindowCompat;
	}
}
