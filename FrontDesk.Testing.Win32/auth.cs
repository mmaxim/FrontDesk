using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FrontDesk.Testing;

namespace FrontDesk.Testing.Win32 {

	/// <summary>
	/// Authentication form handler
	/// </summary>
	public class AuthForm : Form {

		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cmdLogin;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblError;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AuthForm() {
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing)
				if (components != null)
					components.Dispose();
				
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmdLogin = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lblError = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(80, 48);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(176, 20);
			this.txtUsername.TabIndex = 0;
			this.txtUsername.Text = "";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(80, 80);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(176, 20);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Enter username and password for the account the Test Center will run under.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Username:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Password:";
			// 
			// cmdLogin
			// 
			this.cmdLogin.Location = new System.Drawing.Point(8, 112);
			this.cmdLogin.Name = "cmdLogin";
			this.cmdLogin.TabIndex = 5;
			this.cmdLogin.Text = "Login";
			this.cmdLogin.Click += new System.EventHandler(this.cmdLogin_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(96, 112);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.TabIndex = 6;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// lblError
			// 
			this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(176, 112);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(136, 32);
			this.lblError.TabIndex = 7;
			this.lblError.Visible = false;
			// 
			// AuthForm
			// 
			this.AcceptButton = this.cmdLogin;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(320, 151);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdLogin);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUsername);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "AuthForm";
			this.Text = "Authenticate";
			this.ResumeLayout(false);

		}
		#endregion

		private void FormError(string msg) {
			lblError.Visible = true;
			lblError.Text = msg;
		}

		private void cmdLogin_Click(object sender, System.EventArgs e) {
			if (txtUsername.Text == "" || txtPassword.Text == "")
				FormError("Please enter a username and password");
			else {
				if (!TestCenter.GetInstance().Authenticate(txtUsername.Text, txtPassword.Text))
					FormError("Either the username or password is incorrect");
				else 
					DialogResult = DialogResult.OK;
			}
		}

		private void cmdCancel_Click(object sender, System.EventArgs e) {
			DialogResult = DialogResult.Cancel;
		}
	}
}
