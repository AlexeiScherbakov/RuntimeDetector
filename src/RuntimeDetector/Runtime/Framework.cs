using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RuntimeDetector.Runtime
{
	public static class Framework
	{
		private static readonly bool _isMono;

		static Framework()
		{
			_isMono = Type.GetType("Mono.Runtime") != null;
		}

		public static bool IsMono
		{
			get { return _isMono; }
		}
	}
}
