using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SMS.Windows.Forms;

using FrontDesk.Applications.Submission.UserService;

namespace FrontDesk.Applications.Submission {

	public class LoginPage : ExteriorWizardPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ErrorProvider erProvider;

		private System.ComponentModel.IContainer components = null;

		public LoginPage() {
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing)
				if (components != null) 
					components.Dispose();
				
			base.Dispose(disposing);
		}

		protected override bool OnSetActive() {
			if (!base.OnSetActive())
				return false;
            
			Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next );
			this.Focus();
			return true;
		}

		protected override string OnWizardNext() {

			UserDataService uds = new UserDataService();
			
			if (!ValidateInput()) 
				return "LoginPage";
			uds.Url = String.Format("{0}/userdatasvc.asmx", txtURL.Text);
			Cursor.Current = Cursors.WaitCursor;
			try {
				((WizardMain)Parent).Ticket = uds.Authenticate(txtUsername.Text, txtPassword.Text);	
				((WizardMain)Parent).Url = txtURL.Text;
			} catch (Exception er) {
				MessageBox.Show("Error during Authentication: " + er.Message, 
					"Error connecting to FrontDesk", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				Cursor.Current = Cursors.Arrow;
				return "LoginPage";
			}

			Cursor.Current = Cursors.Arrow;
			return base.OnWizardNext();
		}

		private bool ValidateInput() {
			if (txtURL.Text.Length == 0) {
				erProvider.SetError(txtURL, "Please enter a URL");
				return false;
			}
			if (txtUsername.Text.Length == 0) {
				erProvider.SetError(txtUsername, "Please enter a username");
				return false;
			}
			if (txtPassword.Text.Length == 0) {
				erProvider.SetError(txtPassword, "Please enter a password");
				return false;
			}

			try {
				new Uri(txtURL.Text);
			} catch (Exception) {
				erProvider.SetError(txtURL, "Invalid URL specified");
				return false;
			}

			return true;
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LoginPage));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.erProvider = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Name = "m_titleLabel";
			this.m_titleLabel.Text = "Welcome to the FrontDesk Submission System";
			// 
			// m_watermarkPicture
			// 
			this.m_watermarkPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_watermarkPicture.Image")));
			this.m_watermarkPicture.Name = "m_watermarkPicture";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(184, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(288, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "This wizard will guid you through the process of submitting your work to the Fron" +
				"tDesk Web site.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(184, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(288, 40);
			this.label2.TabIndex = 3;
			this.label2.Text = "Enter the FrontDesk Submission URL (typically in the form http://<hostname>/Front" +
				"DeskServices) and your FrontDesk username and password below to begin.";
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(256, 200);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(192, 20);
			this.txtUsername.TabIndex = 2;
			this.txtUsername.Text = "";
			this.txtUsername.Validating += new System.ComponentModel.CancelEventHandler(this.txtUsername_Validating);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(256, 232);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(192, 20);
			this.txtPassword.TabIndex = 3;
			this.txtPassword.Text = "";
			this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(192, 200);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Username:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(192, 232);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Password:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(184, 280);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Click Next to continue";
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(256, 168);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(192, 20);
			this.txtURL.TabIndex = 1;
			this.txtURL.Text = "http://localhost/FrontDeskServices";
			this.txtURL.Validating += new System.ComponentModel.CancelEventHandler(this.txtURL_Validating);
			this.txtURL.Validated += new System.EventHandler(this.txtURL_Validated);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(216, 168);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 10;
			this.label6.Text = "URL:";
			// 
			// erProvider
			// 
			this.erProvider.ContainerControl = this;
			this.erProvider.DataMember = "\"\"";
			// 
			// LoginPage
			// 
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "LoginPage";
			this.Load += new System.EventHandler(this.LoginPage_Load);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.txtUsername, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_watermarkPicture, 0);
			this.Controls.SetChildIndex(this.txtPassword, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			this.Controls.SetChildIndex(this.txtURL, 0);
			this.Controls.SetChildIndex(this.label6, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private void txtURL_Validating(object sender, CancelEventArgs e) {
		
			try {
				new Uri(txtURL.Text);
			} catch (Exception) {
				e.Cancel = true;
				erProvider.SetError(txtURL, "Invalid URL specified");
				return;
			}

			erProvider.SetError(txtURL, "");
		}

		private void txtURL_Validated(object sender, System.EventArgs e) {

		}

		private void LoginPage_Load(object sender, System.EventArgs e) {

		}

		private void txtUsername_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if (txtUsername.Text.Length == 0) {
				e.Cancel = true;
				erProvider.SetError(txtUsername, "Please enter a username");
			}
			else
				erProvider.SetError(txtUsername, "");
		}

		private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if (txtPassword.Text.Length == 0) {
				e.Cancel = true;
				erProvider.SetError(txtPassword, "Please enter a password");
			}
			else
				erProvider.SetError(txtPassword, "");
		}
	}
}

