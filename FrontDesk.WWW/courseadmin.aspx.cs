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

using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Common;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Filesys.Provider;

namespace FrontDesk.Pages {

	/// <summary>
	/// Course admin Matrix host
	/// </summary>
	public class CourseAdminPage : StaffPage {
		protected System.Web.UI.WebControls.LinkButton cmdLogout;
		protected System.Web.UI.HtmlControls.HtmlAnchor CourseMain;
		protected System.Web.UI.WebControls.Label lblID;

		public CourseAdminPage() {

		}

		private void Page_Load(object sender, System.EventArgs e) {
		/*	FileSystem fs = FileSystem.GetInstance();
			OSFileSystemProvider osfs = new OSFileSystemProvider(@"c:\frontdesk\filedata");

			fs.CopyFileSystem(osfs);*/
		//	new FileSystem(Globals.CurrentIdentity).RecoverBaseFilePermissions();
		//	new Rubrics(Globals.CurrentIdentity).SynchronizePoints();
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
