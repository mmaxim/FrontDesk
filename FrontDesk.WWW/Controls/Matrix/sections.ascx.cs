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
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Section view
	/// </summary>
	public class SectionsView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.LinkButton lnkSecExpl;
		protected System.Web.UI.WebControls.DataGrid dgSections;
		protected System.Web.UI.WebControls.Button cmdBatchSubmit;
		protected System.Web.UI.WebControls.CheckBox chkMerge;
		protected System.Web.UI.HtmlControls.HtmlInputFile fiUserList;
		protected System.Web.UI.WebControls.Label lblBatchError;
		protected System.Web.UI.WebControls.DataGrid dgAllUsers;
		protected System.Web.UI.WebControls.Label lblAddError;
		protected System.Web.UI.WebControls.Label lblEditError;

		private void Page_Load(object sender, EventArgs e) {
			lblEditError.Visible = lblBatchError.Visible = lblAddError.Visible = false;
			if (!IsPostBack && ViewState["courseID"] != null) {
				lnkSecExpl.Attributes.Add("onClick", 
					@"window.open('sectionexpl.aspx?CourseID=" + GetCourseID() + 
					@"', '"+GetCourseID()+@"', 'width=430, height=530')");
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgSections.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgSections_Cancel);
			this.dgSections.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgSections_Edit);
			this.dgSections.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgSections_Update);
			this.dgSections.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgSections_Delete);
			this.cmdBatchSubmit.Click += new System.EventHandler(this.cmdBatchSubmit_Click);
			this.dgAllUsers.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllUsers_ItemCommand);
			this.dgAllUsers.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgAllUsers_PageIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		private void PageError(string msg) {
			lblEditError.Text = msg;
			lblEditError.Visible = true;
		}

		private void PageBatchError(string msg) {
			lblBatchError.Text = msg;
			lblBatchError.Visible = true;
		}

		private void PageAddError(string msg) {
			lblAddError.Text = msg;
			lblAddError.Visible = true;
		}

		private void BindData() {
		
			Section.SectionList secs = 
				new Courses(Globals.CurrentIdentity).GetSections(GetCourseID());

			dgSections.DataSource = secs;
			dgSections.DataBind();

			lnkSecExpl.Attributes.Add("onClick", 
				@"window.open('sectionexpl.aspx?CourseID=" + GetCourseID() + 
				@"', '"+GetCourseID()+@"', 'width=430, height=530')");

			BindAllUsers();
		}

		private void BindAllUsers() {
			User.UserList users = (new Users(Globals.CurrentIdentity)).GetAll();

			dgAllUsers.DataSource = users;
			dgAllUsers.DataBind();
		}

		protected void dgSections_Edit(object sender, DataGridCommandEventArgs e) {
			dgSections.EditItemIndex = (int) e.Item.ItemIndex;
			
			BindData();
		}

		protected User.UserList GetEditOwnerList() {
			return new Courses(Globals.CurrentIdentity).GetStaff(GetCourseID(), null);
		}

		protected int GetEditOwnerIndex(string owner) {
			int i;
			int courseID = GetCourseID();
			User.UserList staff = new Courses(Globals.CurrentIdentity).GetStaff(courseID, null);
			
			for (i = 0; i < staff.Count; i++) 
				if (staff[i].UserName == owner)
					return i;
			
			return 0;
		}

		protected void dgSections_Update(object sender, DataGridCommandEventArgs e) {
			Section sec = new Section();
			
			sec.ID = (int) dgSections.DataKeys[e.Item.ItemIndex];
			sec.Name = ((TextBox)e.Item.FindControl("txtName")).Text;
			sec.CourseID = GetCourseID();
			sec.Owner = ((TextBox)e.Item.FindControl("txtOwner")).Text;
			
			lblEditError.Visible = false;
			try {
				new Sections(Globals.CurrentIdentity).Update(sec);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			dgSections.EditItemIndex = -1;

			Refresh(this, new RefreshEventArgs());
			BindData();
		}

		protected void dgSections_Cancel(object sender, DataGridCommandEventArgs e) {
			dgSections.EditItemIndex = -1;

			BindData();
		}

		private void cmdBatchSubmit_Click(object sender, EventArgs ev) {
			if (fiUserList.PostedFile.InputStream.Length == 0)
				PageBatchError("Please select an XML user file"); 
			else {
				try {
					int courseID = 	Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
					bool sync = chkMerge.Checked;
					(new Courses(Globals.CurrentIdentity)).BatchAddUsers(fiUserList.PostedFile.InputStream, courseID, sync);
				} catch (DataAccessException e) {
					PageBatchError(e.Message);
				}

				BindData();
			}
		}

		private void dgAllUsers_ItemCommand(object source, DataGridCommandEventArgs e) {
			
			if (e.CommandName != "Add") return;
			int courseID = GetCourseID();
			string user = (string) dgAllUsers.DataKeys[e.Item.ItemIndex];
			try {
				new Courses(Globals.CurrentIdentity).AddUser(user, "Student", courseID, null);
			} catch (DataAccessException er) {
				PageAddError(er.Message);
			}
			BindData();
		}

		protected void dgSections_Delete(object sender, DataGridCommandEventArgs e) {
			
			int sectionID = Convert.ToInt32(dgSections.DataKeys[e.Item.ItemIndex]);
			
			try {
				new Sections(Globals.CurrentIdentity).Delete(sectionID);
			} catch (DataAccessException er) {
				PageError(er.Message);
				return;
			}

			BindData();
			Refresh(this, new RefreshEventArgs());
		}

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["courseID"] = ap.ID;
			BindData();
		}

		private void dgAllUsers_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgAllUsers.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
