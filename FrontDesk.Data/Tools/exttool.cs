using System;

using FrontDesk.Common;

namespace FrontDesk.Tools {

	/// <summary>
	/// Interface for an external tool (one that is run outside process)
	/// </summary>
	public interface IExternalTool {

		/// <summary>
		/// Execute the tool
		/// </summary>
		int Execute();

		/// <summary>
		/// Execute the tool
		/// </summary>
		int Execute(string wrkdir);

		/// <summary>
		/// Execute the tool
		/// </summary>
		int Execute(int waittime);

		/// <summary>
		/// Execute the tool
		/// </summary>
		int Execute(string wrkdir, int waittime);

		/// <summary>
		/// Any arguments to the tool
		/// </summary>
		string Arguments { get; set; }

		/// <summary>
		/// Output from execution of the tool
		/// </summary>
		string Output { get; }

		/// <summary>
		/// Numerical version for the tool
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Error output from the tool
		/// </summary>
		string ErrorOutput { get; }

		/// <summary>
		/// Input into the tool
		/// </summary>
		string Input { set; }
	}

	public class ToolExecutionException : CustomException {
		public ToolExecutionException() : base("Tool execution error!") { }
		public ToolExecutionException(string msg) : base(msg) { }
	}
}
