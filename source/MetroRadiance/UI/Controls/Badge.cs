using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MetroRadiance.UI.Controls
{
	[TemplatePart(Name = PART_CountHost, Type = typeof(TextBlock))]
	public class Badge : Control
	{
#pragma warning disable IDE1006
		private const string PART_CountHost = "PART_CountHost";
#pragma warning restore IDE1006

		static Badge()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));
		}

		private TextBlock _block;
		private double _initialSize;

		#region Count dependency property

		public int? Count
		{
			get { return (int?)this.GetValue(CountProperty); }
			set { this.SetValue(CountProperty, value); }
		}
		public static readonly DependencyProperty CountProperty =
			DependencyProperty.Register(nameof(Count), typeof(int?), typeof(Badge), new UIPropertyMetadata(null, CountPropertyChangedCallback));

		private static void CountPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (Badge)d;
			source.SetCount((int?)e.NewValue);
		}

		#endregion

		public Badge()
		{
			this.SetCount(null);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this._block = this.GetTemplateChild(PART_CountHost) as TextBlock;
			if (this._block != null)
			{
				this._initialSize = this._block.FontSize;
				this.SetCount(this.Count);
			}
		}

		private void SetCount(int? count)
		{
			if (count.HasValue)
			{
				if (this._block != null)
				{
					this._block.Text = count.Value.ToString(CultureInfo.InvariantCulture);
					this._block.FontSize = count.Value >= 10 ? this._initialSize - 1 : this._initialSize;
				}
				this.Visibility = Visibility.Visible;
			}
			else
			{
				this.Visibility = Visibility.Hidden;
			}
		}
	}
}
