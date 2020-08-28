using System;

namespace MetroRadiance.Platform
{
	public static class WindowsVersion
	{
		static WindowsVersion()
		{
			var version = Environment.OSVersion.Version;
			OSVersion = version;
			switch (version.Major)
			{
				case 10: // Windows 10
					IsVista = false;
					Is7 = false;
					Is8 = false;
					Is8Point1 = false;
					Is10 = true;
					break;

				case 6:
					switch (version.Minor)
					{
						case 0: // Windows Vista
							IsVista = true;
							Is7 = false;
							Is8 = false;
							Is8Point1 = false;
							break;

						case 1: // Windows 7
							IsVista = false;
							Is7 = true;
							Is8 = false;
							Is8Point1 = false;
							break;

						case 2: // Windows 8
							IsVista = false;
							Is7 = false;
							Is8 = true;
							Is8Point1 = false;
							break;

						case 3: // Windows 8.1
							IsVista = false;
							Is7 = false;
							Is8 = false;
							Is8Point1 = true;
							break;

						default: // Unknown OS
							IsVista = false;
							Is7 = false;
							Is8 = false;
							Is8Point1 = false;
							break;
					}
					Is10 = false;
					break;

				default:
					IsVista = false;
					Is7 = false;
					Is8 = false;
					Is8Point1 = false;
					Is10 = false;
					break;
			}
		}

		public static Version OSVersion { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows Vista (NT 6.0) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool IsVista { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows Vista (NT 6.0.6001) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool IsVistaSP1 => IsVista && OSVersion.Build == 6001;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows Vista (NT 6.0.6002) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool IsVistaSP2 => IsVista && OSVersion.Build == 6002;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows Vista (NT 6.0.6001) または Windows Vista (NT 6.0.6002) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool IsVistaSP1OrSP2 => IsVistaSP1 || IsVistaSP2;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 7 (NT 6.1) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is7 { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 7 Service Pack 1 (NT 6.1.7601) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is7SP1 => Is7 && OSVersion.Build == 7601;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows Vista (NT 6.0) または Windows 7 (NT 6.1) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool IsVistaOr7 => IsVista || Is7;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 8 (NT 6.2) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is8 { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 8.1 (NT 6.3) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is8Point1 { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 8 (NT 6.2) または Windows 8.1 (NT 6.3) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is8Or8Point1 => Is8 || Is8Point1;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 (NT 10.0) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10 { get; }

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 8.1 (NT 6.3) または Windows 10 (NT 10.0) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is8Point1OrGreater => Is8Point1 || Is10;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 8 (NT 6.2)、Windows 8.1 (NT 6.3) または Windows 10 (NT 10.0) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is8OrGreater => Is8 || Is8Point1 || Is10;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Threshold 1 [1507] (NT 10.0.10240) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10TH1 => Is10 && OSVersion.Build == 10240;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Threshold 1 [1507] (NT 10.0.10240) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10TH1OrGreater => Is10 && OSVersion.Build >= 10240;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Threshold 2 [1511] (NT 10.0.10586) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10TH2 => Is10 && OSVersion.Build == 10586;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Threshold 2 [1511] (NT 10.0.10586) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10TH2OrGreater => Is10 && OSVersion.Build >= 10586;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 1 [1607] (NT 10.0.14393) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS1 => Is10 && OSVersion.Build == 14393;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 1 [1607] (NT 10.0.14393) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS1OrGreater => Is10 && OSVersion.Build >= 14393;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 2 [1703] (NT 10.0.15063) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS2 => Is10 && OSVersion.Build == 15063;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 2 [1703] (NT 10.0.15063) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS2OrGreater => Is10 && OSVersion.Build >= 15063;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 3 [1709] (NT 10.0.15063) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS3 => Is10 && OSVersion.Build == 16299;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 3 [1709] (NT 10.0.15063) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS3OrGreater => Is10 && OSVersion.Build >= 16299;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 4 [1803] (NT 10.0.17134) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS4 => Is10 && OSVersion.Build == 17134;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 4 [1803] (NT 10.0.17134) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS4OrGreater => Is10 && OSVersion.Build >= 17134;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 5 [1809] (NT 10.0.17763) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS5 => Is10 && OSVersion.Build == 17763;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 Redstone 5 [1809] (NT 10.0.17763) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10RS5OrGreater => Is10 && OSVersion.Build >= 17763;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 19H1 [1903] (NT 10.0.18362) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_19H1 => Is10 && OSVersion.Build == 18362;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 19H1 [1903] (NT 10.0.18362) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_19H1OrGreater => Is10 && OSVersion.Build >= 18362;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 19H2 [1909] (NT 10.0.18363) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_19H2 => Is10 && OSVersion.Build == 18363;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 19H2 [1909] (NT 10.0.18363) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_19H2OrGreater => Is10 && OSVersion.Build >= 18363;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 20H1 [2004] (NT 10.0.19041) の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_20H1 => Is10 && OSVersion.Build == 19041;

		/// <summary>
		/// 動作しているオペレーティング システムが Windows 10 20H1 [2004] (NT 10.0.19041) 以降の場合は true、それ以外の場合は false。
		/// </summary>
		public static bool Is10_20H1OrGreater => Is10 && OSVersion.Build >= 19041;
	}
}
