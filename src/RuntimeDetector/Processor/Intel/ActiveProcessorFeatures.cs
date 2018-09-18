using System;

namespace RuntimeDetector.Processor.Intel
{
	[Flags]
	internal enum ActiveProcessorFeatures
	{
		None = 0,
		Popcnt = 1,
		Sse = 1 << 1,
		Sse2 = 1 << 2,
		Avx = 1 << 3,
		Avx2 = 1 << 4
	}
}
