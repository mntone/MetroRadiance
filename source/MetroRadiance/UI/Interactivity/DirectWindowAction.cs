using System;
using System.Linq;
using System.Windows;
using MetroRadiance.UI.Controls;
using MetroRadiance.Utilities;
using Microsoft.Xaml.Behaviors;

namespace MetroRadiance.UI.Interactivity
{
	internal class DirectWindowAction : TriggerAction<FrameworkElement>
	{
		#region WindowAction dependency property

		public WindowAction WindowAction
		{
			get { return (WindowAction)this.GetValue(WindowActionProperty); }
			set { this.SetValue(WindowActionProperty, value); }
		}

		public static readonly DependencyProperty WindowActionProperty =
			DependencyProperty.Register(nameof(WindowAction), typeof(WindowAction), typeof(DirectWindowAction), new UIPropertyMetadata(WindowAction.Active));

		#endregion

		protected override void Invoke(object parameter)
		{
			this.WindowAction.Invoke(this.AssociatedObject);
		}
	}
}
