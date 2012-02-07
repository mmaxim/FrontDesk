using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	User info page for users
	/// </summary>
	public class DefaultUserInfoPagelet : Pagelet {

		protected System.Web.UI.WebControls.Label lblUserName;
		protected System.Web.UI.WebControls.TextBox txtFirst;
		protected System.Web.UI.WebControls.TextBox txtOldPassword;
		protected System.Web.UI.WebControls.TextBox txtNewPassword;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.Button cmdUpdate;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtRepeat;
		protected System.Web.UI.WebControls.TextBox txtLast;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			User curuser = new Users(Globals.CurrentIdentity).GetInfo(Globals.CurrentUserName, null);
			
			lblUserName.Text = "Username: <b>" + curuser.UserName + "</b>";
			txtFirst.Text = curuser.FirstName;
			txtLast.Text = curuser.LastName;
			txtEmail.Text = curuser.Email;
		}

		private void PageError(string text) {
			lblError.Text = text;
			lblError.Visible = true;
			return;
		}

		private void cmdUpdate_Click(object sender, System.EventArgs e) {
		
			//Validate password
			if (txtNewPassword.Text.Length > 0) {
				if (!(new Users(Globals.CurrentIdentity)).IsValid(Globals.CurrentUserName, txtOldPassword.Text)) {
					PageError("Error: Old password is incorrect");
					return;
				}
				if (txtNewPassword.Text != txtRepeat.Text){
					PageError("Error: New passwords do not match (Make sure you type carefully!)");
					return;
				}
				
				try {
					(new Users(Globals.CurrentIdentity)).ChangePassword(Globals.CurrentUserName, txtNewPassword.Text);
				} catch (DataAccessException er) {
					PageError(er.Message); return;
				}
			}

			User user = new User();
			user.FirstName = txtFirst.Text; user.LastName = txtLast.Text;
			user.Email = txtEmail.Text; user.UserName = Globals.CurrentUserName;
			user.VerifyKey = "";
			
			try {
				(new Users(Globals.CurrentIdentity)).Update(user, null);
			} catch (DataAccessException er) {
				PageError(er.Message); return;
			}
			
			BindData();
			PageError("Success!");
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
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
