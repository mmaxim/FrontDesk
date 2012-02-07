using System;

namespace FrontDesk.Common {

	/// <summary>
	/// Base class for custom exceptions
	/// </summary>
	public class CustomException : Exception {

		public CustomException() { }
		public CustomException(string msg) : base(msg) { }

	}
}
