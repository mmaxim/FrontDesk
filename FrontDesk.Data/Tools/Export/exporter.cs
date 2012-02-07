using System;
using System.Collections;

using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Tools.Export {

	/// <summary>
	/// An abstract notion of a row of data
	/// </summary>
	public class ExportRow {

		public ExportRow() { }

		private ArrayList m_fields = new ArrayList();

		public ArrayList Fields {
			get { return m_fields; }
		}

		public class ExportRowList : ArrayList {
			public ExportRowList() {
			}

			public new ExportRow this[int index] {
				get { return (ExportRow) base[index]; }
				set { base[index] = value; }
			}
		}
	}

	/// <summary>
	/// Export interface
	/// </summary>
	public interface IExporter {
		
		/// <summary>
		/// Export a set of data rows to an external file system sink
		/// </summary>
		CFile Export(FileSystem fs, ExportRow.ExportRowList rows); 
	}
}
