using System;

using FrontDesk.Data.Filesys;
using FrontDesk.Common;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;

namespace FrontDesk.Tools {

	/// <summary>
	/// CheckStyle helper tool
	/// </summary>
	public class CheckStyleTool {

		public CheckStyleTool() {

		}

		//Copy CS helper code into autoevaluation zone
		public void CopySupportFiles(AutoEvaluation eval) {
			
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			//Get zone
			CFile zone = fs.GetFile(eval.ZoneID);

			//Copy CS program over
			CFile.FileList dfiles = new CFile.FileList();
			dfiles.Add(fs.GetFile(@"c:\system\checkstyle\CheckStyle.class"));
			dfiles.Add(fs.GetFile(@"c:\system\checkstyle\checksubj.xslt"));
			fs.CopyFiles(zone, dfiles, true);
		}
	}
}
