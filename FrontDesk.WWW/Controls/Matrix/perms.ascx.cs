using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	/// Permissions matrix view
	/// </summary>
	public class PermissionsView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.DataGrid dgRoles;
		protected System.Web.UI.WebControls.Button cmdApply;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblPrin;
		protected System.Web.UI.WebControls.DataGrid dgPerms;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
		}

		private void PageError(string msg) {
			lblError.Visible = true;
			lblError.Text = msg;
		}

		private int GetID() {
			return (int) ViewState["id"];
		}

		private string GetObjectType() {
			return (string) ViewState["type"];
		}

		private int GetCourseID() {
			string type = GetObjectType();
			if (type == Permission.ASSIGNMENT)
				return new Assignments(Globals.CurrentIdentity).GetInfo(GetID()).CourseID;
			else if (type == Permission.SECTION)
				return new Sections(Globals.CurrentIdentity).GetInfo(GetID()).CourseID;
			else
				return GetID();
		}

		private void BindData() {

			int i;
			int courseID = GetCourseID();
			Courses courseda = new Courses(Globals.CurrentIdentity);
			User.UserList users =  courseda.GetStaff(courseID, null);
			CourseRole.CourseRoleList roles = courseda.GetRoles(courseID, null);
			
			Principal.PrincipalList prins = new Principal.PrincipalList();
			prins.AddRange(roles); prins.AddRange(users); 

			dgRoles.DataSource = prins;
			dgRoles.DataBind();	

			for (i = 0; i < roles.Count; i++) {
				if (roles[i].Staff) {
					dgRoles.SelectedIndex = i;
					BindPermissions();
					break;
				}
			}
		}

		private void BindPermissions() {
			Permission.PermissionList perms =
				new Permissions(Globals.CurrentIdentity).GetTypePermissions(GetObjectType());

			int prinID = (int) dgRoles.DataKeys[dgRoles.SelectedIndex];
			Principal prin = new Principals(Globals.CurrentIdentity).GetInfo(prinID);
			lblPrin.Text = "Selection: " + prin.Name;

			dgPerms.DataSource = perms;
			dgPerms.DataBind();
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
			this.dgRoles.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRoles_ItemCommand);
			this.dgRoles.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgRoles_PageIndexChanged);
			this.dgRoles.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgRoles_ItemDataBound);
			this.dgPerms.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgPerms_ItemDataBound);
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event FrontDesk.Controls.Matrix.RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["id"] = ap.ID;
			ViewState["type"] = ap.Auxiliary;

			BindData();
		}

		private void dgRoles_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblType, lblName;
			LinkButton lnkPermissions;
			if (null != (lblType = (Label) e.Item.FindControl("lblType"))) {
				lblName = (Label) e.Item.FindControl("lblName");
				lnkPermissions = (LinkButton) e.Item.FindControl("lnkPermissions");
				Principal prin = (Principal) e.Item.DataItem;
				if (prin is User) {
					User user = prin as User;
					lblType.Text = "User";
					lblName.Text = user.FullName + " (" + user.UserName + ")";
				} else {
					CourseRole role = prin as CourseRole;
					lblType.Text = "Role"; lblType.Font.Bold = true;
					lblName.Text = prin.Name; lblName.Font.Bold = true;
					if (!role.Staff)
						lnkPermissions.Enabled = false;
				}
			}
		}

		private void dgRoles_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgRoles.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgRoles_ItemCommand(object source, DataGridCommandEventArgs e) {
			if (e.CommandName == "Permissions") {
				dgRoles.SelectedIndex = e.Item.ItemIndex;
				BindPermissions();
			}
		}

		private void dgPerms_ItemDataBound(object sender, DataGridItemEventArgs e) {
			CheckBox chkGrant;
			if (null != (chkGrant = (CheckBox) e.Item.FindControl("chkGrant"))) {
				int prinID = (int) dgRoles.DataKeys[dgRoles.SelectedIndex];
				Permission perm = e.Item.DataItem as Permission;
				Permission.GrantType grant = 
					new Permissions(Globals.CurrentIdentity).CheckPermission(prinID, GetCourseID(),
						GetObjectType(), perm.Name, GetID());
				switch (grant) {
				case Permission.GrantType.DENY:
					chkGrant.Checked = false;
					break;
				case Permission.GrantType.INHERIT:
					chkGrant.Checked = true;
					chkGrant.Enabled = false;
					break;
				case Permission.GrantType.DIRECT:
					chkGrant.Checked = true;
					break;
				}
			}
		}

		private void cmdApply_Click(object sender, System.EventArgs e) {
			CheckBox chkGrant;
			Permission.GrantType grant;
			string perm, otype = GetObjectType();;
			int prinID = (int) dgRoles.DataKeys[dgRoles.SelectedIndex], oid = GetID();
			int courseID = GetCourseID();
			Permissions permda = new Permissions(Globals.CurrentIdentity);
			foreach (DataGridItem item in dgPerms.Items) {
				perm = (string) dgPerms.DataKeys[item.ItemIndex];
				grant = new Permissions(Globals.CurrentIdentity).CheckPermission(prinID, courseID,
					otype, perm, oid);
				chkGrant = (CheckBox) item.FindControl("chkGrant");
				try {
					if (chkGrant.Checked && grant == Permission.GrantType.DENY)
						permda.Assign(prinID, otype, perm, oid, courseID);
					else if (!chkGrant.Checked && grant == Permission.GrantType.DIRECT)
						permda.Deny(prinID, otype, perm, oid, courseID);
				} catch (DataAccessException er) {
					PageError(er.Message);
					break;
				}
			}
			BindPermissions();
		}

	}
}
