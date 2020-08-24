using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;
using ShellChrome = System.Windows.Shell.WindowChrome;
using MetroChrome = MetroRadiance.Chrome.WindowChrome;

namespace MetroRadiance.UI.Controls
{
	/// <summary>
	/// Metro スタイル風のウィンドウを表します。
	/// </summary>
	[TemplatePart(Name = PART_ResizeGrip, Type = typeof(FrameworkElement))]
	public class MetroWindow : WindowCompat
	{
#pragma warning disable IDE1006
		private const string PART_ResizeGrip = "PART_ResizeGrip";
#pragma warning restore IDE1006

		static MetroWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
		}

		private HwndSource _source;
		private FrameworkElement _resizeGrip;
		private FrameworkElement _captionBar;

		#region ShellChrome dependency property

		public static readonly DependencyProperty ShellChromeProperty = DependencyProperty.Register(
			nameof(ShellChrome), typeof(ShellChrome), typeof(MetroWindow), new PropertyMetadata(null, HandleShellChromeChanged));

		public ShellChrome ShellChrome
		{
			get { return (ShellChrome)this.GetValue(ShellChromeProperty); }
			set { this.SetValue(ShellChromeProperty, value); }
		}

		private static void HandleShellChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			var chrome = (ShellChrome)args.NewValue;
			var window = (Window)d;

