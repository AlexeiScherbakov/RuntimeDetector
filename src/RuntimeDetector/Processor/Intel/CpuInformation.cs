using System;

using RuntimeDetector.Interop;

namespace RuntimeDetector.Processor.Intel
{
	/// <summary>
	/// 
	/// </summary>
	/// <see href="https://msdn.microsoft.com/en-us/library/hskdteyh.aspx"/>
	public static class CpuInformation
	{
		private static ActiveProcessorFeatures _activeProcessorFeatures;
		private static CpuIdAssemblyCode _asmCode;

		private static readonly long _osXStateFeatures;

		static CpuInformation()
		{
			if (IntPtr.Size == 4)
			{

			}
			else
			{
				//64bit mode
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse;
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse2;
			}

			_osXStateFeatures = NativeMethods.Kernel32.GetEnabledXStateFeatures();

			_asmCode = new CpuIdAssemblyCode();
			CpuIdInfo info = new CpuIdInfo();
			_asmCode.Call(1, ref info);
			Function1(ref info);
			_asmCode.Call(7, ref info);
			Function7(ref info);
		}


		private static void Function1(ref CpuIdInfo cpuInfo)
		{
			if ((cpuInfo.Ecx | (1 << 23)) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Popcnt;
			}
			if ((cpuInfo.Ecx | (1 << 28)) != 0)
			{
				if ((_osXStateFeatures & 4) != 0)
				{
					_activeProcessorFeatures |= ActiveProcessorFeatures.Avx;
				}
			}
		}

		private static void Function7(ref CpuIdInfo cpuInfo)
		{
			if ((cpuInfo.Ebx | (1 << 5)) != 0)
			{
				if ((_osXStateFeatures & 4) != 0)
				{
					_activeProcessorFeatures |= ActiveProcessorFeatures.Avx2;
				}
			}
		}

		public static bool HasPopcnt
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Popcnt); }
		}

		public static bool HasAvx
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Avx); }
		}

		public static bool HasAvx2
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Avx2); }
		}

		
	}
}
