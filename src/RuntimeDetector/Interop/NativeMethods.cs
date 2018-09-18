using System;
using System.Runtime.InteropServices;

namespace RuntimeDetector.Interop
{
	internal sealed class NativeMethods
	{
		internal static bool GetDelegate<TDelegate>(IntPtr library, string functionName, out TDelegate function)
			where TDelegate : Delegate
		{
			function = default(TDelegate);
			var ptr = NativeMethods.Kernel32.GetProcAddress(library, functionName);
			if (IntPtr.Zero == ptr)
			{
				return false;
			}
#if NET40
			function = (TDelegate) Marshal.GetDelegateForFunctionPointer(ptr, typeof(TDelegate));
#else
			function = Marshal.GetDelegateForFunctionPointer<TDelegate>(ptr);
#endif
			return true;
		}


		internal static class Kernel32
		{
			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.GetProcAddress)]
			internal static extern IntPtr GetProcAddress(IntPtr library, string funcName);

			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.LoadLibrary)]
			internal static extern IntPtr LoadLibrary(string fileName);


			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.FreeLibrary)]
			internal static extern void FreeLibrary(IntPtr library);


			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.GetModuleHandle)]
			internal static extern IntPtr GetModuleHandle(string fileName);


			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.GetEnabledXStateFeatures)]
			internal static extern long GetEnabledXStateFeatures();


			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.VirtualAlloc)]
			internal static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.VirtualFree)]
			internal static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, int dwFreeType);


			[DllImport(DllNames.Kernel32, EntryPoint = FunctionNames.Kernel32.GlobalMemoryStatusEx)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx memoryStatus);
		}
	}
}
