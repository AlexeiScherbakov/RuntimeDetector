using System;
using System.Runtime.InteropServices;
using System.Threading;

using RuntimeDetector.Interop;

namespace RuntimeDetector.Cuda
{
	public sealed class CudaDynamicApi
		: IDisposable
	{
		private readonly bool _freeLibrary;
		private IntPtr _library;

		public CudaDynamicApi()
		{
			_library = NativeMethods.Kernel32.LoadLibrary(DllNames.NvCuda);
		}

		~CudaDynamicApi()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			var lib = Interlocked.Exchange(ref _library, IntPtr.Zero);
			NativeMethods.Kernel32.FreeLibrary(lib);
		}

		public bool IsLibraryAvaliable
		{
			get { return _library != IntPtr.Zero; }
		}

		public bool Init(int flags)
		{
			if (!NativeMethods.GetDelegate<CudaInitDelegate>(_library, FunctionNames.NvCuda.Init, out var func))
			{
				return false;
			}
			int ret = func(flags);
			return ret == 0;
		}

		public bool DeviceGetCount(out int count)
		{
			count = 0;
			if (!NativeMethods.GetDelegate<CudaDeviceGetCountDelegate>(_library, FunctionNames.NvCuda.DeviceGetCount, out var func))
			{
				return false;
			}
			int ret = func(out count);
			return ret == 0;
		}

		public bool DriverGetVersion(out int version)
		{
			if (!NativeMethods.GetDelegate<CudaDriverGetVersion>(_library, FunctionNames.NvCuda.DriverGetVersion, out var func))
			{
				version = 0;
				return false;
			}
			int ret = func(out version);
			return ret == 0;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int CudaInitDelegate(int num);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int CudaDeviceGetCountDelegate(out int count);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int CudaDriverGetVersion(out int version);
	}
}
