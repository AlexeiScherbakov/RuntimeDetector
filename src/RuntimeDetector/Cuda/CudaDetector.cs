using System;

namespace RuntimeDetector.Cuda
{
	public static class CudaDetector
	{
		private static readonly bool _isAvaliable;
		private static readonly string _path;
		private static readonly int _version;

		static CudaDetector()
		{
			_path = Environment.GetEnvironmentVariable("CUDA_PATH", EnvironmentVariableTarget.Machine);

			using (var api = new CudaDynamicApi())
			{
				if (!api.IsLibraryAvaliable)
				{
					_isAvaliable = false;
					return;
				}
				if (!api.Init(0))
				{
					_isAvaliable = false;
					return;
				}
				if (!api.DeviceGetCount(out int deviceCount))
				{
					_isAvaliable = false;
					return;
				}
				_isAvaliable = deviceCount > 0;

				if (api.DriverGetVersion(out int version))
				{
					_version = version;
				}
			}
		}

		public static bool IsAvaliable
		{
			get { return _isAvaliable; }
		}

		public static string Path
		{
			get { return _path; }
		}

		public static int Version
		{
			get { return _version; }
		}
	}
}
