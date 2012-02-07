using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Components.Filesys;
using FrontDesk.Common;
using FrontDesk.Pages;
using FrontDesk.Data.Filesys;

namespace FrontDesk.Controls.Filesys {
	
	/// <summary>
	///	File permissions control
	/// </summary>
	public class FilePermissionsControl : Pagelet {
		protected System.Web.UI.WebControls.DropDownList ddlPrins;
		protected System.Web.UI.WebControls.CheckBox chkRead;
		protected System.Web.UI.WebControls.CheckBox chkWrite;
		protected System.Web.UI.WebControls.Button cmdUpdate;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.CheckBox chkDelete;

		private void Page_Load(object sender, EventArgs e) {
			lblError.Visible = false;	
		}

		public int FileID {
			get { return (int) ViewState["fileID"]; }
			set { ViewState["fileID"] = value; }
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		public void BindData() {	
			BindPrincipals();
			BindBoxes();	
		}

		private void BindBoxes() {
			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			//Get perms
			CFilePermission.FilePermissionList perms = fs.GetPermissions(
				fs.GetFile(FileID), Convert.ToInt32(ddlPrins.SelectedItem.Value));

			chkRead.Checked = chkWrite.Checked = chkDelete.Checked = false;
			foreach (CFilePermission perm in perms)
				if (perm.Grant) {
					if (perm.Action == FileAction.READ)
						chkRead.Checked = true;
					if (perm.Action == FileAction.WRITE)
						chkWrite.Checked = true;
					if (perm.Action == FileAction.DELETE)
						chkDelete.Checked = true;
				}
		}

		private void BindPrincipals() {
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			Courses courseda = new Courses(Globals.CurrentIdentity);
			User.UserList users = courseda.GetMembers(courseID, null);
			CourseRole.CourseRoleList roles = courseda.GetRoles(courseID, null);
			
			ddlPrins.Items.Clear();
			//Add roles
			foreach (CourseRole role in roles) {
				ListItem item = new ListItem("Role: " + role.Name, role.PrincipalID.ToString());
				ddlPrins.Items.Add(item);
			}

			//Add users
			foreach (User user in users) {
				ListItem item = new ListItem(user.FullName + " (" + user.UserName + ")", user.PrincipalID.ToString());
				ddlPrins.Items.Add(item);
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
			this.ddlPrins.SelectedIndexChanged += new System.EventHandler(this.ddlPrins_SelectedIndexChanged);
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cmdUpdate_Click(object sender, System.EventArgs e) {
			
			CFilePermission.FilePermissionList perms = new CFilePermission.FilePermissionList();
			int principalID = Convert.ToInt32(ddlPrins.SelectedItem.Value);
			perms.Add(new CFilePermission(principalID, FileAction.READ, chkRead.Checked));
			perms.Add(new CFilePermission(principalID, FileAction.WRITE, chkWrite.Checked));
			perms.Add(new CFilePermission(principalID, FileAction.DELETE, chkDelete.Checked));

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);
			try {
				fs.SetPermissions(fs.GetFile(FileID), perms);
			} catch (CustomException er) {
				PageError(er.Message);
			}

			BindBoxes();
		}

		private void ddlPrins_SelectedIndexChanged(object sender, System.EventArgs e) {
			BindBoxes();
		}
	}
}
