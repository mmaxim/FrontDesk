using System;
using System.Collections;
using System.IO;	

using FrontDesk.Common;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Tools.Export {

	/// <summary>
	/// CSV data exporter
	/// </summary>
	public class CSVExporter : IExporter {
		
		public CSVExporter() { }

		/// <summary>
		/// Export data into CSV format
		/// </summary>
		public CFile Export(FileSystem fs, ExportRow.ExportRowList rows) {
			
			int i;
			MemoryStream csvstream = new MemoryStream();
			StreamWriter csvwriter = new StreamWriter(csvstream);

			//Write the data out in CSV style rows
			foreach (ExportRow row in rows) {
				for (i = 0; i < row.Fields.Count; i++) {
					csvwriter.Write(row.Fields[i]);
					if (i < row.Fields.Count-1)
						csvwriter.Write(",");
				}
				csvwriter.WriteLine();
			}
			csvwriter.Flush();

			//Commit to a temp file within the FS
			//Create a temp file
			Guid guid = Guid.NewGuid();
			CFile expfile = fs.CreateFile(@"c:\system\export\" + guid.ToString(), false, 
				new CFilePermission.FilePermissionList() );
			expfile.Name = expfile.ID + ".csv";
			fs.UpdateFileInfo(expfile, false);

			//Commit the data
			csvstream.Seek(0, SeekOrigin.Begin);
			expfile.RawData = Globals.ReadStream(csvstream, (int) csvstream.Length);
			fs.Edit(expfile);
			fs.Save(expfile);

			csvwriter.Close();

			return expfile;
		}
	}
}