			ShellChrome.SetWindowChrome(window, chrome);
		}

		#endregion

		#region IsRestoringWindowPlacement dependency property

		/// <summary>
		/// ウィンドウの位置とサイズを復元できるようにするかどうかを示す値を取得または設定します。
		/// </summary>
		public bool IsRestoringWindowPlacement
		{
			get { return (bool)this.GetValue(IsRestoringWindowPlacementProperty); }
			set { this.SetValue(IsRestoringWindowPlacementProperty, value); }
		}
		public static readonly DependencyProperty IsRestoringWindowPlacementProperty =
			DependencyProperty.Register(nameof(IsRestoringWindowPlacement), typeof(bool), typeof(MetroWindow), new UIPropertyMetadata(false));

		#endregion

		#region WindowSettings dependency property

		/// <summary>
		/// ウィンドウの位置とサイズを保存または復元する方法を指定するオブジェクトを取得または設定します。
		/// </summary>
		public IWindowSettings WindowSettings
		{
			get { return (IWindowSettings)this.GetValue(WindowSettingsProperty); }
			set { this.SetValue(WindowSettingsProperty, value); }
		}
		public static readonly DependencyProperty WindowSettingsProperty =
			DependencyProperty.Register(nameof(WindowSettings), typeof(IWindowSettings), typeof(MetroWindow), new UIPropertyMetadata(null));

		#endregion

		#region IsCaptionBar attached property

		public static readonly DependencyProperty IsCaptionBarProperty =
			DependencyProperty.RegisterAttached("IsCaptionBar", typeof(bool), typeof(MetroWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, IsCaptionBarChangedCallback));

		public static void SetIsCaptionBar(FrameworkElement element, bool value)
		{
			element.SetValue(IsCaptionBarProperty, value);
		}
		public static bool GetIsCaptionBar(FrameworkElement element)
		{
			return (bool)element.GetValue(IsCaptionBarProperty);
		}

		private static void IsCaptionBarChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement instance)) return;
			if (!(GetWindow(instance) is MetroWindow window)) return;

			window._captionBar = (bool)e.NewValue ? instance : null;

			instance.Loaded += (sender, args) =>
			{
				window.UpdateIsCaptionBarHeight();
			};
		}

		private void UpdateIsCaptionBarHeight()
		{
			var chrome = ShellChrome.GetWindowChrome(this);
			if (chrome == null) return;

			var captionBar = this._captionBar;
			if (captionBar != null)
			{
				if (this.SystemDpi.Y > 0)
				{
					chrome.CaptionHeight = captionBar.ActualHeight * this.DpiScaleTransform.Value.M22;
				}
				else
				{
					chrome.CaptionHeight = captionBar.ActualHeight;
				}
			}
			else
			{
				chrome.CaptionHeight = 0;
			}
		}

		#endregion

		public MetroWindow()
		{
			var metroChrome = new MetroChrome();
			MetroChrome.SetInstance(this, metroChrome);
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			this._source = PresentationSource.FromVisual(this) as HwndSource;
			if (this._source == null) return;
			this._source.AddHook(this.WndProc);

			if (this.WindowSettings == null)
			{
				this.WindowSettings = new WindowSettings(this);
			}
			if (this.IsRestoringWindowPlacement)
			{
				this.WindowSettings.Reload();

				if (this.WindowSettings.Placement.HasValue)
				{
					var placement = this.WindowSettings.Placement.Value;
					placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
					placement.flags = 0;
					placement.showCmd = placement.showCmd == ShowWindowFlags.SW_SHOWMINIMIZED ? ShowWindowFlags.SW_SHOWNORMAL : placement.showCmd;

					User32.SetWindowPlacement(this._source.Handle, ref placement);
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this._resizeGrip = this.GetTemplateChild(PART_ResizeGrip) as FrameworkElement;
			if (this._resizeGrip != null)
			{
				this._resizeGrip.Visibility = this.ResizeMode == ResizeMode.CanResizeWithGrip
					? Visibility.Visible
					: Visibility.Collapsed;

				ShellChrome.SetIsHitTestVisibleInChrome(this._resizeGrip, true);
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._captionBar != null) this._captionBar.Opacity = 1.0;
		}

		protected override void OnDeactivated(EventArgs e)
		{
			base.OnDeactivated(e);
			if (this._captionBar != null) this._captionBar.Opacity = 0.5;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!e.Cancel && this.WindowSettings != null)
			{
				var hwnd = new WindowInteropHelper(this).Handle;
				var placement = User32.GetWindowPlacement(hwnd);

				this.WindowSettings.Placement = this.IsRestoringWindowPlacement ? (WINDOWPLACEMENT?)placement : null;
				this.WindowSettings.Save();
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this._source?.RemoveHook(this.WndProc);
		}


		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WindowsMessages.WM_NCHITTEST)
			{
				if (this.ResizeMode == ResizeMode.CanResizeWithGrip
					&& this.WindowState == WindowState.Normal
					&& this._resizeGrip != null)
				{
					var ptScreen = lParam.ToPoint();
					var ptClient = this._resizeGrip.PointFromScreen(ptScreen);

					var rectTarget = new Rect(0, 0, this._resizeGrip.ActualWidth, this._resizeGrip.ActualHeight);
					if (rectTarget.Contains(ptClient))
					{
						handled = true;
						return (IntPtr)HitTestValues.HTBOTTOMRIGHT;
					}
				}
			}
			else if (msg == (int)WindowsMessages.WM_NCCALCSIZE)
			{
				if (wParam != IntPtr.Zero)
				{
					var result = this.CalcNonClientSize(hwnd, lParam, ref handled);
					if (handled) return result;
				}
			}

			return IntPtr.Zero;
		}

		private IntPtr CalcNonClientSize(IntPtr hWnd, IntPtr lParam, ref bool handled)
		{
			if (!User32.IsZoomed(hWnd)) return IntPtr.Zero;

			var rcsize = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(lParam, typeof(NCCALCSIZE_PARAMS));
			if (rcsize.lppos.flags.HasFlag(SetWindowPosFlags.SWP_NOSIZE)) return IntPtr.Zero;

			var hMonitor = User32.MonitorFromWindow(hWnd, MonitorDefaultTo.MONITOR_DEFAULTTONEAREST);
			if (hMonitor == IntPtr.Zero) return IntPtr.Zero;

			var monitorInfo = new MONITORINFO()
			{
				cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO))
			};
			if (!User32.GetMonitorInfo(hMonitor, ref monitorInfo)) return IntPtr.Zero;

			var workArea = monitorInfo.rcWork;
			AppBar.ApplyAppbarSpace(monitorInfo.rcMonitor, ref workArea);

			rcsize.rgrc[0] = workArea;
			rcsize.rgrc[1] = workArea;
			Marshal.StructureToPtr(rcsize, lParam, true);
			handled = true;
			return (IntPtr)(WindowValidRects.WVR_ALIGNTOP | WindowValidRects.WVR_ALIGNLEFT | WindowValidRects.WVR_VALIDRECTS);
		}

		protected override void OnDpiChanged(Dpi oldDpi, Dpi newDpi)
			=> this.UpdateIsCaptionBarHeight();
	}
}
