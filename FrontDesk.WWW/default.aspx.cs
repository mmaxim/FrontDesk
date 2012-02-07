//Mike Maxim
//Main FrontDesk page

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
using FrontDesk.Common;
using FrontDesk.Controls;

namespace FrontDesk.Pages {
	
	/// <summary>
	/// Main page for FrontDesk
	/// </summary>
	public class DefaultPage : MasterPage {

		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.LinkButton cmdLogout;
		protected Microsoft.Web.UI.WebControls.TabStrip tsVert;
		protected Microsoft.Web.UI.WebControls.MultiPage mpVert;
		protected System.Web.UI.WebControls.Label lblName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divAdmin;
		protected System.Web.UI.WebControls.Label lblID;
	
		public DefaultPage() : base() {
			HasTabs = true;
		}

		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack) {
				User curuser = new Users(Globals.CurrentIdentity).GetInfo(Globals.CurrentUserName, null);
				lblName.Text = curuser.FirstName + " " + curuser.LastName + "!";
				if (curuser.Admin)
					divAdmin.Visible = true;
				else
					divAdmin.Visible = false;
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
