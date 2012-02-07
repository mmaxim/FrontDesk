using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SMS.Windows.Forms;

namespace FrontDesk.Applications.Submission {

	public class FinishPage : ExteriorWizardPage{
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;

		public FinishPage() {
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FinishPage));
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Name = "m_titleLabel";
			this.m_titleLabel.Text = "Congratulations! Submission successful.";
			// 
			// m_watermarkPicture
			// 
			this.m_watermarkPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_watermarkPicture.Image")));
			this.m_watermarkPicture.Name = "m_watermarkPicture";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(184, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 56);
			this.label1.TabIndex = 2;
			this.label1.Text = "FrontDesk has successfully received and processed your submission. You will be se" +
				"nt an email shortly with automatic on-submission tests if and only if the course" +
				" staff has defined such tests.";
			// 
			// finishpage
			// 
			this.Controls.Add(this.label1);
			this.Name = "finishpage";
			this.Load += new System.EventHandler(this.finishpage_Load);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_watermarkPicture, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private void finishpage_Load(object sender, System.EventArgs e) {
			Wizard.SetWizardButtons(WizardButton.Finish);
		}
	}
}

