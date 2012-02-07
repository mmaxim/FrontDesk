using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Data.Access;
using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {

	/// <summary>
	///	Register pagelet
	/// </summary>
	public class RegisterPagelet : Pagelet {

		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtLastName;
		protected System.Web.UI.WebControls.TextBox txtRepPassword;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtFirstName;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button cmdRegister;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
		}

		private void cmdRegister_Click(object sender, System.EventArgs e) {
			
			if (txtUsername.Text.Length == 0 || txtPassword.Text.Length == 0 ||
				txtEmail.Text.Length == 0 || txtLastName.Text.Length == 0 ||
				txtRepPassword.Text.Length == 0 || txtFirstName.Text.Length == 0)
				lblError.Text = "All field are required. Please fill in the missing fields.";
			else if (txtPassword.Text != txtRepPassword.Text) 
				lblError.Text = "Repeated password is different from initial password.";
			else if (txtUsername.Text.IndexOf(" ") >= 0)
				lblError.Text = "Username cannot have spaces";
			else {
				try {
					new Users(Globals.CurrentIdentity).Create(txtUsername.Text, txtPassword.Text, 
						txtFirstName.Text, txtLastName.Text, txtEmail.Text, null);													   
					lblError.Text = "Thank you for registering! Please check your email for the email verification message and login by clicking on the login tab.";
				} catch (Exception er) {
					lblError.Text = er.Message;				
				}
			}
			lblError.Visible = true;
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
			this.cmdRegister.Click += new System.EventHandler(this.cmdRegister_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
