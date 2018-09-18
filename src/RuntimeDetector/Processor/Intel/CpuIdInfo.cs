using System.Runtime.InteropServices;

namespace RuntimeDetector.Processor.Intel
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct CpuIdInfo
	{
		public uint Eax;
		public uint Ebx;
		public uint Ecx;
		public uint Edx;
	}
}
