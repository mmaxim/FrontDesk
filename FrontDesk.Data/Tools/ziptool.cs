using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

using FrontDesk.Common;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Tools
{
	/// <summary>
	/// ZIP handler
	/// </summary>
	public class ZipTool : IArchiveTool, IMemorySource, IMemorySink {
		
		public ZipTool() { }

		protected ZipInputStream m_source;
		protected ZipOutputStream m_sink;
		protected Stream m_outputback;

		public void CreateSource(string path) {
			m_source = new ZipInputStream(m_outputback = File.OpenRead(path));
		}

		public void CreateSource(Stream stream) {
			m_source = new ZipInputStream(m_outputback = stream);
		}

		public void CreateSink(string path) {
			if (path != null)
				m_sink = new ZipOutputStream(m_outputback = File.Create(path));
			else
				m_sink = new ZipOutputStream(m_outputback = new MemoryStream());

			m_sink.SetLevel(5);
		}

		public byte[] GetSourceData() {
			m_outputback.Seek(0, SeekOrigin.Begin);
			byte[] data = Globals.ReadStream(m_outputback, (int) m_outputback.Length);
			m_outputback.Seek(0, SeekOrigin.Begin);
			return data;
		}

		public byte[] GetSinkData() {
			m_sink.Finish();
			m_sink.Flush();
			m_outputback.Seek(0, SeekOrigin.Begin);
			return Globals.ReadStream(m_outputback, (int) m_outputback.Length);
		}

		public ExternalFile NextFile() {
			ZipEntry e;
			if (null == (e = m_source.GetNextEntry()))
				return null;
			else {
				ExternalFile efile = new ExternalFile();
				efile.DataStream = m_source;
				efile.Path = e.Name;
				efile.Directory = e.IsDirectory;
				efile.Size = Math.Max(0, (int) e.Size);
				return efile;
			}
		}

		public bool PutFile(ExternalFile file) {

			//Ignore directories
			if (file.Directory) return true;

			//Read data from file strea,
			byte[] data = new byte[file.Size];
			file.DataStream.Read(data, 0, file.Size);

			//Create entry
			ZipEntry e = new ZipEntry(file.Path);
			m_sink.PutNextEntry(e);
			m_sink.Write(data, 0, file.Size);

			return true;
		}

		public void CloseFile(ExternalFile file) {
		}

		public void CloseSource() {
			m_source.Close();
			m_source=null;
		}

		public void CloseSink() {
			m_sink.Close();
			m_sink=null;
		}

		public void Extract(string path) {
			Extract(path, Path.GetDirectoryName(path));
		}

		public void Extract(string path, string destpath) {
			ZipInputStream zfile = new ZipInputStream(File.OpenRead(path));
			ZipEntry e;
			byte[] buffer = new byte[2048];
			int nb;
			
			string basedir = destpath;
			while (null != (e = zfile.GetNextEntry())) {
				if (e.IsDirectory)
					Directory.CreateDirectory(Path.Combine(basedir, e.Name));
				else {
					string lpath = Path.Combine(basedir, e.Name);
					Directory.CreateDirectory(Path.GetDirectoryName(lpath));
					FileStream s = File.OpenWrite(lpath);
					while (0 < (nb = zfile.Read(buffer, 0, buffer.Length))) {
						s.Write(buffer, 0, nb);
					}
					s.Close();
				}		
			}
		}
	}
}
