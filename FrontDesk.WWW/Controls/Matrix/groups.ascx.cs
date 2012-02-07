using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Pages;
using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Student Group management page
	/// </summary>
	public class GroupView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DataGrid dgMemberships;
		protected System.Web.UI.WebControls.DataGrid dgInvitations;
		protected System.Web.UI.WebControls.Label lblInvite;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgUsers;
		protected System.Web.UI.WebControls.Label lblJoinError;
		protected System.Web.UI.WebControls.Button cmdCreate;

		private void Page_Load(object sender, System.EventArgs e) {		
			lblError.Visible = lblJoinError.Visible = false;
		}

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;
			BindData();
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private void PageError(string error) {
			lblError.Text = error;
			lblError.Visible = true;
		}

		private void PageErrorJoinDecline(string error) {
			lblJoinError.Text = error;
			lblJoinError.Visible = true;
		}

		private void BindData() {
			BindUserGrid();
			BindMemberGrid();
			BindInviteGrid();
		}

		private void BindUserGrid() {
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			User.UserList users = 
				new Courses(Globals.CurrentIdentity).GetMembers(courseID, null);
			
			dgUsers.DataSource = users;
			dgUsers.DataBind();
		}

		private void BindMemberGrid() {
			int asstID = GetAsstID();
			Group.GroupList groups = 
				new Users(Globals.CurrentIdentity).GetGroups(Globals.CurrentUserName, asstID);

			dgMemberships.DataSource = groups;
			dgMemberships.DataBind();
		}

		private void BindInviteGrid() {
			int asstID = GetAsstID();
			Invitation.InvitationList invites = 
				new Users(Globals.CurrentIdentity).GetInvitations(Globals.CurrentUserName, asstID);

			dgInvitations.DataSource = invites;
			dgInvitations.DataBind();
		}

		private ArrayList HarvestUserNames() {
			ArrayList usernames = new ArrayList();
			int i;
			for (i = 0; i < dgUsers.Items.Count; i++) {
				if ((dgUsers.Items[i].FindControl("Invite") as CheckBox).Checked) {
					Label iname = dgUsers.Items[i].FindControl("UserName") as Label;
					usernames.Add(iname.Text);
				}
			}
			return usernames;
		}

		protected string GetGroupMembers(Group group) {
			User.UserList users = new Groups(Globals.CurrentIdentity).GetMembership(group.PrincipalID);
			string ulist=String.Empty;

			foreach (User user in users) 
				ulist += user.UserName + " ";
			
			return ulist;
		}

		private void dgInvitations_JoinDecline(object sender, DataGridCommandEventArgs ev) {
			
			int invid = (int) dgInvitations.DataKeys[ev.Item.ItemIndex];
			int courseID = Convert.ToInt32(HttpContext.Current.Request.Params["CourseID"]);
			int groupID = Convert.ToInt32(((ev.Item.FindControl("lblGroupID") as Label).Text));
			try {
				if ((ev.CommandSource as LinkButton).Text == "Join")
					new Groups(Globals.CurrentIdentity).AcceptInvitation(courseID, groupID, invid);
				else
					new Groups(Globals.CurrentIdentity).DeclineInvitation(invid);
			} catch (DataAccessException e) {
				PageErrorJoinDecline(e.Message);
			}

			BindData();
		}

		private void dgMemberships_Leave(object sender, DataGridCommandEventArgs ev) {

			int groupid = (int) dgMemberships.DataKeys[ev.Item.ItemIndex];
			try {
				new Groups(Globals.CurrentIdentity).Leave(Globals.CurrentUserName, groupid);
			} catch (DataAccessException e) {
				PageError(e.Message);
			}

			BindData();
		}

		private void cmdCreate_Click(object sender, EventArgs ev) {
			if (txtName.Text.Length == 0)
				PageError("Must enter a group name!");
			else {
				int asstID = GetAsstID();
				try {
					new Groups(Globals.CurrentIdentity).Create(txtName.Text, Globals.CurrentUserName,
						asstID, HarvestUserNames());
				} catch (DataAccessException e) {
					PageError(e.Message);
				}
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
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			this.dgMemberships.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgMemberships_Leave);
			this.dgInvitations.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgInvitations_JoinDecline);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
