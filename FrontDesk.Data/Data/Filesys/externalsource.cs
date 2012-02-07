using System;
using System.IO;

namespace FrontDesk.Data.Filesys {

	public class ExternalFile {
		
		public string Path {
			get { return m_pathname; }
			set { m_pathname = value; }
		}

		public Stream DataStream {
			get { return m_datastream; }
			set { m_datastream = value; }
		}

		public bool Directory {
			get { return m_directory; }
			set { m_directory = value; }
		}

		public int Size {
			get { return m_size; }
			set { m_size = value; }
		}

		protected string m_pathname;
		protected Stream m_datastream;
		protected bool m_directory;
		protected int m_size;
	}

	/// <summary>
	/// External source of file data for the file system
	/// </summary>
	public interface IExternalSource {

		/// <summary>
		/// Returns an external file from the external source
		/// </summary>
		ExternalFile NextFile();

		/// <summary>
		/// End usage of external file
		/// </summary>
		void CloseFile(ExternalFile file);

		/// <summary>
		/// Shut down the external source
		/// </summary>
		void CloseSource();

		/// <summary>
		/// Create the external source
		/// </summary>
		void CreateSource(string desc);

		/// <summary>
		/// Create a source from an existing stream
		/// </summary>
		void CreateSource(Stream stream);
	
	}

	public interface IMemorySource : IExternalSource {

		/// <summary>
		/// Get raw source data
		/// </summary>
		byte[] GetSourceData();

	}

	public class EmptySource : IExternalSource {

		public EmptySource() { }

		public ExternalFile NextFile() { 
			return null; 
		}

		public void CloseFile(ExternalFile file) { }

		public void CloseSource() { }

		public void CreateSource(string desc) { }

		public void CreateSource(Stream str) { }

		public byte[] GetSourceData() { return new byte[0]; }
	}
}
