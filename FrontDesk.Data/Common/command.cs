using System;

namespace FrontDesk.Common {

	public class ICommandEventArgs : EventArgs {
		public object ReturnValue;
		public Exception Exception;

		public ICommandEventArgs() { }
		public ICommandEventArgs(object rv, Exception e) { ReturnValue = rv; Exception = e; }
	}

	/// <summary>
	/// Hooks for command completion
	/// </summary>
	public delegate void ICommandEventHandler(object sender, ICommandEventArgs args);

	/// <summary>
	/// Command pattern interface
	/// </summary>
	public interface ICommand {

		/// <summary>
		/// Event for command completion notification
		/// </summary>
		event ICommandEventHandler Finished; 

		/// <summary>
		/// Execution method
		/// </summary>
		void Execute();

	}
}
