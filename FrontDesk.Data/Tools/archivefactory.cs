using System;
using System.Web;

using FrontDesk.Common;

namespace FrontDesk.Tools {

	public class ArchiveToolFactory : ClassAbstractFactory {

		protected static ArchiveToolFactory m_instance=null; 

		protected ArchiveToolFactory() : base("ArchiveTools") { 
			
		}

		public static ArchiveToolFactory GetInstance() {
			if (m_instance == null)
				return (m_instance = new ArchiveToolFactory());
			else
				return m_instance;
		}

		public IArchiveTool CreateArchiveTool(string subsys) {
			return (IArchiveTool) CreateItem(subsys);			
		}
	}
}
