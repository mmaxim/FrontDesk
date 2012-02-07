using System;
using System.IO;
using System.Collections;

using FrontDesk.Common;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Data.Filesys {

	/// <summary>
	/// Directory external source
	/// </summary>
	public class OSDirectorySource : IExternalSource {

		class PathPair {
			public PathPair() { }
			public PathPair(string ab, string rel) { 
				AbsolutePath=ab; RelativePath=rel; 
			}

			public string AbsolutePath, RelativePath;
		}
		
		private ArrayList m_files=null;

		public OSDirectorySource() { }

		public ExternalFile NextFile() {
			
			//Sanity check
			if (m_files == null || m_files.Count == 0) return null;

			//Prepare external file
			PathPair curpath = (PathPair) m_files[0];
			ExternalFile file = new ExternalFile();
			FileStream data = new FileStream(curpath.AbsolutePath, FileMode.Open);
			
			file.DataStream = data;
			file.Directory = false;
			file.Path = curpath.RelativePath;
			file.Size = (int) data.Length;

			//Pop off a file
			m_files.RemoveAt(0);

			return file;
		}

		public void CloseFile(ExternalFile file) {
			try { file.DataStream.Close(); } catch (Exception) { }
		}

		public void CloseSource() {
			m_files=null;
		}

		public void CreateSource(string desc) {
			//Sanity check
			if (!Directory.Exists(desc))
				throw new FileOperationException("Specified path must refer to a directory");
		
			//Get all the files
			ListDirectory(desc, (m_files = new ArrayList()), "");
		}

		private void ListDirectory(string basedir, ArrayList allfiles, string rel) {
			string[] dirs = Directory.GetDirectories(basedir);
			foreach (string dir in dirs)
				ListDirectory(dir, allfiles, Path.Combine(rel, Path.GetFileName(dir)));

			string[] files = Directory.GetFiles(basedir);
			foreach (string file in files)
				allfiles.Add(new PathPair(file, Path.Combine(rel, Path.GetFileName(file))));
		}

		public void CreateSource(Stream stream) {
			throw new NotImplementedException("Method unsupported for this source: OSDirectorySouce");
		}

	}
}
