using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;
using MetroRadiance.Utilities;

namespace MetroRadiance.Platform
{
	public abstract class WindowsThemeValue
	{
		internal protected static ListenerWindow ListenerWindowTarget { get; } = new ListenerWindow();


		protected abstract IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

		internal protected class ListenerWindow : TransparentWindow
		{
			private readonly Collection<HwndSourceHook> _hooks = new Collection<HwndSourceHook>();

			public ListenerWindow()
			{
				this.Name = "Windows theme listener window";
			}

			public void Add(HwndSourceHook hook)
			{
				lock (this._hooks)
				{
					if (this._hooks.Count == 0)
					{
						ListenerWindowTarget.Show();
					}
					this._hooks.Add(hook);
				}
			}

			public void Remove(HwndSourceHook hook)
			{
				lock (this._hooks)
				{
					this._hooks.Remove(hook);
					if (this._hooks.Count == 0)
					{
						ListenerWindowTarget.Close();
					}
				}
			}

			protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
			{
				lock (this._hooks)
				{
					foreach (var hook in this._hooks)
					{
						hook(hwnd, msg, wParam, lParam, ref handled);
					}
				}
				return handled ? IntPtr.Zero : base.WndProc(hwnd, msg, wParam, lParam, ref handled);
			}
		}
	}

	public abstract class WindowsThemeValue<T> : WindowsThemeValue
	{
		private event EventHandler<T> _changedEvent;
		private readonly HashSet<EventHandler<T>> _handlers = new HashSet<EventHandler<T>>();
		private T _current;
		private bool _hasCache;

		private bool RequireCallGetValue => !this._hasCache;

		/// <summary>
		/// 現在の設定値を取得します。
		/// </summary>
		public T Current
		{
			get
			{
				if (this.RequireCallGetValue)
				{
					this._current = this.GetValue();
					this._hasCache = true;
				}

				return this._current;
			}
			set
			{
				this._current = value;
				this._hasCache = true;
			}
		}

		/// <summary>
		/// テーマ設定が変更されると発生します。
		/// </summary>
		public event EventHandler<T> Changed
		{
			add { this.Add(value); }
			remove { this.Remove(value); }
		}

		/// <summary>
		/// テーマ設定が変更されたときに通知を受け取るメソッドを登録します。
		/// </summary>
		/// <param name="callback">テーマ設定が変更されたときに通知を受け取るメソッド。</param>
		/// <returns>通知の購読を解除するときに使用する <see cref="IDisposable"/> オブジェクト。</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IDisposable RegisterListener(Action<T> callback)
		{
			EventHandler<T> handler = (sender, e) => callback?.Invoke(e);
			this.Changed += handler;

			return Disposable.Create(() => this.Changed -= handler);
		}

		private void Add(EventHandler<T> listener)
		{
			if (this._handlers.Add(listener))
			{
				this._changedEvent += listener;

				if (this._handlers.Count == 1)
				{
					ListenerWindowTarget.Add(this.WndProc);
				}
			}
		}

		private void Remove(EventHandler<T> listener)
		{
			if (this._handlers.Remove(listener))
			{
				this._changedEvent -= listener;

				if (this._handlers.Count == 0)
				{
					ListenerWindowTarget.Remove(this.WndProc);
					this._hasCache = false;
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void Update(T data)
		{
			this.Current = data;
			this._changedEvent?.Invoke(this, data);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected abstract T GetValue();
	}
}
