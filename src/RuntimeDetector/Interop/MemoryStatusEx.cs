using System.Runtime.InteropServices;

namespace RuntimeDetector.Interop
{
	/// <summary>
	/// MEMORYSTATUSEX
	/// <see href="https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa366770(v=vs.85).aspx"/>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal ref struct MemoryStatusEx
	{
		public uint Length;
		public uint MemoryLoad;
		public ulong TotalPhys;
		public ulong AvailPhys;
		public ulong TotalPageFile;
		public ulong AvailPageFile;
		public ulong TotalVirtual;
		public ulong AvailVirtual;
		public ulong AvailExtendedVirtual;
	}
}
