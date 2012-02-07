using System;
using System.IO;

using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Common;

namespace FrontDesk.Data.Filesys.Provider {

	/// <summary>
	/// OS File System File System.
	/// </summary>
	internal class OSFileSystemProvider : IFileSystemProvider	{
		
		public OSFileSystemProvider() {
			m_localpath="";
		}

		public OSFileSystemProvider(string localpath) { 
			m_localpath = localpath;
		}

		private string m_localpath;
	
		public void CreateFile(CFile file) {
			File.Create(Path.Combine(m_localpath, file.ID.ToString())).Close();
		}

		public void DeleteFile(CFile file) {
			File.Delete(Path.Combine(m_localpath, file.ID.ToString()));
		}

		public void CopyFile(CFile dest, CFile src) {
			string dpath = Path.Combine(m_localpath, dest.ID.ToString());
			string spath = Path.Combine(m_localpath, src.ID.ToString());

			File.Copy(spath, dpath, true);
		}

		public void FetchData(CFile file) {
			FileStream efile = File.Open(Path.Combine(m_localpath, file.ID.ToString()), FileMode.Open);
			file.RawData = 
				Globals.ReadStream(efile, (int) efile.Length);
			efile.Close();
		}

		public void CommitData(CFile file) {
			FileStream efile = File.Open(Path.Combine(m_localpath, file.ID.ToString()), FileMode.Create);
			efile.Write(file.RawData, 0, file.RawData.Length);
			efile.Flush();
			efile.Close();
		}
	}
}
