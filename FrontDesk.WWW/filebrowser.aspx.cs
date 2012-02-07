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

using FrontDesk.Common;
using FrontDesk.Data.Filesys;
using FrontDesk.Components.Filesys;
using FrontDesk.Controls.Filesys;

namespace FrontDesk.Pages {

	/// <summary>
	/// Summary description for filebrowser.
	/// </summary>
	public class FileBrowserPage : Page {
		
		protected FileBrowserPagelet ucFiles;

		private void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack)
				SetupBrowser();
		}

		private void SetupBrowser() {

			string idstr = HttpContext.Current.Request.Params["Roots"];
			string [] ids = idstr.Split("|".ToCharArray());

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			foreach (string sid in ids) {
				if (sid.Length == 0) continue;
				int id = Convert.ToInt32(sid);
				
				CFile root = fs.GetFile(id);
				ucFiles.AddDirectoryRoot(root.FullPath);
			}
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
