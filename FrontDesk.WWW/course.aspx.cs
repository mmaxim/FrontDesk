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

namespace FrontDesk.Pages {
	
	/// <summary>
	/// Summary description for course.
	/// </summary>
	public class StudentCoursePage : MasterPage {
		protected System.Web.UI.WebControls.DataGrid dgAssignments;
		protected System.Web.UI.WebControls.LinkButton cmdLogout;
		protected System.Web.UI.HtmlControls.HtmlAnchor CourseMain;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.Label lblID;

		public StudentCoursePage() {
			HasTabs = true;
		}

		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
			
			int courseID = 	Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			Course course = (new Courses(Globals.CurrentIdentity)).GetInfo(courseID);

		//	lblCourseNumber.Text = course.Number;
		//	lblCourseName.Text = course.Name;	

			ViewState["navname"] = course.Number;
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
