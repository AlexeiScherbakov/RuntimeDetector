using System.Runtime.InteropServices;
using System.Text;

namespace RuntimeDetector.Processor.Intel
{
	[StructLayout(LayoutKind.Sequential)]
	internal ref struct CpuIdInfo
	{
		public uint Eax;
		public uint Ebx;
		public uint Ecx;
		public uint Edx;


		public static void AppendAsString(StringBuilder builder,uint value)
		{
			var val = value;

			while (val != 0)
			{
				builder.Append((char) (val & 0xFF));
				val >>= 8;
			}

		}

		public string GetString()
		{
			StringBuilder ret = new StringBuilder(16);
			AppendAsString(ret,Ebx);
			AppendAsString(ret,Edx);
			AppendAsString(ret,Ecx);

			return ret.ToString();
		}
	}
}
