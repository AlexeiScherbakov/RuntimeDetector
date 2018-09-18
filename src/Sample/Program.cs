using System;

namespace Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Operation system platform: {0}", RuntimeDetector.Runtime.OperationSystem.Platform);
			Console.WriteLine("Operation system architecture: {0}", RuntimeDetector.Processor.Information.OsArchitecture);
			Console.WriteLine("Process architecture: {0}", RuntimeDetector.Processor.Information.ProcessArchitecture);
			Console.WriteLine("Free physical memory: {0}", RuntimeDetector.Runtime.OperationSystem.FreePhysicalMemory);
			Console.WriteLine("Started under Mono: {0}", RuntimeDetector.Runtime.Framework.IsMono);

			
			if (RuntimeDetector.Processor.Information.IsX86())
			{
				Console.WriteLine("Intel/AMD/x86 family processor");
				Console.WriteLine("\tPopcnt:\t {0}", RuntimeDetector.Processor.Intel.CpuInformation.HasPopcnt);
				Console.WriteLine("\tAVX:\t {0}", RuntimeDetector.Processor.Intel.CpuInformation.HasAvx);
				Console.WriteLine("\tAVX2:\t {0}", RuntimeDetector.Processor.Intel.CpuInformation.HasAvx2);
			}

			Console.WriteLine("CUDA runtime present: {0}", RuntimeDetector.Cuda.CudaDetector.IsAvaliable);
			Console.WriteLine("CUDA version: {0}", RuntimeDetector.Cuda.CudaDetector.Version);
			Console.WriteLine("CUDA Path: {0}", RuntimeDetector.Cuda.CudaDetector.Path);

			Console.ReadLine();
		}
	}
}
