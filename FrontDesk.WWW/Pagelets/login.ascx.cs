using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.Security;

using FrontDesk.Data.Access;
using FrontDesk.Common;
using FrontDesk.Pages;
using FrontDesk.Components;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	Login page
	/// </summary>
	public class LoginPagelet : Pagelet {

		protected System.Web.UI.WebControls.Label lblUserName;
		protected System.Web.UI.WebControls.Label lblPassword;
		protected System.Web.UI.WebControls.Button cmdLogin;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.LinkButton lnkForgot;
		protected System.Web.UI.WebControls.TextBox txtVerifyKey;
		protected System.Web.UI.WebControls.Label lblVerifyKey;
		protected System.Web.UI.WebControls.TextBox txtUsername;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
			lblVerifyKey.Visible = false;
			txtVerifyKey.Visible = false;
		}

		private void PageError(string msg) {
			lblError.Visible = true;
			lblError.Text = msg;
		}

		private void cmdLogin_Click(object sender, System.EventArgs e) {
			Users userda = new Users(Globals.CurrentIdentity);
			if (userda.IsValid(txtUsername.Text, txtPassword.Text)) {
				User user = userda.GetInfo(txtUsername.Text, null);
				if (user.VerifyKey != "" && txtVerifyKey.Text == "") {
					PageError("You must activate your account now. Enter the verification key found in the FrontDesk activiation email and enter it into the Verify Key box and log in again.");
					lblVerifyKey.Visible = true;
					txtVerifyKey.Visible = true;
				} else {
					if (user.VerifyKey != txtVerifyKey.Text)
						PageError("Incorrect verification key, please check the email to make sure you typed it in correctly");
					else {
						// if set to false, you have to login each time
						FormsAuthentication.SetAuthCookie(txtUsername.Text, true);
						string redirectUrl = Page.Request.QueryString["ReturnUrl"];

						// Update verification key to ""
						user.VerifyKey = "";
						userda.Update(user, null);

						if (redirectUrl != null) {
							Page.Response.Redirect(redirectUrl);
							Page.Response.End();
						} else {
							Page.Response.Redirect(Globals.DefaultUrl);
							Page.Response.End();
						}
					}
				}
			}
			else
				PageError("Username or password are incorrect. Try again, or if you have not registered, click the Register tab above to obtain an account.");
		
			txtVerifyKey.Text = "";
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
			this.cmdLogin.Click += new System.EventHandler(this.cmdLogin_Click);
			this.lnkForgot.Click += new System.EventHandler(this.lnkForgot_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lnkForgot_Click(object sender, System.EventArgs e) {
			try {
				new Users(Globals.CurrentIdentity).ForgotPassword(txtUsername.Text);
			} catch (DataAccessException er) {
				PageError(er.Message);
				return;
			}

			PageError("Password sent successfully.");
		}

	}
}
