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

namespace FrontDesk.Pages {

	/// <summary>
	/// An error page
	/// </summary>
	public class ErrorPage : MasterPage {

		protected System.Web.UI.WebControls.Label lblID;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.LinkButton cmdLogout;

		public enum Codes { PERMISSION, NOTFOUND, CRASH };

		private void Page_Load(object sender, System.EventArgs e) {
			Codes ecode = (Codes) Convert.ToInt32(Request.Params["Code"]);
			switch (ecode) {
			case Codes.PERMISSION:
				lblError.Text = "Permission Denied: You are not allowed to access " +
					"the requested resource. Contact the course administrator " +
					"if you think this is an error";
				break;
			case Codes.NOTFOUND:
				lblError.Text = "The resource you requested could not be found";
				break;
			case Codes.CRASH:
				lblError.Text = "An unexpected error has occurred. Please contact the administrator";
				break;
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
