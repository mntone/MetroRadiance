﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetroRadiance.UI.Controls
{
	/// <summary>
	/// 未入力時にプロンプトを表示できる <see cref="TextBox"/> を表します。
	/// </summary>
	[TemplateVisualState(Name = "Empty", GroupName = "TextStates")]
	[TemplateVisualState(Name = "NotEmpty", GroupName = "TextStates")]
	public class PromptTextBox : TextBox
	{
		static PromptTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptTextBox), new FrameworkPropertyMetadata(typeof(PromptTextBox)));
		}

		public PromptTextBox()
		{
			this.UpdateTextStates(true);
			this.TextChanged += (sender, e) => this.UpdateTextStates(true);
			this.GotKeyboardFocus += (sender, e) => this.UpdateTextStates(true);
		}

		#region Prompt dependency property

		public string Prompt
		{
			get { return (string)this.GetValue(PromptProperty); }
			set { this.SetValue(PromptProperty, value); }
		}
		public static readonly DependencyProperty PromptProperty =
			DependencyProperty.Register(nameof(Prompt), typeof(string), typeof(PromptTextBox), new UIPropertyMetadata("", PromptChangedCallback));

		private static void PromptChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		#endregion

		#region PromptBrush dependency property

		public Brush PromptBrush
		{
			get { return (Brush)this.GetValue(PromptBrushProperty); }
			set { this.SetValue(PromptBrushProperty, value); }
		}
		public static readonly DependencyProperty PromptBrushProperty =
			DependencyProperty.Register(nameof(PromptBrush), typeof(Brush), typeof(PromptTextBox), new UIPropertyMetadata(Brushes.Gray));

		#endregion


		private void UpdateTextStates(bool useTransitions)
		{
			VisualStateManager.GoToState(this, string.IsNullOrEmpty(this.Text) ? "Empty" : "NotEmpty", useTransitions);
		}
	}
}
