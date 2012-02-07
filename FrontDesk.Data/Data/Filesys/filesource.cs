using System;
using System.IO;

using FrontDesk.Common;

namespace FrontDesk.Data.Filesys {

	/// <summary>
	/// Single file source
	/// </summary>
	public class SingleFileSource : IExternalSource {
		
		public SingleFileSource(string name) { m_filename=name; m_read=false; }

		private Stream m_file=null;
		private bool m_read;
		private string m_filepath, m_filename;

		public ExternalFile NextFile() {
			if (!m_read) {
				ExternalFile file = new ExternalFile();

				file.DataStream = m_file;
				file.Directory = false;
				file.Path = m_filename;
				file.Size = (int) m_file.Length;

				m_read=true;
				return file;
			} else
				return null;
		}

		public void CloseFile(ExternalFile file) {
			
		}

		public void CloseSource() {
			m_file.Close();		
		}

		public void CreateSource(string desc) {
			m_file = new FileStream((m_filepath = desc), FileMode.Open);
			m_read = false;
		}

		public void CreateSource(Stream stream) {
			m_file = stream;
			m_read = false;
		}
	}
}
