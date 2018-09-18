namespace RuntimeDetector.Interop
{
	internal static class FunctionNames
	{
		public static class Kernel32
		{
			public const string FreeLibrary = "FreeLibrary";
			public const string GetEnabledXStateFeatures = "GetEnabledXStateFeatures";
			public const string GetModuleHandle = "GetModuleHandle";
			public const string GetProcAddress = "GetProcAddress";
			public const string LoadLibrary = "LoadLibrary";
			public const string VirtualAlloc = "VirtualAlloc";
			public const string VirtualFree = "VirtualFree";
			public const string GlobalMemoryStatusEx = "GlobalMemoryStatusEx";
		}

		public static class NvCuda
		{
			public const string DeviceComputeCapability = "cuDeviceComputeCapability";
			public const string DeviceGetCount = "cuDeviceGetCount";
			public const string Init = "cuInit";
			public const string DriverGetVersion = "cuDriverGetVersion";
		}
	}
}
