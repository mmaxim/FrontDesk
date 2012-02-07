using System;

using FrontDesk.Common;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// File browser image factory
	/// </summary>
	public class FileBrowserImageFactory : StringAbstractFactory {

		public FileBrowserImageFactory() : base("FileBrowserImages") { }

		private static FileBrowserImageFactory m_instance;

		public static FileBrowserImageFactory GetInstance() {
			if (null == m_instance)
				return (m_instance = new FileBrowserImageFactory());
			else
				return m_instance;
		}

		public string GetExtensionImage(string ext) {
			return (string) CreateItem(ext);			
		}
	}
}
