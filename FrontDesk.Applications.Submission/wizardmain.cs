using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SMS.Windows.Forms;

using FrontDesk.Applications.Submission.UserService;

namespace FrontDesk.Applications.Submission {

	public class WizardMain : WizardForm {

		private System.ComponentModel.IContainer components = null;

		public ServiceTicket Ticket;
		public string Url;
		public int AsstID;

		public WizardMain() {
			InitializeComponent();
			Controls.AddRange( 
				new Control[] {
					new LoginPage(),
					new SetupPage(),
					new SubmissionPage(),
					new FinishPage()
				});
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) 
				if (components != null) 
					components.Dispose();
				
			base.Dispose( disposing );
		}


		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// m_backButton
			// 
			this.m_backButton.Name = "m_backButton";
			this.m_backButton.TabIndex = 20;
			// 
			// m_nextButton
			// 
			this.m_nextButton.Name = "m_nextButton";
			this.m_nextButton.TabIndex = 21;
			// 
			// m_cancelButton
			// 
			this.m_cancelButton.CausesValidation = false;
			this.m_cancelButton.Name = "m_cancelButton";
			this.m_cancelButton.TabIndex = 22;
			// 
			// m_finishButton
			// 
			this.m_finishButton.Name = "m_finishButton";
			// 
			// m_separator
			// 
			this.m_separator.Name = "m_separator";
			// 
			// WizardMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(497, 360);
			this.Name = "WizardMain";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FrontDesk Submission System";
			this.Load += new System.EventHandler(this.WizardMain_Load);
			this.Closed += new System.EventHandler(this.WizardMain_Closed);

		}
		#endregion

		private void WizardMain_Closed(object sender, System.EventArgs e) {
			Application.Exit();
		}

		private void WizardMain_Load(object sender, System.EventArgs e) {

		}

	}
}

