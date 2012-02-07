using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Admin.Pagelets {

	/// <summary>
	///	Administer courses
	/// </summary>
	public class AdminCoursePagelet : Pagelet {

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtNumber;
		protected System.Web.UI.WebControls.ListBox lstInstructor;
		protected System.Web.UI.WebControls.Button cmdCreate;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblBackups;
		protected System.Web.UI.WebControls.DataGrid dgCourseList;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = lblBackups.Visible = false;
			if (!IsPostBack) {
				BindUsers();
				BindData();
			}
		}

		protected void BindData() {
			dgCourseList.DataSource = (new Courses(Globals.CurrentIdentity)).GetAll();
			dgCourseList.DataBind();
		}

		protected void BindUsers() {
			User.UserList users = new Users(Globals.CurrentIdentity).GetAll();
			lstInstructor.Items.Clear();
			foreach (User user in users)
				lstInstructor.Items.Add(
					new ListItem(user.FullName + " (" + user.UserName + ")", user.UserName));
		}

		public void dgCourseList_Edit(object send, DataGridCommandEventArgs e) {
			dgCourseList.EditItemIndex = (int) e.Item.ItemIndex;
			BindData();
		}

		public void dgCourseList_Update(object sender, DataGridCommandEventArgs e) {
			
			Course course = new Course();
			Courses courseda = new Courses(Globals.CurrentIdentity);

			course = courseda.GetInfo((int) dgCourseList.DataKeys[e.Item.ItemIndex]);
			course.Name = ((TextBox)(e.Item.Cells[1].Controls[1])).Text;
			course.Number = (e.Item.Cells[2].Controls[1] as TextBox).Text;

			try {
				courseda.Update(course);
			} catch (CustomException er) {
				PageError(er.Message);
			}

			dgCourseList.EditItemIndex = -1;
			BindData();
		}

		public void dgCourseList_Delete(object sender, DataGridCommandEventArgs e) {
			
			int courseID = (int) dgCourseList.DataKeys[e.Item.ItemIndex];

			try {
				new Courses(Globals.CurrentIdentity).Delete(courseID);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			dgCourseList.EditItemIndex = -1;
			BindData();
		}

		public void dgCourseList_Cancel(object sender, DataGridCommandEventArgs e) {
			dgCourseList.EditItemIndex = -1;
			BindData();
		}

		protected void cmdCreate_Click(object sender, System.EventArgs e) {
			
			string name = txtName.Text;
			string number = txtNumber.Text;
			string instructor = lstInstructor.SelectedItem.Value;

			try {
				(new Courses(Globals.CurrentIdentity)).Create(name, number, instructor);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}
	
			BindData();
		}

		private void PageError(string msg) {
			lblError.Visible = true;
			lblError.Text = msg;
		}

		private void BackupPageError(string msg) {
			lblBackups.Visible = true;
			lblBackups.Text = msg;
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
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			this.dgCourseList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCourseList_ItemCommand);
			this.dgCourseList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCourseList_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgCourseList_ItemCommand(object source, DataGridCommandEventArgs e) {
			if (e.CommandName == "Backup") {
				int courseID = (int) dgCourseList.DataKeys[e.Item.ItemIndex];
				try {
					string fname = new Courses(Globals.CurrentIdentity).Backup(courseID);
					BackupPageError("Backup completed successfully. Visit the Backups " +
						"tab to download the file: " + fname);
				} catch (CustomException er) {
					PageError(er.Message);
				} 
			} else if (e.CommandName == "Available") {
				int courseID = (int) dgCourseList.DataKeys[e.Item.ItemIndex];
				Courses courseda = new Courses(Globals.CurrentIdentity);
				Course course = courseda.GetInfo(courseID);
				course.Available = !course.Available;
				try {
					courseda.Update(course);
				} catch (CustomException er) {
					PageError(er.Message);
				}

				BindData();
			}
		}

		private void dgCourseList_ItemDataBound(object sender, DataGridItemEventArgs e) {
			LinkButton lnkAvail;
			Label lblName;

			if (null != (lblName = (Label) e.Item.FindControl("lblName"))) {
				Course course = (Course) e.Item.DataItem;
				if (course.Available) 
					lblName.Text = course.Name;
				else
					lblName.Text = course.Name + " <b>(Unavailable)</b>";

				if (null != (lnkAvail = (LinkButton) e.Item.FindControl("lnkAvail"))) {
					if (course.Available)
						lnkAvail.Text = "Mark Unavailable";
					else
						lnkAvail.Text = "Mark Available";
				}
			}
		}

	}
}
