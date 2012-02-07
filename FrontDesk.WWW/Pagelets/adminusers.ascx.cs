using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Admin.Pagelets {

	/// <summary>
	///	Admin user page
	/// </summary>
	public class AdminUserPagelet : Pagelet {
		
		protected System.Web.UI.WebControls.Button cmdCreate;
		protected System.Web.UI.WebControls.Label lblErrors;
		protected FrontDesk.Controls.FDDataGrid dgAllUsers;
		protected System.Web.UI.WebControls.CheckBox chkMerge;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox TextBox3;
		protected System.Web.UI.HtmlControls.HtmlInputFile fiUserList;

		private void Page_Load(object sender, System.EventArgs e) {
			lblErrors.Visible = false;	
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			dgAllUsers.DataSource = (new Users(Globals.CurrentIdentity)).GetAll();
			dgAllUsers.DataBind();
		}

		private void cmdCreate_Click(object sender, System.EventArgs ev) {
			bool sync = chkMerge.Checked;
			try {
				(new Users(Globals.CurrentIdentity)).BatchCreate(fiUserList.PostedFile.InputStream, sync);
			} catch (DataAccessException e) {
				lblErrors.Text = e.Message;
				lblErrors.Visible = true;
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
			this.dgAllUsers.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllUsers_ItemCommand);
			this.dgAllUsers.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllUsers_EditCommand);
			this.dgAllUsers.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgAllUsers_PageIndexChanged);
			this.dgAllUsers.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllUsers_CancelCommand);
			this.dgAllUsers.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgAllUsers_UpdateCommand);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgAllUsers_CancelCommand(object source, DataGridCommandEventArgs e) {
			dgAllUsers.EditItemIndex = -1;
			BindData();
		}

		private void dgAllUsers_EditCommand(object source, DataGridCommandEventArgs e) {
			dgAllUsers.EditItemIndex = (int) e.Item.ItemIndex;
			BindData();
		}

		private void dgAllUsers_UpdateCommand(object source, DataGridCommandEventArgs e) {
			User user = new User();
				
			user.UserName = (string)dgAllUsers.DataKeys[e.Item.ItemIndex];
			user.Email = ((TextBox)(e.Item.Cells[2].Controls[1])).Text;
			user.FirstName = ((TextBox)(e.Item.Cells[3].Controls[1])).Text;
			user.LastName = ((TextBox)(e.Item.Cells[4].Controls[1])).Text;

			(new Users(Globals.CurrentIdentity)).Update(user, null);
			dgAllUsers.EditItemIndex = -1;
			
			BindData();
		}

		private void dgAllUsers_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgAllUsers.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgAllUsers_ItemCommand(object source, DataGridCommandEventArgs e) {
			if (e.CommandName == "respasswd") {
				try {
					string username = (string) dgAllUsers.DataKeys[e.Item.ItemIndex];
					(new Users(Globals.CurrentIdentity)).ChangePassword(username, 
						Globals.GeneratePassword(Globals.MinPasswordLength));
				} catch (DataAccessException er) {
					lblErrors.Text = er.Message;
					lblErrors.Visible = true;
				} finally {
					lblErrors.Text = "Password reset success!";
					lblErrors.Visible = true;
				}
			}
		}

	}
}
