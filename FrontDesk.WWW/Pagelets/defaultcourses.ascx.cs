using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	Course management page for users
	/// </summary>
	public class DefaultCoursesPagelet : Pagelet {

		protected System.Web.UI.WebControls.Label lblName;
		protected System.Web.UI.WebControls.DataGrid dgAllCourses;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgUserCourses;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			dgUserCourses.DataSource = new Users(Globals.CurrentIdentity).GetCourses(Globals.CurrentUserName);
			dgUserCourses.DataBind();

			dgAllCourses.DataSource = new Courses(Globals.CurrentIdentity).GetAll();
			dgAllCourses.DataBind();
		}

		private void dgAllCourses_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e) {
			dgAllCourses.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgAllCourses_ItemCommand(object source, DataGridCommandEventArgs e) {
		
			if (e.CommandName != "Join") return;
			int courseID = (int) dgAllCourses.DataKeys[e.Item.ItemIndex];
			try {
				new Courses(Globals.CurrentIdentity).AddUser(Globals.CurrentUserName, "Student", courseID, null);
			} catch (DataAccessException er) {
				lblError.Text = "Error: " + er.Message;
				lblError.Visible = true;
			}

			BindData();
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgUserCourses.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserCourses_ItemDataBound);
			this.dgAllCourses.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllCourses_ItemCommand);
			this.dgAllCourses.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgAllCourses_PageIndexChanged);
			this.dgAllCourses.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgAllCourses_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgUserCourses_ItemDataBound(object sender, DataGridItemEventArgs e) {
			HyperLink hypAdmin, hypStudent, hypStudentAdmin;
			if (null != (hypAdmin = (HyperLink) e.Item.FindControl("hypAdmin"))) {
				Course course = (Course) e.Item.DataItem;
				CourseRole role = new Courses(Globals.CurrentIdentity).GetRole(
					Globals.CurrentUserName, course.ID, null);
			
				hypStudent = (HyperLink) e.Item.FindControl("hypStudent");
				hypStudentAdmin = (HyperLink) e.Item.FindControl("hypStudentAdmin");

				hypStudent.Enabled = course.Available;
				hypStudentAdmin.Enabled = course.Available;
				if (!hypStudent.Enabled)
					hypStudent.Text += " (Unavailable)";

				if (role.Staff) {
					hypAdmin.NavigateUrl = "../courseadmin.aspx?CourseID="+course.ID;
					hypAdmin.Visible = true;
					hypStudentAdmin.NavigateUrl = "../course.aspx?CourseID="+course.ID;
					hypStudentAdmin.Visible = true;

				} else {
					hypAdmin.NavigateUrl = "../course.aspx?CourseID="+course.ID;
					hypAdmin.Visible = false;
					hypStudentAdmin.Visible = false;
				}
			}
		}

		private void dgAllCourses_ItemDataBound(object sender, DataGridItemEventArgs e) {
			LinkButton lnkJoin;
			Label lblName;
			if (null != (lnkJoin = (LinkButton) e.Item.FindControl("lnkJoin"))) {
				Course course = (Course) e.Item.DataItem;
				lnkJoin.Enabled = course.Available;
				lblName = (Label) e.Item.FindControl("lblName");
				if (!lnkJoin.Enabled)
					lblName.Text += " (Unavailable)";

			}
		}

	}
}
