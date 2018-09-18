using System;
using System.Runtime.InteropServices;
using RuntimeDetector.Interop;

namespace RuntimeDetector.Processor.Intel
{
	internal sealed class CpuIdAssemblyCode
		: IDisposable
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void CpuIDDelegate(int level, ref CpuIdInfo cpuId);

		private IntPtr _codePointer;
		private uint _size;
		private CpuIDDelegate _delegate;

		public CpuIdAssemblyCode()
		{
			byte[] codeBytes = (IntPtr.Size == 4) ? x86CodeBytes : x64CodeBytes;

			_size = (uint) codeBytes.Length;
			_codePointer = NativeMethods.Kernel32.VirtualAlloc(
					IntPtr.Zero,
					new UIntPtr(_size),
					AllocationType.COMMIT | AllocationType.RESERVE,
					MemoryProtection.EXECUTE_READWRITE
				);

			Marshal.Copy(codeBytes, 0, _codePointer, codeBytes.Length);
#if NET40
			_delegate = (CpuIDDelegate) Marshal.GetDelegateForFunctionPointer(_codePointer, typeof(CpuIDDelegate));
#else
			_delegate = Marshal.GetDelegateForFunctionPointer<CpuIDDelegate>(_codePointer);
#endif

		}

		~CpuIdAssemblyCode()
		{
			Dispose(false);
		}

		public void Call(int level, ref CpuIdInfo cpuInfo)
		{
			_delegate(level, ref cpuInfo);
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			NativeMethods.Kernel32.VirtualFree(_codePointer, _size, 0x8000);
		}





		// Basic ASM strategy --
		// void x86CpuId(int level, byte* buffer) 
		// {
		//    eax = level
		//    cpuid
		//    buffer[0] = eax
		//    buffer[4] = ebx
		//    buffer[8] = ecx
		//    buffer[12] = edx
		// }

		private readonly static byte[] x86CodeBytes = {
		0x55,                   // push        ebp  
		0x8B, 0xEC,             // mov         ebp,esp
		0x53,                   // push        ebx  
		0x57,                   // push        edi

		0x8B, 0x45, 0x08,       // mov         eax, dword ptr [ebp+8] (move level into eax)
		0x0F, 0xA2,              // cpuid

		0x8B, 0x7D, 0x0C,       // mov         edi, dword ptr [ebp+12] (move address of buffer into edi)
		0x89, 0x07,             // mov         dword ptr [edi+0], eax  (write eax, ... to buffer)
		0x89, 0x5F, 0x04,       // mov         dword ptr [edi+4], ebx 
		0x89, 0x4F, 0x08,       // mov         dword ptr [edi+8], ecx 
		0x89, 0x57, 0x0C,       // mov         dword ptr [edi+12],edx 

		0x5F,                   // pop         edi  
		0x5B,                   // pop         ebx  
		0x8B, 0xE5,             // mov         esp,ebp  
		0x5D,                   // pop         ebp 
		0xc3                    // ret
		};

		private readonly static byte[] x64CodeBytes = {
		0x53,                       // push rbx    this gets clobbered by cpuid

		// rcx is level
		// rdx is buffer.
		// Need to save buffer elsewhere, cpuid overwrites rdx
		// Put buffer in r8, use r8 to reference buffer later.

		// Save rdx (buffer addy) to r8
		0x49, 0x89, 0xd0,           // mov r8,  rdx

		// Move ecx (level) to eax to call cpuid, call cpuid
		0x89, 0xc8,                 // mov eax, ecx
		0x0F, 0xA2,                 // cpuid

		// Write eax et al to buffer
		0x41, 0x89, 0x40, 0x00,     // mov    dword ptr [r8+0],  eax
		0x41, 0x89, 0x58, 0x04,     // mov    dword ptr [r8+4],  ebx
		0x41, 0x89, 0x48, 0x08,     // mov    dword ptr [r8+8],  ecx
		0x41, 0x89, 0x50, 0x0c,     // mov    dword ptr [r8+12], edx

		0x5b,                       // pop rbx
		0xc3                        // ret
		};


	}
}
