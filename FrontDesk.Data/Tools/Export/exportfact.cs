using System;
using System.Web;

using FrontDesk.Common;

namespace FrontDesk.Tools.Export {

	public class ExporterFactory : ClassAbstractFactory {

		protected static ExporterFactory m_instance=null; 

		protected ExporterFactory() : base("Exporters") { 
			
		}

		public static ExporterFactory GetInstance() {
			if (m_instance == null)
				return (m_instance = new ExporterFactory());
			else
				return m_instance;
		}

		public IExporter CreateExportTool(string subsys) {
			return (IExporter) CreateItem(subsys);			
		}
	}
}
