using System;

namespace FrontDesk.Data.Filesys {

	/// <summary>
	/// Interface definition for an external sink of files
	/// </summary>
	public interface IExternalSink {

		/// <summary>
		/// Put a file into the sink
		/// </summary>
		bool PutFile(ExternalFile file);

		/// <summary>
		/// End the sink
		/// </summary>
		void CloseSink();

		/// <summary>
		/// Open the sink with the description string
		/// </summary>
		void CreateSink(string desc);

	}

	public interface IMemorySink : IExternalSink {
		
		/// <summary>
		/// Get raw sink data
		/// </summary>
		byte[] GetSinkData();
	}
}
