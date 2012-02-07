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
using System.Web.Security;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Data.Access;

namespace FrontDesk.Pages {

	/// <summary>
	/// A course staff protected page
	/// </summary>
	public class StaffPage : MasterPage {

		public StaffPage() { }

		private void Page_Load(object sender, EventArgs e) {
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			CourseRole role = 
				new Courses(Globals.CurrentIdentity).GetRole(Globals.CurrentUserName, courseID, null);
			
			if (role == null || !role.Staff)
				Page.Response.Redirect("error.aspx?Code="+
					Convert.ToInt32(Pages.ErrorPage.Codes.PERMISSION));	
		}

		override protected void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);
		}
	}
}
