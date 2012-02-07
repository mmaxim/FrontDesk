using System;
using System.IO;
using System.Text;

using FrontDesk.Common;

namespace FrontDesk.Data.Filesys {

	/// <summary>
	/// Sink involving the underlying OS file system
	/// </summary>
	public class OSFileSystemSink : IExternalSink {

		public OSFileSystemSink() { }

		public void CreateSink(string desc) {

		}

		public void CloseSink() {

		}

		public bool PutFile(ExternalFile file) {
			
			if (file.Directory)
				Directory.CreateDirectory(file.Path);
			else {
				string dirname = Path.GetDirectoryName(file.Path);

				Directory.CreateDirectory(dirname);
				FileStream osfs = File.Create(file.Path);

				byte[] data  = Globals.ReadStream(file.DataStream, file.Size);

				osfs.Write(data, 0, data.Length);
				osfs.Flush();
				
				osfs.Close();
			}

			return true;
		}
	}
}
