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
		private static Manufacturer _manufacturer;
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

			_asmCode.Call(0, ref info);
			_manufacturer = Function0(ref info);
			_asmCode.Call(1, ref info);
			Function1(ref info);
			_asmCode.Call(7, ref info);
			Function7(ref info);
		}


		private static Manufacturer Function0(ref CpuIdInfo cpuInfo)
		{
			// Intel - 'GenuineIntel'
			//ebx - 0x756e6547 'uneG'
			//edx - 0x49656e69 'Ieni'
			//ecx - 0x6c65746e 'letn'

			switch (cpuInfo.Ebx)
			{
				case 0x756e6547:
					if ((cpuInfo.Edx == 0x49656e69) && (cpuInfo.Ecx == 0x6c65746e))
					{
						return Manufacturer.Intel;
					}
					break;
			}
			return Manufacturer.Unknown;
		}

		private static void Function1(ref CpuIdInfo cpuInfo)
		{
			#region Ecx
			if ((cpuInfo.Ecx | 1 ) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse3;
			}
			if ((cpuInfo.Ecx | (1 << 19)) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse41;
			}
			if ((cpuInfo.Ecx | (1 << 20)) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse42;
			}

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

			#endregion

			#region Edx
			if ((cpuInfo.Edx | (1 << 25)) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse;
			}
			if ((cpuInfo.Edx | (1 << 26)) != 0)
			{
				_activeProcessorFeatures |= ActiveProcessorFeatures.Sse2;
			}
			#endregion
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
			if ((cpuInfo.Ebx | (1 << 26)) != 0)
			{
				if ((_osXStateFeatures & 4) != 0)
				{
					_activeProcessorFeatures |= ActiveProcessorFeatures.Avx512Pf;
				}
			}
			if ((cpuInfo.Ebx | (1 << 27)) != 0)
			{
				if ((_osXStateFeatures & 4) != 0)
				{
					_activeProcessorFeatures |= ActiveProcessorFeatures.Avx512Er;
				}
			}
			if ((cpuInfo.Ebx | (1 << 28)) != 0)
			{
				if ((_osXStateFeatures & 4) != 0)
				{
					_activeProcessorFeatures |= ActiveProcessorFeatures.Avx512Cd;
				}
			}
		}

		public static Manufacturer Manufacturer
		{
			get { return _manufacturer; }
		}

		public static string GetManufacturerString()
		{
			CpuIdInfo info = new CpuIdInfo();
			_asmCode.Call(0, ref info);
			var ret = info.GetString();
			return ret;
		}

		public static bool HasSse
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Sse); }
		}

		public static bool HasSse2
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Sse2); }
		}

		public static bool HasSse3
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Sse3); }
		}

		public static bool HasSse41
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Sse41); }
		}

		public static bool HasSse42
		{
			get { return _activeProcessorFeatures.HasFlag(ActiveProcessorFeatures.Sse42); }
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
