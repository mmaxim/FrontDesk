using System;

using FrontDesk.Common;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// File viewer factory
	/// </summary>
	public class FileViewerFactory : StringAbstractFactory {

		public FileViewerFactory() : base("FileViewers") { }

		private static FileViewerFactory m_instance;

		public static FileViewerFactory GetInstance() {
			if (null == m_instance)
				return (m_instance = new FileViewerFactory());
			else
				return m_instance;
		}

		public string CreateFileViewer(string subsys) {
			return (string) CreateItem(subsys);			
		}
	}
}
