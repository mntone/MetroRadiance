using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MetroRadiance.Utilities;

namespace MetroRadiance.UI.Controls
{
	/// <summary>
	/// ウィンドウのキャプション部分で使用するために最適化された <see cref="Button"/> コントロールを表します。
	/// </summary>
	public class CaptionButton : Button
	{
		static CaptionButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));
		}

		private Window _owner;

		#region WindowAction dependency property

		/// <summary>
		/// ボタンに割り当てるウィンドウ操作を取得または設定します。
		/// </summary>
		public WindowAction WindowAction
		{
			get { return (WindowAction)this.GetValue(WindowActionProperty); }
			set { this.SetValue(WindowActionProperty, value); }
		}
		public static readonly DependencyProperty WindowActionProperty =
			DependencyProperty.Register(nameof(WindowAction), typeof(WindowAction), typeof(CaptionButton), new UIPropertyMetadata(WindowAction.None));

		#endregion

		#region Mode dependency property

		public CaptionButtonMode Mode
		{
			get { return (CaptionButtonMode)this.GetValue(ModeProperty); }
			set { this.SetValue(ModeProperty, value); }
		}
		public static readonly DependencyProperty ModeProperty =
			DependencyProperty.Register(nameof(Mode), typeof(CaptionButtonMode), typeof(CaptionButton), new UIPropertyMetadata(CaptionButtonMode.Normal));

		#endregion

		#region IsChecked dependency property

		public bool IsChecked
		{
			get { return (bool)this.GetValue(IsCheckedProperty); }
			set { this.SetValue(IsCheckedProperty, value); }
		}
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(CaptionButton), new UIPropertyMetadata(false));

		#endregion

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			this._owner = Window.GetWindow(this);
			if (this._owner != null)
			{
				this._owner.StateChanged += (sender, args) => this.ChangeVisibility();
				this.ChangeVisibility();
			}
		}

		protected override void OnClick()
		{
			this.WindowAction.Invoke(this);

			if (this.Mode == CaptionButtonMode.Toggle) this.IsChecked = !this.IsChecked;

			base.OnClick();
		}

		private void ChangeVisibility()
		{
			switch (this.WindowAction)
			{
				case WindowAction.Maximize:
					this.Visibility = this._owner.WindowState != WindowState.Maximized ? Visibility.Visible : Visibility.Collapsed;
					break;
				case WindowAction.Minimize:
					this.Visibility = this._owner.WindowState != WindowState.Minimized ? Visibility.Visible : Visibility.Collapsed;
					break;
				case WindowAction.Normalize:
					this.Visibility = this._owner.WindowState != WindowState.Normal ? Visibility.Visible : Visibility.Collapsed;
					break;
			}
		}
	}
}
