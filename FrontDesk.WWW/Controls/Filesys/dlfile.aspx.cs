using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using FrontDesk.Common;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Filesys {

	/// <summary>
	/// Page to handle downloading files
	/// </summary>
	public class DownloadFilePage : Page {
		protected System.Web.UI.WebControls.Label lblError;

		private void Page_Load(object sender, System.EventArgs e) {

			lblError.Visible = false;

			byte[] fileData;
			CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(Convert.ToInt32(Request.Params["FileID"]));
			string filename= file.Name;

			if (file.IsDirectory()) {
				fileData = GetDirectoryData(file);
				filename += ".zip";
			}
			else {
				try {
					new FileSystem(Globals.CurrentIdentity).LoadFileData(file);
				} catch (CustomException er) {
					PageError(er.Message);
					return;
				}
				fileData = file.RawData;
			}
	
			Response.Clear();
			Response.ContentType = "application/octet-stream; name=" + filename;
			Response.AddHeader("Content-Disposition","attachment; filename=" + filename); 
			Response.AddHeader("Content-Length", fileData.Length.ToString());
			Response.Flush();

			Response.OutputStream.Write(fileData, 0, fileData.Length);
			Response.Flush();
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private byte[] GetDirectoryData(CFile dir) {
			
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			//Create our external sink (gonna be an archive, so safe cast)
			IMemorySink extsink = 
				ArchiveToolFactory.GetInstance().CreateArchiveTool(".zip") as IMemorySink;
			extsink.CreateSink(null);
			
			//Export data
			fs.ExportData("", dir, extsink, false);
			byte[] data = extsink.GetSinkData();
			extsink.CloseSink();

			return data;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
