using System;
using System.IO;

using FrontDesk.Data.Filesys;

namespace FrontDesk.Tools {
	
	/// <summary>
	/// Interface for tools that serve to extract archives
	/// </summary>
	public interface IArchiveTool {
		
		/// <summary>
		/// Extract the given archive
		/// </summary>
		void Extract(string path);

		/// <summary>
		/// Extract the given archive
		/// </summary>
		void Extract(string path, string destpath);
	}
}
