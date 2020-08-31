using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MetroRadiance.UI.Controls
{
	public class LinkButton : Button
	{
		static LinkButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkButton), new FrameworkPropertyMetadata(typeof(LinkButton)));
		}

		#region Text dependency property

		public string Text
		{
			get { return (string)this.GetValue(TextProperty); }
			set { this.SetValue(TextProperty, value); }
		}
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(nameof(Text), typeof(string), typeof(LinkButton), new UIPropertyMetadata(""));

		#endregion

		#region TextTrimming dependency property

		public TextTrimming TextTrimming
		{
			get { return (TextTrimming)this.GetValue(TextTrimmingProperty); }
			set { this.SetValue(TextTrimmingProperty, value); }
		}
		public static readonly DependencyProperty TextTrimmingProperty =
			DependencyProperty.Register(nameof(TextTrimming), typeof(TextTrimming), typeof(LinkButton), new UIPropertyMetadata(TextTrimming.CharacterEllipsis));

		#endregion

		#region TextWrapping dependency property

		public TextWrapping TextWrapping
		{
			get { return (TextWrapping)this.GetValue(TextWrappingProperty); }
			set { this.SetValue(TextWrappingProperty, value); }
		}
		public static readonly DependencyProperty TextWrappingProperty =
			DependencyProperty.Register(nameof(TextWrapping), typeof(TextWrapping), typeof(LinkButton), new UIPropertyMetadata(TextWrapping.NoWrap));

		#endregion

	}
}
