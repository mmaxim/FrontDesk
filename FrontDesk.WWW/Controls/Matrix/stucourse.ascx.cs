using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Student course assignment main pagelet
	/// </summary>
	public class StudentCourseView : Pagelet, IMatrixControl {
		
		protected System.Web.UI.WebControls.DataList dlAnnouncements;
		protected System.Web.UI.WebControls.Label lblNumber;
		protected System.Web.UI.WebControls.Label lblName;
		protected System.Web.UI.WebControls.DataGrid dgAssignments;

		private void Page_Load(object sender, System.EventArgs e) {

		}

		public event RefreshEventHandler Refresh;

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		private void BindData() {
			int courseID = 	GetCourseID();
			Courses courseDB = (new Courses(Globals.CurrentIdentity));
			Course course = courseDB.GetInfo(courseID);

			lblNumber.Text = course.Number;
			lblName.Text = course.Name;

			dgAssignments.DataSource = courseDB.GetStudentAssignments(courseID);
			dgAssignments.DataBind();

			dlAnnouncements.DataSource = courseDB.GetAnnouncements(courseID);
			dlAnnouncements.DataBind();
		}

		public void Activate(ActivateParams ap) {
			ViewState["courseID"] = ap.ID;
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
			this.dlAnnouncements.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlAnnouncements_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dlAnnouncements_ItemDataBound(object sender, DataListItemEventArgs e) {
			Label lblPoster;
			if (null != (lblPoster = (Label) e.Item.FindControl("lblPoster"))) {
				Announcement ann = e.Item.DataItem as Announcement;
				User user = (new Users(Globals.CurrentIdentity)).GetInfo(ann.Poster, null);
				
				lblPoster.Text = user.FullName + " (" + user.UserName + ")";
			}
		}
	}
}
