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

using FrontDesk.Data.Access;
using FrontDesk.Components;

namespace FrontDesk.Pages.Admin {

	/// <summary>
	/// Summary description for admin.
	/// </summary>
	public class AdminPage : MasterPage {

		protected Microsoft.Web.UI.WebControls.TabStrip tsVert;
		protected Microsoft.Web.UI.WebControls.MultiPage mpVert;
		protected System.Web.UI.WebControls.LinkButton cmdLogout;
		protected System.Web.UI.WebControls.Label lblID;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
	
		public AdminPage() {
			HasTabs = true;
		}

		private void Page_Load(object sender, System.EventArgs e) {

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
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
