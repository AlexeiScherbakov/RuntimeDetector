using System;

namespace RuntimeDetector.Processor
{
	/// <summary>
	/// 
	/// </summary>
	/// <see href="https://docs.microsoft.com/ru-ru/windows/desktop/WinProg64/wow64-implementation-details"/>
	public static class Information
	{
		private static readonly Architecture _processArchitecture;
		private static readonly Architecture _osArchitecture;


#if !(NET40 || NET462 || NET47)
		private static Architecture ConvertArchitecture(System.Runtime.InteropServices.Architecture architecture)
		{
			switch (architecture)
			{
				case System.Runtime.InteropServices.Architecture.Arm:
					return Architecture.Arm;
				case System.Runtime.InteropServices.Architecture.Arm64:
					return Architecture.Arm64;
				case System.Runtime.InteropServices.Architecture.X86:
					return Architecture.X86;
				case System.Runtime.InteropServices.Architecture.X64:
					return Architecture.Amd64;
				default:
					return Architecture.None;
			}
		}
#endif

		static Information()
		{
#if !(NET40 || NET462 || NET47)
			// modern way of detection
			_processArchitecture = ConvertArchitecture(System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture);
			_osArchitecture= ConvertArchitecture(System.Runtime.InteropServices.RuntimeInformation.OSArchitecture);

			if ((_processArchitecture!= Architecture.None)&&(_osArchitecture!= Architecture.None))
			{
				return;
			}
#endif
			// old way
			//https://docs.microsoft.com/ru-ru/windows/desktop/WinProg64/wow64-implementation-details#environment-variables
			var arch = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine);
			_processArchitecture = GetEnvironmentVariableArchitecture(arch);
			if (_processArchitecture == Architecture.X86)
			{
				var archWow = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432", EnvironmentVariableTarget.Machine);
				if (string.IsNullOrWhiteSpace(archWow))
				{
					_osArchitecture = _processArchitecture;
				}
				else
				{
					_osArchitecture = GetEnvironmentVariableArchitecture(archWow);
				}
			}
			else
			{
				_osArchitecture = _processArchitecture;
			}
		}

		public static Architecture GetEnvironmentVariableArchitecture(string arch)
		{
			switch (arch)
			{
				case "x86":
					return Architecture.X86;
				case "AMD64":
				case "EM64T":
					return Architecture.Amd64;
				case "IA64":
					return Architecture.Itanium64;
				case "ARM":
					// TODO check it!
					return Architecture.Arm;
				case "ARM64":
					return Architecture.Arm64;
				default:
					return Architecture.None;
			}
		}


		public static Architecture ProcessArchitecture
		{
			get { return _processArchitecture; }
		}

		public static Architecture OsArchitecture
		{
			get { return _osArchitecture; }
		}


		public static bool IsX86()
		{
			return (_osArchitecture == Architecture.X86) || (_osArchitecture == Architecture.Amd64);
		}

		public static bool IsArm()
		{
			return (_osArchitecture == Architecture.Arm) || (_osArchitecture == Architecture.Arm64);
		}

		public static bool IsX86OnArm()
		{
			return ((_osArchitecture == Architecture.Arm) || (_osArchitecture == Architecture.Arm64)) && (_processArchitecture == Architecture.X86);
		}
	}
}
