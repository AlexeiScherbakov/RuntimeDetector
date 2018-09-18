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
			var ptr = NativeMethods.Kernel32.GetModuleHandle(DllNames.NvCuda);
			// если библиотека загружена не нами, то её выгружать не надо

			if (ptr == IntPtr.Zero)
			{
				_library = NativeMethods.Kernel32.LoadLibrary(DllNames.NvCuda);
				_freeLibrary = true;
			}
			else
			{
				_library = ptr;
				_freeLibrary = false;
			}
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
			if (!_freeLibrary)
			{
				return;
			}
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

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int CudaInitDelegate(int num);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int CudaDeviceGetCountDelegate(out int count);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int CudaDriverGetVersion(out int version);
	}
}
