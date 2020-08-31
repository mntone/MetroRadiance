using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MetroRadiance.Interop.Win32;

namespace MetroRadiance.UI.Controls
{
	/// <summary>
	/// ウィンドウをリサイズするためのグリップ コントロールを表します。
	/// </summary>
	public class ResizeGrip : ContentControl
	{
		static ResizeGrip()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(typeof(ResizeGrip)));
		}

		private bool _canResize;
		private bool _isInitialized;

		public ResizeGrip()
		{
			this.Loaded += this.Initialize;
		}

		private void Initialize(object sender, RoutedEventArgs args)
		{
			if (this._isInitialized) return;

			var window = Window.GetWindow(this);
			if (window == null) return;

			var source = (HwndSource)PresentationSource.FromVisual(window);
			if (source != null) source.AddHook(this.WndProc);

			window.StateChanged += (_, __) => this._canResize = window.WindowState == WindowState.Normal;
			window.ContentRendered += (_, __) => this._canResize = window.WindowState == WindowState.Normal;

			this._isInitialized = true;
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WindowsMessages.WM_NCHITTEST && this._canResize)
			{
				var ptScreen = lParam.ToPoint();
				var ptClient = this.PointFromScreen(ptScreen);

				var rectTarget = new Rect(0, 0, this.ActualWidth, this.ActualHeight);

				if (rectTarget.Contains(ptClient))
				{
					handled = true;
					return (IntPtr)HitTestValues.HTBOTTOMRIGHT;
				}
			}

			return IntPtr.Zero;
		}
	}
}
