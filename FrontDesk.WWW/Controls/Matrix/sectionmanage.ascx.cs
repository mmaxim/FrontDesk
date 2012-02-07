using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	///	Section membership management
	/// </summary>
	public class SectionManagementView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.CheckBox chkSwitch;
		protected System.Web.UI.WebControls.Label lblMemError;
		protected System.Web.UI.WebControls.Button cmdDrop;
		protected System.Web.UI.WebControls.Button cmdAdd;
		protected System.Web.UI.WebControls.ListBox lstAllUsers;
		protected System.Web.UI.WebControls.LinkButton lnkSecExpl;
		protected System.Web.UI.WebControls.ListBox lstSectionUsers;

		private void Page_Load(object sender, System.EventArgs e) {
			lblMemError.Visible = false;
		}

		private int GetSectionID() {
			return (int) ViewState["sectID"];
		}

		private void BindData() {

			Sections sectda = new Sections(Globals.CurrentIdentity);
			Section sect = sectda.GetInfo(GetSectionID());
			
			User.UserList secs = sectda.GetMembers(sect.ID);
			lstSectionUsers.Items.Clear();
			foreach (User user in secs)
				lstSectionUsers.Items.Add(
					new ListItem(user.FullName + " (" + user.UserName + ")", user.UserName));
		
			User.UserList mems = 
				new Courses(Globals.CurrentIdentity).GetMembers(sect.CourseID, null);
			lstAllUsers.Items.Clear();
			foreach (User user in mems)
				lstAllUsers.Items.Add(
					new ListItem(user.FullName + " (" + user.UserName + ")", user.UserName));

			lnkSecExpl.Attributes.Add("onClick", 
				@"window.open('sectionexpl.aspx?CourseID=" + sect.CourseID + 
				@"', '"+sect.CourseID+@"', 'width=430, height=530')");
		}	

		private void cmdAdd_Click(object sender, System.EventArgs e) {

			int sectionID = GetSectionID();
			bool switchu = chkSwitch.Checked;
			ArrayList users = GetSelectedUsers(lstAllUsers);

			lblMemError.Visible = false;
			Sections sectionda = new Sections(Globals.CurrentIdentity);
			try {
				foreach (string user in users)
					sectionda.AddUser(sectionID, user, switchu);
			} catch (DataAccessException er) {
				lblMemError.Text = er.Message;
				lblMemError.Visible = true;
			}

			BindData();
			if (Refresh != null)
				Refresh(this, new RefreshEventArgs());
		}

		private ArrayList GetSelectedUsers(ListBox box) {
			ArrayList users = new ArrayList();
			foreach (ListItem item in box.Items) {
				if (item.Selected)
					users.Add(item.Value);
			}
			return users;
		}

		private void cmdDrop_Click(object sender, System.EventArgs e) {
		
			int sectionID = GetSectionID();
			ArrayList users = GetSelectedUsers(lstSectionUsers);

			lblMemError.Visible = false;
			Sections sectionda = new Sections(Globals.CurrentIdentity);
			try {
				foreach (string user in users)
					sectionda.DropUser(sectionID, user);
			} catch (DataAccessException er) {
				lblMemError.Text = er.Message;
				lblMemError.Visible = true;
			}

			BindData();
			if (Refresh != null)
				Refresh(this, new RefreshEventArgs());
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
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			this.cmdDrop.Click += new System.EventHandler(this.cmdDrop_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["sectID"] = ap.ID;
			BindData();
		}

	}
}
