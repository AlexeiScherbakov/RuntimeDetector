using System;
using System.IO;
using RuntimeDetector.Interop;

namespace RuntimeDetector.Runtime
{
	public static class OperationSystem
	{
		private static Platform _platform;

		static OperationSystem()
		{
			// OS Detection
			var windir = Environment.GetEnvironmentVariable("windir");

			if ((!string.IsNullOrWhiteSpace(windir)) && windir.Contains(@"\") && Directory.Exists(windir))
			{
				_platform = Platform.Windows;
			}
			else if (File.Exists("/proc/sys/kernel/ostype"))
			{
				string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
				if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
				{
					_platform = Platform.Linux;
				}
			}
			else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
			{
				_platform = Platform.MacOS;
			}

		}

		public static Platform Platform
		{
			get { return _platform; }
		}


		public static long FreePhysicalMemory
		{
			get
			{
				switch (_platform)
				{
					case Platform.Windows:
						return (long) WindowsGetFreePhysicalMemory();
				}
				return -1;
			}
		}

		private static ulong WindowsGetFreePhysicalMemory()
		{
			MemoryStatusEx memoryStatus = new MemoryStatusEx();
			memoryStatus.Length = 64;
			bool ok = NativeMethods.Kernel32.GlobalMemoryStatusEx(ref memoryStatus);
			return memoryStatus.AvailPhys;
		}
	}
}
