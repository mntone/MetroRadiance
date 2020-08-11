// ReSharper disable InconsistentNaming

namespace MetroRadiance.Interop.Win32
{
	public enum DeviceCaps : int
	{
		/// <summary>
		/// Number of pixels per logical inch along the screen width.
		/// In a system with multiple display monitors, this value is the same for all monitors.
		/// </summary>
		LOGPIXELSX = 88,

		/// <summary>
		/// Number of pixels per logical inch along the screen height.
		/// In a system with multiple display monitors, this value is the same for all monitors.
		/// </summary>
		LOGPIXELSY = 90,
	}
}
