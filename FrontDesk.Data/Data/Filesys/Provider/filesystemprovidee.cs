using System;

namespace FrontDesk.Data.Filesys.Provider {

	/// <summary>
	/// Interface for file system provider trusted components
	/// </summary>
	internal interface IFileSystemProvidee {
		
		void Acquire(IFileSystemProvider provider);
	}
}
