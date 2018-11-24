using System;

namespace RuntimeDetector.Processor.Intel
{
	/// <summary>
	/// Optimization usefull processor features
	/// </summary>
	[Flags]
	internal enum ActiveProcessorFeatures
	{
		None = 0,
		Popcnt = 1,
		Sse = 1 << 1,
		Sse2 = 1 << 2,
		Sse3 = 1 << 3,
		Sse41 = 1 << 4,
		Sse42 = 1 << 5,
		Avx = 1 << 6,
		Avx2 = 1 << 7,
		Avx512Pf = 1 << 8,
		Avx512Er = 1 << 9,
		Avx512Cd = 1 << 10
	}
}
